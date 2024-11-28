using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.Models.Files;
using SF3.Models.Tables;
using SF3.Models.Tables.X019;
using SF3.RawEditors;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X019 {
    public class X019_Editor : ScenarioTableEditor, IX019_Editor {
        protected X019_Editor(IRawEditor editor, INameGetterContext nameContext, ScenarioType scenario) : base(editor, nameContext, scenario) {
        }

        public static X019_Editor Create(IRawEditor editor, INameGetterContext nameContext, ScenarioType scenario) {
            var newEditor = new X019_Editor(editor, nameContext, scenario);
            if (!newEditor.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newEditor;
        }

        public override IEnumerable<ITable> MakeTables() {
            int monsterTableAddress;
            var isPDX044 = Editor.GetDouble(0x08) == 0x060780A4;

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
                (MonsterTable = new MonsterTable(Editor, ResourceFileForScenario(Scenario, "Monsters.xml"), monsterTableAddress))
            };
        }

        [BulkCopyRecurse]
        public MonsterTable MonsterTable { get; private set; }
    }
}
