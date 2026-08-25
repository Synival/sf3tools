using System.Linq;
using CommonLib.SGL;
using Newtonsoft.Json.Linq;

namespace CommonLib.Extensions {
    public static class ISGL_ModelExtensions {
        public static JObject ToJObject(this ISGL_Model model, bool serializeCollectionId, bool serializeId, bool serializeLoD) {
            var jObj = new JObject {
                { "Vertices", model.Vertices?.Select(x => x.ToJObject())?.ToArray()?.ToJArray() },
                { "Faces",    model.Faces?.Select(x => x.ToJObject())?.ToArray()?.ToJArray() },
            };
            if (model.VertexNormals != null)
                jObj.Add("VertexNormals", model.VertexNormals?.Select(x => x.ToJObject())?.ToArray()?.ToJArray());

            if (serializeLoD)
                jObj.AddFirst(new JProperty("LevelOfDetail", model.LevelOfDetail));
            if (serializeId)
                jObj.AddFirst(new JProperty("ModelID", model.ModelID));
            if (serializeCollectionId)
                jObj.AddFirst(new JProperty("ModelCollectionID", model.ModelCollectionID));

            return jObj;
        }
    }
}
