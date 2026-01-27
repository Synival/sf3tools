using System.Collections.Generic;
using System.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_Collisions : IMPD_Collisions {
        public MPD_Collisions(IMPD_Collisions original) {
            if (original.Points != null)
                _points = original.Points.Select(x => (IMPD_CollisionPoint) new MPD_CollisionPoint(x)).ToList();
            if (original.Lines != null)
                _lines = original.Lines.Select(x => (IMPD_CollisionLine) new MPD_CollisionLine(x, _points)).ToList();
        }

        List<IMPD_CollisionPoint> _points { get; }
        public IEnumerable<IMPD_CollisionPoint> Points => _points;

        List<IMPD_CollisionLine> _lines { get; }
        public IEnumerable<IMPD_CollisionLine> Lines => _lines;
    }
}
