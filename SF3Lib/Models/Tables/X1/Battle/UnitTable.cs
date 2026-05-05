using System.Collections.Generic;
using SF3.Actors;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace SF3.Models.Tables.X1.Battle {
    public class UnitTable : FixedSizeTable<Unit>, IReadOnlyList<IActor> {
        protected UnitTable(IByteData data, string name, int address, int size, ScenarioType scenario, BattleHeader battleHeader, MapLeaderType mapLeader)
        : base(data, name, address, size) {
            Scenario     = scenario;
            BattleHeader = battleHeader;
            MapLeader    = mapLeader;
        }

        public static UnitTable Create(IByteData data, string name, int address, int size, ScenarioType scenario, BattleHeader battleHeader, MapLeaderType mapLeader)
            => Create(() => new UnitTable(data, name, address, size, scenario, battleHeader, mapLeader));

        public override bool Load() {
            Unit prevUnit = null;
            return Load((id, address) => {
                var name = (id < 12) ? ("Character" + id.ToString("D2")) : ("Enemy" + (id - 12).ToString("D2"));
                prevUnit = new Unit(Data, id, name, address, Scenario, prevUnit, BattleHeader, MapLeader);
                return prevUnit;
            });
        }

        public ScenarioType Scenario { get; }
        public BattleHeader BattleHeader { get; }
        public MapLeaderType MapLeader { get; }

        IEnumerator<IActor> IEnumerable<IActor>.GetEnumerator() => GetEnumerator();
        IActor IReadOnlyList<IActor>.this[int index] => Rows[index];
    }
}
