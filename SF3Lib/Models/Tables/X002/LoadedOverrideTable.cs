using SF3.Models.Structs.X002;
using SF3.Models.Tables;
using SF3.ByteData;

namespace SF3.Models.Tables.X002 {
    public class LoadedOverrideTable : Table<LoadedOverride> {
        protected LoadedOverrideTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static LoadedOverrideTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new LoadedOverrideTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new LoadedOverride(Data, id, name, address));

        public override int? MaxSize => 300;
    }
}
