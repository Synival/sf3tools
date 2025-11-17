using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Model {
    public abstract class ModelBase : Struct {
        protected readonly int _pdata0Address;
        protected readonly int _positionXAddress;
        protected readonly int _positionYAddress;
        protected readonly int _positionZAddress;
        protected readonly int _angleXAddress;
        protected readonly int _angleYAddress;
        protected readonly int _angleZAddress;
        protected readonly int _scaleXAddress;
        protected readonly int _scaleYAddress;
        protected readonly int _scaleZAddress;

        public ModelBase(IByteData data, int id, string name, int address, int positionXOffset, int size, ModelCollectionType collectionType)
        : base(data, id, name, address, size) {
            CollectionType = collectionType;

            _pdata0Address = Address + 0x00; // 4 bytes
            _positionXAddress   = Address + positionXOffset + 0x00; // 2 bytes
            _positionYAddress   = Address + positionXOffset + 0x02; // 2 bytes
            _positionZAddress   = Address + positionXOffset + 0x04; // 2 bytes
            _angleXAddress      = Address + positionXOffset + 0x06; // 2 bytes
            _angleYAddress      = Address + positionXOffset + 0x08; // 2 bytes
            _angleZAddress      = Address + positionXOffset + 0x0A; // 2 bytes
            _scaleXAddress      = Address + positionXOffset + 0x0C; // 4 bytes
            _scaleYAddress      = Address + positionXOffset + 0x10; // 4 bytes
            _scaleZAddress      = Address + positionXOffset + 0x14; // 4 bytes
        }

        [TableViewModelColumn(addressField: null, displayOrder: -0.5f, minWidth: 120)]
        public ModelCollectionType CollectionType { get; }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_pdata0Address), displayOrder: 0, displayName: "PDATA*[0]", isPointer: true)]
        public uint PData0 {
            get => (uint) Data.GetDouble(_pdata0Address);
            set => Data.SetDouble(_pdata0Address, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_positionXAddress), displayOrder: 8)]
        public short PositionX {
            get => (short) Data.GetWord(_positionXAddress);
            set => Data.SetWord(_positionXAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_positionYAddress), displayOrder: 9)]
        public short PositionY {
            get => (short) Data.GetWord(_positionYAddress);
            set => Data.SetWord(_positionYAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_positionZAddress), displayOrder: 10)]
        public short PositionZ {
            get => (short) Data.GetWord(_positionZAddress);
            set => Data.SetWord(_positionZAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_angleXAddress), displayOrder: 11)]
        public float AngleX {
            get => Data.GetCompressedFIXED(_angleXAddress).Float * 180.0f;
            set => Data.SetCompressedFIXED(_angleXAddress, new CompressedFIXED(value / 180.0f, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_angleYAddress), displayOrder: 12)]
        public float AngleY {
            get => Data.GetCompressedFIXED(_angleYAddress).Float * 180.0f;
            set => Data.SetCompressedFIXED(_angleYAddress, new CompressedFIXED(value / 180.0f, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_angleZAddress), displayOrder: 13)]
        public float AngleZ {
            get => Data.GetCompressedFIXED(_angleZAddress).Float * 180.0f;
            set => Data.SetCompressedFIXED(_angleZAddress, new CompressedFIXED(value / 180.0f, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_scaleXAddress), displayOrder: 14)]
        public float ScaleX {
            get => Data.GetFIXED(_scaleXAddress).Float;
            set => Data.SetFIXED(_scaleXAddress, new FIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_scaleYAddress), displayOrder: 15)]
        public float ScaleY {
            get => Data.GetFIXED(_scaleYAddress).Float;
            set => Data.SetFIXED(_scaleYAddress, new FIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_scaleZAddress), displayOrder: 16)]
        public float ScaleZ {
            get => Data.GetFIXED(_scaleZAddress).Float;
            set => Data.SetFIXED(_scaleZAddress, new FIXED(value, 0));
        }

        public abstract bool AlwaysFacesCamera { get; set; }
        public abstract ModelDirectionType OnlyVisibleFromDirection { get; set; }
    }
}
