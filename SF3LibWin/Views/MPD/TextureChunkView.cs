using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using SF3.Editors.MPD;
using SF3.Models.MPD.TextureChunk;
using SF3.Win.Controls;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD {
    public class TextureChunkView : TabView {
        public TextureChunkView(string name, TextureChunkEditor editor) : base(name) {
            Editor = editor;

            var ngc = Editor.NameGetterContext;
            HeaderView        = new TableView("Header", Editor.HeaderTable, ngc);
            TextureView       = new TableView("Textures", Editor.TextureTable, ngc);
            TextureViewerView = new ControlView<TextureControl>("Texture");
        }

        public override Control Create() {
            base.Create();

            _ = CreateChild(HeaderView);
            var textureTableControl = CreateChild(TextureView) as ObjectListView;

            // Add a texture viewer on the right side of the 'Textures' tab.
            var textureTabPage = textureTableControl?.Parent;
            if (textureTabPage != null) {
                var textureViewer = (TextureControl) TextureViewerView.Create();
                textureViewer.Dock = DockStyle.Right;
                textureTabPage.Controls.Add(textureViewer);
                textureTableControl.ItemSelectionChanged += OnTextureChanged;
            }

            // Return the top-level control.
            return Control;
        }

        private void OnTextureChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TextureView.OLVControl.SelectedItem;
            ((TextureControl) TextureViewerView.Control).TextureImage = (item != null) ? ((Texture) item.RowObject).CreateBitmap() : null;
        }

        public override void Destroy() {
            Control?.Hide();

            HeaderView?.Destroy();

            if (TextureView != null) {
                TextureView.OLVControl.ItemSelectionChanged -= OnTextureChanged;
                TextureView.Destroy();
            }

            TextureViewerView?.Destroy();

            base.Destroy();
        }

        public TextureChunkEditor Editor { get; }
        public TableView HeaderView { get; }
        public TableView TextureView { get; }
        public ControlView<TextureControl> TextureViewerView { get; }
    }
}
