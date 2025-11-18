using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class AnimationOffset : Struct {
        private readonly int _offsetAddr;

        public AnimationOffset(IByteData data, int id, string name, int address, uint dataOffset, bool isInCHP)
        : base(data, id, name, address, 0x04) {
            DataOffset = dataOffset;
            IsInCHP    = isInCHP;

            _offsetAddr = Address;
        }

        public uint DataOffset { get; }
        public bool IsInCHP { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.1f)]
        public AnimationType AnimationType => (AnimationType) ID;

        [TableViewModelColumn(addressField: nameof(_offsetAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public uint Offset {
            get => (uint) Data.GetDouble(_offsetAddr);
            set => Data.SetDouble(_offsetAddr, (int) value);
        }

        [TableViewModelColumn(displayOrder: 0.1f, displayName: "Offset (In File)", displayFormat: "X2", visibilityProperty: nameof(IsInCHP))]
        public uint OffsetInFile {
            get => Offset + DataOffset;
            set => Offset = value - DataOffset;
        }

    }
}
