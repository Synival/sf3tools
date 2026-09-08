using System;
using System.Collections.Generic;
using CommonLib.Discovery;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables.X8PC;
using SF3.Types;
using SF3.Models.Tables;
using System.Linq;

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

        private static readonly byte[] g_headerSequence = new byte[] { 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>();

            // Discover where PolyChar headers could be. They all starts with 0x00000800 (offset of first chunk in
            // bytes) then 0x0000 (higher bytes of an int with the size of the first chunk, which should definitely
            // never be high enough to need those bytes!)
            var addresses = new List<int>();

            var max = Data.Length - 0x40;
            for (int offset = 0; offset < max; offset += 0x800) {
                var data = Data.Data.GetDataCopyAt(offset, 0x06);
                if (!data.AsSpan().SequenceEqual(g_headerSequence))
                    continue;
                addresses.Add(offset);
            }

            // We have the addresses, now let's make the PolyChars.
            PolyCharTable = PolyCharTable.Create(Data, nameof(PolyCharTable), addresses.ToArray(), Scenario);
            foreach (var pc in PolyCharTable)
                tables.AddRange(pc.Tables);

            return tables.ToArray();
        }

        public override bool OnFinish() {
            base.OnFinish();
            foreach (var pc in PolyCharTable)
                if (!pc.UpdateAndCommitChunks())
                    return false;

            return true;
        }

        public PolyCharTable PolyCharTable { get; private set; }
    }
}
