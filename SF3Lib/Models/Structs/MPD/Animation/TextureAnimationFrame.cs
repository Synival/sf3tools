using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Images;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Animation {
    public class TextureAnimationFrame : TextureStructBase, ITexture {
        private readonly int _bytesPerProperty;
        private readonly int _imageDataOffsetAddr;
        private readonly int _durationAddr;

        public TextureAnimationFrame(
            IByteData data, string name, int address, bool is32Bit, int frameNum, IMPD_File mpdFile, TextureAnimation animation
        ) : base(
            data, mpdFile.ChunkData[3], (int) animation.TextureID, name, address, is32Bit ? 0x08 : 0x04, 
            animation.IsIndexed ? TexturePixelFormat.Palette3 : TexturePixelFormat.ABGR1555, true, true, chunkIndex: 3
        ) {
            Is32Bit          = is32Bit;
            Frame            = frameNum;
            ImportExportName = $"Texture_{ID:X2}_Frame_{frameNum:X2}";
            MPD_File         = mpdFile;
            Animation        = animation;

            _bytesPerProperty = is32Bit ? 0x04 : 0x02;

            _imageDataOffsetAddr = Address + 0 * _bytesPerProperty;
            _durationAddr        = Address + 1 * _bytesPerProperty;

            if (ImageDataOffset >= 0)
                LoadImageData();
        }

        public override void OnSetImageData() => throw new System.NotImplementedException();

        public bool Is32Bit { get; }

        [TableViewModelColumn(displayOrder: 0.0f)]
        public int TexAnimID => Animation.ID;

        [TableViewModelColumn(displayOrder: 1.0f)]
        public override int Width { get => (int) Animation.Width; set {} }

        [TableViewModelColumn(displayOrder: 1.1f)]
        public override int Height { get => (int) Animation.Height; set {} }

        [TableViewModelColumn(displayOrder: 2.0f, displayFormat: "X4")]
        public override int ImageDataOffset {
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

        public override bool HasImage => true;
        public override bool CanLoadImage => false;

        public override Palette Palette {
            get => PixelFormat == TexturePixelFormat.ABGR1555 ? null : MPD_File.CreatePalette(2);
            protected set {}
        }

        public CollectionType Collection => CollectionType.Primary;
        public Dictionary<TagKey, TagValue> Tags => null;

        public string ImportExportName { get; }
        public bool IsIndexed => Animation.IsIndexed;
        public IMPD_File MPD_File { get; }
        public TextureAnimation Animation { get; }
    }
}
