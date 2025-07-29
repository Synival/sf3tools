using System;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class AnimationTable : AddressedTable<Animation> {
        protected AnimationTable(IByteData data, string name, int spriteDirections, AnimationCommandTable[] animationCommandTables, FrameTable frameTable, string rowPrefix)
        : base(data, name, animationCommandTables.Select(x => x.Address).ToArray()) {
            SpriteDirections       = spriteDirections;
            AnimationCommandTables = animationCommandTables;
            FrameTable             = frameTable;
            RowPrefix              = rowPrefix ?? "";
        }

        public static AnimationTable Create(IByteData data, string name, int spriteDirections, AnimationCommandTable[] animationCommandTables, FrameTable frameTable, string rowPrefix) {
            var newTable = new AnimationTable(data, name, spriteDirections, animationCommandTables, frameTable, rowPrefix);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new Animation(Data, id, $"{RowPrefix}{nameof(Animation)}{id:D2}", addr, AnimationCommandTables[id].AnimationIndex, SpriteDirections, AnimationCommandTables[id], FrameTable)
            );
        }

        public int SpriteDirections { get; }
        public AnimationCommandTable[] AnimationCommandTables { get; }
        public FrameTable FrameTable { get; }
        public string RowPrefix { get; }
    }
}
