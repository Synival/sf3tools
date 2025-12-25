using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Images;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public class TextureStructData : TextureData {
        public TextureStructData(
            IByteData data, TexturePixelFormat pixelFormat, bool isCompressed, bool zeroIsTransparent,
            TextureStructBase @struct
        ) : base(data, 0, 0, 0, pixelFormat, null, isCompressed, zeroIsTransparent) {
            Struct = @struct;
        }

        public override string Validate8BitImageData(byte[,] data, Palette palette) {
            var baseRval = base.Validate8BitImageData(data, palette);
            if (baseRval != null)
                return baseRval;
            if (data.GetLength(0) != Width || data.GetLength(1) != Height)
                return $"Incoming texture height ({data.GetLength(0)}x{data.GetLength(1)}) should be {Width}x{Height}";
            return null;
        }

        public override string Validate16BitImageData(ushort[,] data) {
            var baseRval = base.Validate16BitImageData(data);
            if (baseRval != null)
                return baseRval;
            if (data.GetLength(0) != Width || data.GetLength(1) != Height)
                return $"Incoming texture height ({data.GetLength(0)}x{data.GetLength(1)}) should be {Width}x{Height}";
            return null;
        }

        public override int Address {
            get => Struct.ImageDataOffset;
            set => Struct.ImageDataOffset = value;
        }

        public override int Width {
            get => Struct.Width;
            set => Struct.Width = value;
        }

        public override int Height {
            get => Struct.Height;
            set => Struct.Height = value;
        }

        public override Palette Palette {
            get => Struct.Palette;
            set => Struct.Palette = value;
        }

        public override bool CanSetImageData8Bit => base.CanSetImageData8Bit && BytesPerPixel == 1 && Struct.CanLoadImage;
        public override bool CanSetImageData16Bit => base.CanSetImageData16Bit && BytesPerPixel == 2 && Struct.CanLoadImage;

        public TextureStructBase Struct { get; }
    }
}
