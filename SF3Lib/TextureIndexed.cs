using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CommonLib.Extensions;
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

            using (var md5 = MD5.Create())
                Hash = (hashPrefix == "" ? "" : (hashPrefix + "-")) + BitConverter.ToString(md5.ComputeHash(data.To1DArray())).Replace("-", "").ToLower();
        }

        private readonly byte[,] _data;

        public int ID { get; }
        public int Frame { get; }
        public int Duration { get; }

        public int Width => _data.GetLength(0);
        public int Height => _data.GetLength(1);
        public int BytesPerPixel => 1;
        public TexturePixelFormat PixelFormat { get; }

        public byte[,] ImageData8Bit => (byte[,]) _data.Clone();
        public byte[] BitmapDataIndexed => _data.To1DArrayTransposed();

        public ushort[,] ImageData16Bit => throw new NotSupportedException();
        public byte[] BitmapDataARGB1555 => throw new NotSupportedException();

        public string Hash { get; }
        public Dictionary<TagKey, TagValue> Tags { get; }
    }
}
