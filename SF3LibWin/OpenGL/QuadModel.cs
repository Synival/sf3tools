using System;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using SF3.Win.ThirdParty.TexturePacker;

namespace SF3.Win.OpenGL {
    public class QuadModel : IDisposable {
        public QuadModel(Quad[] quads) {
            if (quads == null)
                throw new ArgumentNullException(nameof(quads));

            var textures = quads
                .Where(x => x.Texture != null)
                .Select(x => x.Texture)
                .Distinct()
                .ToArray();
            TextureAtlas = new TextureAtlas(textures, 0, true);
            var textureBitmap = TextureAtlas.CreateBitmap();

            if (textureBitmap != null)
                Texture = new Texture(textureBitmap);

            _vertices = quads
                .SelectMany(x => x.Vertices.Select((y, i) => new {
                    Quad = x,
                    Vertex = y,
                    Color = x.Colors[i],
                    TexCoords = TextureAtlas.GetUVCoordinatesByTextureID(x.Texture.ID, textureBitmap.Width, textureBitmap.Height, x.TextureFlags),
                    Index = i
                }))
                .SelectMany(x => new float[] {
                    x.Vertex.X, x.Vertex.Y, x.Vertex.Z,
                    x.Color.X,  x.Color.Y,  x.Color.Z,
                    x.TexCoords[x.Index].X, x.TexCoords[x.Index].Y
                })
                .ToArray();

            _indices = quads
                .Select((x, i) => new { Quad = x, StartIndex = (uint) i * 4 })
                .SelectMany(x => new uint[] {
                    x.StartIndex + 0, x.StartIndex + 1, x.StartIndex + 2,
                    x.StartIndex + 0, x.StartIndex + 2, x.StartIndex + 3
                })
                .ToArray();

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

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

        public void Draw() {
            Texture?.Use();
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                TextureAtlas?.Dispose();
                Texture?.Dispose();
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

        private readonly float[] _vertices;
        private readonly uint[] _indices;
        private TextureAtlas TextureAtlas { get; }
        private Texture Texture { get; }

        private readonly int _vertexBufferObject;
        private readonly int _elementBufferObject;
        private readonly int _vertexArrayObject;
    }
}
