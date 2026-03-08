using System;
using System.Collections.Generic;
using System.Reflection;
using CommonLib.Attributes;
using CommonLib.Discovery;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X032 {
    public class X032_File : ScenarioTableFile, IX032_File {
        public override int RamAddress => 0x06088000;
        public override int RamAddressLimit => 0x0609D800;

        protected X032_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
            Discoveries = new DiscoveryContext(Data.GetDataCopy(), (uint) RamAddress);
            Discoveries.DiscoverUnknownPointersToValueRange((uint) RamAddress, (uint) RamAddressLimit - 1);
        }

        public static X032_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X032_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException($"Couldn't initialize {MethodBase.GetCurrentMethod().DeclaringType.Name}");
            return newFile;
        }

        public static int GetSpellIconTableOffset(ScenarioType scenario) {
            switch (scenario) {
                case ScenarioType.Scenario1:   return 0x7D00;
                case ScenarioType.Scenario2:   return 0x82DC;
                case ScenarioType.Scenario3:   return 0x8488;
                case ScenarioType.PremiumDisk: return 0x8538;
                default: throw new ArgumentException(nameof(Scenario));
            }
        }

        public override IEnumerable<ITable> MakeTables() {
            int spellIconAddress = GetSpellIconTableOffset(Scenario);

            return new List<ITable>() {
                (SpellIconTable = SpellIconTable.Create(Data, "SpellIcons", ResourceFileForScenario(Scenario, "SpellIcons.xml"), spellIconAddress, false, Scenario)),
            };
        }

        [BulkCopyRecurse]
        public SpellIconTable SpellIconTable { get; private set; }
    }
}
