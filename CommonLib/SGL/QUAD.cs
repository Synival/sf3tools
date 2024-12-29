using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.SGL {
    public class QUAD {
        public QUAD(VECTOR[] vertices) {
            if (vertices.Length != 4)
                throw new ArgumentException(nameof(vertices));
            Vertices = vertices;
        }

        public VECTOR GetCornerNormal(int corner) {
            return VECTOR.Cross(
                Vertices[(2 + corner) % 4] - Vertices[(0 + corner) % 4],
                Vertices[(1 + corner) % 4] - Vertices[(0 + corner) % 4]
            ).Normalized();
        }

        public VECTOR GetNormal(bool useMoreAccurateMath) {
            if (!useMoreAccurateMath)
                return GetCornerNormal(0);

            var vertexNormals = new VECTOR[] {
                GetCornerNormal(0),
                GetCornerNormal(1),
                GetCornerNormal(2),
                GetCornerNormal(3),
            };

            return vertexNormals.Aggregate((a, b) => a + b).Normalized();
        }

        public VECTOR GetAbnormal(bool useMoreAccurateMath) {
            var quadNormal = GetNormal(useMoreAccurateMath);
            return quadNormal.GetAbnormalFromNormal();
        }

        VECTOR[] Vertices { get; }
    }
}
