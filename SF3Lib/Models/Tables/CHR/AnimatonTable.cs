using System;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class AnimationTable : AddressedTable<Animation> {
        protected AnimationTable(IByteData data, string name, int spriteDirections, AnimationFrameTable[] animationFrameTables, FrameTable frameTable, string rowPrefix)
        : base(data, name, animationFrameTables.Select(x => x.Address).ToArray()) {
            SpriteDirections     = spriteDirections;
            AnimationFrameTables = animationFrameTables;
            FrameTable           = frameTable;
            RowPrefix            = rowPrefix ?? "";
        }

        public static AnimationTable Create(IByteData data, string name, int spriteDirections, AnimationFrameTable[] animationFrameTables, FrameTable frameTable, string rowPrefix) {
            var newTable = new AnimationTable(data, name, spriteDirections, animationFrameTables, frameTable, rowPrefix);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new Animation(Data, id, $"{RowPrefix}{nameof(Animation)}{id:D2}", addr, AnimationFrameTables[id].AnimationIndex, SpriteDirections, AnimationFrameTables[id], FrameTable)
            );
        }

        public int SpriteDirections { get; }
        public AnimationFrameTable[] AnimationFrameTables { get; }
        public FrameTable FrameTable { get; }
        public string RowPrefix { get; }
    }
}
