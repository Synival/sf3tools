using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class SpriteTable : TerminatedTable<Sprite> {
        protected SpriteTable(IByteData data, string name, int address)
        : base(data, name, address, terminatedBytes: 4, maxSize: 100) {
        }

        public static SpriteTable Create(IByteData data, string name, int address) {
            var newTable = new SpriteTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new Sprite(Data, id, nameof(Sprite) + id.ToString("D2"), addr),
                (rows, prevRow) => prevRow.SpriteId != 0xFFFF,
                false
            );
        }
    }
}
