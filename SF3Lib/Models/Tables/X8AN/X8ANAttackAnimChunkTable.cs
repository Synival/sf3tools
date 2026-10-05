using System.Collections.Generic;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs.X8AN;
using SF3.Models.Tables.X8PC;

namespace SF3.Models.Tables.X8AN {
    public class X8ANAttackAnimChunkTable : Table<X8ANAttackAnimChunkStruct>, IReadOnlyList<PCBoneKeyframesTable> {
        protected X8ANAttackAnimChunkTable(IByteData data, string name, int address, bool hasFixedRotations)
        : base(data, name, address) {
            HasFixedRotations = hasFixedRotations;
        }

        public static X8ANAttackAnimChunkTable Create(IByteData data, string name, int address, bool hasFixedRotations)
            => Create(() => new X8ANAttackAnimChunkTable(data, name, address, hasFixedRotations));

        public override bool Load() {
            var rows = new List<X8ANAttackAnimChunkStruct>();
            int pos = Address;

            for (int id = 0; pos < Data.Length; id++) {
                var newChunk = new X8ANAttackAnimChunkStruct(Data, id, $"AttackAnimChunk_{id:D02}", pos, HasFixedRotations);
                rows.Add(newChunk);
                var chunkSize = ((newChunk.Size + 0x7FF) / 0x800) * 0x800;
                pos += chunkSize;
            }
            _rows = rows.ToArray();
            return true;
        }

        IEnumerator<PCBoneKeyframesTable> IEnumerable<PCBoneKeyframesTable>.GetEnumerator() => _rows.Select(x => x.BoneKeyframesTable).AsEnumerable().GetEnumerator();
        PCBoneKeyframesTable IReadOnlyList<PCBoneKeyframesTable>.this[int index] => _rows[index].BoneKeyframesTable;

        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;

        public bool HasFixedRotations { get; }
    }
}
