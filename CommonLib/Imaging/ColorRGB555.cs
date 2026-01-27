namespace CommonLib.Imaging {
    public class ColorRGB555 : IColorRGB555 {
        public ColorRGB555() { }

        public ColorRGB555(IColorRGB555 original) {
            R = original.R;
            G = original.G;
            B = original.B;
        }

        public ColorRGB555(byte r, byte g, byte b) {
            R = r;
            G = g;
            B = b;
        }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }
}
