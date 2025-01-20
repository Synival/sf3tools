using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class PDataTable : FixedSizeTable<PDataModel> {
        protected PDataTable(IByteData data, int[] addresses) : base(data, 0, addresses.Length) {
            Addresses = addresses;
        }

        public static PDataTable Create(IByteData data, int[] addresses) {
            var newTable = new PDataTable(data, addresses);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new PDataModel(Data, id, "PDATA" + id.ToString("D4"), Addresses[id]));

        public int[] Addresses { get; }
    }
}
