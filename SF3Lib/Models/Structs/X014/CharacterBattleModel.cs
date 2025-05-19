using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class CharacterBattleModel : Struct {
        private readonly int _modelFileIdAddr;
        private readonly int _unknown0x04Addr;
        private readonly int _unknown0x06Addr;

        public CharacterBattleModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x08) {
            _modelFileIdAddr = Address + 0x00; // 4 bytes
            _unknown0x04Addr = Address + 0x04; // 2 bytes
            _unknown0x06Addr = Address + 0x06; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int ModelFileID {
            get => Data.GetDouble(_modelFileIdAddr);
            set => Data.SetDouble(_modelFileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public ushort Unknown0x04 {
            get => (ushort) Data.GetWord(_unknown0x04Addr);
            set => Data.SetWord(_unknown0x04Addr, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public ushort Unknown0x06 {
            get => (ushort) Data.GetWord(_unknown0x06Addr);
            set => Data.SetWord(_unknown0x06Addr, value);
        }
    }
}
