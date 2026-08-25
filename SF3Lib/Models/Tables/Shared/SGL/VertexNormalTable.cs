using System.Collections.Generic;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Structs.Shared.SGL;

namespace SF3.Models.Tables.Shared.SGL {
    public class VertexNormalTable : FixedSizeTable<VertexNormalStruct>, IReadOnlyList<VECTOR> {
        protected VertexNormalTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static VertexNormalTable Create(IByteData data, string name, int address, int size)
            => Create(() => new VertexNormalTable(data, name, address, size));

        public override bool Load()
            => Load((id, address) => new VertexNormalStruct(Data, id, "NORMAL_" + id.ToString("D4"), address));

        IEnumerator<VECTOR> IEnumerable<VECTOR>.GetEnumerator() {
            foreach (var row in Rows)
                yield return row.Vector;
        }

        VECTOR IReadOnlyList<VECTOR>.this[int index] => Rows[index].Vector;
    }
}
