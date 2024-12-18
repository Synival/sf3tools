using System;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using SF3.Win.ThirdParty.TexturePacker;

namespace SF3.Win.OpenGL {
    public class QuadModel : IDisposable {
        public QuadModel(Quad[] quads) {
            if (quads == null)
                throw new ArgumentNullException(nameof(quads));

            _quadCount = quads.Length;
            _textureAnims = quads.Select(x => x.TextureAnim).ToArray();
            _textureFlags = quads.Select(x => x.TextureFlags).ToArray();

            var textures = quads
                .Where(x => x.TextureAnim != null)
                .SelectMany(x => x.TextureAnim.Frames)
                .Distinct()
                .ToArray();

            _textureAtlas = new TextureAtlas(textures, 0, true);
            _textureBitmap = _textureAtlas.CreateBitmap();

            if (_textureBitmap != null)
                _texture = new Texture(_textureBitmap);

            _vertices = quads
                .SelectMany(x => x.Vertices.Select((y, i) => new {
                    Quad = x,
                    Vertex = y,
                    Color = x.Colors[i],
                    Index = i
                }))
                .SelectMany(x => new float[] {
                    x.Vertex.X, x.Vertex.Y, x.Vertex.Z,
                    x.Color.X,  x.Color.Y,  x.Color.Z,
                    0, 0
                })
                .ToArray();

            UpdateAnimatedTextures(0);

            _indices = quads
                .Select((x, i) => new { Quad = x, StartIndex = (uint) i * 4 })
                .SelectMany(x => new uint[] {
                    x.StartIndex + 0, x.StartIndex + 1, x.StartIndex + 2,
                    x.StartIndex + 0, x.StartIndex + 2, x.StartIndex + 3
                })
                .ToArray();

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            var stride = 8 * sizeof(float);

            // Position
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);

            // Color
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Texture Coordinates
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        }

        private int _frame = 0;

        public bool UpdateAnimatedTextures(int frameIncrement = 1) {
            if (_texture == null)
                return false;

            _frame += frameIncrement;

            // Update UV coordinates
            int pos = 6;
            bool modified = false;
            for (var i = 0; i < _quadCount; i++) {
                var textureAnim = _textureAnims[i];
                if (textureAnim == null)
                    continue;

                var frame = textureAnim.GetFrame(_frame);
                var texCoords = _textureAtlas.GetUVCoordinatesByTextureIDFrame(
                    frame.ID, frame.Frame, _textureBitmap.Width, _textureBitmap.Height, _textureFlags[i]);

                for (var j = 0; j < 4; j++) {
                    if (!modified && (_vertices[pos] != texCoords[j].X || _vertices[pos + 1] != texCoords[j].Y))
                        modified = true;
                    _vertices[pos]     = texCoords[j].X;
                    _vertices[pos + 1] = texCoords[j].Y;
                    pos += 8;
                }
            }

            if (modified) {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);
            }

            return modified;
        }

        public void Draw() {
            _texture?.Use();
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                _textureAtlas?.Dispose();
                _texture?.Dispose();
                GL.DeleteBuffer(_elementBufferObject);
                GL.DeleteVertexArray(_vertexArrayObject);
                GL.DeleteBuffer(_vertexBufferObject);
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~QuadModel() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        private readonly int _quadCount;

        private readonly TextureAnimation[] _textureAnims;
        private readonly byte[] _textureFlags;

        private readonly TextureAtlas _textureAtlas;
        private readonly Bitmap _textureBitmap;
        private readonly Texture _texture;

        private readonly float[] _vertices;
        private readonly uint[] _indices;

        private readonly int _vertexBufferObject;
        private readonly int _elementBufferObject;
        private readonly int _vertexArrayObject;
    }
}
