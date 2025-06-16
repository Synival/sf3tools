using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class SpriteFrameTable : TerminatedTable<SpriteFrame> {
        protected SpriteFrameTable(IByteData data, string name, int address, uint dataOffset, int width, int height)
        : base(data, name, address, 0x04, 100) {
            DataOffset = dataOffset;
            Width      = width;
            Height     = height;
        }

        public static SpriteFrameTable Create(IByteData data, string name, int address, uint dataOffset, int width, int height) {
            var newTable = new SpriteFrameTable(data, name, address, dataOffset, width, height);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new SpriteFrame(Data, id, $"{nameof(SpriteFrame)}{id:D2}", addr, DataOffset, Width, Height),
                (rows, prevRow) => prevRow.TextureOffset != 0,
                addEndModel: false
            );
        }

        public uint DataOffset { get; }
        public int Width { get; }
        public int Height { get; }
    }
}
