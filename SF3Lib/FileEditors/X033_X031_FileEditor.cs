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
                (StatsList = new StatsTable(this)),
                (InitialInfoList = new InitialInfoTable(this)),
                (WeaponLevelList = new WeaponLevelTable(this)),
            };
        }

        public override void DestroyTables() {
            StatsList = null;
            InitialInfoList = null;
            WeaponLevelList = null;
        }

        [BulkCopyRecurse]
        public StatsTable StatsList { get; private set; }

        [BulkCopyRecurse]
        public InitialInfoTable InitialInfoList { get; private set; }

        [BulkCopyRecurse]
        public WeaponLevelTable WeaponLevelList { get; private set; }
    }
}
