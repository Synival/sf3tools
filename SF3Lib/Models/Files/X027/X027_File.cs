using System;
using System.Collections.Generic;
using CommonLib.Discovery;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Types;

namespace SF3.Models.Files.X027 {
    public class X027_File : ScenarioTableFile, IX027_File {
        public override int RamAddress => 0x06078000;
        public override int RamAddressLimit => 0x06080000;

        protected X027_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
            Discoveries = new DiscoveryContext(Data.GetDataCopy(), (uint) RamAddress);
            Discoveries.DiscoverUnknownPointersToValueRange((uint) RamAddress, (uint) RamAddressLimit - 1);
        }

        public static X027_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X027_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        private int[] GetBlacksmithTableAddrs() {
            switch (Scenario) {
                case ScenarioType.Scenario3: {
                    return new int[] {
                        Data.GetDouble(0x34f0) - RamAddress,
                        Data.GetDouble(0x3e58) - RamAddress
                    };
                }

                case ScenarioType.PremiumDisk: {
                    return new int[] {
                        Data.GetDouble(0x34f8) - RamAddress,
                        Data.GetDouble(0x3e60) - RamAddress
                    };
                }

                default: return new int[] {};
            }
        }

        public override IEnumerable<ITable> MakeTables() {            
            var tables = new List<ITable>();

            var blacksmithAddrs = GetBlacksmithTableAddrs();
            var blacksmithTables = new List<BlacksmithTable>();
            foreach (var addr in blacksmithAddrs)
                blacksmithTables.Add(BlacksmithTable.Create(Data, $"{nameof(BlacksmithTable)}{blacksmithTables.Count + 1:D2} (@0x{addr + RamAddress:X4})", addr));
            BlacksmithTables = blacksmithTables.ToArray();

            tables.AddRange(blacksmithTables);
            return tables;
        }

        public BlacksmithTable[] BlacksmithTables { get; private set; }
    }
}
