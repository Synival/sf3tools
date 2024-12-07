﻿using System;
using CommonLib.Utils;
using SF3.Types;

namespace SF3 {
    public class TextureABGR1555 : ITexture {
        public TextureABGR1555(ushort[,] data) {
            _data = data;
            _bitmapDataARGB1555 = BitmapUtils.ConvertABGR1555DataToABGR1555BitmapData(data);
        }

        private readonly ushort[,] _data;
        private readonly byte[] _bitmapDataARGB1555;

        public int Width => _data.GetLength(0);
        public int Height => _data.GetLength(1);
        public int BytesPerPixel => 2;
        public TexturePixelFormat PixelFormat => TexturePixelFormat.ABGR1555;

        public ushort[,] ImageData16Bit => (ushort[,]) _data.Clone();
        public byte[] BitmapDataARGB1555 => _bitmapDataARGB1555;

        public byte[,] ImageData8Bit => throw new NotSupportedException();
        public byte[] BitmapDataIndexed => throw new NotSupportedException();
    }
}