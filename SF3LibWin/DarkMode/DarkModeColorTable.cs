using System.Drawing;
using System.Windows.Forms;

namespace SF3.Win.DarkMode {
    // Color table used by dark mode for tool strips and menu strips.
    public class DarkModeColorTable : ProfessionalColorTable {
        // Background colors
        public override Color ToolStripDropDownBackground => DarkModeColors.BackColor;
        public override Color ImageMarginGradientBegin  => DarkModeColors.ImageMarginColor;
        public override Color ImageMarginGradientMiddle => DarkModeColors.ImageMarginColor;
        public override Color ImageMarginGradientEnd    => DarkModeColors.ImageMarginColor;
        public override Color CheckBackground => DarkModeColors.HalfHighlightedBackColor;

        // Selected colors
        public override Color MenuItemPressedGradientBegin => DarkModeColors.HighlightedBackColor;
        public override Color MenuItemPressedGradientMiddle => DarkModeColors.HighlightedBackColor;
        public override Color MenuItemPressedGradientEnd => DarkModeColors.HighlightedBackColor;
        public override Color ButtonCheckedHighlightBorder => DarkModeColors.HighlightedBackColor;
        public override Color ButtonSelectedBorder => DarkModeColors.BorderColor;
        public override Color CheckSelectedBackground => DarkModeColors.HighlightedBackColor;

        // Highlighted/mouseover colors
        public override Color MenuItemSelectedGradientBegin => DarkModeColors.HalfHighlightedBackColor;
        public override Color MenuItemSelectedGradientEnd => DarkModeColors.HalfHighlightedBackColor;

        // Border colors
        public override Color MenuBorder => DarkModeColors.BorderColor;
        public override Color MenuItemBorder => DarkModeColors.BorderColor;
    };
}
