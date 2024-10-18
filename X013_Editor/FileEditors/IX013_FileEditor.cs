using SF3.FileEditors;
using SF3.X013_Editor.Models.CritMod;
using SF3.X013_Editor.Models.Critrate;
using SF3.X013_Editor.Models.ExpLimit;
using SF3.X013_Editor.Models.HealExp;
using SF3.X013_Editor.Models.MagicBonus;
using SF3.X013_Editor.Models.Presets;
using SF3.X013_Editor.Models.Soulfail;
using SF3.X013_Editor.Models.Soulmate;
using SF3.X013_Editor.Models.SpecialChance;
using SF3.X013_Editor.Models.Specials;
using SF3.X013_Editor.Models.StatusEffects;
using SF3.X013_Editor.Models.SupportStats;
using SF3.X013_Editor.Models.SupportTypes;
using SF3.X013_Editor.Models.WeaponSpellRank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X013_Editor.FileEditors
{
    public interface IX013_FileEditor : ISF3FileEditor
    {
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
