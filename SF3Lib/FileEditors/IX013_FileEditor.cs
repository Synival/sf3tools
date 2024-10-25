using SF3.Models.X013.CritMod;
using SF3.Models.X013.Critrate;
using SF3.Models.X013.ExpLimit;
using SF3.Models.X013.HealExp;
using SF3.Models.X013.MagicBonus;
using SF3.Models.X013.Presets;
using SF3.Models.X013.Soulfail;
using SF3.Models.X013.Soulmate;
using SF3.Models.X013.SpecialChance;
using SF3.Models.X013.Specials;
using SF3.Models.X013.StatusEffects;
using SF3.Models.X013.SupportStats;
using SF3.Models.X013.SupportTypes;
using SF3.Models.X013.WeaponSpellRank;

namespace SF3.FileEditors {
    public interface IX013_FileEditor : ISF3FileEditor {
        SpecialList SpecialsList { get; }
        SupportTypeList SupportTypeList { get; }
        FriendshipExpList FriendshipExpList { get; }
        SupportStatsList SupportStatsList { get; }
        SoulmateList SoulmateList { get; }
        SoulfailList SoulfailList { get; }
        MagicBonusList MagicBonusList { get; }
        CritModList CritModList { get; }
        CritrateList CritrateList { get; }
        SpecialChanceList SpecialChanceList { get; }
        ExpLimitList ExpLimitList { get; }
        HealExpList HealExpList { get; }
        WeaponSpellRankList WeaponSpellRankList { get; }
        StatusEffectList StatusEffectList { get; }
    }
}
