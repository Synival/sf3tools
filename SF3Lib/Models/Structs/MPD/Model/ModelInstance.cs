using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Model {
    public class ModelInstance : ModelInstanceBase {
        private readonly int _pdata1Address;
        private readonly int _pdata2Address;
        private readonly int _pdata3Address;
        private readonly int _pdata4Address;
        private readonly int _pdata5Address;
        private readonly int _pdata6Address;
        private readonly int _pdata7Address;
        private readonly int _tagAddress;
        private readonly int _flagsAddress;

        public ModelInstance(IByteData data, IMPD_ModelCollection collection, int id, string name, int address, bool hasTagsAndFlags)
        : base(data, collection, id, name, address, 0x20, hasTagsAndFlags ? 0x3C : 0x38) {
            PDatas = new PDataAccessorCollection(this);
            HasTagsAndFlags = hasTagsAndFlags;

            _pdata1Address = Address + 0x04; // 4 bytes
            _pdata2Address = Address + 0x08; // 4 bytes
            _pdata3Address = Address + 0x0C; // 4 bytes
            _pdata4Address = Address + 0x10; // 4 bytes
            _pdata5Address = Address + 0x14; // 4 bytes
            _pdata6Address = Address + 0x18; // 4 bytes
            _pdata7Address = Address + 0x1C; // 4 bytes
            _tagAddress    = hasTagsAndFlags ? (Address + 0x38) : -1; // 2 bytes
            _flagsAddress  = hasTagsAndFlags ? (Address + 0x3A) : -1; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_pdata1Address), displayOrder: 0.1f, displayName: "PDATA*[1]", isPointer: true)]
        public uint PData1 {
            get => Data.GetUInt32(_pdata1Address);
            set => Data.SetUInt32(_pdata1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_pdata2Address), displayOrder: 0.2f, displayName: "PDATA*[2]", isPointer: true)]
        public uint PData2 {
            get => Data.GetUInt32(_pdata2Address);
            set => Data.SetUInt32(_pdata2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_pdata3Address), displayOrder: 0.3f, displayName: "PDATA*[3]", isPointer: true)]
        public uint PData3 {
            get => Data.GetUInt32(_pdata3Address);
            set => Data.SetUInt32(_pdata3Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_pdata4Address), displayOrder: 0.4f, displayName: "PDATA*[4]", isPointer: true)]
        public uint PData4 {
            get => Data.GetUInt32(_pdata4Address);
            set => Data.SetUInt32(_pdata4Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_pdata5Address), displayOrder: 0.5f, displayName: "PDATA*[5]", isPointer: true)]
        public uint PData5 {
            get => Data.GetUInt32(_pdata5Address);
            set => Data.SetUInt32(_pdata5Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_pdata6Address), displayOrder: 0.6f, displayName: "PDATA*[6]", isPointer: true)]
        public uint PData6 {
            get => Data.GetUInt32(_pdata6Address);
            set => Data.SetUInt32(_pdata6Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_pdata7Address), displayOrder: 0.7f, displayName: "PDATA*[7]", isPointer: true)]
        public uint PData7 {
            get => Data.GetUInt32(_pdata7Address);
            set => Data.SetUInt32(_pdata7Address, value);
        }

        public class PDataAccessor {
            public PDataAccessor(ModelInstance model, int index) {
                Model = model;
                Index = index;
            }

            public uint Value {
                get => Model.Data.GetUInt32(Model._pdata0Address + Index * 0x04);
                set => Model.Data.SetUInt32(Model._pdata0Address + Index * 0x04, value);
            }

            public ModelInstance Model { get; }
            public int Index { get; }
        };

        // Helper class to index PData's
        // TODO: This should be the other way around!!!! Make an array first, and let the properties access it
        public class PDataAccessorCollection : IEnumerable<PDataAccessor> {
            public PDataAccessorCollection(ModelInstance model) {
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
        [TableViewModelColumn(addressField: nameof(_tagAddress), displayOrder: 17)]
        public override ushort Tag {
            get => HasTagsAndFlags ? Data.GetUInt16(_tagAddress) : (ushort) 0;
            set {
                if (HasTagsAndFlags)
                    Data.SetUInt16(_tagAddress, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_flagsAddress), displayOrder: 18, displayFormat: "X4")]
        public override ushort Flags {
            get => HasTagsAndFlags ? Data.GetUInt16(_flagsAddress) : (ushort) 0;
            set {
                if (HasTagsAndFlags)
                    Data.SetUInt16(_flagsAddress, value);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 18.1f)]
        public override bool AlwaysFacesCamera {
            get => (Flags & 0x08) == 0x08;
            set => Flags = (ushort) ((Flags & ~0x08) | (value ? 0x08 : 0x00));
        }

        [TableViewModelColumn(addressField: null, displayOrder: 18.2f, minWidth: 100)]
        public override ModelDirectionType OnlyVisibleFromDirection {
            get => ((Flags & 0x10) == 0x10) ? (ModelDirectionType) (Flags & 0x07) : ModelDirectionType.Unset;
            set {
                Flags = (value < 0x00 || (ushort) value > 0x07)
                    ? (ushort) (Flags & ~0x17)
                    : (ushort) (Flags | 0x10 | ((ushort) value & 0x07));
            }
        }

        public override int LevelsOfDetail {
            get => PDatas.FirstOrDefault(x => x.Value == 0)?.Index ?? 8;
            set {}
        }

        protected override void UpdatePDatas() {
            if (ModelIDToPDataMap == null)
                return;
            for (int i = 0; i < 8; i++)
                PDatas[i] = ModelIDToPDataMap.TryGetValue((ModelID, i), out var addr) ? addr : 0;
        }
    }
}
