namespace CommonLib.Types {
    /// <summary>
    /// The calculations used for determining the normal of a POLYGON.
    /// </summary>
    public enum POLYGON_NormalCalculationMethod {
        /// <summary>
        /// (Shining Force 3 method) Only uses the top-right triangle.
        /// </summary>
        TopRightTriangle,

        /// <summary>
        /// Gathers all corner triangles for each corner, averages them, and normalizes them.
        /// </summary>
        AverageOfAllTriangles,

        /// <summary>
        /// Uses the corner triangle with the highest value for abs(Y).
        /// </summary>
        MostExtremeVerticalTriangle,

        /// <summary>
        /// Gathers all corner triangles, weights them by the 1.01 - abs(Y), and normalizes them.
        /// </summary>
        WeightedVerticalTriangles
    }
}
