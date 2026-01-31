using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Extensions {
    public static class IMPD_ModelInstanceExtensions {
        public static JToken ToJToken(this IMPD_ModelInstance instance) => instance.ToJObject();
        public static JObject ToJObject(this IMPD_ModelInstance instance) {
            // 'Collection' is not serialized.
            var properties = new List<JProperty>() {
                new JProperty("ID",        instance.ID),
                new JProperty("ModelID",   instance.ModelID),
                new JProperty("PositionX", instance.PositionX),
                new JProperty("PositionY", instance.PositionY),
                new JProperty("PositionZ", instance.PositionZ),
                new JProperty("AngleX",    instance.AngleX),
                new JProperty("AngleY",    instance.AngleY),
                new JProperty("AngleZ",    instance.AngleZ),
                new JProperty("ScaleX",    instance.ScaleX),
                new JProperty("ScaleY",    instance.ScaleY),
                new JProperty("ScaleZ",    instance.ScaleZ),
            };

            if (instance.Collection == MPD_CollectionType.Primary) {
                properties.Add(new JProperty("Tag",   instance.Tag));
                properties.Add(new JProperty("Flags", instance.Flags));
                properties.Add(new JProperty("LevelsOfDetail", instance.LevelsOfDetail));
            }

            return new JObject(properties.ToArray());
        }
    }
}
