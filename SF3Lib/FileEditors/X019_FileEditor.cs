using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.Tables;
using SF3.Types;

namespace SF3.FileEditors {
    public class X019_FileEditor : SF3FileEditor, IX019_FileEditor {
        public X019_FileEditor(ScenarioType scenario, bool isX044) : base(scenario) {
            IsX044 = isX044;
        }

        public override IEnumerable<ITable> MakeTables() {
            int monsterTableAddress;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    monsterTableAddress = 0x0000000C;
                    break;
                case ScenarioType.Scenario2:
                    monsterTableAddress = 0x0000000C;
                    break;
                case ScenarioType.Scenario3:
                    monsterTableAddress = 0x00000eb0;
                    break;
                case ScenarioType.PremiumDisk:
                    monsterTableAddress = IsX044 ? 0x00007e40 : 0x00000eb0;
                    break;
                default:
                    throw new ArgumentException(nameof(Scenario));
            }

            return new List<ITable>() {
                (MonsterTable = new MonsterTable(this, monsterTableAddress))
            };
        }

        public override void DestroyTables() {
            MonsterTable = null;
        }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + (IsPDX044 ? " (PD X044)" : "")
            : base.BaseTitle;

        public bool IsX044 { get; }
        public bool IsPDX044 => Scenario == ScenarioType.PremiumDisk && IsX044;

        [BulkCopyRecurse]
        public MonsterTable MonsterTable { get; private set; }
    }
}
