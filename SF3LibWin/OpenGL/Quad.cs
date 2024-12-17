using System;
using System.Linq;
using CommonLib.Extensions;
using OpenTK.Mathematics;

namespace SF3.Win.OpenGL {
    public class Quad {
        public Quad(Vector3[] vertices, ITexture texture, byte textureFlags) {
            if (vertices == null || vertices.Length != 4)
                throw new ArgumentException(nameof(vertices));

            Vertices = vertices;
            Texture = texture;

            Colors = new Vector3[4];

            var bmpData = texture.ImageData16Bit.To1DArray();
            var avgR = bmpData.Average(x => (float) ((x >> 0)  & 0x1f) / 0x1f);
            var avgG = bmpData.Average(x => (float) ((x >> 5)  & 0x1f) / 0x1f);
            var avgB = bmpData.Average(x => (float) ((x >> 10) & 0x1f) / 0x1f);

            Colors[0] = new Vector3(avgR, avgG, avgB);
            Colors[1] = new Vector3(avgR, avgG, avgB);
            Colors[2] = new Vector3(avgR, avgG, avgB);
            Colors[3] = new Vector3(avgR, avgG, avgB);
        }

        public Vector3[] Vertices { get; }
        public ITexture Texture { get; }
        public Vector3[] Colors { get; }
    }
}
