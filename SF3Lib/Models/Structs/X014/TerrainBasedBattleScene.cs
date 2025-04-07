using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class TerrainBasedBattleScene : Struct {
        public TerrainBasedBattleScene(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x04) {
        }

        [TableViewModelColumn(displayOrder: 0, minWidth: 100)]
        public TerrainType TerrainType => (TerrainType) ID;

        [TableViewModelColumn(displayOrder: 1, minWidth: 120, displayFormat: "X3")]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int BattleSceneFileID {
            get => Data.GetDouble(Address);
            set => Data.SetDouble(Address, value);
        }
    }
}
