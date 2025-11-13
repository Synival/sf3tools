using System.Drawing;
using System.Windows.Forms;

namespace CommonLib.Win.DarkMode {
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
        public override Color ToolStripBorder => DarkModeColors.BorderColor;

        // Checked tool strip buttons
        public override Color ButtonCheckedGradientBegin => DarkModeColors.StrongHighlightedBackColor;
        public override Color ButtonCheckedGradientEnd => DarkModeColors.StrongHighlightedBackColor;
        public override Color ButtonCheckedGradientMiddle => DarkModeColors.StrongHighlightedBackColor;
        public override Color ButtonCheckedHighlight => DarkModeColors.StrongHighlightedBackColor;

        // Hover over checked tool stip buttons / pressed buttons
        public override Color ButtonPressedGradientBegin => DarkModeColors.VeryStrongHighlightedBackColor;
        public override Color ButtonPressedGradientEnd => DarkModeColors.VeryStrongHighlightedBackColor;
        public override Color ButtonPressedGradientMiddle => DarkModeColors.VeryStrongHighlightedBackColor;
        public override Color ButtonPressedBorder => DarkModeColors.VeryStrongHighlightedBackColor;

        // Hover over unchecked tool strip buttons
        public override Color ButtonSelectedGradientBegin => DarkModeColors.HalfHighlightedBackColor;
        public override Color ButtonSelectedGradientEnd => DarkModeColors.HalfHighlightedBackColor;
        public override Color ButtonSelectedGradientMiddle => DarkModeColors.HalfHighlightedBackColor;
        public override Color ButtonSelectedHighlight => DarkModeColors.HalfHighlightedBackColor;
        public override Color ButtonSelectedHighlightBorder => DarkModeColors.HalfHighlightedBackColor;

        // Misc. colors
        public override Color GripLight => DarkModeColors.BorderColor;
        public override Color GripDark => Utils.MathHelpers.Lerp(DarkModeColors.BackColor, GripLight, 0.50f);
    };
}
