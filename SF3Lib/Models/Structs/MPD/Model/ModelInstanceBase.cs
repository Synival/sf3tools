using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.MPD.Interfaces;
using SF3.Types;
using static CommonLib.Extensions.VECTOR_Extensions;

namespace SF3.Models.Structs.MPD.Model {
    public abstract class ModelInstanceBase : Struct, IMPD_ModelInstance {
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

        public ModelInstanceBase(IByteData data, IMPD_ModelCollection collection, int id, string name, int address, int positionXOffset, int size)
        : base(data, id, name, address, size) {
            Collection = collection;

            _pdata0Address    = Address + 0x00;                   // 4 bytes
            _positionXAddress = Address + positionXOffset + 0x00; // 2 bytes
            _positionYAddress = Address + positionXOffset + 0x02; // 2 bytes
            _positionZAddress = Address + positionXOffset + 0x04; // 2 bytes
            _angleXAddress    = Address + positionXOffset + 0x06; // 2 bytes
            _angleYAddress    = Address + positionXOffset + 0x08; // 2 bytes
            _angleZAddress    = Address + positionXOffset + 0x0A; // 2 bytes
            _scaleXAddress    = Address + positionXOffset + 0x0C; // 4 bytes
            _scaleYAddress    = Address + positionXOffset + 0x10; // 4 bytes
            _scaleZAddress    = Address + positionXOffset + 0x14; // 4 bytes
        }

        public IMPD_ModelLoD GetModel(int lod)
            => Collection.GetModel(ModelID, lod);

        public IMPD_ModelCollection Collection { get; }

        [TableViewModelColumn(addressField: null, displayName: "Collection", displayOrder: -0.5f, minWidth: 120)]
        public MPD_CollectionType CollectionType => Collection.Collection;

        // (Updated elsewhere)
        private int _modelId = -1;
        [TableViewModelColumn(addressField: nameof(_pdata0Address), displayOrder: -0.1f, displayName: "PDATA*[0]", isPointer: true)]
        public int ModelID {
            get => _modelId;
            set {
                _modelId = value;
                UpdatePDatas();
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_pdata0Address), displayOrder: 0, displayName: "PDATA*[0]", isPointer: true)]
        public uint PData0 {
            get => (uint) Data.GetInt32(_pdata0Address);
            set => Data.SetInt32(_pdata0Address, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_positionXAddress), displayOrder: 8)]
        public short PositionX {
            get => Data.GetInt16(_positionXAddress);
            set {
                Data.SetInt16(_positionXAddress, value);
                _boundingBox = null;
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_positionYAddress), displayOrder: 9)]
        public short PositionY {
            get => Data.GetInt16(_positionYAddress);
            set {
                Data.SetInt16(_positionYAddress, value);
                _boundingBox = null;
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_positionZAddress), displayOrder: 10)]
        public short PositionZ {
            get => Data.GetInt16(_positionZAddress);
            set {
                Data.SetInt16(_positionZAddress, value);
                _boundingBox = null;
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_angleXAddress), displayOrder: 11)]
        public float AngleX {
            get => Data.GetCompressedFIXED(_angleXAddress).Float * 180.0f;
            set {
                Data.SetCompressedFIXED(_angleXAddress, new CompressedFIXED(value / 180.0f, 0));
                _boundingBox = null;
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_angleYAddress), displayOrder: 12)]
        public float AngleY {
            get => Data.GetCompressedFIXED(_angleYAddress).Float * 180.0f;
            set {
                Data.SetCompressedFIXED(_angleYAddress, new CompressedFIXED(value / 180.0f, 0));
                _boundingBox = null;
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_angleZAddress), displayOrder: 13)]
        public float AngleZ {
            get => Data.GetCompressedFIXED(_angleZAddress).Float * 180.0f;
            set {
                Data.SetCompressedFIXED(_angleZAddress, new CompressedFIXED(value / 180.0f, 0));
                _boundingBox = null;
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_scaleXAddress), displayOrder: 14)]
        public float ScaleX {
            get => Data.GetFIXED(_scaleXAddress).Float;
            set {
                Data.SetFIXED(_scaleXAddress, new FIXED(value, 0));
                _boundingBox = null;
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_scaleYAddress), displayOrder: 15)]
        public float ScaleY {
            get => Data.GetFIXED(_scaleYAddress).Float;
            set {
                Data.SetFIXED(_scaleYAddress, new FIXED(value, 0));
                _boundingBox = null;
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_scaleZAddress), displayOrder: 16)]
        public float ScaleZ {
            get => Data.GetFIXED(_scaleZAddress).Float;
            set {
                Data.SetFIXED(_scaleZAddress, new FIXED(value, 0));
                _boundingBox = null;
            }
        }

        public abstract ushort Tag { get; set; }
        public abstract ushort Flags { get; set; }
        public abstract bool AlwaysFacesCamera { get; set; }
        public abstract ModelDirectionType OnlyVisibleFromDirection { get; set; }

        public abstract int LevelsOfDetail { get; set; }

        private BoundingBox? _boundingBox = null;
        public BoundingBox BoundingBox {
            get {
                if (!_boundingBox.HasValue) {
                    _boundingBox = GetModel(0).Vertices.ToArray()
                        .CreateBoundingBox()
                        .ToVECTORs()
                        .Scale(ScaleX, ScaleY, ScaleZ)
                        .RotateXYZ(AngleX, AngleY, AngleZ)
                        .CreateBoundingBox();
                }
                return _boundingBox.Value;
            }
        }

        protected abstract void UpdatePDatas();
        public Dictionary<(int ModelID, int LoD), uint> ModelIDToPDataMap { get; set; }
    }
}
