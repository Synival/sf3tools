using CommonLib.Imaging;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_TiledPlane : IMPD_TiledPlane {
        public MPD_TiledPlane(IMPD_TiledPlane original) {
            if (original.Tileset != null)
                Tileset = new TextureData(original.Tileset);
            if (original.TileAssignment != null)
                TileAssignment = new MPD_PlaneTileAssignment(original.TileAssignment);
            if (original.TiledImage != null)
                TiledImage = new TextureData(original.TiledImage);
        }

        public ITextureData Tileset { get; set; }
        public IMPD_PlaneTileAssignment TileAssignment { get; set; }
        public ITextureData TiledImage { get; set; }
    }
}
