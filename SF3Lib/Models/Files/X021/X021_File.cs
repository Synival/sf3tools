using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X021 {
    public class X021_File : ScenarioTableFile, IX021_File {
        public override int RamAddress => 0x06068000;
        public override int RamAddressLimit => 0x06070000;

        protected X021_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X021_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X021_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        private int GetIconRealOffset() {
            switch (Scenario) {
                case ScenarioType.Scenario1:   return 0xFF8E;
                case ScenarioType.Scenario2:   return 0xFC86;
                case ScenarioType.Scenario3:   return 0x12A48;
                case ScenarioType.PremiumDisk: return 0x12A32;
                default:
                    throw new ArgumentException(nameof(Scenario));
            }
        }

        public override IEnumerable<ITable> MakeTables() {
            var spellIconAddress = Data.GetDouble(0x0030) - RamAddress;
            var itemIconAddress  = Data.GetDouble(0x003C) - RamAddress;
            int spellIconRealOffsetStart = GetIconRealOffset();

            return new List<ITable>() {
                (SpellIconTable = SpellIconTable.Create(Data, "SpellIcons", ResourceFileForScenario(Scenario, "SpellIcons.xml"), spellIconAddress, false, spellIconRealOffsetStart)),
                (ItemIconTable  = ItemIconTable.Create (Data, "ItemIcons",  ResourceFileForScenario(Scenario, "Items.xml"), itemIconAddress, false))
            };
        }

        [BulkCopyRecurse]
        public SpellIconTable SpellIconTable { get; private set; }

        [BulkCopyRecurse]
        public ItemIconTable ItemIconTable { get; private set; }
    }
}
