using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SoulmateTable : ResourceTable<Soulmate> {
        protected SoulmateTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 1771) {
        }

        public static SoulmateTable Create(IByteData data, string name, string resourceFile, int address)
            => CreateBase(() => new SoulmateTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new Soulmate(Data, id, name, address));
    }
}
