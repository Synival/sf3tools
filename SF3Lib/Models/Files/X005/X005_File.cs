using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.X005;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.X005 {
    public class X005_File : ScenarioTableFile, IX005_File {
        public override int RamAddress { get; }

        public static int GetRamOffset(ScenarioType scenario) {
            switch (scenario) {
                case ScenarioType.Scenario1:   return 0x0603DC00;
                case ScenarioType.Scenario2:   return 0x0603C100;
                case ScenarioType.Scenario3:   return 0x0603C900;
                case ScenarioType.PremiumDisk: return 0x0603C900;

                default:
                    throw new ArgumentException("Unhandled '" + nameof(scenario) + "': " + scenario.ToString());
            }
        }

        protected X005_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
            RamAddress = GetRamOffset(scenario);
        }

        public static X005_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X005_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
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
