using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    /// <summary>
    /// A single line connected by two MPD_CollisionPoint's in an MPD.
    /// </summary>
    public class MPD_CollisionLine : IMPD_CollisionLine {
        public MPD_CollisionLine(int id, IMPD_CollisionPoint point1, IMPD_CollisionPoint point2) {
            if (point1 == null)
                throw new ArgumentNullException(nameof(point1));
            if (point2 == null)
                throw new ArgumentNullException(nameof(point2));

            ID = id;
            Point1 = point1;
            Point2 = point2;
        }

        public MPD_CollisionLine(IMPD_CollisionLine original, IEnumerable<IMPD_CollisionPoint> points) {
            var point1id = original.Point1.ID;
            var point2id = original.Point2.ID;

            ID = original.ID;
            Point1 = points.First(x => x.ID == point1id);
            Point2 = points.First(x => x.ID == point2id);

            Angle         = original.Angle;
            FlagToDisable = original.FlagToDisable;
            Tag           = original.Tag;
        }

        public static MPD_CollisionLine FromJToken(JToken token, Dictionary<int, IMPD_CollisionPoint> pointsById) => new MPD_CollisionLine(token, pointsById);
        private MPD_CollisionLine(JToken token, Dictionary<int, IMPD_CollisionPoint> pointsById) {
            var jObject = (JObject) token;

            ID            = (int) jObject["ID"];
            Point1        = pointsById[(int) jObject["Point1ID"]];
            Point2        = pointsById[(int) jObject["Point2ID"]];
            Angle         = (float) jObject["Angle"];
            FlagToDisable = (int?) jObject["FlagToDisable"];
            Tag           = (byte) jObject["Tag"];
        }

        public int ID { get; }

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
        public float Angle { get; private set; }

        // TODO: Restict between 0x201 and 0x2FF (inclusive).
        public int? FlagToDisable { get; set; }

        public byte Tag { get; set; }

        // TODO: Actually calculate this!
        public IEnumerable<(int X, int Y)> GetReferencingBlocks() => new (int X, int Y)[0];
    }
}
