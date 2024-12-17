using System;
using OpenTK.Mathematics;

namespace SF3.Win.OpenGL {
    public class Quad {
        public Quad(Vector3[] vertices, ITexture texture, byte textureFlags) {
            if (vertices == null || vertices.Length != 4)
                throw new ArgumentException(nameof(vertices));

            Vertices = vertices;
            Texture = texture;
            TextureFlags = textureFlags;

            var white = new Vector3(1, 1, 1);
            Colors = [white, white, white, white];
        }

        public Vector3[] Vertices { get; }
        public ITexture Texture { get; }
        public byte TextureFlags { get; }
        public Vector3[] Colors { get; }
    }
}
