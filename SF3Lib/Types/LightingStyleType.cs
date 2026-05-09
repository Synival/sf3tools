using CommonLib.Attributes;

namespace SF3.Types {
    public enum LightingStyleType {
        [EnumDisplayName("Light, Indoors")]
        LightIndoors  = 0,

        [EnumDisplayName("Dark, Indoors")]
        DarkIndoors   = 1,

        [EnumDisplayName("Light, Outdoors")]
        LightOutdoors = 2,

        [EnumDisplayName("Dark, Outdoors (Scenario 3+)")]
        DarkOutdoors  = 3
    }
}
