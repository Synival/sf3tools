using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SF3.Types;

namespace SF3.Sprites {
    public class FrameRef {
        public string SpriteName;
        public int FrameWidth;
        public int FrameHeight;
        public string FrameGroupName;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteFrameDirection FrameDirection;

        [JsonIgnore]
        public string ImageHash;

        public override string ToString() => $"{SpriteName} ({FrameWidth}x{FrameHeight}).{FrameGroupName} ({FrameDirection})";

        public override bool Equals(object obj) {
            return obj is FrameRef lookup
                && SpriteName     == lookup.SpriteName
                && FrameWidth     == lookup.FrameWidth
                && FrameHeight    == lookup.FrameHeight
                && FrameGroupName == lookup.FrameGroupName
                && FrameDirection == lookup.FrameDirection;
        }

        public override int GetHashCode() {
            var hashCode = 1259825651;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SpriteName);
            hashCode = hashCode * -1521134295 + FrameWidth.GetHashCode();
            hashCode = hashCode * -1521134295 + FrameHeight.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FrameGroupName);
            hashCode = hashCode * -1521134295 + FrameDirection.GetHashCode();
            return hashCode;
        }
    }
}
