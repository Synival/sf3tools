using CommonLib.Types;

namespace CommonLib.SGL {
    public interface IPOLYGON {
        /// <summary>
        /// Calculates the normal for a corner.
        /// Assumes a quad in clockwise order starting with the bottom-right corner.
        /// </summary>
        /// <param name="corner">The corner whose value should be calculated.</param>
        /// <returns>A normalized vector representing a normal.</returns>
        VECTOR GetCornerNormal(CornerType corner);

        /// <summary>
        /// Calculates the normal for a vertex.
        /// </summary>
        /// <param name="vertex">The vertex whose value should be calculated.</param>
        /// <returns>A normalized vector representing a normal.</returns>
        VECTOR GetVertexNormal(int vertex);

        /// <summary>
        /// Calculates one of four components used in the calculation of normals for a shared vertex in a mesh.
        /// Assumes a quad in clockwise order starting with the bottom-right corner.
        /// </summary>
        /// <param name="vertexCorner">The corner of this quad that matches the vertex whose normals need calculation.</param>
        /// <param name="calculationMethod">The formula used for calculating this component.</param>
        /// <returns>A normalized vector representing a normal.</returns>
        VECTOR GetMeshNormalComponent(CornerType vertexCorner, POLYGON_NormalCalculationMethod calculationMethod);

        /// <summary>
        /// The four vertices of the quad.
        /// </summary>
        VECTOR[] Vertices { get; }
    }
}
