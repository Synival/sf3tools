using CommonLib.Attributes;

namespace SF3.Types {
    public enum AttackChoice {
        [EnumDisplayName("Attack")]
        Attack        = 0,

        [EnumDisplayName("Spell")]
        Spell         = 1,

        [EnumDisplayName("Monster Attack")]
        MonsterAttack = 2,

        [EnumDisplayName("X1 Attack 1 (Special)")]
        X1Attack1     = 3,

        [EnumDisplayName("Use Eq/Item")]
        UseEqOrItem   = 4,

        [EnumDisplayName("X1 Attack 2 (Usually Effect Spell)")]
        X1Attack2_UsuallyEffecttSpell = 5
    }
}
