using System.Collections.Generic;
using SF3.Models;
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
using SF3.Types;

namespace SF3.FileEditors {
    public class X013_FileEditor : SF3FileEditor, IX013_FileEditor {
        public X013_FileEditor(ScenarioType scenario) : base(scenario) {
        }

        public override IEnumerable<IModelArray> MakeModelArrays() {
            return new List<IModelArray>() {
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

        public override void DestroyModelArrays() {
            SpecialsList = null;
            SupportTypeList = null;
            FriendshipExpList = null;
            SupportStatsList = null;
            SoulmateList = null;
            SoulfailList = null;
            MagicBonusList = null;
            CritModList = null;
            CritrateList = null;
            SpecialChanceList = null;
            ExpLimitList = null;
            HealExpList = null;
            WeaponSpellRankList = null;
            StatusEffectList = null;
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
