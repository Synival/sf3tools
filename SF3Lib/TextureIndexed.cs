using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.Types;

namespace SF3 {
    public class TextureIndexed : ITexture {
        public TextureIndexed(
            CollectionType collection,
            int id,
            int frame,
            int duration,
            byte[,] data,
            TexturePixelFormat format,
            Palette palette,
            bool zeroIsTransparent,
            Dictionary<TagKey, TagValue> tags = null,
            string hashPrefix = ""
        ) {
            Collection  = collection;
            ID          = id;
            Frame       = frame;
            Duration    = duration;
            _data       = data;
            PixelFormat = format;
            _palette    = palette;
            ZeroIsTransparent = zeroIsTransparent;
            Tags        = (tags == null) ? new Dictionary<TagKey, TagValue>() : tags.ToDictionary(x => x.Key, x => x.Value);
            _hashPrefix = hashPrefix;
        }

        private byte[,] _data;
        private byte[] _bitmapDataARGB1555 = null;
        private byte[] _bitmapDataARGB8888 = null;

        public CollectionType Collection { get; }
        public int ID { get; }
        public int Frame { get; }
        public int Duration { get; }

        public int Width => _data.GetLength(0);
        public int Height => _data.GetLength(1);
        public int BytesPerPixel => 1;
        public TexturePixelFormat PixelFormat { get; }

        public byte[,] ImageData8Bit => (byte[,]) _data.Clone();
        public void SetImageData8Bit(byte[,] data, Palette palette)  {
            if (!CanSetImageData16Bit)
                throw new NotSupportedException();
            if (_data == data)
                return;

            var error = Validate8BitImageData(data, palette);
            if (error != null)
                throw new ArgumentException(error);

            _data               = data;
            _palette            = palette;
            _hash               = null;
            _bitmapDataARGB8888 = null;
            _bitmapDataARGB1555 = null;
            ReplaceAction(_data, palette);
        }

        public ushort[,] ImageData16Bit {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public byte[] BitmapDataARGB1555 => GetBitmapDataARGB1555(false);

        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false /* unused */) {
            if (_bitmapDataARGB1555 == null)
                _bitmapDataARGB1555 = BitmapUtils.ConvertIndexedDataToARGB1555BitmapData(_data, Palette, ZeroIsTransparent);
            return _bitmapDataARGB1555;
        }

        public byte[] BitmapDataARGB8888 => GetBitmapDataARGB8888(false);

        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false /* unused */) {
            if (_bitmapDataARGB8888 == null)
                _bitmapDataARGB8888 = BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(_data, Palette, ZeroIsTransparent);
            return _bitmapDataARGB8888;
        }

        public string Validate8BitImageData(byte[,] data, Palette palette) => CanSetImageData8Bit ? ReplaceValidator(data, palette) : " Not supported";
        public string Validate16BitImageData(ushort[,] data) => "Not applicable; 8-bit images cannot be set with 16-bit ABGR data";

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
        public Palette Palette => _palette;

        public bool ZeroIsTransparent { get; }

        public virtual bool CanSetImageData8Bit => false;
        public bool CanSetImageData16Bit => false;

        /// <summary>
        /// Checker to see if 8-bit indexed data can be set/imported.
        /// </summary>
        /// <param name="data">Data to set.</param>
        /// <param name="palette">Palette to set. Set to 'null' to leave unchanged.</param>
        /// <returns>Returns 'null' if no error was detected, otherwise returns an error string.</returns>
        public virtual string ReplaceValidator(byte[,] data, Palette palette) => "Unimplemented";

        /// <summary>
        /// Action to perform when an the image data is set.
        /// <param name="data">Data to set.</param>
        /// <param name="palette">Palette to set. Set to 'null' to leave unchanged.</param>
        /// </summary>
        public virtual void ReplaceAction(byte[,] data, Palette palette) => throw new NotImplementedException();
    }
}
