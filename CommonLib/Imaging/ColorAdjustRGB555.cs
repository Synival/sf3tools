namespace CommonLib.Imaging {
    public class ColorAdjustRGB555 : IColorAdjustRGB555 {
        public ColorAdjustRGB555(IColorAdjustRGB555 original) {
            R = original.R;
            G = original.G;
            B = original.B;
        }

        public sbyte R { get; set; }
        public sbyte G { get; set; }
        public sbyte B { get; set; }
    }
}
