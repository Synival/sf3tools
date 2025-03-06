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
            // Not sure why the components need to be flipped in every permutation of this function,
            // but that's how it appears to be...
            int cornerV = (int) corner;
            var vec = VECTOR.Cross(
                Vertices[(3 + cornerV) % 4] - Vertices[(0 + cornerV) % 4],
                Vertices[(1 + cornerV) % 4] - Vertices[(0 + cornerV) % 4]
            ).Normalized();
            vec.X = -vec.X;
            return vec;
        }

        public VECTOR GetNormal(POLYGON_NormalCalculationMethod calculationMethod) {
            // Shortcut for very common flat polygons.
            var height = Vertices[0].Y;
            if (Vertices.Skip(1).All(x => x.Y == height))
                return new VECTOR(0, -1, 0);

            switch (calculationMethod) {
                case POLYGON_NormalCalculationMethod.TopRightTriangle:
                    return GetCornerNormal(CornerType.TopRight);

                case POLYGON_NormalCalculationMethod.AverageOfAllTriangles:
                case POLYGON_NormalCalculationMethod.MostExtremeVerticalTriangle:
                case POLYGON_NormalCalculationMethod.WeightedVerticalTriangles:
                    var vertexNormals = ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                        .Select(c => GetCornerNormal(c)).ToArray();

                    switch (calculationMethod) {
                        case POLYGON_NormalCalculationMethod.AverageOfAllTriangles:
                            return vertexNormals.Aggregate((a, b) => a + b).Normalized();

                        case POLYGON_NormalCalculationMethod.MostExtremeVerticalTriangle:
                            return vertexNormals.OrderBy(x => 1.05f - Math.Abs(x.Y.Float)).First();

                        case POLYGON_NormalCalculationMethod.WeightedVerticalTriangles:
                            return vertexNormals
                                .Select(x => x * (1.05f - Math.Abs(x.Y.Float)))
                                .Aggregate((a, b) => a + b)
                                .Normalized();

                        default:
                            throw new NotImplementedException("Unreachable code!");
                    }

                default:
                    throw new ArgumentException(nameof(calculationMethod));
            }
        }

        public VECTOR[] Vertices { get; }
    }
}
