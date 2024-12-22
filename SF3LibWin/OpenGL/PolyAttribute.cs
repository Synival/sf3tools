using System;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class PolyAttribute : BaseAttribute {
        public PolyAttribute(int elements, ActiveAttribType type, string name, int vertices, float[,] data) : base(elements, type, name) {
            // Only floats accepted for now.
            if (PointerType != VertexAttribPointerType.Float)
                throw new ArgumentException(nameof(type));
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.GetLength(0) != vertices)
                throw new ArgumentException(nameof(type) + "[]");

            var elementsPerVertex = SizeInBytes / sizeof(float);
            if (data.GetLength(1) != elementsPerVertex)
                throw new ArgumentException(nameof(type) + "[][]");

            Vertices = vertices;
            Data     = data;
        }

        public int Vertices { get; }

        public float[,] Data { get; }
    }
}
