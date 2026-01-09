namespace SF3.MPD {
    /// <summary>
    /// Interface for settings to adjust the game's lighting palette, ground palette, and shadow transparency.
    /// Availability depends on scenario. Ground adjustment is not guaranteed to be applied to all maps; it must
    /// be explicitly programmed into the X1???.BIN file.
    /// </summary>
    public interface IMPD_PaletteAdjustment {
        /// <summary>
        /// When true, the light palette can be adjusted. Only present in Scenario 2+.
        /// </summary>
        bool HasLightAdjustment { get; }

        /// <summary>
        /// When true, the ground palette can be adjusted. Only present in Scenario 3+ and will only selectively
        /// be applied.
        /// </summary>
        bool HasGroundAdjustment { get; }

        /// <summary>
        /// When true, models rendered as transparency (applied via the (decimal) 2000 tag) can have their transparency
        /// amount adjusted.
        /// </summary>
        bool HasShadowTransparency { get; }

        /// <summary>
        /// Red component to add to the light palette. Range is (-0x1F ... +0x1F).
        /// </summary>
        short LightRAdjustment { get; set; }

        /// <summary>
        /// Green component to add to the light palette. Range is (-0x1F ... +0x1F).
        /// </summary>
        short LightGAdjustment { get; set; }

        /// <summary>
        /// Blue component to add to the light palette. Range is (-0x1F ... +0x1F).
        /// </summary>
        short LightBAdjustment { get; set; }

        /// <summary>
        /// Red component to add to the ground palette. Range is (-0x1F ... +0x1F).
        /// </summary>
        short GroundRAdjustment { get; set; }

        /// <summary>
        /// Green component to add to the ground palette. Range is (-0x1F ... +0x1F).
        /// </summary>
        short GroundGAdjustment { get; set; }

        /// <summary>
        /// Blue component to add to the ground palette. Range is (-0x1F ... +0x1F).
        /// </summary>
        short GroundBAdjustment { get; set; }

        /// <summary>
        /// The transparency level of models rendered as transparent via the (decimal) 2000 tag.
        /// Range is (0x00 ... 0x1F).
        /// </summary>
        ushort ShadowTransparency { get; set; }
    }
}
