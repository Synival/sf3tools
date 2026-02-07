using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_Collisions : IMPD_Collisions {
        public MPD_Collisions(IMPD_Collisions original) {
            if (original.Points != null)
                _points = original.Points.Select(x => (IMPD_CollisionPoint) new MPD_CollisionPoint(x)).ToList();
            if (original.Lines != null)
                _lines = original.Lines.Select(x => (IMPD_CollisionLine) new MPD_CollisionLine(x, _points)).ToList();
        }

        public static MPD_Collisions FromJToken(JToken token) => new MPD_Collisions(token);
        private MPD_Collisions(JToken token) {
            var jObject = (JObject) token;

            _points = jObject.GetValueIfExists("Points", x => ((JArray) x).Select(y => (IMPD_CollisionPoint) MPD_CollisionPoint.FromJToken(y)).ToList());
            var pointsById = _points.ToDictionary(x => x.ID, x => x);

            _lines = jObject.GetValueIfExists("Lines",  x => ((JArray) x).Select(y => (IMPD_CollisionLine)  MPD_CollisionLine.FromJToken(y, pointsById)).ToList());
        }

        List<IMPD_CollisionPoint> _points { get; }
        public IEnumerable<IMPD_CollisionPoint> Points => _points;

        List<IMPD_CollisionLine> _lines { get; }
        public IEnumerable<IMPD_CollisionLine> Lines => _lines;
    }
}
