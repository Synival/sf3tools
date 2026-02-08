using System;
using System.Linq;
using CommonLib.Types;

namespace CommonLib.SGL {
    public class POLYGON : IPOLYGON {
        public POLYGON(VECTOR[] vertices) {
            if (vertices.Length != 4)
                throw new ArgumentException(nameof(vertices));
            Vertices = vertices;
        }

        public VECTOR GetCornerNormal(CornerType corner) {
            // Swap order of 'corner' to match what's seen on surface meshes (BottomRight going clockwise).
            int vertex = (4 - (int) corner) % 4;
            return GetVertexNormal(vertex);
        }

        public VECTOR GetVertexNormal(int vertex) {
            return VECTOR.Cross(
                Vertices[(0 + vertex) % 4] - Vertices[(2 + vertex) % 4],
                Vertices[(3 + vertex) % 4] - Vertices[(2 + vertex) % 4]
            ).Normalized();
        }

        public VECTOR GetMeshNormalComponent(CornerType vertexCorner, POLYGON_NormalCalculationMethod calculationMethod) {
            // Shortcut for very common flat polygons.
            var height = Vertices[0].Y;
            if (Vertices[1].Y == height && Vertices[2].Y == height && Vertices[3].Y == height)
                return new VECTOR(0, -1, 0);

            switch (calculationMethod) {
                case POLYGON_NormalCalculationMethod.TopRightTriangle:
                    return GetCornerNormal(CornerType.TopRight);

                case POLYGON_NormalCalculationMethod.AdjacentTriangles:
                    return GetCornerNormal(vertexCorner);

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
