using SF3.Types;

namespace SF3.MPD {
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
