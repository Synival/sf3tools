using System;
using OpenTK.Mathematics;

namespace SF3.Win.OpenGL {
    public class Quad {
        private static readonly Vector3 c_white = new Vector3(1, 1, 1);
        private static readonly Vector3[] c_allWhite = [c_white, c_white, c_white, c_white];

        public Quad(Vector3[] vertices, Vector3 color) : this(vertices, null, 0, [color, color, color, color]) { }
        public Quad(Vector3[] vertices, Vector3[] colors) : this(vertices, null, 0, colors) { }
        public Quad(Vector3[] vertices, TextureAnimation textureAnim, byte textureFlags) : this(vertices, textureAnim, textureFlags, c_allWhite) { }
        public Quad(Vector3[] vertices, TextureAnimation textureAnim, byte textureFlags, Vector3 color)
            : this(vertices, textureAnim, textureFlags, [color, color, color, color]) { }

        public Quad(Vector3[] vertices, TextureAnimation textureAnim, byte textureFlags, Vector3[] colors) {
            if (vertices == null || vertices.Length != 4)
                throw new ArgumentException(nameof(vertices));
            if (colors == null || colors.Length != 4)
                throw new ArgumentException(nameof(colors));

            Vertices = vertices;
            TextureAnim = textureAnim;
            TextureFlags = textureFlags;
            Colors = colors;
        }

        public Vector3[] Vertices { get; }
        public TextureAnimation TextureAnim { get; }
        public byte TextureFlags { get; }
        public Vector3[] Colors { get; }
    }
}
