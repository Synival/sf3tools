using SF3.Models.Files.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class SurfaceMapView : ControlView<SurfaceMapControl> {
        public SurfaceMapView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public void UpdateMap() {
            var textureData = Model.TileSurfaceCharacterRows?.Make2DTextureData();
            SurfaceMapControl.UpdateTextures(textureData, Model.TextureChunks);
        }

        public override void RefreshContent() {
            // TODO: how to refresh???
        }

        public IMPD_File Model { get; }

        public SurfaceMapControl SurfaceMapControl => (SurfaceMapControl) Control;
    }
}
