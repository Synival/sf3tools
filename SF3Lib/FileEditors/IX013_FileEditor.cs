using SF3.Tables.X013;

namespace SF3.FileEditors {
    public interface IX013_FileEditor : ISF3FileEditor {
        SpecialTable SpecialsList { get; }
        SupportTypeTable SupportTypeList { get; }
        FriendshipExpTable FriendshipExpList { get; }
        SupportStatsTable SupportStatsList { get; }
        SoulmateTable SoulmateList { get; }
        SoulfailTable SoulfailList { get; }
        MagicBonusTable MagicBonusList { get; }
        CritModTable CritModList { get; }
        CritrateTable CritrateList { get; }
        SpecialChanceTable SpecialChanceList { get; }
        ExpLimitTable ExpLimitList { get; }
        HealExpTable HealExpList { get; }
        WeaponSpellRankTable WeaponSpellRankList { get; }
        StatusEffectTable StatusEffectList { get; }
    }
}
