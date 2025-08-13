using System;
using System.Collections.Generic;
using CommonLib.Discovery;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.X005;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.X005 {
    public class X005_File : ScenarioTableFile, IX005_File {
        public override int RamAddress { get; }
        public override int RamAddressLimit { get; }

        protected X005_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
            RamAddress = GetRamAddress();
            RamAddressLimit = GetRamAddressLimit();

            Discoveries = new DiscoveryContext(Data.GetDataCopy(), (uint) RamAddress);
            Discoveries.DiscoverUnknownPointersToValueRange((uint) RamAddress, (uint) RamAddressLimit - 1);
        }

        public int GetRamAddress() {
            switch (Scenario) {
                case ScenarioType.Scenario1:   return 0x0603DC00;
                case ScenarioType.Scenario2:   return 0x0603C100;
                case ScenarioType.Scenario3:   return 0x0603C900;
                case ScenarioType.PremiumDisk: return 0x0603C900;
                default:
                    throw new ArgumentException("Unhandled '" + nameof(Scenario) + "': " + Scenario.ToString());
            }
        }

        public int GetRamAddressLimit() {
            switch (Scenario) {
                case ScenarioType.Scenario1:   return 0x06046000;
                case ScenarioType.Scenario2:   return 0x06044800;
                case ScenarioType.Scenario3:   return 0x06043D00;
                case ScenarioType.PremiumDisk: return 0x06043D00;
                default:
                    throw new ArgumentException("Unhandled '" + nameof(Scenario) + "': " + Scenario.ToString());
            }
        }

        public static X005_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X005_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize X005_File");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            CameraSettings = new CameraSettings(Data, 0, "CameraSettings", Scenario);

            var tables = new List<ITable>() {
            };

            // TODO: what tables?
            return tables;
        }

        public CameraSettings CameraSettings { get; private set; }
    }
}
