using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Images;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Animation {
    public class UniqueAnimationFrame : TextureStructBase, ITexture {
        public UniqueAnimationFrame(IByteData data, int id, string name, int address, int width, int height, bool isIndexed, bool isReferenced, IMPD_File mpdFile)
        : base(
            data, id, name, address, width * height * (isIndexed ? 1 : 2), isIndexed ? TexturePixelFormat.Palette3 : TexturePixelFormat.ABGR1555,
            isCompressed: true, zeroIsTransparent: true
        ) {
            MPD_File = mpdFile;
            _width   = width;
            _height  = height;
            IsReferenced = isReferenced;
            LoadImageData();
        }

        public IMPD_File MPD_File { get; }

        private int _width;
        [TableViewModelColumn(displayOrder: 0)]
        public override int Width { get => _width; set {} }

        private int _height;
        [TableViewModelColumn(displayOrder: 1)]
        public override int Height { get => _height; set {} }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X4", isReadOnly: true)]
        public override int ImageDataOffset { get => Address; set {} }

        [TableViewModelColumn(displayOrder: 2.1f)]
        public bool IsReferenced { get; }

        public override bool HasImage => true;
        public override bool CanLoadImage => false;
        public override Palette Palette { get => PixelFormat == TexturePixelFormat.ABGR1555 ? null : MPD_File.CreatePalette(2); protected set {} }
        public CollectionType Collection => CollectionType.Primary;
        public int Frame => 0;
        public int Duration => 0;
        public Dictionary<TagKey, TagValue> Tags => null;

        public override void OnSetImageData() => throw new System.NotImplementedException();
    }
}
