using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Extensions {
    public static class IMPD_ModelCollectionExtensions {
        public static bool IsHeaderModelCollection(this IMPD_ModelCollection mc)
            => mc.Collection.IsHeaderModelCollection();

        public static string ToJSON_String(this IMPD_ModelCollection mc)
            => mc.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_ModelCollection mc) => mc.ToJObject();
        public static JObject ToJObject(this IMPD_ModelCollection mc) {
            // "Collection" and "HasMissingModels" are not serialized.
            // "IsUnreferences" is only applicable to main/header models.
            var properties = new List<JProperty>() {
                new JProperty("Models",             mc.Models?.Select(x => x.ToJObject())?.ToArray()?.ToJArray()),
                new JProperty("ModelInstances",     mc.ModelInstances?.Select(x => x.ToJObject())?.ToArray()?.ToJArray()),
                new JProperty("Textures",           mc.Textures?.Select(x => x.ToJObject())?.ToArray()?.ToJArray()),
                new JProperty("DataAfterInstances", mc.DataAfterInstances?.ToArray()?.ToJArray()),
            };

            if (mc.Collection.IsHeaderModelCollection())
                properties.Add(new JProperty("IsUnreferenced", mc.IsUnreferenced));

            return new JObject(properties.ToArray());
        }
    }
}
