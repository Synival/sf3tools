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
                (SpecialsTable = new SpecialTable(this)),
                (SupportTypeTable = new SupportTypeTable(this)),
                (FriendshipExpTable = new FriendshipExpTable(this)),
                (SupportStatsTable = new SupportStatsTable(this)),
                (SoulmateTable = new SoulmateTable(this)),
                (SoulfailTable = new SoulfailTable(this)),
                (MagicBonusTable = new MagicBonusTable(this)),
                (CritModTable = new CritModTable(this)),
                (CritrateTable = new CritrateTable(this)),
                (SpecialChanceTable = new SpecialChanceTable(this)),
                (ExpLimitTable = new ExpLimitTable(this)),
                (HealExpTable = new HealExpTable(this)),
                (WeaponSpellRankTable = new WeaponSpellRankTable(this)),
                (StatusEffectTable = new StatusEffectTable(this)),
            };
        }

        public override void DestroyTables() {
            SpecialsTable = null;
            SupportTypeTable = null;
            FriendshipExpTable = null;
            SupportStatsTable = null;
            SoulmateTable = null;
            SoulfailTable = null;
            MagicBonusTable = null;
            CritModTable = null;
            CritrateTable = null;
            SpecialChanceTable = null;
            ExpLimitTable = null;
            HealExpTable = null;
            WeaponSpellRankTable = null;
            StatusEffectTable = null;
        }

        public SpecialTable SpecialsTable { get; private set; }
        public SupportTypeTable SupportTypeTable { get; private set; }
        public FriendshipExpTable FriendshipExpTable { get; private set; }
        public SupportStatsTable SupportStatsTable { get; private set; }
        public SoulmateTable SoulmateTable { get; private set; }
        public SoulfailTable SoulfailTable { get; private set; }
        public MagicBonusTable MagicBonusTable { get; private set; }
        public CritModTable CritModTable { get; private set; }
        public CritrateTable CritrateTable { get; private set; }
        public SpecialChanceTable SpecialChanceTable { get; private set; }
        public ExpLimitTable ExpLimitTable { get; private set; }
        public HealExpTable HealExpTable { get; private set; }
        public WeaponSpellRankTable WeaponSpellRankTable { get; private set; }
        public StatusEffectTable StatusEffectTable { get; private set; }
    }
}
