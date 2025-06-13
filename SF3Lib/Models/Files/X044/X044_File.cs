using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X044 {
    public class X044_File : ScenarioTableFile, IX044_File {
        public override int RamAddress => 0x06078000;

        protected X044_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X044_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X044_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        private int GetMonsterTableAddress() {
            switch (Scenario) {
                case ScenarioType.PremiumDisk: return 0x7e40;
                default:                       return 0;
            }
        }

        public override IEnumerable<ITable> MakeTables() {
            int monsterTableAddress = GetMonsterTableAddress();
            var tables = new List<ITable>();

            if (monsterTableAddress > 0)
                tables.Add(MonsterTable = MonsterTable.Create(Data, "Monsters", ResourceFileForScenario(Scenario, "Monsters.xml"), monsterTableAddress));

            return tables;
        }

        [BulkCopyRecurse]
        public MonsterTable MonsterTable { get; private set; }
    }
}
