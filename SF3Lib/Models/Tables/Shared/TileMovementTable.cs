using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Types;
using static CommonLib.Utils.EnumHelpers;

namespace SF3.Models.Tables.Shared {
    public class TileMovementTable : FixedSizeTable<TileMovement> {
        protected TileMovementTable(IByteData data, string name, int address, bool hasExtra) : base(data, name, address, hasExtra ? 14 : 13) {
            HasExtra = hasExtra;
        }

        public static TileMovementTable Create(IByteData data, string name, int address, bool hasExtra)
            => CreateBase(() => new TileMovementTable(data, name, address, hasExtra));

        public override bool Load()
            => Load((id, address) => new TileMovement(Data, id, EnumNameOr((MovementType) id, x => $"Unknown0x{((int) x):X2}"), address));

        public bool HasExtra { get; }
    }
}
