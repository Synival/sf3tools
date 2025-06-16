using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class SpriteOffset2SubTable : TerminatedTable<SpriteOffset2Sub> {
        protected SpriteOffset2SubTable(IByteData data, string name, int address)
        : base(data, name, address, 0x00, 100) {
        }

        public static SpriteOffset2SubTable Create(IByteData data, string name, int address) {
            var newTable = new SpriteOffset2SubTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new SpriteOffset2Sub(Data, id, $"{nameof(SpriteOffset2Sub)}{id:D2}", addr),
                (rows, prevRow) => ((sbyte) prevRow.FrameID) >= 0,
                addEndModel: true // This last row has important data; we always want to include it
            );
        }
    }
}
