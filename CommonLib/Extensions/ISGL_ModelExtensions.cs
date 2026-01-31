using System.Linq;
using CommonLib.SGL;
using Newtonsoft.Json.Linq;

namespace CommonLib.Extensions {
    public static class ISGL_ModelExtensions {
        public static JObject ToJObject(this ISGL_Model model) {
            return new JObject {
                { "Vertices", model.Vertices?.Select(x => x.ToJObject())?.ToArray()?.ToJArray() },
                { "Faces",    model.Faces?.Select(x => x.ToJObject())?.ToArray()?.ToJArray() },
            };
        }
    }
}
