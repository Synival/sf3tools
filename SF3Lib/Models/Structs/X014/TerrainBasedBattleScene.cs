using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class TerrainBasedBattleScene : Struct {
        private readonly int _battleSceneFileIdAddr;

        public TerrainBasedBattleScene(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x04) {
            _battleSceneFileIdAddr = Address + 0x00; // 4 bytes
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0, minWidth: 100)]
        public TerrainType TerrainType => (TerrainType) ID;

        [TableViewModelColumn(addressField: nameof(_battleSceneFileIdAddr), displayOrder: 1, minWidth: 120, displayFormat: "X3")]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int BattleSceneFileID {
            get => Data.GetDouble(_battleSceneFileIdAddr);
            set => Data.SetDouble(_battleSceneFileIdAddr, value);
        }
    }
}
