using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Editors.MPD;
using SF3.Models.MPD.TextureAnimation;
using SF3.Win.Controls;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD {
    public class TextureAnimFramesView : ViewBase {
        private static int _s_tableCounter = 1;
        private static int _s_containerCounter = 1;
        private static int _s_textureViewerCounter = 1;

        public TextureAnimFramesView(string name, IMPD_Editor editor, INameGetterContext nameGetterContext) : base(name) {
            Editor = editor;
            TableView = new TableView("TextureAnimFramesViewTable" + (_s_tableCounter++), editor.TextureAnimFrames, nameGetterContext);
        }

        public override Control Create() {
            var framesControl = (ObjectListView) TableView.Create();
            if (framesControl == null)
                return null;

            var container = new Control(null, "TextureAnimFramesViewContainer" + _s_containerCounter++);
            container.Padding = new Padding();
            container.Margin = new Padding();

            framesControl.Parent = container;
            framesControl.Dock = DockStyle.Fill;

            var textureViewer = new TextureControl();
            textureViewer.Name = "TextureAnimFramesViewTextureViewer" + _s_textureViewerCounter++;
            textureViewer.Parent = container;
            textureViewer.Dock = DockStyle.Right;

            void OnTextureChanged(object sender, EventArgs e) {
                var item = (OLVListItem) framesControl.SelectedItem;
                var frame = (FrameModel) item?.RowObject;
                textureViewer.TextureImage = (frame == null || Editor.TextureAnimFrameEditors[frame.ID] == null)
                    ? (System.Drawing.Image) null
                    : frame.CreateBitmap(Editor.TextureAnimFrameEditors[frame.ID].DecompressedEditor);
            };

            framesControl.ItemSelectionChanged += (s, e) => OnTextureChanged(s, e);

            Control = container;
            OLVControl = framesControl;
            TextureViewer = textureViewer;
            return Control;
        }

        public override void Destroy() {
            TableView.Destroy();
            TextureViewer.Dispose();
            TextureViewer = null;

            base.Destroy();
        }

        public IMPD_Editor Editor { get; }
        public TableView TableView { get; }

        public ObjectListView OLVControl { get; private set; }
        public TextureControl TextureViewer { get; private set; }
    }
}