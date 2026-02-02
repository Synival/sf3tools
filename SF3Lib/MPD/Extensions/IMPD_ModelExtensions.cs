using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Extensions {
    public static class IMPD_ModelExtensions {
        public static JToken ToJToken(this IMPD_Model model) => model.ToJObject();
        public static JObject ToJObject(this IMPD_Model model) {
            var lod0 = model.ModelLoDs[0];
            var jObject = lod0.ToJObject();

            if (!model.Collection.IsHeaderModelCollection())
                jObject.Add(new JProperty("LevelsOfDetail", model.LevelsOfDetail));

            return jObject;
        }
    }
}
