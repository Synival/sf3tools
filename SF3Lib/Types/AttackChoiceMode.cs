using CommonLib.Attributes;

namespace SF3.Types {
    public enum AttackChoiceMode {
        [EnumDisplayName("Random (Equal Chance)")]
        Random = 0,

        [EnumDisplayName("Random (Earlier More Likely)")]
        Random_EarlierMoreLikely = 1,

        [EnumDisplayName("Random (Earlier Much More Likely)")]
        Random_EarlierMuchMoreLikely = 2,

        [EnumDisplayName("Cycle (Start at Second)")]
        Cycle_StartAtSecond = 3,

        [EnumDisplayName("Cycle (Start at Second, Stop when Complete)")]
        Cycle_StartAtSecond_StopWhenComplete = 4,

        [EnumDisplayName("Cycle (Start at First)")]
        Cycle_StartAtFirst = 5,
    }
}
