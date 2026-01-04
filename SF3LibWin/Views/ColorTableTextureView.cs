using System;
using SF3.Models.Tables.Shared;

namespace SF3.Win.Views {
    public class ColorTableTextureView : TextureView {
        public ColorTableTextureView(string name, ColorTable table) : base(name) {
            Texture = table;
        }

        protected override void OnImageSet() {
            base.OnImageSet();
            if (Texture != null)
                ImageScale = (int) Math.Ceiling(128.0f / Texture.Width);
        }

        public ColorTable Table {
            get => Texture as ColorTable;
            set => Texture = value;
        }
    }
}
