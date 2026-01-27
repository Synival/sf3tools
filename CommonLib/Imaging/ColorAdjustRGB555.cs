namespace CommonLib.Imaging {
    public class ColorAdjustRGB555 : IColorAdjustRGB555 {
        public ColorAdjustRGB555() { }

        public ColorAdjustRGB555(IColorAdjustRGB555 original) {
            R = original.R;
            G = original.G;
            B = original.B;
        }

        public ColorAdjustRGB555(sbyte r, sbyte g, sbyte b) {
            R = r;
            G = g;
            B = b;
        }

        public sbyte R { get; set; }
        public sbyte G { get; set; }
        public sbyte B { get; set; }
    }
}
