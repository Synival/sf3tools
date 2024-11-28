using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Win.Controls;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD {
    public class TextureChunkView : TabView {
        public TextureChunkView(string name, MPD_FileTextureChunk editor) : base(name) {
            Editor = editor;

            var ngc = Editor.NameGetterContext;
            HeaderView   = new TableView("Header", Editor.TextureHeaderTable, ngc);
            TexturesView = new TableView("Textures", Editor.TextureTable, ngc);
            TextureView  = new TextureView("Texture");
        }

        public override Control Create() {
            base.Create();

            _ = CreateChild(HeaderView);
            var textureTableControl = CreateChild(TexturesView) as ObjectListView;

            // Add a texture viewer on the right side of the 'Textures' tab.
            var textureTabPage = textureTableControl?.Parent;
            if (textureTabPage != null) {
                var textureControl = (TextureControl) TextureView.Create();
                if (textureControl != null) {
                    textureControl.Dock = DockStyle.Right;
                    textureTabPage.Controls.Add(textureControl);
                    textureTableControl.ItemSelectionChanged += OnTextureChanged;
                }
            }

            // Return the top-level control.
            return Control;
        }

        private void OnTextureChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TexturesView.OLVControl.SelectedItem;
            TextureView.Image = (item != null) ? ((Texture) item.RowObject).CreateBitmap() : null;
        }

        public override void Destroy() {
            Control?.Hide();

            if (TexturesView.OLVControl != null)
                TexturesView.OLVControl.ItemSelectionChanged -= OnTextureChanged;

            HeaderView.Destroy();
            TexturesView.Destroy();
            TextureView.Destroy();

            base.Destroy();
        }

        public MPD_FileTextureChunk Editor { get; }
        public TableView HeaderView { get; }
        public TableView TexturesView { get; }
        public TextureView TextureView { get; }
    }
}
