using SF3.Models.Structs.X1.Town;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X1.Town {
    public class ArrowTable : Table<Arrow> {
        public ArrowTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Arrow(Data, id, name, address),
                (rows, model) => model.ArrowUnknown0 != 0xFFFF);

        public override int? MaxSize => 100;
    }
}
