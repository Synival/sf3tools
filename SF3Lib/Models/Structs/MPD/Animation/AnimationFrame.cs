using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Imaging;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Animation {
    public class AnimationFrame : Struct, ITexture {
        private readonly int _bytesPerProperty;
        private readonly int _imageDataOffsetAddr;
        private readonly int _durationAddr;

        public AnimationFrame(
            IByteData data, string name, int address, bool is32Bit, int frameNum, IMPD_File mpdFile, AnimationStruct animation
        ) : base(data, (int) animation.TextureID, name, address, is32Bit ? 0x08 : 0x04) {
            Is32Bit          = is32Bit;
            Frame            = frameNum;
            ImportExportName = $"Texture_{ID:X2}_Frame_{frameNum:X2}";
            MPD_File         = mpdFile;
            Animation        = animation;

            _bytesPerProperty = is32Bit ? 0x04 : 0x02;

            _imageDataOffsetAddr = Address + 0 * _bytesPerProperty;
            _durationAddr        = Address + 1 * _bytesPerProperty;
        }

        public void SetImageData8Bit(byte[,] data, Palette palette) => Chunk3Texture?.SetImageData8Bit(data, palette);
        public byte[] GetBitmapDataARGB1555(bool highlightEndcodes = false) => Chunk3Texture?.GetBitmapDataARGB1555(highlightEndcodes);
        public byte[] GetBitmapDataARGB8888(bool highlightEndcodes = false) => Chunk3Texture?.GetBitmapDataARGB8888(highlightEndcodes);

        public string Validate8BitImageData(byte[,] data, Palette palette, int oldStoredSize, int newStoredSize)
            => Chunk3Texture?.Validate8BitImageData(data, palette, oldStoredSize, newStoredSize);

        public string Validate16BitImageData(ushort[,] data, int oldStoredSize, int newStoredSize)
            => Chunk3Texture?.Validate16BitImageData(data, oldStoredSize, newStoredSize);

        private ITexture Chunk3Texture
            => MPD_File?.AnimationFrameChunk?.UniqueAnimationFrameTable?.AtOffset(ImageDataOffset);

        public bool Is32Bit { get; }

        [TableViewModelColumn(displayOrder: 0.0f)]
        public int TexAnimID => Animation.ID;

        [TableViewModelColumn(displayOrder: 1.0f)]
        public int Width => Chunk3Texture?.Width ?? 0;

        [TableViewModelColumn(displayOrder: 1.1f)]
        public int Height => Chunk3Texture?.Height ?? 0;

        [TableViewModelColumn(displayOrder: 2.0f, displayFormat: "X4")]
        public int ImageDataOffset {
            get => (int) Data.GetData(_imageDataOffsetAddr, _bytesPerProperty);
            set => Data.SetData(_imageDataOffsetAddr, (uint) value, _bytesPerProperty);
        }

        [TableViewModelColumn(displayOrder: 2.1f)]
        public int Duration {
            get => (int) Data.GetData(_durationAddr, _bytesPerProperty);
            set => Data.SetData(_durationAddr, (uint) value, _bytesPerProperty);
        }

        [TableViewModelColumn(displayOrder: 2.2f)]
        public int Frame { get; }

        [TableViewModelColumn(displayOrder: 3f)]
        public string Hash => Chunk3Texture?.Hash;

        public string ImportExportName { get; }
        public IMPD_File MPD_File { get; }
        public AnimationStruct Animation { get; }

        public CollectionType Collection => Chunk3Texture?.Collection ?? (CollectionType) (-1);
        public Dictionary<TagKey, TagValue> Tags => Chunk3Texture?.Tags;
        public int BytesPerPixel => Chunk3Texture?.BytesPerPixel ?? 0;
        public TexturePixelFormat PixelFormat => Chunk3Texture?.PixelFormat ?? TexturePixelFormat.Unknown;
        public byte[,] ImageData8Bit => Chunk3Texture?.ImageData8Bit;

        public ushort[,] ImageData16Bit {
            get => Chunk3Texture?.ImageData16Bit;
            set {
                var tex = Chunk3Texture;
                if (tex != null)
                    tex.ImageData16Bit = value;
            }
        }

        public byte[] BitmapDataARGB1555 => Chunk3Texture?.BitmapDataARGB1555;
        public byte[] BitmapDataARGB8888 => Chunk3Texture?.BitmapDataARGB8888;
        public Palette Palette => Chunk3Texture?.Palette;
        public bool CanSetImageData8Bit => Chunk3Texture?.CanSetImageData8Bit ?? false;
        public bool CanSetImageData16Bit => Chunk3Texture?.CanSetImageData16Bit ?? false;
        public bool ZeroIsTransparent => false;

        public event EventHandler Invalidated;
    }
}
