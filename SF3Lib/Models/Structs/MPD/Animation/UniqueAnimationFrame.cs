using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Imaging;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Animation {
    public class UniqueAnimationFrame : TextureStructBase, IMPD_AnimationFrame {
        public UniqueAnimationFrame(IByteData data, int id, string name, int address, int width, int height, TexturePixelFormat? pixelFormat, bool isReferenced, IMPD_File mpdFile)
        : base(
            data, id, name, address, width * height * (pixelFormat ?? TexturePixelFormat.ABGR1555).BytesPerPixel(), pixelFormat ?? TexturePixelFormat.ABGR1555,
            isCompressed: true, zeroIsTransparent: true, IndexedColorUpdateStrategy.MatchToExistingPalette
        ) {
            MPD_File = mpdFile;
            _width   = width;
            _height  = height;
            IsReferenced = isReferenced;
            PixelFormatKnown = pixelFormat.HasValue;

            _textureData.Add8BitValidator((_1, _2, _3, newStoredSize) => {
                return (newStoredSize > _originalStoredSize)
                    ? $"New stored image size ({newStoredSize} / 0x{newStoredSize:X2}) cannot be larger than existing stored image size ({_originalStoredSize} / 0x{_originalStoredSize:X2})"
                    : null;
            });

            _textureData.Add16BitValidator((_1, _2, newStoredSize) => {
                return (newStoredSize > _originalStoredSize)
                    ? $"New stored image size ({newStoredSize} / 0x{newStoredSize:X2}) cannot be larger than existing stored image size ({_originalStoredSize} / 0x{_originalStoredSize:X2})"
                    : null;
            });

            LoadImageData();
            _originalStoredSize = StoredImageDataSize.Value;
        }

        public int TextureID => ID;

        protected override void OnImageUpdated() {}

        public IMPD_File MPD_File { get; }

        private int _width;
        protected override int StructWidth { get => _width; set {} }

        private int _height;
        protected override int StructHeight { get => _height; set {} }

        protected override int StructImageDataOffset { get => Address; set {} }

        [TableViewModelColumn(displayOrder: 2.5f)]
        public bool PixelFormatKnown { get; }

        [TableViewModelColumn(displayOrder: 2.1f)]
        public bool IsReferenced { get; }

        public override bool HasImage => true;
        public override bool CanLoadImage => true;

        protected override IPalette StructPalette {
            get => PixelFormat == TexturePixelFormat.ABGR1555 ? null : MPD_File.TexturePalette;
            set {}
        }

        public MPD_CollectionType Collection => MPD_CollectionType.Primary;
        public int Frame => 0;
        public int Duration => 0;
        public Dictionary<TagKey, TagValue> Tags => null;
        public bool IsIgnored => false;

        private readonly int _originalStoredSize;
    }
}
