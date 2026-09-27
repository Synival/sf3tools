using System;
using System.Linq;
using CommonLib.Rigging;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCAnimationArrayView : ArrayView<IAnimation, PCAnimationView> {
        public PCAnimationArrayView(string name, PolyChar model) : base(
            name,
            model?.AnimationChunkHeader?.AnimationDefTable?.ToArray(),
            "AnimationName",
            new PCAnimationView(name + "_Table", model, null)
        ) {
            _polyChar = model;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selected = (PCAnimationDefStruct) DropdownList.SelectedValue;
            ElementView.SetAnimation(_polyChar, selected);
        }

        private PolyChar _polyChar = null;
        public PolyChar PolyChar {
            get => _polyChar;
            set {
                if (_polyChar != value) {
                    _polyChar = value;
                    Elements = _polyChar?.AnimationChunkHeader?.AnimationDefTable?.ToArray();
                }
            }
        }
    }
}
