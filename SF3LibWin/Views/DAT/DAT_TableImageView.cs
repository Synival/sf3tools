using System;
using System.Drawing;
using CommonLib.NamedValues;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;

namespace SF3.Win.Views.DAT {
    public class DAT_TableImageView : TableTextureView<TextureModelBase, Table<TextureModelBase>> {
        public DAT_TableImageView(string name, Table<TextureModelBase> table, INameGetterContext nameGetterContext, float? imageScale = null)
        : base(name, table, nameGetterContext, imageScale) {}

        protected override void SetImage(TextureModelBase item) {
            base.SetImage(item);
            if (item?.CanLoadImage == true) {
                ImageView.Control.ImportAction = ImageView.ImportImageDialog;
                ImageView.LoadImageAction = (Image image, string filename) => {
                    item.LoadImageAction(image, filename);
                    ImageView.Texture = item.Texture;
                    TableView.RefreshContent();
                };
            }
            else {
                ImageView.Control.ImportAction = null;
                ImageView.LoadImageAction = null;
            }
        }

        protected override ITexture GetTextureFromModel(TextureModelBase frame)
            => frame?.Texture;
    }
}