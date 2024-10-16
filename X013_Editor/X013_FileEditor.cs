using SF3.Models;
using SF3.Types;
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

namespace SF3.X013_Editor
{
    public class X013_FileEditor : SF3FileEditor, IX013_FileEditor
    {
        public X013_FileEditor(ScenarioType scenario) : base(scenario)
        {
        }

        public override IEnumerable<IModelArray> MakeModelArrays()
        {
            return new List<IModelArray>()
            {
                (SpecialsList = new SpecialList(this)),
                (SupportTypeList = new SupportTypeList(this)),
                (FriendshipExpList = new FriendshipExpList(this)),
                (SupportStatsList = new SupportStatsList(this)),
                (SoulmateList = new SoulmateList(this)),
                (SoulfailList = new SoulfailList(this)),
                (MagicBonusList = new MagicBonusList(this)),
                (CritModList = new CritModList(this)),
                (CritrateList = new CritrateList(this)),
                (SpecialChanceList = new SpecialChanceList(this)),
                (ExpLimitList = new ExpLimitList(this)),
                (HealExpList = new HealExpList(this)),
                (WeaponSpellRankList = new WeaponSpellRankList(this)),
                (StatusEffectList = new StatusEffectList(this)),
            };
        }

        public SpecialList SpecialsList { get; private set; }
        public SupportTypeList SupportTypeList { get; private set; }
        public FriendshipExpList FriendshipExpList { get; private set; }
        public SupportStatsList SupportStatsList { get; private set; }
        public SoulmateList SoulmateList { get; private set; }
        public SoulfailList SoulfailList { get; private set; }
        public MagicBonusList MagicBonusList { get; private set; }
        public CritModList CritModList { get; private set; }
        public CritrateList CritrateList { get; private set; }
        public SpecialChanceList SpecialChanceList { get; private set; }
        public ExpLimitList ExpLimitList { get; private set; }
        public HealExpList HealExpList { get; private set; }
        public WeaponSpellRankList WeaponSpellRankList { get; private set; }
        public StatusEffectList StatusEffectList { get; private set; }
    }
}
