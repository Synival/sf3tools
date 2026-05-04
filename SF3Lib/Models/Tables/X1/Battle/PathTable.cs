using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class PathTable : FixedSizeTable<Path> {
        protected PathTable(IByteData data, string name, int address) : base(data, name, address, 32) {
        }

        public static PathTable Create(IByteData data, string name, int address)
            => Create(() => new PathTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new Path(Data, id, $"{nameof(Path)}{id:D2}", address));
    }
}
