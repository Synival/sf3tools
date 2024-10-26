using SF3.Tables.X013.CritMod;
using SF3.Tables.X013.Critrate;
using SF3.Tables.X013.ExpLimit;
using SF3.Tables.X013.FriendshipExp;
using SF3.Tables.X013.HealExp;
using SF3.Tables.X013.MagicBonus;
using SF3.Tables.X013.Soulfail;
using SF3.Tables.X013.Soulmate;
using SF3.Tables.X013.SpecialChance;
using SF3.Tables.X013.Specials;
using SF3.Tables.X013.StatusEffect;
using SF3.Tables.X013.SupportStats;
using SF3.Tables.X013.SupportType;
using SF3.Tables.X013.WeaponSpellRank;

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
