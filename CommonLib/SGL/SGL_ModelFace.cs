using System;
using CommonLib.Extensions;

namespace CommonLib.SGL {
    public class SGL_ModelFace : ISGL_ModelFace {
        public SGL_ModelFace() {
            VertexIndices = new int[4].ToEnumerableWithLength();
            Normal        = new VECTOR(0, 0, 0);
            Attributes    = new ATTR();
        }

        public SGL_ModelFace(ISGL_ModelFace original) {
            if (original.VertexIndices != null)
                VertexIndices = ((int[]) (original.VertexIndices.AsArray().Clone())).ToEnumerableWithLength();
            Normal = original.Normal;
            Attributes = new ATTR(original.Attributes);
        }

        public SGL_ModelFace(int[] vertexIndices, VECTOR normal, IATTR attributes) {
            if (vertexIndices == null)
                throw new ArgumentNullException(nameof(vertexIndices));
            if (vertexIndices.Length != 4)
                throw new ArgumentException($"{nameof(vertexIndices)} must have 4 values");
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes));

            VertexIndices = vertexIndices.ToEnumerableWithLength();
            Normal        = normal;
            Attributes    = attributes;
        }

        public IIndexedEnumerableWithLength<int> VertexIndices { get; }
        public VECTOR Normal { get; set; }
        public IATTR Attributes { get; set; }
    }
}
