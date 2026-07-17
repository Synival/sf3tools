using CommonLib.Attributes;

namespace SF3.Types {
    public enum OrderChoiceMode {
        [EnumDisplayName("Move, Attack, Heal, Debuffs (X1)")]
        MoveAttackHealX1 = 0,

        [EnumDisplayName("Move, Heal, Attack, Debuffs (X1)")]
        MoveHealAttackX1 = 1,

        [EnumDisplayName("Move, Debuffs (X1), Attack, Heal")]
        MoveX1HealAttack = 2,

        [EnumDisplayName("Attack, Heal, Debuffs (X1), Move")]
        AttackHealX1Move = 3,
    }
}
