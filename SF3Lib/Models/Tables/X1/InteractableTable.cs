using System;
using SF3.ByteData;
using SF3.Models.Structs.X1;

namespace SF3.Models.Tables.X1 {
    public class InteractableTable : TerminatedTable<Interactable> {
        protected InteractableTable(IByteData data, string name, int address)
        : base(data, name, address, 2, 255) {
        }

        public static InteractableTable Create(IByteData data, string name, int address) {
            var newTable = new InteractableTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new Interactable(Data, id, "Interactable" + id.ToString("D2"), address),
                (rows, model) => model.Searched != 0xFFFF,
                false);
        }
    }
}
