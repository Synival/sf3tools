using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Main;
using SF3.Models.Tables.Shared;

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
                SetLightingTexture(textureBitmap != null ? new Texture(textureBitmap, minNearest: false, magNearest: false, clampToEdge: false) : null);
        }

        public void Update(Palette lightPal, LightAdjustment lightAdjustment) {
            using (var textureBitmap = CreateLightPaletteBitmap(lightPal, lightAdjustment))
                SetLightingTexture(textureBitmap != null ? new Texture(textureBitmap, minNearest: false, magNearest: false, clampToEdge: false) : null);
        }

        private Bitmap CreateLightPaletteBitmap(IMPD_File mpdFile)
            => CreateLightPaletteBitmap(mpdFile?.LightPalette, mpdFile?.LightAdjustment);

        private Bitmap CreateLightPaletteBitmap(Palette lightPal, LightAdjustment lightAdjustment) {
            if (lightPal == null)
                return null;

            var adjR = (lightAdjustment?.RAdjustment ?? 0) * 255 / 31;
            var adjG = (lightAdjustment?.GAdjustment ?? 0) * 255 / 31;
            var adjB = (lightAdjustment?.BAdjustment ?? 0) * 255 / 31;

            var numColors = lightPal.Channels.Length;

            var colorData = new byte[numColors * 4];
            int pos = 0;
            foreach (var color in lightPal.Channels) {
                var colorR = (byte) MathHelpers.Clamp(color.r + adjR, 0x00, 0xFF);
                var colorG = (byte) MathHelpers.Clamp(color.g + adjG, 0x00, 0xFF);
                var colorB = (byte) MathHelpers.Clamp(color.b + adjB, 0x00, 0xFF);

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
