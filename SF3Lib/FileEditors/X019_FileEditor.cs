using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Types;

namespace SF3.FileEditors {
    public class X019_FileEditor : SF3FileEditor, IX019_FileEditor {
        public X019_FileEditor(ScenarioType scenario) : base(scenario, new NameGetterContext(scenario)) {
        }

        public override IEnumerable<ITable> MakeTables() {
            int monsterTableAddress;
            bool isPDX044 = GetDouble(0x08) == 0x060780A4;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    monsterTableAddress = 0x000C;
                    break;
                case ScenarioType.Scenario2:
                    monsterTableAddress = 0x000C;
                    break;
                case ScenarioType.Scenario3:
                    monsterTableAddress = 0x0eb0;
                    break;
                case ScenarioType.PremiumDisk:
                    monsterTableAddress = isPDX044 ? 0x7e40 : 0x0eb0;
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

        [BulkCopyRecurse]
        public MonsterTable MonsterTable { get; private set; }
    }
}
