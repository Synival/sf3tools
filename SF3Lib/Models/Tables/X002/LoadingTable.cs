using SF3.ByteData;
using SF3.Models.Structs.X002;

namespace SF3.Models.Tables.X002 {
    public class LoadingTable : ResourceTable<Loading> {
        protected LoadingTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 300) {
        }

        public static LoadingTable Create(IByteData data, string name, string resourceFile, int address)
            => Create(() => new LoadingTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new Loading(Data, id, name, address));
    }
}
