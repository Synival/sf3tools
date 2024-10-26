using System.Collections.Generic;
using SF3.Tables;
using SF3.Tables.X019.Monster;
using SF3.Types;

namespace SF3.FileEditors {
    public class X019_FileEditor : SF3FileEditor, IX019_FileEditor {
        public X019_FileEditor(ScenarioType scenario, bool isX044) : base(scenario) {
            IsX044 = isX044;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (MonsterList = new MonsterTable(this, IsX044))
            };
        }

        public override void DestroyTables() {
            MonsterList = null;
        }

        public MonsterTable MonsterList { get; private set; }

        public bool IsX044 { get; }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + ((Scenario == ScenarioType.PremiumDisk && IsX044) ? " (PD X044)" : "")
            : base.BaseTitle;
    }
}
