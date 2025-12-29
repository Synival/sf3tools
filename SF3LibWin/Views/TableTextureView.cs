using System;
using CommonLib.NamedValues;
using SF3.Images;
using SF3.Models.Structs;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public abstract class TableTextureView<TTableItem, TTable> : TableImageViewBase<TTableItem, TTable, TextureView>
        where TTableItem : class, IStruct
        where TTable : class, ITable<TTableItem>
    {
        public TableTextureView(string name, TTable table, INameGetterContext ngc, float? imageScale = null)
        : base(name, table, ngc, imageScale) {}

        protected override TextureView CreateImageView(float? imageScale)
            => new TextureView("Texture", imageScale);

        protected override void SetImage(TTableItem item) {
            var tex = GetTextureFromModel(item);
            if (ImageView.Texture == tex)
                return;

            if (ImageView.Texture != null)
                ImageView.Texture.Invalidated -= OnInvalidated;
            ImageView.Texture = tex;
            if (tex != null)
                tex.Invalidated += OnInvalidated;
        }

        public override void Destroy() {
            if (ImageView.Texture != null)
                ImageView.Texture.Invalidated -= OnInvalidated;
            base.Destroy();
        }

        private void OnInvalidated(object sender, EventArgs eventArgs)
            => ImageView.ReloadImage();

        protected abstract ITextureData GetTextureFromModel(TTableItem item);
    }
}
