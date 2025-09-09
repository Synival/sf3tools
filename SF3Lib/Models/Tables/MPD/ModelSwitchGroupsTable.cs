using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class ModelSwitchGroupsTable : TerminatedTable<ModelSwitchGroup> {
        protected ModelSwitchGroupsTable(IByteData data, string name, int address) : base(data, name, address, 4, null) {
        }

        public static ModelSwitchGroupsTable Create(IByteData data, string name, int address)
            => CreateBase(() => new ModelSwitchGroupsTable(data, name, address));

        public override bool Load() {
            return Load(
                (id, address) => new ModelSwitchGroup(Data, id, "ModelSwitchGroup" + id.ToString("D2"), address),
                (currentRows, model) => model.Flag != -1,
                false);
        }
    }
}
