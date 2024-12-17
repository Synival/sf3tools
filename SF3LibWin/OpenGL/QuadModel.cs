using System;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class QuadModel : IDisposable {
        public QuadModel(Quad[] quads) {
            if (quads == null)
                throw new ArgumentException(nameof(quads));

            _vertices = quads
                .SelectMany(x => x.Vertices.Select((y, i) => new { Quad = x, Vertex = y, Color = x.Colors[i] }))
                .SelectMany(x => new float[] {
                    x.Vertex.X, x.Vertex.Y, x.Vertex.Z,
                    x.Color.X,  x.Color.Y,  x.Color.Z
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

            var stride = 6 * sizeof(float);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        }

        public void Draw() {
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                GL.DeleteBuffer(_elementBufferObject);
                GL.DeleteVertexArray(_vertexArrayObject);
                GL.DeleteBuffer(_vertexBufferObject);
                disposedValue = true;
            }
        }

        ~QuadModel() {
            if (disposedValue == false)
                System.Diagnostics.Debug.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private readonly float[] _vertices;
        private readonly uint[] _indices;

        private readonly int _vertexBufferObject;
        private readonly int _elementBufferObject;
        private readonly int _vertexArrayObject;
    }
}
