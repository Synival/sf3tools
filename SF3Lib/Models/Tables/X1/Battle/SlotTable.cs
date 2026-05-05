using System.Collections.Generic;
using SF3.Actors;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace SF3.Models.Tables.X1.Battle {
    public class SlotTable : FixedSizeTable<Slot>, IReadOnlyList<IActor> {
        protected SlotTable(IByteData data, string name, int address, int size, ScenarioType scenario, BattleHeader battleHeader, MapLeaderType mapLeader)
        : base(data, name, address, size) {
            Scenario     = scenario;
            BattleHeader = battleHeader;
            MapLeader    = mapLeader;
        }

        public static SlotTable Create(IByteData data, string name, int address, int size, ScenarioType scenario, BattleHeader battleHeader, MapLeaderType mapLeader)
            => Create(() => new SlotTable(data, name, address, size, scenario, battleHeader, mapLeader));

        public override bool Load() {
            Slot lastSlot = null;
            return Load((id, address) => {
                var name = (id < 12) ? ("CharacterSlot" + id.ToString("D2")) : ("EnemySlot" + (id - 12).ToString("D2"));
                lastSlot = new Slot(Data, id, name, address, Scenario, lastSlot, BattleHeader, MapLeader);
                return lastSlot;
            });
        }

        public ScenarioType Scenario { get; }
        public BattleHeader BattleHeader { get; }
        public MapLeaderType MapLeader { get; }

        IEnumerator<IActor> IEnumerable<IActor>.GetEnumerator() => GetEnumerator();
        IActor IReadOnlyList<IActor>.this[int index] => Rows[index];
    }
}
