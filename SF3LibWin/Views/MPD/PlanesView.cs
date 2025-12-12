using System.Windows.Forms;
using SF3.MPD;

namespace SF3.Win.Views.MPD {
    public class PlanesView : TabView {
        public PlanesView(string name, IMPD_Planes planes, bool lazyLoad = true, TabAlignment tabAlignment = TabAlignment.Top)
        : base(name, lazyLoad, tabAlignment) {
            Planes = planes;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            if (Planes.GroundImage != null)
                CreateChild(new TextureView("Ground (Image)", Planes.GroundImage, 1));
            if (Planes.GroundTiledImage?.Tileset != null)
                CreateChild(new TextureView("Ground Tileset", Planes.GroundTiledImage.Tileset, 1));
            if (Planes.GroundTiledImage?.TiledImage != null)
                CreateChild(new TextureView("Ground (Tiled Image)", Planes.GroundTiledImage.TiledImage, 0.50f));
            if (Planes.SkyBoxImage != null)
                CreateChild(new TextureView("Sky Box", Planes.SkyBoxImage, 1));
            if (Planes.BackgroundImage != null)
                CreateChild(new TextureView("Background", Planes.BackgroundImage, 1));
            if (Planes.ForegroundTiledImage?.Tileset != null)
                CreateChild(new TextureView("Foreground Tileset", Planes.ForegroundTiledImage.Tileset, 1));
            if (Planes.ForegroundTiledImage?.TiledImage != null)
                CreateChild(new TextureView("Foreground (Tiled Image)", Planes.ForegroundTiledImage.TiledImage, 1));

            return Control;
        }

        public IMPD_Planes Planes { get; }
    }
}
