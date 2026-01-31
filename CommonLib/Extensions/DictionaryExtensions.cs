using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CommonLib.Extensions {
    public static class DictionaryExtensions {
        public static JObject ToJObject<T>(this Dictionary<string, T> dictionary)
            => JObject.FromObject(dictionary);
    }
}
