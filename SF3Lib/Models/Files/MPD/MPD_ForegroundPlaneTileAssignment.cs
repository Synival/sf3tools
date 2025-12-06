using System;
using SF3.Models.Tables.MPD.Plane;

namespace SF3.Models.Files.MPD {
    public class MPD_ForegroundPlaneTileAssignment : MPD_PlaneTileAssignmentBase {
        public MPD_ForegroundPlaneTileAssignment(PlaneTileTextureRowTable table) {
            if (table == null)
                throw new ArgumentNullException();
            if (table.Width != 64 || table.Height != 32)
                throw new ArgumentException();
            Table = table;
        }

        public PlaneTileTextureRowTable Table { get; }

        public override int Width => 64;
        public override int Height => 32;

        public override (byte X, byte Y) this[byte x, byte y] {
            get => ValueToCoords(Table[x, y]);
            set => Table[x, y] = CoordsToValue(x, y);
        }
    }
}
