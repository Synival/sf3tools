using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class BattleTalk : Struct {
        private readonly int _characterId;
        private readonly int _unitIdAddr;
        private readonly int _gameFlagAddr;
        private readonly int _functionAddr;

        public BattleTalk(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            _characterId  = Address + 0x00; // 4 bytes
            _unitIdAddr   = Address + 0x04; // 4 bytes
            _gameFlagAddr = Address + 0x08; // 4 bytes
            _functionAddr = Address + 0x0C; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_characterId), displayOrder: 0, displayFormat: "X2")]
        [NameGetter(NamedValueType.Character)]
        [BulkCopy]
        public int CharacterID {
            get => Data.GetInt32(_characterId);
            set => Data.SetInt32(_characterId, value);
        }

        [TableViewModelColumn(addressField: nameof(_unitIdAddr), displayOrder: 1, displayFormat: "X2")]
        [NameGetter(NamedValueType.Character)]
        [BulkCopy]
        public int UnitID {
            get => Data.GetInt32(_unitIdAddr);
            set => Data.SetInt32(_unitIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_gameFlagAddr), displayOrder: 2, displayFormat: "X2", minWidth: 200)]
        [NameGetter(NamedValueType.GameFlag)]
        [BulkCopy]
        public int GameFlag {
            get => Data.GetInt32(_gameFlagAddr);
            set => Data.SetInt32(_gameFlagAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_functionAddr), displayOrder: 3, isPointer: true)]
        [BulkCopy]
        public int Function {
            get => Data.GetInt32(_functionAddr);
            set => Data.SetInt32(_functionAddr, value);
        }
    }
}
