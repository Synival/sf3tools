using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_Planes : IMPD_Planes {
        public MPD_Planes() { }

        public MPD_Planes(IMPD_Planes original) {
            GroundX         = original.GroundX;
            GroundY         = original.GroundY;
            GroundZ         = original.GroundZ;
            GroundXRotation = original.GroundXRotation;
            BackgroundX     = original.BackgroundX;
            BackgroundY     = original.BackgroundY;

            if (original.GroundImage != null)
                GroundImage = new TextureData(original.GroundImage);
            if (original.GroundTiledImage != null)
                GroundTiledImage = new MPD_TiledPlane(original.GroundTiledImage);
            if (original.BackgroundImage != null)
                BackgroundImage = new TextureData(original.BackgroundImage);
            if (original.SkyImage != null)
                SkyImage = new TextureData(original.SkyImage);
            if (original.ForegroundTiledImage != null)
                ForegroundTiledImage = new MPD_TiledPlane(original.ForegroundTiledImage);

            if (original.GroundPalette != null)
                GroundPalette = new Palette(original.GroundPalette);
            if (original.SkyPalette != null)
                SkyPalette = new Palette(original.SkyPalette);
        }

        public static MPD_Planes FromJToken(JToken token) => new MPD_Planes(token);
        private MPD_Planes(JToken token) {
            var jObject = (JObject) token;

            GroundX         = (short) jObject["GroundX"];
            GroundY         = (short) jObject["GroundY"];
            GroundZ         = (short) jObject["GroundZ"];
            GroundXRotation = (float) jObject["GroundXRotation"];
            BackgroundX     = (short) jObject["BackgroundX"];
            BackgroundY     = (short) jObject["BackgroundY"];

            GroundPalette   = jObject.GetValueIfExists("GroundPalette", t => Palette.FromJToken(t));
            SkyPalette      = jObject.GetValueIfExists("SkyPalette", t => Palette.FromJToken(t));

            GroundImage = jObject.GetValueIfExists("GroundImage",
                t => TextureData.FromJToken(t, 512, 256, TexturePixelFormat.Indexed8Bit, false, GroundPalette, true));
            SkyImage = jObject.GetValueIfExists("SkyImage",
                t => TextureData.FromJToken(t, 512, 256, TexturePixelFormat.Indexed8Bit, false, GroundPalette, true));
            //public IMPD_TiledPlane GroundTiledImage { get; }

            BackgroundImage = jObject.GetValueIfExists("BackgroundImage",
                t => TextureData.FromJToken(t, 512, 256, TexturePixelFormat.Indexed8Bit, false, GroundPalette, true));
            //public IMPD_TiledPlane ForegroundTiledImage { get; }
        }

        public ITextureData GroundImage { get; }
        public IMPD_TiledPlane GroundTiledImage { get; }
        public ITextureData BackgroundImage { get; }
        public ITextureData SkyImage { get; }
        public IMPD_TiledPlane ForegroundTiledImage { get; }

        public short GroundX { get; set; }
        public short GroundY { get; set; }
        public short GroundZ { get; set; }
        public float GroundXRotation { get; set; }
        public short BackgroundX { get; set; }
        public short BackgroundY { get; set; }

        public Palette GroundPalette { get; }
        public Palette SkyPalette { get; }
    }
}
