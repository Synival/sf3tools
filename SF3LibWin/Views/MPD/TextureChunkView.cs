using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using SF3.Models.Files.MPD.Objects;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Win.Controls;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD {
    public class TextureChunkView : TabView {
        public TextureChunkView(string name, MPD_FileTextureChunkObj model) : base(name) {
            Model = model;

            var ngc = Model.NameGetterContext;
            HeaderView   = new TableView("Header", Model.TextureHeaderTable, ngc);
            TexturesView = new TableView("Textures", Model.TextureTable, ngc);
            TextureView  = new TextureView("Texture");
        }

        public override Control Create() {
            base.Create();

            CreateChild(HeaderView);
            CreateChild(TexturesView, (c) => {
                var textureTableControl = (ObjectListView) c;

                // Add a texture viewer on the right side of the 'Textures' tab.
                var textureTabPage = textureTableControl?.Parent;
                if (textureTabPage != null) {
                    var textureControl = (TextureControl) TextureView.Create();
                    if (textureControl != null) {
                        textureControl.Dock = DockStyle.Right;
                        textureTabPage.Controls.Add(textureControl);
                        textureTableControl.ItemSelectionChanged += OnTextureChanged;
                    }
                    TabControl.SelectedTab = (TabPage) textureTabPage;
                }
            });

            // Return the top-level control.
            return Control;
        }

        private void OnTextureChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TexturesView.OLVControl.SelectedItem;
            TextureView.Image = ((TextureModel) item?.RowObject)?.Texture?.CreateBitmap();
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            Control?.Hide();

            if (TexturesView.OLVControl != null)
                TexturesView.OLVControl.ItemSelectionChanged -= OnTextureChanged;

            HeaderView.Destroy();
            TexturesView.Destroy();
            TextureView.Destroy();

            base.Destroy();
        }

        public MPD_FileTextureChunkObj Model { get; }
        public TableView HeaderView { get; }
        public TableView TexturesView { get; }
        public TextureView TextureView { get; }
    }
}
