using System.Collections.Generic;
using SF3.Tables;
using SF3.Tables.X013;
using SF3.Types;

namespace SF3.FileEditors {
    public class X013_FileEditor : SF3FileEditor, IX013_FileEditor {
        public X013_FileEditor(ScenarioType scenario) : base(scenario) {
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (SpecialsList = new SpecialTable(this)),
                (SupportTypeList = new SupportTypeTable(this)),
                (FriendshipExpList = new FriendshipExpTable(this)),
                (SupportStatsList = new SupportStatsTable(this)),
                (SoulmateList = new SoulmateTable(this)),
                (SoulfailList = new SoulfailTable(this)),
                (MagicBonusList = new MagicBonusTable(this)),
                (CritModList = new CritModTable(this)),
                (CritrateList = new CritrateTable(this)),
                (SpecialChanceList = new SpecialChanceTable(this)),
                (ExpLimitList = new ExpLimitTable(this)),
                (HealExpList = new HealExpTable(this)),
                (WeaponSpellRankList = new WeaponSpellRankTable(this)),
                (StatusEffectList = new StatusEffectTable(this)),
            };
        }

        public override void DestroyTables() {
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

        public SpecialTable SpecialsList { get; private set; }
        public SupportTypeTable SupportTypeList { get; private set; }
        public FriendshipExpTable FriendshipExpList { get; private set; }
        public SupportStatsTable SupportStatsList { get; private set; }
        public SoulmateTable SoulmateList { get; private set; }
        public SoulfailTable SoulfailList { get; private set; }
        public MagicBonusTable MagicBonusList { get; private set; }
        public CritModTable CritModList { get; private set; }
        public CritrateTable CritrateList { get; private set; }
        public SpecialChanceTable SpecialChanceList { get; private set; }
        public ExpLimitTable ExpLimitList { get; private set; }
        public HealExpTable HealExpList { get; private set; }
        public WeaponSpellRankTable WeaponSpellRankList { get; private set; }
        public StatusEffectTable StatusEffectList { get; private set; }
    }
}
