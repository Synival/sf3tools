using System;
using SF3.Models.Tables.MPD.Plane;

namespace SF3.Models.Files.MPD {
    public class MPD_GroundPlaneTileAssignment : MPD_PlaneTileAssignmentBase {
        public MPD_GroundPlaneTileAssignment(PlaneTileTextureRowTable table1, PlaneTileTextureRowTable table2) {
            if (table1 == null || table2 == null)
                throw new ArgumentNullException();
            if (table1.Width != 256 || table2.Width != 256 || table1.Height != 128 || table2.Height != 128)
                throw new ArgumentException();
            Tables = new PlaneTileTextureRowTable[] { table1, table2 };
        }

        public PlaneTileTextureRowTable[] Tables { get; }

        public override int Width => 256;
        public override int Height => 256;

        public override (byte X, byte Y) this[byte x, byte y] {
            get => ValueToCoords(Tables[y / 128][x, y % 128]);
            set => Tables[y / 128][x, y % 128] = CoordsToValue(x, y);
        }
    }
}
