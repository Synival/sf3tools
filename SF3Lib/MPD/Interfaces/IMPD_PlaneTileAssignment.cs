namespace SF3.MPD.Interfaces {
    public interface IMPD_PlaneTileAssignment {
        /// <summary>
        /// Width of the tile assignment table.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of the tile assignment table.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets or sets the tileset coordinate for a specific coordinate in the assignment map.
        /// </summary>
        /// <param name="x">X coordinate of the tile assignment.</param>
        /// <param name="y">Y coordinate of the tile assignment.</param>
        /// <returns>A coordinate of the tile from the tileset to use.</returns>
        (byte X, byte Y) this[byte x, byte y] { get; set; }
    }
}
