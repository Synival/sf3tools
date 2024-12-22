using System;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using SF3.Win.ThirdParty.TexturePacker;

namespace SF3.Win.OpenGL {
    public class QuadModel : IDisposable {
        public QuadModel(Quad[] quads, Shader shader) {
            if (quads == null)
                throw new ArgumentNullException(nameof(quads));
            if (shader == null)
                throw new ArgumentNullException(nameof(quads));

            Quads = quads;
            Shader = shader;

            // Create a texture atlas for this model and generate a texture for it.
            // TODO: let QuadModels share TextureAtlas' + Textures
            var textures = quads
                .Where(x => x.TextureAnim != null)
                .SelectMany(x => x.TextureAnim.Frames)
                .Distinct()
                .ToArray();

            _textureAtlas = new TextureAtlas(textures, 0, true);
            _textureBitmap = _textureAtlas.CreateBitmap();
            _texture = _textureBitmap != null ? new Texture(_textureBitmap) : null;

            // Create the vertex buffer with all the data needed for each vertex.
            _vertexBuffer = new float[shader.GetVertexBufferSize(Quads.Length * 4) / sizeof(float)];
            AssignVertexBufferPositions();
            AssignVertexBufferColors();
            _ = AssignVertexBufferTexCoords();

            // Create indices for DrawElements().
            // TODO: quads are malformed :( :( :(
            _elementBuffer = quads
                .Select((x, i) => new { Quad = x, StartIndex = (uint) i * 4 })
                .SelectMany(x => new uint[] {
                    x.StartIndex + 2, x.StartIndex + 1, x.StartIndex + 0,
                    x.StartIndex + 3, x.StartIndex + 2, x.StartIndex + 0
                })
                .ToArray();

            // Create the VBO.
            _vertexBufferObject = new Buffer();
            UpdateVBO();

            // Create the EBO.
            _elementBufferObject = new Buffer();
            UpdateEBO();

            // Create the VAO.
            _vertexArrayObject = new VertexArray();
            using (_vertexArrayObject.Use())
            using (_vertexBufferObject.Use(BufferTarget.ArrayBuffer))
                foreach (var attr in new string[] { "position", "color", "texCoord0" })
                    Shader.EnableVAO_Attribute(attr);
        }

        private void AssignVertexBufferColors() {
            var attr = Shader.GetAttributeByName("position");
            if (attr == null)
                return;

            // TODO: offset should not come from the shader, but a layout defined in the VBO
            var pos = attr.Offset / sizeof(float);
            int i = 0;
            foreach (var quad in Quads) {
                for (var vertexIndex = 0; vertexIndex < 4; vertexIndex++) {
                    var color = quad.Colors[vertexIndex];
                    _vertexBuffer[pos + 0] = color.X;
                    _vertexBuffer[pos + 1] = color.Y;
                    _vertexBuffer[pos + 2] = color.Z;
                    pos += Shader.VertexBufferStride / sizeof(float);
                }
                i++;
            }
        }

        private void AssignVertexBufferPositions() {
            var attr = Shader.GetAttributeByName("color");
            if (attr == null)
                return;

            // TODO: offset should not come from the shader, but a layout defined in the VBO
            var pos = attr.Offset / sizeof(float);
            foreach (var quad in Quads) {
                for (var vertexIndex = 0; vertexIndex < 4; vertexIndex++) {
                    var vertex = quad.Vertices[vertexIndex];
                    _vertexBuffer[pos + 0] = vertex.X;
                    _vertexBuffer[pos + 1] = vertex.Y;
                    _vertexBuffer[pos + 2] = vertex.Z;
                    pos += Shader.VertexBufferStride / sizeof(float);
                }
            }
        }

        private bool AssignVertexBufferTexCoords() {
            if (_texture == null)
                return false;

            var attr = Shader.GetAttributeByName("texCoord0");
            if (attr == null)
                return false;

            // Update UV coordinates

            // TODO: offset should not come from the shader, but a layout defined in the VBO
            var pos = attr.Offset / sizeof(float);
            var modified = false;
            foreach (var quad in Quads) {
                var textureAnim = quad.TextureAnim;
                if (textureAnim == null)
                    continue;

                var frame = textureAnim.GetFrame(_frame);
                var texCoords = _textureAtlas.GetUVCoordinatesByTextureIDFrame(
                    frame.ID, frame.Frame, _textureBitmap.Width, _textureBitmap.Height, quad.TextureFlags);

                for (var vertexIndex = 0; vertexIndex < 4; vertexIndex++) {
                    if (!modified && (_vertexBuffer[pos] != texCoords[vertexIndex].X || _vertexBuffer[pos + 1] != texCoords[vertexIndex].Y))
                        modified = true;
                    _vertexBuffer[pos + 0] = texCoords[vertexIndex].X;
                    _vertexBuffer[pos + 1] = texCoords[vertexIndex].Y;
                    pos += Shader.VertexBufferStride / sizeof(float);
                }
            }

            return modified;
        }

        private void UpdateVBO() {
            using (_vertexBufferObject.Use(BufferTarget.ArrayBuffer))
                GL.BufferData(BufferTarget.ArrayBuffer, _vertexBuffer.Length * sizeof(float), _vertexBuffer, BufferUsageHint.DynamicDraw);

        }

        private void UpdateEBO() {
            using (_elementBufferObject.Use(BufferTarget.ElementArrayBuffer))
                GL.BufferData(BufferTarget.ElementArrayBuffer, _elementBuffer.Length * sizeof(uint), _elementBuffer, BufferUsageHint.StaticDraw);
        }

        private int _frame = 0;

        public bool UpdateAnimatedTextures(int frameIncrement = 1) {
            _frame += frameIncrement;
            var result = AssignVertexBufferTexCoords();
            if (result)
                UpdateVBO();
            return result;
        }

        public void Draw() {
            using (_texture?.Use())
            using (_vertexArrayObject.Use())
            using (_elementBufferObject.Use(BufferTarget.ElementArrayBuffer))
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
                _elementBufferObject?.Dispose();
                _vertexArrayObject?.Dispose();
                _vertexBufferObject?.Dispose();
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
        public Shader Shader { get; }

        private TextureAtlas _textureAtlas { get; }
        private Bitmap _textureBitmap { get; }
        private Texture _texture { get; }

        private Buffer _vertexBufferObject { get; }
        private VertexArray _vertexArrayObject { get; }
        private Buffer _elementBufferObject { get; }

        private readonly float[] _vertexBuffer;
        private readonly uint[] _elementBuffer;
    }
}
