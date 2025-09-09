using SF3.ByteData;
using SF3.Models.Structs.X012;

namespace SF3.Models.Tables.X012 {
    public class ClassTargetUnknownTable : TerminatedTable<ClassTargetUnknown> {
        protected ClassTargetUnknownTable(IByteData data, string name, int address)
        : base(data, name, address, 1, null) {
        }

        public static ClassTargetUnknownTable Create(IByteData data, string name, int address)
            => Create(() => new ClassTargetUnknownTable(data, name, address));

        public override bool Load() {
            return Load(
                (id, address) => new ClassTargetUnknown(Data, id, "ClassUnknown" + id.ToString("D2"), address),
                (rows, model) => model.ClassID != 0xFF,
                false
            );
        }
    }
}
