using System.Collections;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class Model : Struct {
        private readonly int _pdata1Address;
        private readonly int _pdata2Address;
        private readonly int _pdata3Address;
        private readonly int _pdata4Address;
        private readonly int _pdata5Address;
        private readonly int _pdata6Address;
        private readonly int _pdata7Address;
        private readonly int _pdata8Address;
        private readonly int _positionXAddress;
        private readonly int _positionYAddress;
        private readonly int _positionZAddress;
        private readonly int _angleXAddress;
        private readonly int _angleYAddress;
        private readonly int _angleZAddress;
        private readonly int _scaleXAddress;
        private readonly int _scaleYAddress;
        private readonly int _scaleZAddress;
        private readonly int _modelIdAddress;
        private readonly int _flagsAddress;

        public Model(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x3C) {
            PDatas = new PDataAccessorCollection(this);

            _pdata1Address    = Address + 0x00; // 4 bytes
            _pdata2Address    = Address + 0x04; // 4 bytes
            _pdata3Address    = Address + 0x08; // 4 bytes
            _pdata4Address    = Address + 0x0C; // 4 bytes
            _pdata5Address    = Address + 0x10; // 4 bytes
            _pdata6Address    = Address + 0x14; // 4 bytes
            _pdata7Address    = Address + 0x18; // 4 bytes
            _pdata8Address    = Address + 0x1C; // 4 bytes
            _positionXAddress = Address + 0x20; // 2 bytes
            _positionYAddress = Address + 0x22; // 2 bytes
            _positionZAddress = Address + 0x24; // 2 bytes
            _angleXAddress    = Address + 0x26; // 2 bytes
            _angleYAddress    = Address + 0x28; // 2 bytes
            _angleZAddress    = Address + 0x2A; // 2 bytes
            _scaleXAddress    = Address + 0x2C; // 4 bytes
            _scaleYAddress    = Address + 0x30; // 4 bytes
            _scaleZAddress    = Address + 0x34; // 4 bytes
            _modelIdAddress   = Address + 0x38; // 2 bytes
            _flagsAddress     = Address + 0x3A; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, isPointer: true)]
        public int PData1 {
            get => Data.GetDouble(_pdata1Address);
            set => Data.SetDouble(_pdata1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, isPointer: true)]
        public int PData2 {
            get => Data.GetDouble(_pdata2Address);
            set => Data.SetDouble(_pdata2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, isPointer: true)]
        public int PData3 {
            get => Data.GetDouble(_pdata3Address);
            set => Data.SetDouble(_pdata3Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 3, isPointer: true)]
        public int PData4 {
            get => Data.GetDouble(_pdata4Address);
            set => Data.SetDouble(_pdata4Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 4, isPointer: true)]
        public int PData5 {
            get => Data.GetDouble(_pdata5Address);
            set => Data.SetDouble(_pdata5Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 5, isPointer: true)]
        public int PData6 {
            get => Data.GetDouble(_pdata6Address);
            set => Data.SetDouble(_pdata6Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 6, isPointer: true)]
        public int PData7 {
            get => Data.GetDouble(_pdata7Address);
            set => Data.SetDouble(_pdata7Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 7, isPointer: true)]
        public int PData8 {
            get => Data.GetDouble(_pdata8Address);
            set => Data.SetDouble(_pdata8Address, value);
        }

        public class PDataAccessor {
            public PDataAccessor(Model model, int index) {
                Model = model;
                Index = index;
            }

            public int Value {
                get => Model.Data.GetDouble(Model._pdata1Address + Index * 0x04);
                set => Model.Data.SetDouble(Model._pdata1Address + Index * 0x04, value);
            }

            public Model Model { get; }
            public int Index { get; }
        };

        // Helper class to index PData's
        // TODO: This should be the other way around!!!! Make an array first, and let the properties access it
        public class PDataAccessorCollection : IEnumerable<PDataAccessor> {
            public PDataAccessorCollection(Model model) {
                _accessors = new PDataAccessor[] {
                    new PDataAccessor(model, 0),
                    new PDataAccessor(model, 1),
                    new PDataAccessor(model, 2),
                    new PDataAccessor(model, 3),
                    new PDataAccessor(model, 4),
                    new PDataAccessor(model, 5),
                    new PDataAccessor(model, 6),
                    new PDataAccessor(model, 7),
                };
            }

            public int this[int index] {
                get => _accessors[index].Value;
                set => _accessors[index].Value = value;
            }

            private PDataAccessor[] _accessors;

            IEnumerator IEnumerable.GetEnumerator() => _accessors.GetEnumerator();
            public IEnumerator<PDataAccessor> GetEnumerator() => ((IEnumerable<PDataAccessor>) _accessors).GetEnumerator();
        }

        public readonly PDataAccessorCollection PDatas;

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 8)]
        public short PositionX {
            get => (short) Data.GetWord(_positionXAddress);
            set => Data.SetWord(_positionXAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 9)]
        public short PositionY {
            get => (short) Data.GetWord(_positionYAddress);
            set => Data.SetWord(_positionYAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 10)]
        public short PositionZ {
            get => (short) Data.GetWord(_positionZAddress);
            set => Data.SetWord(_positionZAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 11)]
        public float AngleX {
            get => Data.GetCompressedFIXED(_angleXAddress).Float;
            set => Data.SetCompressedFIXED(_angleXAddress, new CompressedFIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 12)]
        public float AngleY {
            get => Data.GetCompressedFIXED(_angleYAddress).Float;
            set => Data.SetCompressedFIXED(_angleYAddress, new CompressedFIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 13)]
        public float AngleZ {
            get => Data.GetCompressedFIXED(_angleZAddress).Float;
            set => Data.SetCompressedFIXED(_angleZAddress, new CompressedFIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 14)]
        public float ScaleX {
            get => Data.GetFIXED(_scaleXAddress).Float;
            set => Data.SetFIXED(_scaleXAddress, new FIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 15)]
        public float ScaleY {
            get => Data.GetFIXED(_scaleYAddress).Float;
            set => Data.SetFIXED(_scaleYAddress, new FIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 16)]
        public float ScaleZ {
            get => Data.GetFIXED(_scaleZAddress).Float;
            set => Data.SetFIXED(_scaleZAddress, new FIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 17, displayName: "Tag (unused?)")]
        public ushort Tag {
            get => (ushort) Data.GetWord(_modelIdAddress);
            set => Data.SetWord(_modelIdAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 18, displayFormat: "X4")]
        public ushort Flags {
            get => (ushort) Data.GetWord(_flagsAddress);
            set => Data.SetWord(_flagsAddress, value);
        }

        [TableViewModelColumn(displayOrder: 18.1f)]
        public bool AlwaysFacesCamera {
            get => (Flags & 0x08) == 0x08;
            set => Flags = (ushort) ((Flags & ~0x08) | (value ? 0x08 : 0x00));
        }
    }
}
