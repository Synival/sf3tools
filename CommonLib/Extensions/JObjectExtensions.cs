using System;
using Newtonsoft.Json.Linq;

namespace CommonLib.Extensions {
    public static class JObjectExtensions {
        public static JToken GetValueIfExists(this JObject jObject, string propertyName) {
            if (!jObject.TryGetValue(propertyName, out var jToken))
                return null;
            return (jToken.Type == JTokenType.Null || jToken.Type == JTokenType.Undefined)
                ? null : jToken;
        }

        public static T GetValueIfExists<T>(this JObject jObject, string propertyName, Func<JToken, T> getter) where T : class {
            if (!jObject.TryGetValue(propertyName, out var jToken))
                return null;
            return (jToken.Type == JTokenType.Null || jToken.Type == JTokenType.Undefined)
                ? null : getter(jToken);
        }
    }
}
