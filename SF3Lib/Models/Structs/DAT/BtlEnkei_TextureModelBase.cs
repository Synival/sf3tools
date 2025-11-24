using System;
using System.Drawing;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Extensions;
using SF3.Types;

namespace SF3.Models.Structs.DAT {
    public abstract class BtlEnkei_TextureModelBase : TextureModelBase {
        protected BtlEnkei_TextureModelBase(IByteData data, int id, string name, int address, int size, int paletteOffset, bool fetchImmediately)
        : base(data, id, name, address, size, 512, 256, TexturePixelFormat.Palette1, MakePalette(data, paletteOffset), true, false, fetchImmediately: fetchImmediately) {
        }

        private static Palette MakePalette(IByteData data, int paletteOffset) {
            var colors = data.GetDataCopyAt(paletteOffset, 0x200).ToUShorts();
            return new Palette(colors);
        }

        public abstract int PaletteOffset { get; }

        public override void LoadImageAction(Image image, string filename) {
            if (image.Width != Width || image.Height != Height)
                throw new ArgumentException($"Incoming image dimensions ({image.Width}x{image.Height}) should be {Width}x{Height}");
            if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                throw new ArgumentException($"Incoming image pixel format ({image.PixelFormat}) should be 'Format8bppIndexed'");
            Texture = image.CreateTextureIndexed(CollectionType.Primary, 0, 0, 0, ZeroIsTransparent);
        }

        public override void LoadPaletteFromImage(ITexture texture) {
            var newColors = texture.Palette.Channels.Select(x => x.ToABGR1555()).ToArray();
            int pos = PaletteOffset;
            for (int i = 0; i < 0x100; i++) {
                Data.SetWord(pos, (i < newColors.Length) ? newColors[i] : 0x0000);
                pos += 2;
            }
            Palette = new Palette(newColors);
        }
    }
}
