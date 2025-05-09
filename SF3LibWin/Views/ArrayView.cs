using System;
using System.Windows.Forms;

namespace SF3.Win.Views {
    public abstract class ArrayView<TElement, TView> : ControlSpaceView where TView : IView {
        public ArrayView(string name, TElement[] elements, string keyProperty, TView elementView) : base(name) {
            Elements = elements;
            KeyProperty = keyProperty;
            ElementView = elementView;
        }

        protected abstract void OnSelectValue(object sender, EventArgs args);

        public override Control Create() {
            var control = base.Create();
            if (control == null)
                return control;            

            DropdownList = new ComboBox();
            DropdownList.Width = 400;
            DropdownList.DataSource = new BindingSource(Elements, null);
            DropdownList.DisplayMember = KeyProperty;
            DropdownList.SelectedValueChanged += OnSelectValue;
            control.Controls.Add(DropdownList);

            CreateChild(ElementView, (c) => {}, autoFill: false);
            Control.Resize += (s, e) => {
                var tableControl = ElementView.Control;
                tableControl.SetBounds(0, DropdownList.Bottom + 8, Control.Width, Control.Height - DropdownList.Height - 8);
            };

            return control;
        }

        public override void Destroy() {
            Control?.Controls.Remove(DropdownList);
            DropdownList = null;
            base.Destroy();
        }

        public TElement[] Elements { get; }
        public string KeyProperty { get; }
        public TView ElementView { get; }

        public ComboBox DropdownList { get; private set; } = null;
    }
}
