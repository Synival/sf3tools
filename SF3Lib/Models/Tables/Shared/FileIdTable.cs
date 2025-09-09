using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class FileIdTable : ResourceTable<FileIdModel> {
        protected FileIdTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 0x100) {
        }

        public static FileIdTable Create(IByteData data, string name, string resourceFile, int address)
            => Create(() => new FileIdTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new FileIdModel(Data, id, name, address));
    }
}
