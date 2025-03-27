using System;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.X1;
using SF3.Models.Tables.X1.Town;

namespace SF3.Models.Tables.X1 {
    public class InteractableTable : TerminatedTable<Interactable> {
        protected InteractableTable(IByteData data, string name, int address, INameGetterContext nameGetterContext, NpcTable npcTable)
        : base(data, name, address, 2, 255) {
            NameGetterContext = nameGetterContext;
            NpcTable = npcTable;
        }

        public static InteractableTable Create(IByteData data, string name, int address, INameGetterContext nameGetterContext, NpcTable npcTable) {
            var newTable = new InteractableTable(data, name, address, nameGetterContext, npcTable);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new Interactable(Data, id, "Interactable" + id.ToString("D2"), address, NameGetterContext, NpcTable),
                (rows, model) => model.Searched != 0xFFFF,
                false);
        }

        public INameGetterContext NameGetterContext { get; }
        public NpcTable NpcTable { get; }
    }
}
