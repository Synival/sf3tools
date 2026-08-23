using CommonLib.Extensions;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Extensions {
    public static class IMPD_ModelLoDExtensions {
        public static JToken ToJToken(this IMPD_ModelLoD model) => model.ToJObject();
        public static JObject ToJObject(this IMPD_ModelLoD model) {
            // We want to serialize 'ModelID' as "ID" instead of "ModelID", so don't do it automatically.
            // 'Collection' is not serialized, and 'LevelOfDetail' is only serialized for the primary models.
            var jObject = ISGL_ModelExtensions.ToJObject(model, serializeCollectionId: false, serializeId: false, serializeLoD: false);
            jObject.AddFirst(new JProperty("ID", model.ModelID));
            if (model.Collection == MPD_CollectionType.Primary)
                jObject.Add("LevelOfDetail", model.LevelOfDetail);

            return jObject;
        }
    }
}
