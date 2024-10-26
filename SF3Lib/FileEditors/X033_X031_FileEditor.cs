using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.Tables;
using SF3.Tables.X033_X031.InitialInfo;
using SF3.Tables.X033_X031.Stats;
using SF3.Tables.X033_X031.WeaponLevel;
using SF3.Types;

namespace SF3.FileEditors {
    public class X033_X031_FileEditor : SF3FileEditor, IX033_X031_FileEditor {
        public X033_X031_FileEditor(ScenarioType scenario) : base(scenario) {
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (StatsList = new StatsTables(this)),
                (InitialInfoList = new InitialInfoTables(this)),
                (WeaponLevelList = new WeaponLevelTables(this)),
            };
        }

        public override void DestroyTables() {
            StatsList = null;
            InitialInfoList = null;
            WeaponLevelList = null;
        }

        [BulkCopyRecurse]
        public StatsTables StatsList { get; private set; }

        [BulkCopyRecurse]
        public InitialInfoTables InitialInfoList { get; private set; }

        [BulkCopyRecurse]
        public WeaponLevelTables WeaponLevelList { get; private set; }
    }
}
