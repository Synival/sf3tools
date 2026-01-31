using CommonLib.Extensions;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Extensions {
    public static class IMPD_ModelLoDExtensions {
        public static JToken ToJToken(this IMPD_ModelLoD model) => model.ToJObject();
        public static JObject ToJObject(this IMPD_ModelLoD model) {
            var jObject = ISGL_ModelExtensions.ToJObject(model);

            // 'Collection' is not serialized, and 'LevelOfDetail' is only serialized for the primary models.
            jObject.AddFirst(new JProperty("ID", model.ModelID));
            if (model.Collection == MPD_CollectionType.Primary)
                jObject.Add("LevelOfDetail", model.LevelOfDetail);

            return jObject;
        }
    }
}
