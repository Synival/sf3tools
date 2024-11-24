using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Editors.MPD;
using SF3.Models.MPD.TextureAnimation;
using SF3.Win.Controls;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD {
    public class TextureAnimFramesView : ControlSpaceView {
        private static int _s_tableCounter = 1;
        private static int _s_containerCounter = 1;
        private static int _s_textureViewerCounter = 1;

        public TextureAnimFramesView(string name, IMPD_Editor editor, INameGetterContext nameGetterContext) : base(name) {
            Editor = editor;
            TableView = new TableView("TextureAnimFramesViewTable" + (_s_tableCounter++), editor.TextureAnimFrames, nameGetterContext);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            OLVControl = (ObjectListView) CreateChild(TableView);
            if (OLVControl == null)
                return null;

            TextureViewer = (TextureControl) CreateChild("TextureAnimFramesViewTextureViewer" + _s_textureViewerCounter++, new TextureControl(), autoFill: false);
            TextureViewer.Dock = DockStyle.Right;

            void OnTextureChanged(object sender, EventArgs e) {
                var item = (OLVListItem) OLVControl.SelectedItem;
                var frame = (FrameModel) item?.RowObject;
                TextureViewer.TextureImage = (frame == null || Editor.TextureAnimFrameEditors[frame.ID] == null)
                    ? (System.Drawing.Image) null
                    : frame.CreateBitmap(Editor.TextureAnimFrameEditors[frame.ID].DecompressedEditor);
            };

            OLVControl.ItemSelectionChanged += (s, e) => OnTextureChanged(s, e);
            return Control;
        }

        public override void Destroy() {
            TableView.Destroy();
            TableView = null;

            TextureViewer.Dispose();
            TextureViewer = null;

            base.Destroy();
        }

        public IMPD_Editor Editor { get; }
        public TableView TableView { get; private set; }

        public ObjectListView OLVControl { get; private set; }
        public TextureControl TextureViewer { get; private set; }
    }
}