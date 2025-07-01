using System;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Files.CHR;
using SF3.Models.Structs.CHR;

namespace SF3.Win.Views.CHR {
    public class CHR_View : ArrayView<Sprite, SpriteView> {
        public CHR_View(string name, ICHR_File chrFile, INameGetterContext nameGetterContext) : base(
            name,
            chrFile?.SpriteTable?.ToArray() ?? [],
            "DropdownName",
            new SpriteView("Sprite", null, nameGetterContext, TabAlignment.Left)
        ) {
            _model = chrFile;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selectedSprite = (Sprite) DropdownList.SelectedValue;
            ElementView.Sprite = selectedSprite;
        }

        private ICHR_File _model = null;
        public ICHR_File Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    Elements = value?.SpriteTable?.ToArray() ?? [];
                }
            }
        }
    }
}
