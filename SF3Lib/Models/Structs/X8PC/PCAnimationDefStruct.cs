using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X8PC {
    public class PCAnimationDefStruct : Struct {
        public readonly int _startFrameAddr;
        public readonly int _frameCountAddr;
        public readonly int _animIdAddr;
        public readonly int _distanceFromEnemyAddr;
        public readonly int _aniCommandsOffsetAddr;

        public PCAnimationDefStruct(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x0c) {
            _startFrameAddr        = Address + 0x00; // 2 bytes
            _frameCountAddr        = Address + 0x02; // 2 bytes
            _animIdAddr            = Address + 0x04; // 2 bytes
            _distanceFromEnemyAddr = Address + 0x06; // 2 bytes
            _aniCommandsOffsetAddr = Address + 0x08; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_startFrameAddr), displayOrder: 0)]
        [BulkCopy]
        public ushort StartFrame {
            get => (ushort) (Data.GetUInt16(_startFrameAddr) & 0xEFFF);
            set => Data.SetUInt16(_startFrameAddr, (ushort) ((value & 0xEFFF) | (Data.GetUInt16(_startFrameAddr) & 0x1000)));
        }

        [TableViewModelColumn(addressField: nameof(_frameCountAddr), displayOrder: 0.5f)]
        [BulkCopy]
        public bool IsSeparateChunk {
            get => Data.GetBit(_startFrameAddr, 5);
            set => Data.SetBit(_startFrameAddr, 5, value);
        }

        [TableViewModelColumn(addressField: nameof(_frameCountAddr), displayOrder: 1)]
        [BulkCopy]
        public ushort FrameCount {
            get => Data.GetUInt16(_frameCountAddr);
            set => Data.SetUInt16(_frameCountAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_animIdAddr), displayOrder: 2, minWidth: 120)]
        [NameGetter(NamedValueType.PCAnimationType)]
        [BulkCopy]
        public ushort AnimID {
            get => Data.GetUInt16(_animIdAddr);
            set => Data.SetUInt16(_animIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_distanceFromEnemyAddr), displayOrder: 3)]
        [BulkCopy]
        public ushort DistanceFromEnemy {
            get => Data.GetUInt16(_distanceFromEnemyAddr);
            set => Data.SetUInt16(_distanceFromEnemyAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_aniCommandsOffsetAddr), displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public uint AniCommandsOffset {
            get => Data.GetUInt32(_aniCommandsOffsetAddr);
            set => Data.SetUInt32(_aniCommandsOffsetAddr, value);
        }
    }
}
