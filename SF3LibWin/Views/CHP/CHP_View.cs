using System;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Files.CHP;
using SF3.Models.Structs.CHR;
using SF3.Win.Views.CHR;

namespace SF3.Win.Views.CHP {
    public class CHP_View : ArrayView<Sprite, SpriteView> {
        public CHP_View(string name, ICHP_File chpFile, INameGetterContext nameGetterContext) : base(
            name,
            chpFile?.CHR_EntriesByOffset?.Values?.SelectMany(x => x.SpriteTable)?.ToArray() ?? [],
            "DropdownName",
            new SpriteView("Sprite", null, nameGetterContext, TabAlignment.Left)
        ) {
            _model = chpFile;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selectedSprite = (Sprite) DropdownList.SelectedValue;
            ElementView.Sprite = selectedSprite;
        }

        private ICHP_File _model = null;
        public ICHP_File Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    Elements = value?.CHR_EntriesByOffset?.Values?.SelectMany(x => x.SpriteTable)?.ToArray() ?? [];
                }
            }
        }
    }
}
