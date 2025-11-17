using System;

namespace CommonLib.SGL {
    public class SGL_ModelFace {
        public SGL_ModelFace() {
            VertexIndices = new int[4];
            Normal        = new VECTOR(0, 0, 0);
            Attributes    = new ATTR();
        }

        public SGL_ModelFace(int[] vertexIndices, VECTOR normal, IATTR attributes) {
            if (vertexIndices == null)
                throw new ArgumentNullException(nameof(vertexIndices));
            if (vertexIndices.Length != 4)
                throw new ArgumentException($"{nameof(vertexIndices)} must have 4 values");
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes));

            VertexIndices = vertexIndices;
            Normal        = normal;
            Attributes    = attributes;
        }

        public int[] VertexIndices { get; }
        public VECTOR Normal { get; set; }
        public IATTR Attributes { get; set; }
    }
}
