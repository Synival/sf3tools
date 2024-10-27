using System;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static SF3.Utils.Resources;

namespace SF3.Tables {
    public class MonsterTable : Table<Monster> {
        public MonsterTable(ISF3FileEditor fileEditor, bool isX044) : base(fileEditor) {
            IsX044       = isX044;
            ResourceFile = (Scenario == ScenarioType.PremiumDisk && isX044)
                ? "Resources/PD/Monsters_X044.xml"
                : ResourceFileForScenario(Scenario, "Monsters.xml");

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    Address = 0x0000000C;
                    break;
                case ScenarioType.Scenario2:
                    Address = 0x0000000C;
                    break;
                case ScenarioType.Scenario3:
                    Address = 0x00000eb0;
                    break;
                case ScenarioType.PremiumDisk:
                    Address = isX044 ? 0x00007e40 : 0x00000eb0;
                    break;
                default:
                    throw new ArgumentException(nameof(Scenario));
            }
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Monster(FileEditor, id, name, address, Scenario, IsX044));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 256;

        public bool IsX044 { get; }
    }
}
