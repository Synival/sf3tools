using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class BoundaryTable : ResourceTable<Boundary> {
        protected BoundaryTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address) { }

        public static BoundaryTable Create(IByteData data, string name, string resourceFile, int address)
            => CreateBase(() => new BoundaryTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new Boundary(Data, id, name, address));
    }
}
