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
        }

        public override Control Create() {
            base.Create();

            var ngc = Editor.NameGetterContext;

            _ = CreateChild(new TableView("Header", Editor.HeaderTable, ngc));
            var textureTableControl = CreateChild(new TableView("Textures", Editor.TextureTable, ngc)) as ObjectListView;
            var textureTabPage = textureTableControl?.Parent;

            // Add a texture viewer on the right side of the 'Textures' tab.
            if (textureTabPage != null) {
                var textureViewer = new TextureControl();
                textureViewer.Dock = DockStyle.Right;
                textureTabPage.Controls.Add(textureViewer);

                void OnTextureChanged(object sender, EventArgs e) {
                    var item = (OLVListItem) textureTableControl.SelectedItem;
                    textureViewer.TextureImage = (item != null) ? ((Texture) item.RowObject).CreateBitmap() : null;
                };

                textureTableControl.ItemSelectionChanged += (s, e) => OnTextureChanged(s, e);
            }

            // Return the top-level control.
            return Control;
        }

        public TextureChunkEditor Editor { get; }
    }
}
