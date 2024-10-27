using System.Collections.Generic;
using SF3.Tables;
using SF3.Tables.X019;
using SF3.Types;

namespace SF3.FileEditors {
    public class X019_FileEditor : SF3FileEditor, IX019_FileEditor {
        public X019_FileEditor(ScenarioType scenario, bool isX044) : base(scenario) {
            IsX044 = isX044;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (MonsterTable = new MonsterTable(this, IsX044))
            };
        }

        public override void DestroyTables() {
            MonsterTable = null;
        }

        public MonsterTable MonsterTable { get; private set; }

        public bool IsX044 { get; }
        public bool IsPDX044 => Scenario == ScenarioType.PremiumDisk && IsX044;

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + (IsPDX044 ? " (PD X044)" : "")
            : base.BaseTitle;
    }
}
