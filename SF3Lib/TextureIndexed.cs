using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.Types;

namespace SF3 {
    public class TextureIndexed : ITexture {
        public TextureIndexed(int id, int frame, int duration, byte[,] data,
            TexturePixelFormat format, Palette palette, bool zeroIsTransparent, Dictionary<TagKey, TagValue> tags = null, string hashPrefix = ""
        ) {
            ID          = id;
            Frame       = frame;
            Duration    = duration;
            _data       = data;
            PixelFormat = format;
            Palette     = palette;
            ZeroIsTransparent = zeroIsTransparent;
            Tags        = (tags == null) ? new Dictionary<TagKey, TagValue>() : tags.ToDictionary(x => x.Key, x => x.Value);
            _hashPrefix = hashPrefix;
        }

        private readonly byte[,] _data;
        private byte[] _bitmapDataARGB1555 = null;
        private byte[] _bitmapDataARGB8888 = null;

        public int ID { get; }
        public int Frame { get; }
        public int Duration { get; }

        public int Width => _data.GetLength(0);
        public int Height => _data.GetLength(1);
        public int BytesPerPixel => 1;
        public TexturePixelFormat PixelFormat { get; }

        public byte[,] ImageData8Bit => _data;
        public ushort[,] ImageData16Bit => throw new NotSupportedException();

        public byte[] BitmapDataARGB1555 {
            get {
                if (_bitmapDataARGB1555 == null)
                    _bitmapDataARGB1555 = BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(_data, Palette, ZeroIsTransparent);
                return _bitmapDataARGB1555;
            }
        }

        public byte[] BitmapDataARGB8888 {
            get {
                if (_bitmapDataARGB8888 == null)
                    _bitmapDataARGB8888 = BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(_data, Palette, ZeroIsTransparent);
                return _bitmapDataARGB8888;
            }
        }

        private string _hash = null;
        private readonly string _hashPrefix;
        public string Hash {
            get {
                if (_hash == null) {
                    using (var md5 = MD5.Create())
                        _hash = (_hashPrefix == "" ? "" : (_hashPrefix + "-")) + BitConverter.ToString(md5.ComputeHash(BitmapDataARGB1555)).Replace("-", "").ToLower();
                }
                return _hash;
            }
        }

        public Dictionary<TagKey, TagValue> Tags { get; }

        private Palette _palette = null;
        public Palette Palette {
            get => _palette;
            set {
                if (value != _palette) {
                    _hash = null;
                    _bitmapDataARGB1555 = null;
                    _bitmapDataARGB8888 = null;
                    _palette = value;
                }
            }
        }

        public bool ZeroIsTransparent { get; }
    }
}
