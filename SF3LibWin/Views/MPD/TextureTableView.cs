using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Models.Tables;
using SF3.Win.Controls;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD {
    public class TextureTableView : ControlSpaceView {
        public TextureTableView(string name, Table<TextureModel> model, INameGetterContext ngc) : base(name) {
            Model = model;
            TexturesView = new TableView("Textures", model, ngc);
            TextureView  = new TextureView("Texture");
        }

        public override Control Create() {
            base.Create();

            CreateChild(TexturesView, (c) => {
                var textureTableControl = (ObjectListView) c;

                // Add a texture viewer on the right side of the 'Textures' tab.
                var tableParent = textureTableControl?.Parent;
                if (tableParent != null) {
                    var textureControl = (TextureControl) TextureView.Create();
                    if (textureControl != null) {
                        textureControl.Dock = DockStyle.Right;
                        tableParent.Controls.Add(textureControl);
                        textureTableControl.ItemSelectionChanged += OnTextureChanged;
                    }
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
            TexturesView.Destroy();

            base.Destroy();
        }

        public Table<TextureModel> Model { get; }
        public TableView TexturesView { get; }
        public TextureView TextureView { get; }

    }
}
