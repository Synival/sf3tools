using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.Types;

namespace SF3 {
    public class TextureIndexed : ITexture {
        public TextureIndexed(int id, int frame, int duration, byte[,] data, TexturePixelFormat format = TexturePixelFormat.UnknownPalette, Dictionary<TagKey, TagValue> tags = null, string hashPrefix = "") {
            ID = id;
            Frame = frame;
            Duration = duration;

            _data = data;
            Tags = (tags == null) ? new Dictionary<TagKey, TagValue>() : tags.ToDictionary(x => x.Key, x => x.Value);
            PixelFormat = format;

            _bitmapDataARGB1555 = BitmapUtils.ConvertIndexedDataToABGR1555BitmapData(_data);
            _bitmapDataARGB8888 = BitmapUtils.ConvertIndexedDataToABGR8888BitmapData(_data);

            using (var md5 = MD5.Create())
                Hash = (hashPrefix == "" ? "" : (hashPrefix + "-")) + BitConverter.ToString(md5.ComputeHash(data.To1DArray())).Replace("-", "").ToLower();
        }

        private readonly byte[,] _data;
        private readonly byte[] _bitmapDataARGB1555;
        private readonly byte[] _bitmapDataARGB8888;

        public int ID { get; }
        public int Frame { get; }
        public int Duration { get; }

        public int Width => _data.GetLength(0);
        public int Height => _data.GetLength(1);
        public int BytesPerPixel => 1;
        public TexturePixelFormat PixelFormat { get; }

        public byte[,] ImageData8Bit => (byte[,]) _data.Clone();
        public ushort[,] ImageData16Bit => throw new NotSupportedException();

        public byte[] BitmapDataARGB1555 => (byte[]) _bitmapDataARGB1555.Clone();
        public byte[] BitmapDataARGB8888 => (byte[]) _bitmapDataARGB8888.Clone();

        public string Hash { get; }
        public Dictionary<TagKey, TagValue> Tags { get; }
    }
}
