using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class AttrTable : AddressedTable<AttrModel> {
        protected AttrTable(IByteData data, int[] addresses) : base(data, addresses) {
        }

        public static AttrTable Create(IByteData data, int[] addresses) {
            var newTable = new AttrTable(data, addresses);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new AttrModel(Data, id, "ATTR" + id.ToString("D4"), address));
    }
}
