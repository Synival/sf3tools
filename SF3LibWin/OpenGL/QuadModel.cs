using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Win.OpenGL.MPD_File;
using SF3.Win.ThirdParty.TexturePacker;

namespace SF3.Win.OpenGL {
    public class QuadModel : IDisposable {
        public QuadModel(Quad[] quads) {
            if (quads == null)
                throw new ArgumentNullException(nameof(quads));

            Quads = quads;

            // Create a texture atlas for this model and generate a texture for it.
            // TODO: let QuadModels share TextureAtlas' + Textures
            var textures = quads
                .Where(x => x.TextureAnim != null)
                .SelectMany(x => x.TextureAnim.Frames)
                .Distinct()
                .ToArray();

            if (textures.Length > 0) {
                _textureAtlas = new TextureAtlas(textures, 0, true);
                _textureBitmap = _textureAtlas.CreateBitmap();
                _texture = _textureBitmap != null ? new Texture(_textureBitmap) : null;
            }
            else {
                _textureAtlas = null;
                _textureBitmap = null;
                _texture = null;
            }

            // Create the VBO.
            var quadAttrs = quads[0].Attributes.Select(x => new VBO_Attribute(x.Elements, x.Type, x.Name)).ToDictionary(x => x.Name);

            var expectedAttrs = new List<VBO_Attribute>() {
                new VBO_Attribute(1, ActiveAttribType.FloatVec3, "position"),
                new VBO_Attribute(1, ActiveAttribType.FloatVec3, "color"),
                new VBO_Attribute(1, ActiveAttribType.FloatVec2, Shader.GetTextureInfo(TextureUnit.Texture0).TexCoordName),
                new VBO_Attribute(1, ActiveAttribType.FloatVec2, Shader.GetTextureInfo(TextureUnit.Texture1).TexCoordName),
            };
            if (_textureAtlas != null)
                expectedAttrs.Add(new VBO_Attribute(1, ActiveAttribType.FloatVec2, Shader.GetTextureInfo(MPD_TextureUnit.TextureAtlas).TexCoordName));

            foreach (var ea in expectedAttrs.Where(ea => !quadAttrs.ContainsKey(ea.Name)).ToList())
                if (!quadAttrs.ContainsKey(ea.Name))
                    quadAttrs.Add(ea.Name, ea);

            _vbo = new VBO(quadAttrs.Select(x => x.Value).ToArray());

            // Create the vertex buffer with all the data needed for each vertex.
            _vertexBuffer = new float[_vbo.GetSizeInBytes(Quads.Length * 4) / sizeof(float)];

            foreach (var texInfo in Shader.TextureInfos)
                AssignVertexBufferDefaultTexCoords(texInfo);

            var polyAttrNames = Quads.SelectMany(x => x.Attributes).Select(x => x.Name).Distinct().ToArray();
            foreach (var polyAttrName in polyAttrNames)
                AssignVertexBuffer(polyAttrName);

            _ = AssignVertexBufferAtlasTexCoords();

            // Assign data to the VBO.
            AssignVBO_Data();

            // Create indices for DrawElements().
            // TODO: quads are malformed :( :( :(
            _elementBuffer = quads
                .Select((x, i) => new { Quad = x, StartIndex = (uint) i * 4 })
                .SelectMany(x => new uint[] {
                    x.StartIndex + 2, x.StartIndex + 1, x.StartIndex + 0,
                    x.StartIndex + 3, x.StartIndex + 2, x.StartIndex + 0
                })
                .ToArray();

            // Create the EBO.
            _ebo = new EBO();
            AssignEBO_Data();

            // Create the VAO.
            _vao = new VAO();
        }

        private void AssignVertexBuffer(string attrName) {
            var vboAttr = _vbo.GetAttributeByName(attrName);
            if (vboAttr == null || !vboAttr.OffsetInBytes.HasValue)
                return;

            var pos = vboAttr.OffsetInBytes.Value / sizeof(float);
            foreach (var quad in Quads) {
                var polyAttr = quad.GetAttributeByName(attrName);
                if (polyAttr == null || !vboAttr.IsAssignable(polyAttr))
                    continue;

                var vertices        = polyAttr.Data.GetLength(0);
                var floatsPerVertex = polyAttr.Data.GetLength(1);

                for (var i = 0; i < vertices; i++) {
                    for (var j = 0; j < floatsPerVertex; j++)
                        _vertexBuffer[pos + j] = polyAttr.Data[i, j];
                    pos += _vbo.StrideInBytes / sizeof(float);
                }
            }
        }

