using System.Drawing;

namespace SF3.Win.DarkMode {
    /// <summary>
    /// All colors used when dark mode is enabled.
    /// </summary>
    public static class DarkModeColors {
        // Standad contol colors
        public static readonly Color BackColor = Color.FromArgb(0x20, 0x20, 0x20);
        public static readonly Color ForeColor = Color.FromArgb(0xC0, 0xC0, 0xC0);
        public static readonly Color DisabledColor = Utils.MathHelpers.Lerp(BackColor, ForeColor, 0.5f);
        public static readonly Color BorderColor = Color.FromArgb(0x40, 0x40, 0x40);

        // Highlighted colors
        public static readonly Color HighlightedBackColor = Color.FromArgb(0x60, 0x60, 0x60);
        public static readonly Color HighlightedForeColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
        public static readonly Color HighlightedDisabledColor = Utils.MathHelpers.Lerp(HighlightedBackColor, ForeColor, 0.5f);
        public static readonly Color HalfHighlightedBackColor = Utils.MathHelpers.Lerp(HighlightedBackColor, BackColor, 0.5f);
        public static readonly Color StrongHighlightedBackColor = Color.FromArgb(0x90, 0x90, 0x90);
        public static readonly Color VeryStrongHighlightedBackColor = Color.FromArgb(0xC0, 0xC0, 0xC0);

        // Misc. colors
        public static readonly Color ImageMarginColor = Color.FromArgb(0x28, 0x28, 0x28);
    }
}
