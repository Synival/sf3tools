using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.ByteData;

namespace SF3.Models.Tables.X013 {
    public class SpecialTable : Table<Special> {
        protected SpecialTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static SpecialTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new SpecialTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Special(Data, id, name, address));

        public override int? MaxSize => 256;
    }
}
