using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class Formatter {
        public string Format(string text)
            => Format(JToken.Parse(text));

        public string Format(JToken token) {
            return token.ToString(Newtonsoft.Json.Formatting.Indented);
        }
    }
}
