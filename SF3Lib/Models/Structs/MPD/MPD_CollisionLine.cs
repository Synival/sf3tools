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

        public override string ToString()
            => $"({X1,4}, {Y1,4}), ({X2,4}, {Y2,4}) (Angle={Angle,7:0.00}) (Unknown={Tag,2})" + (Flag2XXToDisable > 0 ? $" (Flag={0x200 + Flag2XXToDisable:X2})" : "");

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

        public float Angle {
            get => Line.Angle;
            set => Line.Angle = value;
        }

        public byte Flag2XXToDisable {
            get => Line.IfFlagIn2XXOff;
            set => Line.IfFlagIn2XXOff = value;
        }

        public byte Tag {
            get => Line.Tag;
            set => Line.Tag = value;
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
