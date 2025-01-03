using SF3.Models.Structs.X033_X031;
using SF3.Models.Tables;
using SF3.ByteData;

namespace SF3.Models.Tables.X033_X031 {
    public class InitialInfoTable : Table<InitialInfo> {
        protected InitialInfoTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static InitialInfoTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new InitialInfoTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new InitialInfo(Data, id, name, address));

        public override int? MaxSize => 100;
    }
}
