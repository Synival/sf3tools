using SF3.Tables.X013;

namespace SF3.FileModels.X013 {
    public interface IX013_Editor : IScenarioTableEditor {
        SpecialTable SpecialsTable { get; }
        SpecialEffectTable SpecialEffectTable { get; }
        SupportTypeTable SupportTypeTable { get; }
        FriendshipExpTable FriendshipExpTable { get; }
        SupportStatsTable SupportStatsTable { get; }
        SoulmateTable SoulmateTable { get; }
        SoulfailTable SoulfailTable { get; }
        MagicBonusTable MagicBonusTable { get; }
        CritModTable CritModTable { get; }
        CritrateTable CritrateTable { get; }
        SpecialChanceTable SpecialChanceTable { get; }
        ExpLimitTable ExpLimitTable { get; }
        HealExpTable HealExpTable { get; }
        WeaponSpellRankTable WeaponSpellRankTable { get; }
        StatusEffectTable StatusEffectTable { get; }
    }
}
