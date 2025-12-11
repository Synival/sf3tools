using System.Collections.Generic;

namespace SF3.MPD {
    /// <summary>
    /// A single line connected by two MPD_CollisionPoint's in an MPD.
    /// </summary>
    public class MPD_CollisionLine : IMPD_CollisionLine {
        public MPD_CollisionLine(IMPD_CollisionPoint point1, IMPD_CollisionPoint point2) {
            // TODO: Enforce non-null assignment.
            Point1 = point1;
            Point2 = point2;
        }

        // TODO: Enforce non-null assignment.
        public IMPD_CollisionPoint Point1 { get; set; }
        // TODO: Enforce non-null assignment.
        public IMPD_CollisionPoint Point2 { get; set; }

        public short X1 {
            get => Point1?.X ?? 0;
            set {
                var point = Point1;
                if (point != null)
                    point.X = value;
            }
        }

        public short Y1 {
            get => Point1?.Y ?? 0;
            set {
                var point = Point1;
                if (point != null)
                    point.Y = value;
            }
        }

        public short X2 {
            get => Point2?.X ?? 0;
            set {
                var point = Point2;
                if (point != null)
                    point.X = value;
            }
        }

        public short Y2 {
            get => Point2?.Y ?? 0;
            set {
                var point = Point2;
                if (point != null)
                    point.Y = value;
            }
        }

        // TODO: Calculate angle properly
        public float Angle => 0.00f;

        // TODO: Restict between 0x201 and 0x2FF (inclusive).
        public int? FlagToDisable { get; set; }

        public byte Tag { get; set; }

        // TODO: Actually calculate this!
        public IEnumerable<(int X, int Y)> GetReferencingBlocks() => new (int X, int Y)[0];
    }
}
