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
                (StatsList = new StatsList(this)),
                (InitialInfoList = new InitialInfoList(this)),
                (WeaponLevelList = new WeaponLevelList(this)),
            };
        }

        public override void DestroyTables() {
            StatsList = null;
            InitialInfoList = null;
            WeaponLevelList = null;
        }

        [BulkCopyRecurse]
        public StatsList StatsList { get; private set; }

        [BulkCopyRecurse]
        public InitialInfoList InitialInfoList { get; private set; }

        [BulkCopyRecurse]
        public WeaponLevelList WeaponLevelList { get; private set; }
    }
}
