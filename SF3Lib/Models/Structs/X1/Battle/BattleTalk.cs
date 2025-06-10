using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class BattleTalk : Struct {
        private readonly int _unknown0x00Addr;
        private readonly int _battleIdAddr;
        private readonly int _gameFlagAddr;
        private readonly int _functionAddr;

        public BattleTalk(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            _unknown0x00Addr = Address + 0x00; // 4 bytes
            _battleIdAddr    = Address + 0x04; // 4 bytes
            _gameFlagAddr    = Address + 0x08; // 4 bytes
            _functionAddr    = Address + 0x0C; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x00Addr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x00 {
            get => Data.GetDouble(_unknown0x00Addr);
            set => Data.SetDouble(_unknown0x00Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_battleIdAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int BattleID {
            get => Data.GetDouble(_battleIdAddr);
            set => Data.SetDouble(_battleIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_gameFlagAddr), displayOrder: 2, displayFormat: "X2", minWidth: 200)]
        [NameGetter(NamedValueType.GameFlag)]
        [BulkCopy]
        public int GameFlag {
            get => Data.GetDouble(_gameFlagAddr);
            set => Data.SetDouble(_gameFlagAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_functionAddr), displayOrder: 3, isPointer: true)]
        [BulkCopy]
        public int Function {
            get => Data.GetDouble(_functionAddr);
            set => Data.SetDouble(_functionAddr, value);
        }
    }
}
