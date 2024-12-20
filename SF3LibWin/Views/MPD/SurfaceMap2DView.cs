using SF3.Models.Files.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class SurfaceMap2DView : ControlView<SurfaceMap2DControl> {
        public SurfaceMap2DView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public void UpdateMap() {
            var textureData = Model.TileSurfaceCharacterRows?.Make2DTextureData();
            SurfaceMapControl.UpdateTextures(textureData, Model.TextureChunks);
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            // TODO: how to refresh???
        }

        public IMPD_File Model { get; }

        public SurfaceMap2DControl SurfaceMapControl => (SurfaceMap2DControl) Control;
    }
}
