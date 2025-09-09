using SF3.ByteData;
using SF3.Models.Structs.X014;

namespace SF3.Models.Tables.X014 {
    public class TerrainBasedBattleSceneTable : FixedSizeTable<TerrainBasedBattleScene> {
        public TerrainBasedBattleSceneTable(IByteData data, string name, int address) : base(data, name, address, 16) {
        }

        public static TerrainBasedBattleSceneTable Create(IByteData data, string name, int address)
            => Create(() => new TerrainBasedBattleSceneTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new TerrainBasedBattleScene(Data, id, "TerrainBattleScene" + id.ToString("D2"), address));
    }
}
