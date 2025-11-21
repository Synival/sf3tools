namespace SF3.MPD {
    public interface IMPD_Tile {
        /// <summary>
        /// Flipping, rotation, and "is flat" tile flags.
        /// </summary>
        byte ModelTextureFlags { get; set; }

        /// <summary>
        /// Texture ID for this paticular tile. Set to 0xFF for an empty tile.
        /// </summary>
        byte ModelTextureID { get; set; }
    }
}
