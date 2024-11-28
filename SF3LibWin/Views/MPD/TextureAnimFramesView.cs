using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD {
    public class TextureAnimFramesView : ControlSpaceView {
        public TextureAnimFramesView(string name, IMPD_Editor editor, INameGetterContext nameGetterContext) : base(name) {
            Editor      = editor;
            TableView   = new TableView("Frames", editor.TextureAnimFrames, nameGetterContext);
            TextureView = new TextureView("Texture");
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            if (CreateChild(TableView) == null) {
                Destroy();
                return null;
            }

            if (CreateChild(TextureView, autoFill: false) == null) {
                Destroy();
                return null;
            }

            TextureView.Control.Dock = DockStyle.Right;
            TableView.OLVControl.ItemSelectionChanged += OnTextureChanged;
            return Control;
        }

        void OnTextureChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;
            var frame = (FrameModel) item?.RowObject;
            TextureView.Image = (frame == null || Editor.TextureAnimFrameEditors[frame.ID] == null)
                ? (System.Drawing.Image) null
                : frame.CreateBitmap(Editor.TextureAnimFrameEditors[frame.ID].DecompressedEditor);
        }

        public override void Destroy() {
            Control?.Hide();

            if (TableView.OLVControl != null)
                TableView.OLVControl.ItemSelectionChanged -= OnTextureChanged;

            TableView.Destroy();
            TextureView.Destroy();

            base.Destroy();
        }

        public IMPD_Editor Editor { get; }
        public TableView TableView { get; private set; }
        public TextureView TextureView { get; private set; }
    }
}