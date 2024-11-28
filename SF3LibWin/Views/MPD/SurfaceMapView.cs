using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class SurfaceMapView : ControlView<SurfaceMapControl> {
        public SurfaceMapView(string name, IMPD_File editor) : base(name) {
            Editor = editor;
        }

        public override Control Create()
            => (Editor.TileSurfaceCharacterRows == null) ? null : base.Create();

        public void UpdateMap() {
            var textureData = Editor.TileSurfaceCharacterRows?.Make2DTextureData();
            SurfaceMapControl.UpdateTextures(textureData, Editor.TextureChunks);
        }

        public override void RefreshContent() {
            if (SurfaceMapControl == null)
                return;

            // TODO: how to refresh???
        }

        public IMPD_File Editor { get; }

        public SurfaceMapControl SurfaceMapControl => (SurfaceMapControl) Control;
    }
}
