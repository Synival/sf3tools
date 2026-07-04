using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables.X1.Battle;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class BattleMapPointer : Struct {
        private readonly int _pointerAddr;

        public BattleMapPointer(IByteData data, int id, string name, int address, bool hasLargeEnemyTable, ScenarioType scenario, MapLeaderType mapLeader, BattleHeader battleHeader)
        : base(data, id, name, address, 0x04) {
            HasLargeEnemyTable = hasLargeEnemyTable;
            Scenario           = scenario;
            BattleHeader       = battleHeader;
            MapLeader          = mapLeader;

            _pointerAddr = Address; // 2 bytes

            UpdateBattle();
        }

        [TableViewModelColumn(addressField: nameof(_pointerAddr), displayOrder: 0, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int Pointer {
            get => Data.GetInt32(_pointerAddr);
            set => Data.SetInt32(_pointerAddr, value);
        }

        private void UpdateBattle() {
            if (Pointer == 0) {
                _battleMap = null;
                return;
            }

            if (_battleMap != null && Pointer == _battleMap.Address)
                return;

            _battleMap = new BattleMap(Data, ID, $"Battle_{MapLeader}", MapLeader, Pointer - BattleHeader.RamAddress, HasLargeEnemyTable, Scenario, BattleHeader);
        }

        public MapLeaderType MapLeader { get; }
        public bool HasLargeEnemyTable { get; }
        public ScenarioType Scenario { get; }
        public BattleHeader BattleHeader { get; }

        private BattleMap _battleMap = null;
        public BattleMap BattleMap => _battleMap;
    }
}
