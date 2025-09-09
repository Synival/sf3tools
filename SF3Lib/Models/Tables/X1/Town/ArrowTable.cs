using SF3.ByteData;
using SF3.Models.Structs.X1.Town;

namespace SF3.Models.Tables.X1.Town {
    public class ArrowTable : TerminatedTable<Arrow> {
        protected ArrowTable(IByteData data, string name, int address)
        : base(data, name, address, 2, 100) {
        }

        public static ArrowTable Create(IByteData data, string name, int address)
            => CreateBase(() => new ArrowTable(data, name, address));

        public override bool Load()
            => Load(
                (id, address) => new Arrow(Data, id, "Arrow" + id.ToString("D2"), address),
                (rows, model) => model.Unknown0x00 != 0xFFFF,
                false);
    }
}
