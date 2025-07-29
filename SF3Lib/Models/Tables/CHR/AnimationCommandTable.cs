using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class AnimationCommandTable : TerminatedTable<AnimationCommand> {
        protected AnimationCommandTable(IByteData data, string name, int address, string rowPrefix, int spriteIndex, int spriteId, int spriteDirections, int animationType, FrameTable frameTable, int max)
        : base(data, name, address, 0x00, max) {
            RowPrefix        = rowPrefix ?? "";
            SpriteIndex      = spriteIndex;
            SpriteID         = spriteId;
            SpriteDirections = spriteDirections;
            AnimationIndex   = animationType;
            FrameTable       = frameTable;
        }

        public static AnimationCommandTable Create(IByteData data, string name, int address, string rowPrefix, int spriteIndex, int spriteId, int spriteDirections, int animationIndex, FrameTable frameTable, int max) {
            var newTable = new AnimationCommandTable(data, name, address, rowPrefix, spriteIndex, spriteId, spriteDirections, animationIndex, frameTable, max);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            AnimationCommand prevCommand = null;
            return Load(
                (id, addr) => {
                    var directions = (prevCommand == null) ? SpriteDirections : prevCommand.Directions;
                    prevCommand = new AnimationCommand(Data, id, $"{RowPrefix}{nameof(AnimationCommand)}{id:D2}", addr, SpriteIndex, SpriteID, directions, AnimationIndex, FrameTable);
                    return prevCommand;
                },
                (rows, prevRow) => !prevRow.IsFinalCommand,
                addEndModel: true // This last row has important data; we always want to include it
            );
        }

        public int SpriteIndex { get; }
        public string RowPrefix { get; }
        public int SpriteID { get; }
        public int SpriteDirections { get; }
        public int AnimationIndex { get; }
        public FrameTable FrameTable { get; }
    }
}
