using SF3.Tables.X013;

namespace SF3.FileEditors {
    public interface IX013_FileEditor : ISF3FileEditor {
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
