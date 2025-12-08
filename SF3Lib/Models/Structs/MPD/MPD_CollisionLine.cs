using System;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables.MPD.Model;
using SF3.MPD;

namespace SF3.Models.Structs.MPD {
    public class MPD_CollisionLine : IMPD_CollisionLine {
        public MPD_CollisionLine(CollisionLine line, CollisionPointTable pointTable) {
            Line = line;
            PointTable = pointTable;
        }

        public short X1 {
            get => Point1.X;
            set => Point1.X = value;
        }

        public short Y1 {
            get => Point1.Y;
            set => Point1.Y = value;
        }

        public short X2 {
            get => Point2.X;
            set => Point2.X = value;
        }

        public short Y2 {
            get => Point2.Y;
            set => Point2.Y = value;
        }

        public CollisionLine Line { get; }
        public CollisionPointTable PointTable { get; }

        public CollisionPoint Point1 {
            get => PointTable[Line.Point1Index];
            set => Line.Point1Index = (ushort) value.ID;
        }

        public CollisionPoint Point2 {
            get => PointTable[Line.Point2Index];
            set => Line.Point2Index = (ushort) value.ID;
        }
    }
}
