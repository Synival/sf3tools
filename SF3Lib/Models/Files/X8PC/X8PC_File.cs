using System;
using System.Collections.Generic;
using CommonLib.Discovery;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.X8PC {
    public class X8PC_File : ScenarioTableFile, IX8PC_File {
        public override int RamAddress      => 0x060A0000;
        public override int RamAddressLimit => 0x060A8000; // TODO: confirm this!

        protected X8PC_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario)
        : base(data, nameContext, scenario) {

            Discoveries = new DiscoveryContext(Data.GetDataCopy(), (uint) RamAddress);
            Discoveries.DiscoverUnknownPointersToValueRange((uint) RamAddress, (uint) RamAddressLimit - 1);
        }

        public static X8PC_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X8PC_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException($"Couldn't initialize {nameof(X8PC_File)}");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            Header = new BattleModelHeader(Data, 0, nameof(BattleModelHeader), 0x00);

            var tables = new List<ITable>();
            tables.AddRange(Header.Tables);

            return tables.ToArray();
        }

        public BattleModelHeader Header { get; private set; }
    }
}
