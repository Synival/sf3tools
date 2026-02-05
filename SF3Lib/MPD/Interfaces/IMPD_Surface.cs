using CommonLib.SGL;
using SF3.Types;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Abstract representation of an MPD surface (tiled grid).
    /// </summary>
    public interface IMPD_Surface {
        /// <summary>
        /// Fetches an individual tile on the surface.
        /// </summary>
        IMPD_Tile GetTile(int x, int y);

        /// <summary>
        /// Fetches all tiles as a 1D array in row-major order.
        /// </summary>
        /// <returns></returns>
        IMPD_Tile[] GetAllTiles();

        /// <summary>
        /// Sets the normal for vertex on the surface model.
        /// </summary>
        /// <param name="vx">Vertex X</param>
        /// <param name="vy">Vertex Y</param>
        /// <param name="normal">Normal to set</param>
        void SetVertexNormal(int vx, int vy, VECTOR normal);

        /// <summary>
        /// Gets the normal for vertex on the surface model.
        /// </summary>
        /// <param name="vx">Vertex X</param>
        /// <param name="vy">Vertex Y</param>
        VECTOR GetVertexNormal(int vx, int vy);

        /// <summary>
        /// Updates a vertex normal.
        /// </summary>
        /// <param name="vx">X coordinate of the vertex to update.</param>
        /// <param name="vy">Y coordinate of the vertex to update.</param>
        void UpdateVertexNormal(int vx, int vy);

        /// <summary>
        /// Updates all vertex normals in a range.
        /// </summary>
        /// <param name="vx1">Lowest X coordinate of the vertices to update.</param>
        /// <param name="vy1">Lowest Y coordinate of the vertices to update.</param>
        /// <param name="vx2">Highest X coordinate of the vertices to update.</param>
        /// <param name="vy2">Highest Y coordinate of the vertices to update.</param>
        void UpdateVertexNormals(int vx1, int vy1, int vx2, int vy2);

        /// <summary>
        /// The width of the surface (in tiles).
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The height of the surface (in tiles).
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Settings to use when calculating normals for tile model vertices.
        /// </summary>
        NormalCalculationSettings NormalSettings { get; set; }

        /// <summary>
        /// When true, there is a render surface model as well as height + event data.
        /// </summary>
        bool HasModel { get; }

        /// <summary>
        /// When true, the textures in the surface model can be rotated, which is a Scenario 3+ feature that must be enabled.
        /// </summary>
        bool HasRotatableTextures { get; }
    }
}
