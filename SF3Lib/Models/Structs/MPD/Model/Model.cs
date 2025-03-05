using System.Collections;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class Model : ModelBase {
        private readonly int _pdata1Address;
        private readonly int _pdata2Address;
        private readonly int _pdata3Address;
        private readonly int _pdata4Address;
        private readonly int _pdata5Address;
        private readonly int _pdata6Address;
        private readonly int _pdata7Address;
        private readonly int _modelIdAddress;
        private readonly int _flagsAddress;

        public Model(IByteData data, int id, string name, int address, bool hasTagsAndFlags)
        : base(data, id, name, address, 0x20, hasTagsAndFlags ? 0x3C : 0x38) {
            PDatas = new PDataAccessorCollection(this);
            HasTagsAndFlags = hasTagsAndFlags;

            _pdata1Address    = Address + 0x04; // 4 bytes
            _pdata2Address    = Address + 0x08; // 4 bytes
            _pdata3Address    = Address + 0x0C; // 4 bytes
            _pdata4Address    = Address + 0x10; // 4 bytes
            _pdata5Address    = Address + 0x14; // 4 bytes
            _pdata6Address    = Address + 0x18; // 4 bytes
            _pdata7Address    = Address + 0x1C; // 4 bytes
            _modelIdAddress   = hasTagsAndFlags ? (Address + 0x38) : -1; // 2 bytes
            _flagsAddress     = hasTagsAndFlags ? (Address + 0x3A) : -1; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.1f, displayName: "PDATA*[1]", isPointer: true)]
        public uint PData1 {
            get => (uint) Data.GetDouble(_pdata1Address);
            set => Data.SetDouble(_pdata1Address, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.2f, displayName: "PDATA*[2]", isPointer: true)]
        public uint PData2 {
            get => (uint) Data.GetDouble(_pdata2Address);
            set => Data.SetDouble(_pdata2Address, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.3f, displayName: "PDATA*[3]", isPointer: true)]
        public uint PData3 {
            get => (uint) Data.GetDouble(_pdata3Address);
            set => Data.SetDouble(_pdata3Address, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.4f, displayName: "PDATA*[4]", isPointer: true)]
        public uint PData4 {
            get => (uint) Data.GetDouble(_pdata4Address);
            set => Data.SetDouble(_pdata4Address, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.5f, displayName: "PDATA*[5]", isPointer: true)]
        public uint PData5 {
            get => (uint) Data.GetDouble(_pdata5Address);
            set => Data.SetDouble(_pdata5Address, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.6f, displayName: "PDATA*[6]", isPointer: true)]
        public uint PData6 {
            get => (uint) Data.GetDouble(_pdata6Address);
            set => Data.SetDouble(_pdata6Address, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0.7f, displayName: "PDATA*[7]", isPointer: true)]
        public uint PData7 {
            get => (uint) Data.GetDouble(_pdata7Address);
            set => Data.SetDouble(_pdata7Address, (int) value);
        }

        public class PDataAccessor {
            public PDataAccessor(Model model, int index) {
                Model = model;
                Index = index;
            }

            public uint Value {
                get => (uint) Model.Data.GetDouble(Model._pdata0Address + Index * 0x04);
                set => Model.Data.SetDouble(Model._pdata0Address + Index * 0x04, (int) value);
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

            public uint this[int index] {
                get => _accessors[index].Value;
                set => _accessors[index].Value = value;
            }

            private PDataAccessor[] _accessors;

            IEnumerator IEnumerable.GetEnumerator() => _accessors.GetEnumerator();
            public IEnumerator<PDataAccessor> GetEnumerator() => ((IEnumerable<PDataAccessor>) _accessors).GetEnumerator();
        }

        public readonly PDataAccessorCollection PDatas;
        public bool HasTagsAndFlags { get; }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 17, displayName: "Tag (unused?)")]
        public ushort Tag {
            get => HasTagsAndFlags ? (ushort) Data.GetWord(_modelIdAddress) : (ushort) 0;
            set {
                if (HasTagsAndFlags)
                    Data.SetWord(_modelIdAddress, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 18, displayFormat: "X4")]
        public ushort Flags {
            get => HasTagsAndFlags ? (ushort) Data.GetWord(_flagsAddress) : (ushort) 0;
            set {
                if (HasTagsAndFlags)
                    Data.SetWord(_flagsAddress, value);
            }
        }

        [TableViewModelColumn(displayOrder: 18.1f)]
        public override bool AlwaysFacesCamera {
            get => (Flags & 0x08) == 0x08;
            set => Flags = (ushort) ((Flags & ~0x08) | (value ? 0x08 : 0x00));
        }
    }
}
