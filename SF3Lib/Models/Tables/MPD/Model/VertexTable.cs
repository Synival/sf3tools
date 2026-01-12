using System.Collections.Generic;
using CommonLib;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class VertexTable : FixedSizeTable<VertexStruct>, IIndexedEnumerableWithLength<VECTOR> {
        protected VertexTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static VertexTable Create(IByteData data, string name, int address, int size)
            => Create(() => new VertexTable(data, name, address, size));

        public override bool Load()
            => Load((id, address) => new VertexStruct(Data, id, "VERTEX" + id.ToString("D4"), address));

        IEnumerator<VECTOR> IEnumerable<VECTOR>.GetEnumerator() {
            foreach (var row in Rows)
                yield return row.Vector;
        }

        VECTOR IIndexedEnumerableWithLength<VECTOR>.this[int index] => Rows[index].Vector;
    }
}
