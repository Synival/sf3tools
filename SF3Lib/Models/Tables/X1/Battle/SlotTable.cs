using System.Collections.Generic;
using CommonLib;
using SF3.Actors;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace SF3.Models.Tables.X1.Battle {
    public class SlotTable : FixedSizeTable<Slot>, IIndexedEnumerableWithLength<IActor> {
        protected SlotTable(IByteData data, string name, int address, int size, ScenarioType scenario, Slot prevSlot) : base(data, name, address, size) {
            Scenario = scenario;
            PrevSlot = prevSlot;
        }

        public static SlotTable Create(IByteData data, string name, int address, int size, ScenarioType scenario, Slot prevSlot)
            => Create(() => new SlotTable(data, name, address, size, scenario, prevSlot));

        public override bool Load() {
            return Load((id, address) => {
                var name = (id < 12) ? ("CharacterSlot" + id.ToString("D2")) : ("EnemySlot" + (id - 12).ToString("D2"));
                PrevSlot = new Slot(Data, id, name, address, Scenario, PrevSlot);
                return PrevSlot;
            });
        }

        public ScenarioType Scenario { get; }
        public Slot PrevSlot { get; private set; }

        IActor[] IIndexedEnumerableWithLength<IActor>.AsArray() => AsArray();
        IEnumerator<IActor> IEnumerable<IActor>.GetEnumerator() => GetEnumerator();
        IActor IIndexedEnumerableWithLength<IActor>.this[int index] => Rows[index];
    }
}
