using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.Types;

namespace SF3.Imaging {
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

            Tags = tags == null ? new Dictionary<TagKey, TagValue>() : tags.ToDictionary(x => x.Key, x => x.Value);
        }

        private ushort[,] _data;
        private byte[] _bitmapDataARGB1555          = null;
        private byte[] _bitmapDataARGB1555_Endcodes = null;
        private byte[] _bitmapDataARGB8888          = null;
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
        public void SetImageData8Bit(byte[,] data, Palette palette) => throw new NotImplementedException();

        public ushort[,] ImageData16Bit {
            get => (ushort[,]) _data.Clone();
            set {
                if (!CanSetImageData16Bit)
                    throw new NotSupportedException();
                if (_data == value)
                    return;

                var error = Validate16BitImageData(value, Width * Height * 2, value.GetLength(0) * value.GetLength(1) * 2);
                if (error != null)
                    throw new ArgumentException(error);

                _data                        = value;
                _hash                        = null;
                _bitmapDataARGB1555          = null;
                _bitmapDataARGB1555_Endcodes = null;
                _bitmapDataARGB8888          = null;
                _bitmapDataARGB8888_Endcodes = null;
                ReplaceAction(value);
            }
        }

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

        public string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize)
            => "Not applicable; 16-bit images cannot be set with 8-bit indexed data";

        public string Validate16BitImageData(ushort[,] data, int oldStoredSize, int newStoredSize)
            => CanSetImageData16Bit ? ReplaceValidator(data) : "Not supported";

        private string _hash = null;
        private readonly string _hashPrefix;
        public string Hash {
            get {
                if (_hash == null)
                    _hash = BitmapDataARGB1555.CreateTextureHash(_hashPrefix);
                return _hash;
            }
        }

        public Dictionary<TagKey, TagValue> Tags { get; }

        public Palette Palette => null;

        public bool CanSetImageData8Bit => false;
        public virtual bool CanSetImageData16Bit => false;
        public bool ZeroIsTransparent => false;

        /// <summary>
        /// Checker to see if 16-bit ABGR1555 data can be set/imported.
        /// </summary>
        /// <param name="data">Data to set.</param>
        /// <returns>Returns 'null' if no error was detected, otherwise returns an error string.</returns>
        public virtual string ReplaceValidator(ushort[,] data) => "Unimplemented";

        /// <summary>
        /// Action to perform when an the image data is set.
        /// <param name="data">Data to set.</param>
        /// </summary>
        public virtual void ReplaceAction(ushort[,] data) => throw new NotImplementedException();

        public event EventHandler Invalidated;
    }
}
