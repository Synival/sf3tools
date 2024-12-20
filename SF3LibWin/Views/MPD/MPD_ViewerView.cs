using SF3.Models.Files.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class MPD_ViewerView : ControlView<MPD_ViewerControl> {
        public MPD_ViewerView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public void UpdateMap() {
            var textureData = Model.TileSurfaceCharacterRows?.Make2DTextureData();
            ViewerControl.UpdateModel(textureData, Model.TextureChunks, Model.TextureAnimations, Model.TileSurfaceHeightmapRows.Rows);
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            // TODO: how to refresh???
        }

        public IMPD_File Model { get; }

        public MPD_ViewerControl ViewerControl => (MPD_ViewerControl) Control;
    }
}
