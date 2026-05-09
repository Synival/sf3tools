using System;
using CommonLib.Attributes;
using Newtonsoft.Json.Linq;
using SF3.Utils;

namespace SF3.Types {
    public enum SpriteDirectionCountType {
        [EnumDisplayName("One (No Mirror)")]
        OneNoFlip    = 0x01,

        [EnumDisplayName("Two (No Mirror)")]
        TwoNoFlip    = 0x02,

        [EnumDisplayName("Four (Mirror)")]
        Four         = 0x04,

        [EnumDisplayName("Five (Mirror)")]
        Five         = 0x05,

        [EnumDisplayName("Six (Mirror)")]
        Six          = 0x06,

        [EnumDisplayName("Eight")]
        Eight        = 0x08,

        [EnumDisplayName("One (Mirrorable)")]
        OneFlippable = 0x11,
    }

    public static class SpriteDirectionCountTypeExtensions {
        public static bool ShouldSerializeAsNumber(this SpriteDirectionCountType type)
            => (type == SpriteDirectionCountType.Four) || (type == SpriteDirectionCountType.Five) || (type == SpriteDirectionCountType.Six) || (type == SpriteDirectionCountType.Eight);

        public static string ToSerializedString(this SpriteDirectionCountType type)
            => type.ShouldSerializeAsNumber() ? ((int) type).ToString() : type.ToString();

        public static JToken ToJToken(this SpriteDirectionCountType type) => type.ToJValue();
        public static JValue ToJValue(this SpriteDirectionCountType type)
            => type.ShouldSerializeAsNumber() ? new JValue((int) type) : new JValue(type.ToString());

        public static SpriteDirectionCountType FromSerializedString(string str)
            => int.TryParse(str, out var intVal) ? (SpriteDirectionCountType) intVal : (SpriteDirectionCountType) Enum.Parse(typeof(SpriteDirectionCountType), str);

        public static SpriteDirectionCountType FromJToken(JToken jToken)
            => (jToken.Type == JTokenType.Integer) ? (SpriteDirectionCountType) ((int) jToken) : jToken.ToObject<SpriteDirectionCountType>();

        public static int GetAnimationFrameCount(this SpriteDirectionCountType directions) {
            switch (directions) {
                case SpriteDirectionCountType.TwoNoFlip: return 2;
                case SpriteDirectionCountType.Four:      return 4;
                case SpriteDirectionCountType.Five:      return 5;
                case SpriteDirectionCountType.Six:       return 6;
                case SpriteDirectionCountType.Eight:     return 8;
                default: return 1;
            }
        }

        public static bool IsFlippable(this SpriteDirectionCountType directions) {
            switch (directions) {
                case SpriteDirectionCountType.OneFlippable:
                case SpriteDirectionCountType.Four:
                case SpriteDirectionCountType.Five:
                case SpriteDirectionCountType.Six:
                    return true;

                default:
                    return false;
            }
        }

        public static SpriteFrameDirection[] ToAnimationFrameDirections(this SpriteDirectionCountType directions)
            => CHR_Utils.GetFrameGroupDirections(directions.GetAnimationFrameCount());

        public static SpriteFrameDirection[] ToSpritesheetDirections(this SpriteDirectionCountType directions)
            => SpriteUtils.GetFrameGroupDirections(directions.GetAnimationFrameCount());
    }
}
