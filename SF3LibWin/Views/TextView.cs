using System.Drawing;
using System.Windows.Forms;

namespace SF3.Win.Views {
    public class TextView : ControlView<TextBox> {
        public TextView(string name, string text) : base(name) {
            _text = text;
        }

        public override Control Create() {
            var rval = base.Create();
            if (rval == null)
                return rval;

            Control.Font = new Font("Courier New", 10.0f);
            Control.ReadOnly = true;
            Control.Multiline = true;
            Control.ScrollBars = ScrollBars.Both;
            Control.WordWrap = false;

            UpdateData();
            return rval;
        }

        public override void RefreshContent() {
            if (IsCreated)
                UpdateData();
        }

        public void UpdateData() {
            Control.Text = Text;
        }

        private string _text;
        public string Text {
            get => _text;
            set {
                if (_text != value) {
                    _text = value;
                    UpdateData();
                }
            }
        }
    }
}
