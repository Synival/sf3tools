using CommonLib.Attributes;

namespace SF3.Types {
    public enum OrderChoiceOrder {
        [EnumDisplayName("Move, Attack, Heal, X1 Func")]
        MoveAttackHealX1 = 0,

        [EnumDisplayName("Move, Heal, Attack, X1 Func")]
        MoveHealAttackX1 = 1,

        [EnumDisplayName("Move, X1 Func, Attack, Heal")]
        MoveX1HealAttack = 2,

        [EnumDisplayName("Attack, Heal, X1 Func, Move")]
        AttackHealX1Move = 3,
    }
}
