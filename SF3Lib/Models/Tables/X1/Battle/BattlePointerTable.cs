using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace SF3.Models.Tables.X1.Battle {
    public class BattlePointerTable : ResourceTable<BattlePointer> {
        protected BattlePointerTable(IByteData data, string name, string resourceFile, int address, bool hasLargeEnemyTable, ScenarioType scenario, int ramAddress)
        : base(data, name, resourceFile, address, 5) {
            HasLargeEnemyTable = hasLargeEnemyTable;
            Scenario           = scenario;
            RamAddress         = ramAddress;
        }

        public static BattlePointerTable Create(IByteData data, string name, string resourceFile, int address, bool hasLargeEnemyTable, ScenarioType scenario, int ramAddress)
            => Create(() => new BattlePointerTable(data, name, resourceFile, address, hasLargeEnemyTable, scenario, ramAddress));

        public override bool Load()
            => Load((id, name, address) => new BattlePointer(Data, id, name, address, HasLargeEnemyTable, Scenario, (MapLeaderType) id, this));

        public bool HasLargeEnemyTable { get; }
        public ScenarioType Scenario { get; }
        public int RamAddress { get; }
    }
}
