using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.Types;

namespace SF3 {
    public class TextureABGR1555 : ITexture {
        public TextureABGR1555(
            CollectionType collection,
            int id,
            int frame,
            int duration,
            ushort[,] data,
            Dictionary<TagKey, TagValue> tags = null,
            string hashPrefix = ""
        ) {
            Collection  = collection;
            ID          = id;
            Frame       = frame;
            Duration    = duration;
            _data       = data;
            _hashPrefix = hashPrefix;

            Tags = (tags == null) ? new Dictionary<TagKey, TagValue>() : tags.ToDictionary(x => x.Key, x => x.Value);
        }

        private readonly ushort[,] _data;
        private byte[] _bitmapDataARGB1555 = null;
        private byte[] _bitmapDataARGB1555_Endcodes = null;
        private byte[] _bitmapDataARGB8888 = null;
        private byte[] _bitmapDataARGB8888_Endcodes = null;

        public CollectionType Collection { get; }
        public int ID { get; }
        public int Frame { get; }
        public int Duration { get; }

        public int Width => _data.GetLength(0);
        public int Height => _data.GetLength(1);
        public int BytesPerPixel => 2;
        public TexturePixelFormat PixelFormat => TexturePixelFormat.ABGR1555;

        public byte[,] ImageData8Bit => throw new NotSupportedException();
        public ushort[,] ImageData16Bit => (ushort[,]) _data.Clone();

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(false);

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) {
            if (highlightEndcodes) {
                if (_bitmapDataARGB1555_Endcodes == null)
                    _bitmapDataARGB1555_Endcodes = BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(_data, true);
                return _bitmapDataARGB1555_Endcodes;
            }
            else {
                if (_bitmapDataARGB1555 == null)
                    _bitmapDataARGB1555 = BitmapUtils.ConvertABGR1555DataToARGB1555BitmapData(_data, false);
                return _bitmapDataARGB1555;
            }
        }

        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(false);

        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) {
            if (highlightEndcodes) {
                if (_bitmapDataARGB8888_Endcodes == null)
                    _bitmapDataARGB8888_Endcodes = BitmapUtils.ConvertABGR1555DataToARGB8888BitmapData(_data, true);
                return _bitmapDataARGB8888_Endcodes;
            }
            else {
                if (_bitmapDataARGB8888 == null)
                    _bitmapDataARGB8888 = BitmapUtils.ConvertABGR1555DataToARGB8888BitmapData(_data, false);
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

        public Palette Palette {
            get => null;
            set => throw new InvalidOperationException();
        }
    }
}
