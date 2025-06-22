using System;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class AnimationTable : AddressedTable<Animation> {
        protected AnimationTable(IByteData data, string name, AnimationFrameTable[] animationFrameTables, string rowPrefix)
        : base(data, name, animationFrameTables.Select(x => x.Address).ToArray()) {
            AnimationFrameTables = animationFrameTables;
            RowPrefix = rowPrefix ?? "";
        }

        public static AnimationTable Create(IByteData data, string name, AnimationFrameTable[] animationFrameTables, string rowPrefix) {
            var newTable = new AnimationTable(data, name, animationFrameTables, rowPrefix);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, addr) => new Animation(Data, id, $"{RowPrefix}{nameof(Animation)}{id:D2}", addr, AnimationFrameTables[id])
            );
        }

        public AnimationFrameTable[] AnimationFrameTables { get; }
        public string RowPrefix { get; }
    }
}
