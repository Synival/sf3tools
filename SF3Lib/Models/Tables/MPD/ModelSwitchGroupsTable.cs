using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class ModelSwitchGroupsTable : TerminatedTable<ModelSwitchGroup> {
        protected ModelSwitchGroupsTable(IByteData data, string name, int address) : base(data, name, address) {
        }

        public static ModelSwitchGroupsTable Create(IByteData data, string name, int address) {
            var newTable = new ModelSwitchGroupsTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new ModelSwitchGroup(Data, id, "ModelSwitchGroup" + id, address),
                (currentRows, model) => model.TriggerFlag != 0xFFFF_FFFF,
                false);
        }
    }
}
