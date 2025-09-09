using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables {
    public class ModelIDTable : TerminatedTable<ModelIDStruct> {
        protected ModelIDTable(IByteData data, string name, int address)
        : base(data, name, address, 2, null) {
        }

        public static ModelIDTable Create(IByteData data, string name, int address)
            => Create(() => new ModelIDTable(data, name, address));

        public override bool Load() {
            return Load((id, address) => new ModelIDStruct(Data, id, "ModelID" + id.ToString("D2"), address),
                (rows, newModel) => newModel.ModelID != 0xFFFF, false);
        }
    }
}
