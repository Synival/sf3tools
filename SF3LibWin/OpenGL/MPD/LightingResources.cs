using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.MPD;

namespace SF3.Win.OpenGL.MPD {
    public class LightingResources : ResourcesBase, IMPD_Resources {
        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            ResetLightingTexture();
        }

        public void ResetLightingTexture() {
            if (LightingTexture != null) {
                LightingTexture.Dispose();
                LightingTexture = null;
            }
        }

        public void SetLightingTexture(Texture texture) {
            ResetLightingTexture();
            LightingTexture = texture;
        }

        public void Update(IMPD mpdFile) {
            using (var textureBitmap = CreateLightPaletteBitmap(mpdFile))
                SetLightingTexture(textureBitmap != null ? new Texture(textureBitmap, minNearest: false, magNearest: false, clampToEdge: false) : null);
        }

        public void Update(Palette lightPal, IMPD_PaletteAdjustColor lightAdj) {
            using (var textureBitmap = CreateLightPaletteBitmap(lightPal, lightAdj))
                SetLightingTexture(textureBitmap != null ? new Texture(textureBitmap, minNearest: false, magNearest: false, clampToEdge: false) : null);
        }

        private Bitmap CreateLightPaletteBitmap(IMPD mpdFile)
            => CreateLightPaletteBitmap(mpdFile?.LightPalette, mpdFile?.Settings?.LightPaletteAdjustment);

        private Bitmap CreateLightPaletteBitmap(Palette lightPal, IMPD_PaletteAdjustColor lightAdj) {
            if (lightPal == null)
                return null;

            var adjR = 0;
            var adjG = 0;
            var adjB = 0;

            if (lightAdj != null) {
                adjR = lightAdj.R * 255 / 31;
                adjG = lightAdj.G * 255 / 31;
                adjB = lightAdj.B * 255 / 31;
            }

            var numColors = lightPal.Channels.Length;

            var colorData = new byte[numColors * 4];
            var pos = 0;
            foreach (var color in lightPal.Channels) {
                var colorR = (byte) MathHelpers.Clamp(color.R + adjR, 0x00, 0xFF);
                var colorG = (byte) MathHelpers.Clamp(color.G + adjG, 0x00, 0xFF);
                var colorB = (byte) MathHelpers.Clamp(color.B + adjB, 0x00, 0xFF);

                colorData[pos++] = colorB;
                colorData[pos++] = colorG;
                colorData[pos++] = colorR;
                colorData[pos++] = 255;
            }

            var textureBitmap = new Bitmap(1, numColors, PixelFormat.Format32bppArgb);
            var bitmapData = textureBitmap.LockBits(new Rectangle(0, 0, 1, numColors), ImageLockMode.WriteOnly, textureBitmap.PixelFormat);
            Marshal.Copy(colorData, 0, bitmapData.Scan0, colorData.Length);
            textureBitmap.UnlockBits(bitmapData);

            return textureBitmap;
        }

        public Texture LightingTexture { get; private set; } = null;
    }
}
