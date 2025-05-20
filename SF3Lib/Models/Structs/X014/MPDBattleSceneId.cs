using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class MPDBattleSceneId : Struct {
        private readonly int _mpdFileIdAddr;
        private readonly int _battleSceneIdAddr;

        public MPDBattleSceneId(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x08) {
            _mpdFileIdAddr     = Address + 0x00;
            _battleSceneIdAddr = Address + 0x04;
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int MPDFileID {
            get => Data.GetDouble(_mpdFileIdAddr);
            set => Data.SetDouble(_mpdFileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int BattleSceneID {
            get => Data.GetDouble(_battleSceneIdAddr);
            set => Data.SetDouble(_battleSceneIdAddr, value);
        }
    }
}
