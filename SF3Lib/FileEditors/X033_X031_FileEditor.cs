using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.Tables;
using SF3.Tables.X033_X031;
using SF3.Types;

namespace SF3.FileEditors {
    public class X033_X031_FileEditor : SF3FileEditor, IX033_X031_FileEditor {
        public X033_X031_FileEditor(ScenarioType scenario) : base(scenario) {
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (StatsTable = new StatsTable(this)),
                (InitialInfoTable = new InitialInfoTable(this)),
                (WeaponLevelTable = new WeaponLevelTable(this)),
            };
        }

        public override void DestroyTables() {
            StatsTable = null;
            InitialInfoTable = null;
            WeaponLevelTable = null;
        }

        [BulkCopyRecurse]
        public StatsTable StatsTable { get; private set; }

        [BulkCopyRecurse]
        public InitialInfoTable InitialInfoTable { get; private set; }

        [BulkCopyRecurse]
        public WeaponLevelTable WeaponLevelTable { get; private set; }
    }
}
