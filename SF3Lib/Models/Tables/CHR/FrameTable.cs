using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;
using SF3.Types;

namespace SF3.Models.Tables.CHR {
    public class FrameTable : TerminatedTable<Frame> {
        protected FrameTable(IByteData data, string name, int address, uint dataOffset, int width, int height, string rowPrefix, int spriteIndex, int spriteId, int spriteDirections)
        : base(data, name, address, 0x04, 0xF0) {
            DataOffset       = dataOffset;
            Width            = width;
            Height           = height;
            RowPrefix        = rowPrefix ?? "";
            SpriteIndex      = spriteIndex;
            SpriteID         = spriteId;
            SpriteDirections = spriteDirections;
        }

        public static FrameTable Create(IByteData data, string name, int address, uint dataOffset, int width, int height, string rowPrefix, int spriteIndex, int spriteId, int spriteDirections) {
            var newTable = new FrameTable(data, name, address, dataOffset, width, height, rowPrefix, spriteIndex, spriteId, spriteDirections);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new Frame(Data, id, $"{RowPrefix}Frame{id:D2}", addr, DataOffset, Width, Height, SpriteIndex, SpriteID),
                (rows, prevRow) => prevRow.TextureOffset != 0,
                addEndModel: false
            );
        }

        public uint DataOffset { get; }
        public int Width { get; }
        public int Height { get; }
        public string RowPrefix { get; }
        public int SpriteIndex { get; }
        public int SpriteID { get; }
        public int SpriteDirections { get; }
    }
}
