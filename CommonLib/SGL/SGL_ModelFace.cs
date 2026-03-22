using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonLib.SGL {
    public class SGL_ModelFace : ISGL_ModelFace {
        public SGL_ModelFace() {
            VertexIndices = new int[4];
            Normal        = new VECTOR(0, 0, 0);
            Attributes    = new ATTR();
        }

        public SGL_ModelFace(ISGL_ModelFace original) {
            if (original.VertexIndices != null)
                VertexIndices = original.VertexIndices.ToArray();
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

            VertexIndices = vertexIndices;
            Normal        = normal;
            Attributes    = attributes;
        }

        public static SGL_ModelFace FromJToken(JToken token) => new SGL_ModelFace(token);
        private SGL_ModelFace(JToken token) {
            var jObject = (JObject) token;

            VertexIndices = jObject.GetValueIfExists("VertexIndices", t => ((JArray) t).Select(x => (int) x).ToArray());
            if (VertexIndices == null || VertexIndices.Count != 4)
                throw new JsonSerializationException("VertexIndices must exist and have exactly 4 integers");

            Normal     = VECTOR.FromJToken(jObject["Normal"]);
            Attributes = ATTR.FromJToken(jObject["Attributes"]);
        }

        public IReadOnlyList<int> VertexIndices { get; }
        public VECTOR Normal { get; set; }
        public IATTR Attributes { get; set; }
    }
}
