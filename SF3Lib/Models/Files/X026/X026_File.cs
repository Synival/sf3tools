using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.Discovery;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X026 {
    public class X026_File : ScenarioTableFile, IX026_File {
        public override int RamAddress => 0x06078000;
        public override int RamAddressLimit => 0x06080000;

        protected X026_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
            Discoveries = new DiscoveryContext(Data.GetDataCopy(), (uint) RamAddress);
            Discoveries.DiscoverUnknownPointersToValueRange((uint) RamAddress, (uint) RamAddressLimit - 1);
        }

        public static X026_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X026_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize X026_File");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            int spellIconAddress;
            int itemIconAddress;
            int spellIconRealOffsetStart;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    spellIconAddress = Data.GetDouble(0x0a30) - RamAddress;
                    itemIconAddress  = Data.GetDouble(0x08f0) - RamAddress;
                    spellIconRealOffsetStart = 0xFF8E;
                    break;

                case ScenarioType.Scenario2:
                    spellIconAddress = Data.GetDouble(0x0a1c) - RamAddress;
                    itemIconAddress  = Data.GetDouble(0x0a08) - RamAddress;
                    spellIconRealOffsetStart = 0xFC86;
                    break;

                case ScenarioType.Scenario3:
                    spellIconAddress = Data.GetDouble(0x09cc) - RamAddress;
                    itemIconAddress  = Data.GetDouble(0x09b4) - RamAddress;
                    spellIconRealOffsetStart = 0x12A48;
                    break;

                case ScenarioType.PremiumDisk:
                    spellIconAddress = Data.GetDouble(0x07a0) - RamAddress;
                    itemIconAddress  = Data.GetDouble(0x072c) - RamAddress;
                    spellIconRealOffsetStart = 0x12A32;
                    break;

                default:
                    throw new ArgumentException(nameof(Scenario));
            }

            var has16BitIconAddr = Scenario == ScenarioType.Scenario1;
            return new List<ITable>() {
                (SpellIconTable = SpellIconTable.Create(Data, "SpellIcons", ResourceFileForScenario(Scenario, "SpellIcons.xml"), spellIconAddress, has16BitIconAddr, spellIconRealOffsetStart)),
                (ItemIconTable  = ItemIconTable.Create (Data, "ItemIcons",  ResourceFileForScenario(Scenario, "Items.xml"), itemIconAddress, has16BitIconAddr))
            };
        }

        [BulkCopyRecurse]
        public SpellIconTable SpellIconTable { get; private set; }

        [BulkCopyRecurse]
        public ItemIconTable ItemIconTable { get; private set; }
    }
}
