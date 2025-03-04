using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Utils;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables.MPD;

namespace SF3.Win.OpenGL.MPD_File {
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

        public void Update(IMPD_File mpdFile) {
            using (var textureBitmap = CreateLightPaletteBitmap(mpdFile))
                SetLightingTexture(textureBitmap != null ? new Texture(textureBitmap, clampToEdge: false) : null);
        }

        public void Update(ColorTable lightPal, LightAdjustmentModel lightAdjustment) {
            using (var textureBitmap = CreateLightPaletteBitmap(lightPal, lightAdjustment))
                SetLightingTexture(textureBitmap != null ? new Texture(textureBitmap, clampToEdge: false) : null);
        }

        private Bitmap CreateLightPaletteBitmap(IMPD_File mpdFile)
            => CreateLightPaletteBitmap(mpdFile?.LightPalette, mpdFile?.LightAdjustment);

        private Bitmap CreateLightPaletteBitmap(ColorTable lightPal, LightAdjustmentModel lightAdjustment) {
            if (lightPal == null)
                return null;

            var adjR = lightAdjustment?.RAdjustment ?? 0;
            var adjG = lightAdjustment?.GAdjustment ?? 0;
            var adjB = lightAdjustment?.BAdjustment ?? 0;

            var numColors = lightPal.Length;

            var colorData = new byte[numColors * 4];
            int pos = 0;
            foreach (var color in lightPal) {
                var colorValue = color.ColorABGR1555;
                var colorR = MathHelpers.Clamp((short) ((colorValue >>  0) & 0x1F) + adjR, 0x00, 0x1F);
                var colorG = MathHelpers.Clamp((short) ((colorValue >>  5) & 0x1F) + adjG, 0x00, 0x1F);
                var colorB = MathHelpers.Clamp((short) ((colorValue >> 10) & 0x1F) + adjB, 0x00, 0x1F);

                colorData[pos++] = (byte) (colorB * 255 / 31);
                colorData[pos++] = (byte) (colorG * 255 / 31);
                colorData[pos++] = (byte) (colorR * 255 / 31);
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
