namespace SF3.Images {
    public class TextureDataBuffer {
        public string Hash { get; set; }
        public byte[,] ImageData8Bit { get; set; }
        public ushort[,] ImageData16Bit { get; set; }
        public byte[] BitmapDataARGB1555 { get; set; }
        public byte[] BitmapDataARGB1555_Endcodes { get; set; }
        public byte[] BitmapDataARGB8888 { get; set; }
        public byte[] BitmapDataARGB8888_Endcodes { get; set; }

        public void Invalidate() {
            Hash                        = null;
            ImageData8Bit               = null;
            ImageData16Bit              = null;
            BitmapDataARGB1555          = null;
            BitmapDataARGB1555_Endcodes = null;
            BitmapDataARGB8888          = null;
            BitmapDataARGB8888_Endcodes = null;
        }
    }
}
