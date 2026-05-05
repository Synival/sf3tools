using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace SF3.Models.Tables.X1.Battle {
    public class BattleMapPointerTable : FixedSizeTable<BattleMapPointer> {
        protected BattleMapPointerTable(IByteData data, string name, int address, bool hasLargeEnemyTable, ScenarioType scenario, BattleHeader battleHeader)
        : base(data, name, address, 5) {
            HasLargeEnemyTable = hasLargeEnemyTable;
            Scenario           = scenario;
            BattleHeader       = battleHeader;
        }

        public static BattleMapPointerTable Create(IByteData data, string name, int address, bool hasLargeEnemyTable, ScenarioType scenario, BattleHeader battleHeader)
            => Create(() => new BattleMapPointerTable(data, name, address, hasLargeEnemyTable, scenario, battleHeader));

        public override bool Load()
            => Load((id, address) => new BattleMapPointer(Data, id, $"{(MapLeaderType) id} Map", address, HasLargeEnemyTable, Scenario, (MapLeaderType) id, BattleHeader));

        public bool HasLargeEnemyTable { get; }
        public ScenarioType Scenario { get; }
        public BattleHeader BattleHeader { get; }
    }
}
