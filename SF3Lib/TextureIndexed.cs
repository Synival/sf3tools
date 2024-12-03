using System;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.Types;

namespace SF3 {
    public class TextureIndexed : ITexture {
        public TextureIndexed(byte[,] data, TexturePixelFormat format = TexturePixelFormat.UnknownPalette) {
            _data = data;
            PixelFormat = format;
        }

        private readonly byte[,] _data;

        public int Width => _data.GetLength(0);
        public int Height => _data.GetLength(1);
        public int BytesPerPixel => 1;
        public TexturePixelFormat PixelFormat { get; }

        public byte[,] ImageData8Bit => (byte[,]) _data.Clone();
        public byte[] BitmapDataIndexed => _data.To1DArray();

        public ushort[,] ImageData16Bit => throw new NotSupportedException();
        public byte[] BitmapDataARGB1555 => throw new NotSupportedException();
    }
}
