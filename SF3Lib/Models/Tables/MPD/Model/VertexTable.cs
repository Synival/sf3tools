using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class VertexTable : AddressedTable<VertexModel> {
        protected VertexTable(IByteData data, int[] addresses) : base(data, addresses) {
        }

        public static VertexTable Create(IByteData data, int[] addresses) {
            var newTable = new VertexTable(data, addresses);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new VertexModel(Data, id, "VERTEX" + id.ToString("D4"), address));
    }
}
