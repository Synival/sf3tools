using SF3.ByteData;
using SF3.Models.Structs.X019;
using SF3.Models.Tables.X002;

namespace SF3.Models.Tables.Shared {
    public class MonsterTable : ResourceTable<Monster> {
        protected MonsterTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 256) {
        }

        public static MonsterTable Create(IByteData data, string name, string resourceFile, int address)
            => CreateBase(() => new MonsterTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new Monster(Data, id, name, address));

        /// <summary>
        /// Applies or unapplies stats for equipment to all Monsters in the table.
        /// </summary>
        /// <param name="itemTable">Table from which to get equipment.</param>
        /// <param name="apply">When true, stat changes are applied. When false, stat changes are unapplied.</param>
        /// <returns>The total number of monsters affected and items applied.</returns>
        public (int, int) ApplyEquipmentStats(ItemTable itemTable, bool apply) {
            var totalMonsters = 0;
            var totalItems = 0;
            foreach (var monster in Rows) {
                var items = monster.ApplyEquipmentStats(itemTable, apply);
                totalItems += items;
                totalMonsters += items > 0 ? 1 : 0;
            }
            return (totalMonsters, totalItems);
        }
    }
}
