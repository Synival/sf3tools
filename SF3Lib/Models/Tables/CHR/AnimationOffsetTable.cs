using System;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class AnimationOffsetTable : FixedSizeTable<AnimationOffset> {
        protected AnimationOffsetTable(IByteData data, string name, int address) : base(data, name, address, 16) {}

        public static AnimationOffsetTable Create(IByteData data, string name, int address) {
            var newTable = new AnimationOffsetTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new AnimationOffset(Data, id, $"{nameof(AnimationOffset)}{id:D2}", address));
    }
}
