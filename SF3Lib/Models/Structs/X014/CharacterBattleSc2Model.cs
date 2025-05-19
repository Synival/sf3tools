using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class CharacterBattleSc2Model : Struct {
        private readonly int _modelFileIdAddr;
        private readonly int _subModelStartAddr;

        public CharacterBattleSc2Model(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x08) {
            _modelFileIdAddr   = Address + 0x00; // 4 bytes
            _subModelStartAddr = Address + 0x04; // 4 bytes (8 "nibbles")
        }

        private byte GetSubmodel(int index)
            => (byte) ((Data.GetDouble(Address + 0x04) >> (index * 4)) & 0x0F);

        private void SetSubmodel(int index, byte value)
            => Data.SetDouble(Address + 0x04, Data.GetDouble(Address + 0x04) & ~(0x0F << (index * 4)) | ((value & 0x0F) << (index * 4)));

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int ModelFileID {
            get => Data.GetDouble(_modelFileIdAddr);
            set => Data.SetDouble(_modelFileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X1")]
        [BulkCopy]
        public byte UWp1 {
            get => GetSubmodel(0);
            set => SetSubmodel(0, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X1")]
        [BulkCopy]
        public byte UWp2 {
            get => GetSubmodel(1);
            set => SetSubmodel(1, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X1")]
        [BulkCopy]
        public byte UWp3 {
            get => GetSubmodel(2);
            set => SetSubmodel(2, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X1")]
        [BulkCopy]
        public byte UWp4 {
            get => GetSubmodel(3);
            set => SetSubmodel(3, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayFormat: "X1")]
        [BulkCopy]
        public byte PWp1 {
            get => GetSubmodel(4);
            set => SetSubmodel(4, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayFormat: "X1")]
        [BulkCopy]
        public byte PWp2 {
            get => GetSubmodel(5);
            set => SetSubmodel(5, value);
        }

        [TableViewModelColumn(displayOrder: 7, displayFormat: "X1")]
        [BulkCopy]
        public byte PWp3 {
            get => GetSubmodel(6);
            set => SetSubmodel(6, value);
        }

        [TableViewModelColumn(displayOrder: 8, displayFormat: "X1")]
        [BulkCopy]
        public byte PWp4 {
            get => GetSubmodel(7);
            set => SetSubmodel(7, value);
        }
    }
}
