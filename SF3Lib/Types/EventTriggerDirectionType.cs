using CommonLib.Attributes;

namespace SF3.Types {
    public enum EventTriggerDirectionType {
        [EnumDisplayName("South")]           South           = 0x0,
        [EnumDisplayName("South-Southwest")] South_Southwest = 0x1,
        [EnumDisplayName("Southwest")]       Southwest       = 0x2,
        [EnumDisplayName("West-Southwest")]  West_Southwest  = 0x3,
        [EnumDisplayName("West")]            West            = 0x4,
        [EnumDisplayName("West-Northwest")]  West_Northwest  = 0x5,
        [EnumDisplayName("Northwest")]       Northwest       = 0x6,
        [EnumDisplayName("North-Northwest")] North_Northwest = 0x7,
        [EnumDisplayName("North")]           North           = 0x8,
        [EnumDisplayName("North-Northeast")] North_Northeast = 0x9,
        [EnumDisplayName("Northeast")]       Northeast       = 0xA,
        [EnumDisplayName("East-Northeast")]  East_Northeast  = 0xB,
        [EnumDisplayName("East")]            East            = 0xC,
        [EnumDisplayName("East-Southeast")]  East_Southeast  = 0xD,
        [EnumDisplayName("Southeast")]       Southeast       = 0xE,
        [EnumDisplayName("South-Southeast")] South_Southeast = 0xF,
    }
}
