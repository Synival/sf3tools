using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Main;

namespace SF3.Models.Tables.MPD.Main {
    public class ModelSwitchGroupsTable : TerminatedTable<ModelSwitchGroup> {
        protected ModelSwitchGroupsTable(IByteData data, string name, int address, INameGetterContext nameGetterContext) : base(data, name, address, 4, null) {
            NameGetterContext = nameGetterContext;
        }

        public static ModelSwitchGroupsTable Create(IByteData data, string name, int address, INameGetterContext nameGetterContext)
            => Create(() => new ModelSwitchGroupsTable(data, name, address, nameGetterContext));

        public override bool Load() {
            return Load(
                (id, address) => new ModelSwitchGroup(Data, id, "ModelSwitchGroup" + id.ToString("D2"), address, NameGetterContext),
                (currentRows, model) => model.Flag != -1,
                false);
        }

        public INameGetterContext NameGetterContext { get; }
    }
}
