using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;
using SF3.Types;

namespace SF3.Models.Tables.CHR {
    public class AnimationFrameTable : TerminatedTable<AnimationFrame> {
        protected AnimationFrameTable(IByteData data, string name, int address, string rowPrefix, int spriteIndex, int spriteId, int spriteDirections, AnimationType animationType, FrameTable frameTable)
        : base(data, name, address, 0x00, 100) {
            RowPrefix        = rowPrefix ?? "";
            SpriteIndex      = spriteIndex;
            SpriteID         = spriteId;
            SpriteDirections = spriteDirections;
            AnimationType    = animationType;
            FrameTable       = frameTable;
        }

        public static AnimationFrameTable Create(IByteData data, string name, int address, string rowPrefix, int spriteIndex, int spriteId, int spriteDirections, AnimationType animationType, FrameTable frameTable) {
            var newTable = new AnimationFrameTable(data, name, address, rowPrefix, spriteIndex, spriteId, spriteDirections, animationType, frameTable);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            AnimationFrame prevFrame = null;
            return Load(
                (id, addr) => {
                    var directions = (prevFrame == null) ? SpriteDirections : prevFrame.Directions;
                    prevFrame = new AnimationFrame(Data, id, $"{RowPrefix}{nameof(AnimationFrame)}{id:D2}", addr, SpriteIndex, SpriteID, directions, AnimationType, FrameTable);
                    return prevFrame;
                },
                (rows, prevRow) => !prevRow.IsFinalFrame,
                addEndModel: true // This last row has important data; we always want to include it
            );
        }

        public int SpriteIndex { get; }
        public string RowPrefix { get; }
        public int SpriteID { get; }
        public int SpriteDirections { get; }
        public AnimationType AnimationType { get; }
        public FrameTable FrameTable { get; }
    }
}
