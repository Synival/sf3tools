using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X019 {
    public class X019_File : ScenarioTableFile, IX019_File {
        public override int RamAddress => 0x060A0000;

        protected X019_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X019_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X019_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        private int GetMonsterTableAddress() {
            switch (Scenario) {
                case ScenarioType.Scenario1:   return 0x000C;
                case ScenarioType.Scenario2:   return 0x000C;
                case ScenarioType.Scenario3:   return 0x0eb0;
                case ScenarioType.PremiumDisk: return 0x0eb0;
                default:
                    throw new ArgumentException(nameof(Scenario));
            }

        }

        public override IEnumerable<ITable> MakeTables() {
            int monsterTableAddress = GetMonsterTableAddress();
            return new List<ITable>() {
                (MonsterTable = MonsterTable.Create(Data, "Monsters", ResourceFileForScenario(Scenario, "Monsters.xml"), monsterTableAddress))
            };
        }

        [BulkCopyRecurse]
        public MonsterTable MonsterTable { get; private set; }
    }
}
