using System.Drawing;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class TextureView : ControlView<TextureControl> {
        public TextureView(string name) : base(name) {
        }

        public TextureControl TextureControl => (TextureControl) Control;

        public Image Image {
            get => TextureControl.TextureImage;
            set => TextureControl.TextureImage = value;
        }

        public override void RefreshContent() {
            var old = TextureControl.TextureImage;
            TextureControl.TextureImage = null;
            TextureControl.TextureImage = old;
        }
    }
}
