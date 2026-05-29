using SF3.ByteData;
using SF3.Models.Structs.X014;

namespace SF3.Models.Tables.X014 {
    public class ModelAnimLoadInfoSc1Table : TerminatedTable<ModelAnimLoadInfoSc1> {
        protected ModelAnimLoadInfoSc1Table(IByteData data, string name, int address) : base(data, name, address, 0x04, 0x400) {
        }

        public static ModelAnimLoadInfoSc1Table Create(IByteData data, string name, int address)
            => Create(() => new ModelAnimLoadInfoSc1Table(data, name, address));

        public override bool Load() {
            return Load(
                (id, address) => new ModelAnimLoadInfoSc1(Data, id, $"{nameof(ModelAnimLoadInfoSc1)}{id:D2}", address),
                (rows, lastRow) => lastRow.ModelFileID != -1,
                false
            );
        }
    }
}
