using SF3.MPD.Interfaces;

namespace SF3.Models.Files.MPD {
    public abstract class MPD_PlaneTileAssignmentBase : IMPD_PlaneTileAssignment {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract (byte X, byte Y) this[byte x, byte y] { get; set; }

        protected (byte X, byte Y) ValueToCoords(ushort value)
            => ((byte) ((value & 0x7E) >> 1), (byte) ((value & 0xFF80) >> 7));

        protected ushort CoordsToValue(byte x, byte y)
            => (ushort) (((x & 0x3F) << 1) | (y << 7));
    }
}
