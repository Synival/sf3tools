using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables.X1.Battle;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class BattlePointer : Struct {
        private readonly int _pointerAddr;

        public BattlePointer(IByteData data, int id, string name, int address, bool hasLargeEnemyTable, ScenarioType scenario, MapLeaderType mapLeader, BattlePointerTable battles)
        : base(data, id, name, address, 0x04) {
            HasLargeEnemyTable = hasLargeEnemyTable;
            Scenario           = scenario;
            Battles            = battles;
            MapLeader          = mapLeader;

            _pointerAddr = Address; // 2 bytes

            UpdateBattle();
        }

        [TableViewModelColumn(addressField: nameof(_pointerAddr), displayOrder: 0, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int Pointer {
            get => Data.GetDouble(_pointerAddr);
            set => Data.SetDouble(_pointerAddr, value);
        }

        private void UpdateBattle() {
            if (Pointer == 0) {
                _battle = null;
                return;
            }

            if (_battle != null && Pointer == _battle.Address)
                return;

            _battle = new Battle(Data, ID, $"Battle_{MapLeader}", MapLeader, Pointer - Battles.RamAddress, HasLargeEnemyTable, Scenario, Battles);
        }

        public MapLeaderType MapLeader { get; }
        public bool HasLargeEnemyTable { get; }
        public ScenarioType Scenario { get; }
        public BattlePointerTable Battles { get; }

        private Battle _battle = null;
        public Battle Battle => _battle;
    }
}
