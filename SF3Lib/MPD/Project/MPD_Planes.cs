using CommonLib.Imaging;
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

            if (original.GroundPalette != null)
                GroundPalette = new Palette(original.GroundPalette);
            if (original.SkyPalette != null)
                SkyPalette = new Palette(original.SkyPalette);
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
