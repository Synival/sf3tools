using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Discovery;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X007;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X007 {
    public class X007_File : ScenarioTableFile, IX007_File {
        public override int RamAddress { get; }
        public override int RamAddressLimit { get; }

        protected X007_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
            switch (scenario) {
                case ScenarioType.Scenario1:
                    RamAddress = 0x06056000;
                    RamAddressLimit = 0x0605F000;
                    break;
                case ScenarioType.Scenario2:
                    RamAddress = 0x06054800;
                    RamAddressLimit = 0x0605E000;
                    break;
                case ScenarioType.Scenario3:
                    RamAddress = 0x06054500;
                    RamAddressLimit = 0x0605E000;
                    break;
                case ScenarioType.PremiumDisk:
                    RamAddress = 0x06056000;
                    RamAddressLimit = 0x0605E000;
                    break;
                default:
                    throw new AggregateException($"Unhandled scenario '{scenario}'");
            }

            Discoveries = new DiscoveryContext(Data.GetDataCopy(), (uint) RamAddress);
            Discoveries.DiscoverUnknownPointersToValueRange((uint) RamAddress, (uint) RamAddressLimit - 1);
        }

        public static X007_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X007_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize X007_File");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var chpSectorSizeAddresses = new int[0];

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    // TODO: chpSectorAddresses?
                    break;

                case ScenarioType.Scenario2:
                    // TODO: chpSectorAddresses?
                    break;

                case ScenarioType.Scenario3:
                    chpSectorSizeAddresses = new int[] { 0x8150 };
                    break;

                case ScenarioType.PremiumDisk:
                    chpSectorSizeAddresses = new int[] { 0x8110 };
                    break;
            }

            CHPSectorSizesTables = chpSectorSizeAddresses
                .Select(x => CHPSectorSizesTable.Create(Data, $"CHP Sector + Sizes @0x{(x + RamAddress):X8}", ResourceFileForScenario(Scenario, "Characters.xml"), x, 60)).ToArray();

            return new List<ITable>() {
                // TODO: sizes?
            };
        }

        public CHPSectorSizesTable[] CHPSectorSizesTables { get; private set; }
    }
}
