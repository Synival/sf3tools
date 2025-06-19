using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class AnimationFrameTable : TerminatedTable<AnimationFrame> {
        protected AnimationFrameTable(IByteData data, string name, int address, string rowPrefix, int spriteIndex, int spriteId, int directions, int animIndex)
        : base(data, name, address, 0x00, 100) {
            RowPrefix   = rowPrefix ?? "";
            SpriteIndex = spriteIndex;
            SpriteID    = spriteId;
            Directions  = directions;
            AnimIndex   = animIndex;
        }

        public static AnimationFrameTable Create(IByteData data, string name, int address, string rowPrefix, int spriteIndex, int spriteId, int directions, int animIndex) {
            var newTable = new AnimationFrameTable(data, name, address, rowPrefix, spriteIndex, spriteId, directions, animIndex);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new AnimationFrame(Data, id, $"{RowPrefix}{nameof(AnimationFrame)}{id:D2}", addr, SpriteIndex, SpriteID, Directions, AnimIndex),
                (rows, prevRow) => ((sbyte) prevRow.FrameID) >= 0,
                addEndModel: true // This last row has important data; we always want to include it
            );
        }

        public int SpriteIndex { get; }
        public string RowPrefix { get; }
        public int SpriteID { get; }
        public int Directions { get; }
        public int AnimIndex { get; }
    }
}
