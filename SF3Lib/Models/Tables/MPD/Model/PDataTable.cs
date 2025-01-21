using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class PDataTable : AddressedTable<PDataModel> {
        protected PDataTable(IByteData data, int[] addresses, int[] refs) : base(data, addresses) {
            if (refs == null && refs.Length != addresses.Length)
                throw new ArgumentException(nameof(refs));
            Refs = refs;
        }

        public static PDataTable Create(IByteData data, int[] addresses, int[] refs) {
            var newTable = new PDataTable(data, addresses, refs);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new PDataModel(Data, id, "PDATA" + id.ToString("D4"), address, Refs[id]));

        private int[] Refs { get; }
    }
}
