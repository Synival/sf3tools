using SF3.Models.Files.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class MPD_ViewerView : ControlView<MPD_ViewerControl2> {
        public MPD_ViewerView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public void UpdateMap() {
            var textureData = Model.TileSurfaceCharacterRows?.Make2DTextureData();
            ViewerGLControl.UpdateSurfaceModels(textureData, Model.TextureChunks, Model.TextureAnimations, Model.TileSurfaceHeightmapRows.Rows);
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            // TODO: how to refresh???
        }

        public IMPD_File Model { get; }

        public MPD_ViewerControl2 ViewerControl => (MPD_ViewerControl2) Control;
        public MPD_ViewerGLControl ViewerGLControl => ((MPD_ViewerControl2) Control).GLControl;
    }
}
