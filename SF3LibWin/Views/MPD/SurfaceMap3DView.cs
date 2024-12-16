using SF3.Models.Files.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class SurfaceMap3DView : ControlView<SurfaceMap3DControl> {
        public SurfaceMap3DView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public void UpdateMap() {
            var textureData = Model.TileSurfaceCharacterRows?.Make2DTextureData();
            SurfaceMapControl.UpdateModel(textureData, Model.TextureChunks, Model.TileSurfaceHeightmapRows.Rows);
        }

        public override void RefreshContent() {
            // TODO: how to refresh???
        }

        public IMPD_File Model { get; }

        public SurfaceMap3DControl SurfaceMapControl => (SurfaceMap3DControl) Control;
    }
}
