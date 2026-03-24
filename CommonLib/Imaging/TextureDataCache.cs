using System;

namespace CommonLib.Imaging {
    /// <summary>
    /// A set of buffers that cache important texture data.
    /// </summary>
    public class TextureDataCache {
        public string SetHash(string value) => _hash = value;
        public string GetOrCacheHash(Func<string> getter) => _hash ?? (_hash = getter());

        public byte[,] SetImageData8Bit(byte[,] value) => _imageData8Bit = value;
        public byte[,] GetOrCacheImageData8Bit(Func<byte[,]> getter) => _imageData8Bit ?? (_imageData8Bit = getter());

        public ushort[,] SetImageData16Bit(ushort[,] value) => _imageData16Bit = value;
        public ushort[,] GetOrCacheImageData16Bit(Func<ushort[,]> getter) => _imageData16Bit ?? (_imageData16Bit = getter());

        public byte[] SetBitmapDataARGB1555(byte[] value) => _bitmapDataARGB1555 = value;
        public byte[] GetOrCacheBitmapDataARGB1555(Func<byte[]> getter) => _bitmapDataARGB1555 ?? (_bitmapDataARGB1555 = getter());

        public byte[] SetBitmapDataARGB1555_Endcodes(byte[] value) => _bitmapDataARGB1555_Endcodes = value;
        public byte[] GetOrCacheBitmapDataARGB1555_Endcodes(Func<byte[]> getter) => _bitmapDataARGB1555_Endcodes ?? (_bitmapDataARGB1555_Endcodes = getter());

        public byte[] SetBitmapDataARGB8888(byte[] value) => _bitmapDataARGB8888 = value;
        public byte[] GetOrCacheBitmapDataARGB8888(Func<byte[]> getter) => _bitmapDataARGB8888 ?? (_bitmapDataARGB8888 = getter());

        public byte[] SetBitmapDataARGB8888_Endcodes(byte[] value) => _bitmapDataARGB8888_Endcodes = value;
        public byte[] GetOrCacheBitmapDataARGB8888_Endcodes(Func<byte[]> getter) => _bitmapDataARGB8888_Endcodes ?? (_bitmapDataARGB8888_Endcodes = getter());

        public void Invalidate() {
            _hash                        = null;
            _imageData8Bit               = null;
            _imageData16Bit              = null;
            _bitmapDataARGB1555          = null;
            _bitmapDataARGB1555_Endcodes = null;
            _bitmapDataARGB8888          = null;
            _bitmapDataARGB8888_Endcodes = null;
        }

        private string _hash                        = null;
        private byte[,] _imageData8Bit              = null;
        private ushort[,] _imageData16Bit           = null;
        private byte[] _bitmapDataARGB1555          = null;
        private byte[] _bitmapDataARGB1555_Endcodes = null;
        private byte[] _bitmapDataARGB8888          = null;
        private byte[] _bitmapDataARGB8888_Endcodes = null;
    }
}
