using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.ByteData;

namespace SF3.Models.Tables.X013 {
    public class SoulfailTable : Table<Soulfail> {
        protected SoulfailTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static SoulfailTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new SoulfailTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Soulfail(Data, id, name, address));

        public override int? MaxSize => 1;
    }
}
