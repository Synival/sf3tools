using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CommonLib.Utils;
using SF3.Types;

namespace SF3 {
    public class TextureABGR1555 : ITexture {
        public TextureABGR1555(int id, ushort[,] data, Dictionary<TagKey, TagValue> tags = null, string hashPrefix = "") {
            ID = id;
            _data = data;
            Tags = (tags == null) ? new Dictionary<TagKey, TagValue>() : tags.ToDictionary(x => x.Key, x => x.Value);
            _bitmapDataARGB1555 = BitmapUtils.ConvertABGR1555DataToABGR1555BitmapData(data);

            using (var md5 = MD5.Create())
                Hash = (hashPrefix == "" ? "" : (hashPrefix + "-")) + BitConverter.ToString(md5.ComputeHash(_bitmapDataARGB1555)).Replace("-", "").ToLower();
        }

        private readonly ushort[,] _data;
        private readonly byte[] _bitmapDataARGB1555;

        public int ID { get; }
        public int Width => _data.GetLength(0);
        public int Height => _data.GetLength(1);
        public int BytesPerPixel => 2;
        public TexturePixelFormat PixelFormat => TexturePixelFormat.ABGR1555;

        public ushort[,] ImageData16Bit => (ushort[,]) _data.Clone();
        public byte[] BitmapDataARGB1555 => _bitmapDataARGB1555;

        public byte[,] ImageData8Bit => throw new NotSupportedException();
        public byte[] BitmapDataIndexed => throw new NotSupportedException();

        public string Hash { get; }
        public Dictionary<TagKey, TagValue> Tags { get; }
    }
}
