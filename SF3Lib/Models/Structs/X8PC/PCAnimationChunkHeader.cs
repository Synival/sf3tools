using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.X8PC {
    public class PCAnimationChunkHeader : Struct {
        private readonly int _centerXAddr;
        private readonly int _centerYAddr;
        private readonly int _centerZAddr;
        private readonly int _halfBoundingBoxXAddr;
        private readonly int _halfBoundingBoxYAddr;
        private readonly int _halfBoundingBoxZAddr;
        private readonly int _boneKeyframesTableOffsetAddr;

        public PCAnimationChunkHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x1C) {
            _centerXAddr                  = Address + 0x00; // 4 bytes
            _centerYAddr                  = Address + 0x04; // 4 bytes
            _centerZAddr                  = Address + 0x08; // 4 bytes
            _halfBoundingBoxXAddr         = Address + 0x0C; // 4 bytes
            _halfBoundingBoxYAddr         = Address + 0x10; // 4 bytes
            _halfBoundingBoxZAddr         = Address + 0x14; // 4 bytes
            _boneKeyframesTableOffsetAddr = Address + 0x18; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_centerXAddr), displayOrder: 0)]
        [BulkCopy]
        public float CenterX {
            get => Data.GetFIXED(_centerXAddr).Float;
            set => Data.SetFIXED(_centerXAddr, new FIXED(value, 0));
        }

        [TableViewModelColumn(addressField: nameof(_centerYAddr), displayOrder: 1)]
        [BulkCopy]
        public float CenterY {
            get => Data.GetFIXED(_centerYAddr).Float;
            set => Data.SetFIXED(_centerYAddr, new FIXED(value, 0));
        }

        [TableViewModelColumn(addressField: nameof(_centerZAddr), displayOrder: 2)]
        [BulkCopy]
        public float CenterZ {
            get => Data.GetFIXED(_centerZAddr).Float;
            set => Data.SetFIXED(_centerZAddr, new FIXED(value, 0));
        }

        [TableViewModelColumn(addressField: nameof(_halfBoundingBoxXAddr), displayOrder: 3)]
        [BulkCopy]
        public float HalfBoundingBoxX {
            get => Data.GetFIXED(_halfBoundingBoxXAddr).Float;
            set => Data.SetFIXED(_halfBoundingBoxXAddr, new FIXED(value, 0));
        }

        [TableViewModelColumn(addressField: nameof(_halfBoundingBoxYAddr), displayOrder: 4)]
        [BulkCopy]
        public float HalfBoundingBoxY {
            get => Data.GetFIXED(_halfBoundingBoxYAddr).Float;
            set => Data.SetFIXED(_halfBoundingBoxYAddr, new FIXED(value, 0));
        }

        [TableViewModelColumn(addressField: nameof(_halfBoundingBoxZAddr), displayOrder: 5)]
        [BulkCopy]
        public float HalfBoundingBoxZ {
            get => Data.GetFIXED(_halfBoundingBoxZAddr).Float;
            set => Data.SetFIXED(_halfBoundingBoxZAddr, new FIXED(value, 0));
        }

        [TableViewModelColumn(addressField: nameof(_boneKeyframesTableOffsetAddr), displayOrder: 6, displayFormat: "X2")]
        [BulkCopy]
        public uint BoneKeyframesTableOffset {
            get => Data.GetUInt32(_boneKeyframesTableOffsetAddr);
            set => Data.SetUInt32(_boneKeyframesTableOffsetAddr, value);
        }
    }
}
