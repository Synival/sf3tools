using CommonLib.SGL;
using Newtonsoft.Json.Linq;

namespace CommonLib.Extensions {
    public static class ISGL_ModelFaceExtensions {
        public static JObject ToJObject(this ISGL_ModelFace face) {
            return new JObject {
                { "VertexIndices", face.VertexIndices?.AsArray()?.ToJArray() },
                { "Normal",        face.Normal.ToJObject() },
                { "Attributes",    face.Attributes?.ToJObject() },
            };
        }
    }
}
