using System;
using System.Linq;
using CommonLib.Types;

namespace CommonLib.SGL {
    public class POLYGON {
        public POLYGON(VECTOR[] vertices) {
            if (vertices.Length != 4)
                throw new ArgumentException(nameof(vertices));
            Vertices = vertices;
        }

        public VECTOR GetCornerNormal(CornerType corner) {
            return VECTOR.Cross(
                Vertices[(2 + (int) corner) % 4] - Vertices[(0 + (int) corner) % 4],
                Vertices[(1 + (int) corner) % 4] - Vertices[(0 + (int) corner) % 4]
            ).Normalized();
        }

        public VECTOR GetNormal(bool useMoreAccurateMath) {
            if (!useMoreAccurateMath)
                return GetCornerNormal(0);

            var vertexNormals = new VECTOR[] {
                GetCornerNormal(CornerType.TopLeft),
                GetCornerNormal(CornerType.TopRight),
                GetCornerNormal(CornerType.BottomRight),
                GetCornerNormal(CornerType.BottomLeft),
            };

            // Return the normal that will produce the most extreme lighting!
            return vertexNormals.OrderBy(x => Math.Abs(x.Y.Float)).First();
        }

        public VECTOR GetAbnormal(bool useMoreAccurateMath) {
            var quadNormal = GetNormal(useMoreAccurateMath);
            return quadNormal.GetAbnormalFromNormal();
        }

        VECTOR[] Vertices { get; }
    }
}
