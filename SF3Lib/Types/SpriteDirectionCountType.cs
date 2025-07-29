using System;
using Newtonsoft.Json.Linq;

namespace SF3.Types {
    public enum SpriteDirectionCountType {
        OneNoFlip    = 0x01,
        TwoNoFlip    = 0x02,
        Four         = 0x04,
        Five         = 0x05,
        Six          = 0x06,
        Eight        = 0x08,
        OneFlippable = 0x11,
    }

    public static class SpriteDirectionCountTypeExtensions {
        public static bool ShouldSerializeAsNumber(this SpriteDirectionCountType type)
            => (type == SpriteDirectionCountType.Four) || (type == SpriteDirectionCountType.Five) || (type == SpriteDirectionCountType.Six) || (type == SpriteDirectionCountType.Eight);

        public static string ToSerializedString(this SpriteDirectionCountType type)
            => type.ShouldSerializeAsNumber() ? ((int) type).ToString() : type.ToString();

        public static JToken ToJToken(this SpriteDirectionCountType type)
            => type.ShouldSerializeAsNumber() ? new JValue((int) type) : new JValue(type.ToString());

        public static SpriteDirectionCountType FromSerializedString(string str)
            => int.TryParse(str, out var intVal) ? (SpriteDirectionCountType) intVal : (SpriteDirectionCountType) Enum.Parse(typeof(SpriteDirectionCountType), str);

        public static SpriteDirectionCountType FromJToken(JToken jToken)
            => (jToken.Type == JTokenType.Integer) ? (SpriteDirectionCountType) ((int) jToken) : jToken.ToObject<SpriteDirectionCountType>();
    }
}
