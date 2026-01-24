using System.Linq;
using CommonLib.Arrays;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class BlacksmithTable : TerminatedTable<Blacksmith> {
        protected BlacksmithTable(IByteData data, string name, int address)
        : base(data, name, address, terminatedBytes: 4, maxSize: 100) {
        }

        public static BlacksmithTable Create(IByteData data, string name, int address)
            => Create(() => new BlacksmithTable(data, name, address));

        public override bool Load() {
            return Load(
                (id, addr) => new Blacksmith(Data, id, "BlacksmithIndex" + id.ToString("D2"), addr),
                (rows, prevRow) => prevRow.MaterialItem != 0xFFFF,
                false
            );
        }

        public void SortByMaterialAndItemType() {
            var sortedRows = Rows.OrderBy(x => x.MaterialItem).ThenBy(x => x.RequestItemType).ToArray();

            var craftData = new ByteArray(0x28 * sortedRows.Length);
            var pos = 0;
            foreach (var row in sortedRows) {
                var rowData = row.Data.GetDataCopyAt(row.Address, row.Size);
                craftData.SetDataAtTo(pos, rowData.Length, rowData);
                pos += row.Size;
            }

            Data.Data.SetDataAtTo(Address, craftData.Length, craftData.GetDataCopyOrReference());
        }
    }
}
