using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CommonLib.Utils;
using SF3.Types;

namespace SF3 {
    public class TextureABGR1555 : ITexture {
        public TextureABGR1555(int id, int frame, int duration, ushort[,] data, Dictionary<TagKey, TagValue> tags = null, string hashPrefix = "") {
            ID = id;
            Frame = frame;
            Duration = duration;

            _data = data;
            Tags = (tags == null) ? new Dictionary<TagKey, TagValue>() : tags.ToDictionary(x => x.Key, x => x.Value);

            _bitmapDataARGB1555 = BitmapUtils.ConvertABGR1555DataToABGR1555BitmapData(data);
            _bitmapDataARGB8888 = BitmapUtils.ConvertABGR1555DataToABGR8888BitmapData(data);

            using (var md5 = MD5.Create())
                Hash = (hashPrefix == "" ? "" : (hashPrefix + "-")) + BitConverter.ToString(md5.ComputeHash(_bitmapDataARGB1555)).Replace("-", "").ToLower();
        }

        private readonly ushort[,] _data;
        private readonly byte[] _bitmapDataARGB1555;
        private readonly byte[] _bitmapDataARGB8888;

        public int ID { get; }
        public int Frame { get; }
        public int Duration { get; }

        public int Width => _data.GetLength(0);
        public int Height => _data.GetLength(1);
        public int BytesPerPixel => 2;
        public TexturePixelFormat PixelFormat => TexturePixelFormat.ABGR1555;

        public byte[,] ImageData8Bit => throw new NotSupportedException();
        public ushort[,] ImageData16Bit => (ushort[,]) _data.Clone();

        public byte[] BitmapDataARGB1555 => (byte[]) _bitmapDataARGB1555.Clone();
        public byte[] BitmapDataARGB8888 => (byte[]) _bitmapDataARGB8888.Clone();

        public string Hash { get; }
        public Dictionary<TagKey, TagValue> Tags { get; }
    }
}
