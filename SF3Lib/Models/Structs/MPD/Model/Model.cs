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
        private readonly int _angle1Address;
        private readonly int _angle2Address;
        private readonly int _angle3Address;
        private readonly int _scaleXAddress;
        private readonly int _scaleYAddress;
        private readonly int _scaleZAddress;

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
            _angle1Address    = Address + 0x26; // 2 bytes
            _angle2Address    = Address + 0x28; // 2 bytes
            _angle3Address    = Address + 0x2A; // 2 bytes
            _scaleXAddress    = Address + 0x2C; // 4 bytes
            _scaleYAddress    = Address + 0x30; // 4 bytes
            _scaleZAddress    = Address + 0x34; // 4 bytes
            // (4 bytes of padding)
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

            int this[int index] {
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
        public CompressedFIXED Angle1 {
            get => Data.GetCompressedFIXED(_angle1Address);
            set => Data.SetCompressedFIXED(_angle1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 12)]
        public CompressedFIXED Angle2 {
            get => Data.GetCompressedFIXED(_angle2Address);
            set => Data.SetCompressedFIXED(_angle2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 13)]
        public CompressedFIXED Angle3 {
            get => Data.GetCompressedFIXED(_angle3Address);
            set => Data.SetCompressedFIXED(_angle3Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 14)]
        public FIXED ScaleX {
            get => Data.GetFIXED(_scaleXAddress);
            set => Data.SetFIXED(_scaleXAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 15)]
        public FIXED ScaleY {
            get => Data.GetFIXED(_scaleYAddress);
            set => Data.SetFIXED(_scaleYAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 16)]
        public FIXED ScaleZ {
            get => Data.GetFIXED(_scaleZAddress);
            set => Data.SetFIXED(_scaleZAddress, value);
        }
    }
}
