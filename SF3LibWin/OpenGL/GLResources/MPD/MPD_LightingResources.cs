using System.Drawing;
using SF3.MPD.Interfaces;
using SF3.Win.OpenGL.GLResources.Shared;

namespace SF3.Win.OpenGL.GLResources.MPD {
    public class MPD_LightingResources : LightingResources, IMPD_Resources {
        public void Update(IMPD mpdFile) {
            using (var textureBitmap = CreateLightPaletteBitmap(mpdFile))
                SetLightingTexture(textureBitmap != null ? new Texture(textureBitmap, minNearest: false, magNearest: false, clampToEdge: false) : null);
        }

        private static Bitmap CreateLightPaletteBitmap(IMPD mpdFile)
            => CreateLightPaletteBitmap(mpdFile?.Lighting?.Palette, mpdFile?.Settings?.LightPaletteAdjustment);
    }
}
