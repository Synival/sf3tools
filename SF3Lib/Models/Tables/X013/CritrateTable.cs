using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class CritrateTable : ResourceTable<Critrate> {
        protected CritrateTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 3) {
        }

        public static CritrateTable Create(IByteData data, string name, string resourceFile, int address)
            => CreateBase(() => new CritrateTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new Critrate(Data, id, name, address));
    }
}
