using SF3.Models.Structs.X002;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X002 {
    public class LoadingTable : Table<Loading> {
        protected LoadingTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static LoadingTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new LoadingTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Loading(Data, id, name, address));

        public override int? MaxSize => 300;
    }
}
