using System;
using System.Windows.Forms;
using CommonLib.Win.Controls;

namespace SF3.Win.Views {
    public abstract class ArrayView<TElement, TView> : ControlSpaceView where TView : IView {
        public ArrayView(string name, TElement[] elements, string keyProperty, TView elementView) : base(name) {
            _elements = elements;
            KeyProperty = keyProperty;
            ElementView = elementView;
        }

        protected abstract void OnSelectValue(object sender, EventArgs args);

        public override Control Create() {
            var control = base.Create();
            if (control == null)
                return control;            

            DropdownList = new DarkModeComboBox();
            DropdownList.Width = 400;
            DropdownList.DisplayMember = KeyProperty;
            DropdownList.DataSource = new BindingSource(Elements ?? [], null);
            DropdownList.SelectedValueChanged += OnSelectValue;
            control.Controls.Add(DropdownList);

            CreateChild(ElementView, (c) => {}, autoFill: false);
            Control.Resize += (s, e) => {
                var elementControl = ElementView.Control;
                elementControl.SetBounds(0, DropdownList.Bottom + 8, Control.Width, Control.Height - DropdownList.Height - 8);
            };

            return control;
        }

        public override void Destroy() {
            Control?.Controls.Remove(DropdownList);
            DropdownList = null;
            base.Destroy();
        }

        private TElement[] _elements;
        public TElement[] Elements {
            get => _elements;
            set {
                if (_elements != value) {
                    _elements = value;
                    if (DropdownList != null) {
                        if (DropdownList.DataSource is BindingSource bs)
                            bs.DataSource = _elements;
                        else
                            DropdownList.DataSource = new BindingSource(_elements, null);
                    }
                }
            }
        }

        public string KeyProperty { get; }
        public TView ElementView { get; }

        public DarkModeComboBox DropdownList { get; private set; } = null;
    }
}
