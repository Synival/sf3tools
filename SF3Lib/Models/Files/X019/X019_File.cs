using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X019;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X019 {
    public class X019_File : ScenarioTableFile, IX019_File {
        protected X019_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X019_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X019_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            int monsterTableAddress;
            var isPDX044 = Data.GetDouble(0x08) == 0x060780A4;

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
                (MonsterTable = MonsterTable.Create(Data, ResourceFileForScenario(Scenario, "Monsters.xml"), monsterTableAddress))
            };
        }

        [BulkCopyRecurse]
        public MonsterTable MonsterTable { get; private set; }
    }
}
