using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class AnimationOffsetTable : FixedSizeTable<AnimationOffset> {
        protected AnimationOffsetTable(IByteData data, string name, int address, int size, uint dataOffset, bool isInCHP)
        : base(data, name, address, size) {
            DataOffset = dataOffset;
            IsInCHP    = isInCHP;
        }

        public static AnimationOffsetTable Create(IByteData data, string name, int address, int size, uint dataOffset, bool isInCHP)
            => Create(() => new AnimationOffsetTable(data, name, address, size, dataOffset, isInCHP));

        public override bool Load()
            => Load((id, address) => new AnimationOffset(Data, id, $"{nameof(AnimationOffset)}{id:D2}", address, DataOffset, IsInCHP));

        public uint DataOffset { get; }
        public bool IsInCHP { get; }
    }
}