        private static readonly Vector2[] c_noTextureCoords = [
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0)
        ];

        private bool AssignVertexBufferAtlasTexCoords() {
            if (_texture == null)
                return false;

            var vboAttr = _vbo.GetAttributeByName(Shader.GetTextureInfo(MPD_TextureUnit.TextureAtlas).TexCoordName);
            if (vboAttr == null || !vboAttr.OffsetInBytes.HasValue)
                return false;

            var pos = vboAttr.OffsetInBytes.Value / sizeof(float);

            // Update UV coordinates
            var modified = false;
            foreach (var quad in Quads) {
                var frame = quad.TextureAnim?.GetFrame(_frame);
                var texCoords = (frame != null)
                    ? _textureAtlas.GetUVCoordinatesByTextureIDFrame(
                        frame.ID, frame.Frame, _textureBitmap.Width, _textureBitmap.Height, quad.TextureRotate, quad.TextureFlip)
                    : c_noTextureCoords;

                for (var vertexIndex = 0; vertexIndex < 4; vertexIndex++) {
                    if (!modified && (_vertexBuffer[pos] != texCoords[vertexIndex].X || _vertexBuffer[pos + 1] != texCoords[vertexIndex].Y))
                        modified = true;
                    _vertexBuffer[pos + 0] = texCoords[vertexIndex % 4].X;
                    _vertexBuffer[pos + 1] = texCoords[(vertexIndex + 2) % 4].Y; // Y coordinate is flipped.
                    pos += _vbo.StrideInBytes / sizeof(float);
                }
            }

            return modified;
        }

        private void AssignVertexBufferDefaultTexCoords(Shader.TextureInfo texInfo) {
            var vboAttr = _vbo.GetAttributeByName(texInfo.TexCoordName);
            if (vboAttr == null || !vboAttr.OffsetInBytes.HasValue)
                return;

            var pos = vboAttr.OffsetInBytes.Value / sizeof(float);
            void SetTexCoord(int x, int y) {
                _vertexBuffer[pos + 0] = x;
                _vertexBuffer[pos + 1] = y;
                pos += _vbo.StrideInBytes / sizeof(float);
            };

            foreach (var quad in Quads) {
                SetTexCoord(0, 1);
                SetTexCoord(1, 1);
                SetTexCoord(1, 0);
                SetTexCoord(0, 0);
            }
        }

        private void AssignVBO_Data() {
            using (_vbo.Use(BufferTarget.ArrayBuffer))
                GL.BufferData(BufferTarget.ArrayBuffer, _vertexBuffer.Length * sizeof(float), _vertexBuffer, BufferUsageHint.DynamicDraw);
        }

        private void AssignEBO_Data() {
            using (_ebo.Use(BufferTarget.ElementArrayBuffer))
                GL.BufferData(BufferTarget.ElementArrayBuffer, _elementBuffer.Length * sizeof(uint), _elementBuffer, BufferUsageHint.StaticDraw);
        }

        private int _frame = 0;

        public bool UpdateAnimatedTextures(int frameIncrement = 1) {
            _frame += frameIncrement;
            var result = AssignVertexBufferAtlasTexCoords();
            if (result)
                AssignVBO_Data();
            return result;
        }

        public void Draw(Shader shader)
            => Draw(shader, _texture);

        public void Draw(Shader shader, Texture textureAtlas) {
            using (_vao.Use())
                shader.AssignAttributes(_vbo);

            using ((textureAtlas != null) ? textureAtlas.Use(MPD_File.MPD_TextureUnit.TextureAtlas) : null)
            using (_vao.Use())
            using (_ebo.Use(BufferTarget.ElementArrayBuffer))
            using (shader.Use())
                GL.DrawElements(PrimitiveType.Triangles, _elementBuffer.Length, DrawElementsType.UnsignedInt, 0);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                _textureAtlas?.Dispose();
                _texture?.Dispose();
                _textureBitmap?.Dispose();
                _ebo?.Dispose();
                _vao?.Dispose();
                _vbo?.Dispose();
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~QuadModel() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine("QuadModel: GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public Quad[] Quads { get; }

        private TextureAtlas _textureAtlas { get; }
        private Bitmap _textureBitmap { get; }
        private Texture _texture { get; }

        private VBO _vbo { get; }
        private VAO _vao { get; }
        private EBO _ebo { get; }

        private readonly float[] _vertexBuffer;
        private readonly uint[] _elementBuffer;
    }
}
