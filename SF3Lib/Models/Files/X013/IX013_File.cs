using SF3.Models.Tables.X013;

namespace SF3.Models.Files.X013 {
    public interface IX013_File : IScenarioTableFile {
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
