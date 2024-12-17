using System;
using OpenTK.Mathematics;

namespace SF3.Win.OpenGL {
    public struct Quad {
        public Quad(Vector3[] vertices) {
            if (vertices == null || vertices.Length != 4)
                throw new ArgumentException(nameof(vertices));
            Vertices = vertices;
        }

        public Vector3[] Vertices { get; }
    }
}
