using SF3.Models.Structs.Shared;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.Shared {
    public class TileMovementTable : Table<TileMovement> {
        public TileMovementTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new TileMovement(Data, id, name, address));

        public override int? MaxSize => 31;
    }
}
