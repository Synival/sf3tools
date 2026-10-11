using SF3.ByteData;
using SF3.Models.Structs.X014;

namespace SF3.Models.Tables.X014 {
    public class ModelAnimLoadInfoSc1Table : TerminatedTable<ModelAnimLoadInfoSc1> {
        protected ModelAnimLoadInfoSc1Table(IByteData data, string name, int address, bool isAllInts) : base(data, name, address, 0x04, 0x400) {
            IsAllInts = isAllInts;
        }

        public static ModelAnimLoadInfoSc1Table Create(IByteData data, string name, int address, bool isAllInts)
            => Create(() => new ModelAnimLoadInfoSc1Table(data, name, address, isAllInts));

        public override bool Load() {
            return Load(
                (id, address) => new ModelAnimLoadInfoSc1(Data, id, $"{nameof(ModelAnimLoadInfoSc1)}{id:D2}", address, IsAllInts),
                (rows, lastRow) => lastRow.ModelFileID != -1,
                false
            );
        }

        public bool IsAllInts { get; }
    }
}
