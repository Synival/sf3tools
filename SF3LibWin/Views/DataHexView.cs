using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonLib.Arrays;

namespace SF3.Win.Views {
    public class DataHexView : ControlView<TextBox> {
        public DataHexView(string name, IByteArray data, int bytesPerRow = 16) : base(name) {
            Data = data;
            BytesPerRow = bytesPerRow;
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
            var rawData = Data.GetDataCopy();
            var stringBuilder = new StringBuilder("", rawData.Length * 3 + (rawData.Length / 16) + 1);

            var pos = 0;
            foreach (var b in rawData) {
                if (pos != 0)
                    stringBuilder.Append((pos % BytesPerRow == 0) ? "\r\n" : " ");
                stringBuilder.Append(b.ToString("X2"));
                pos++;
            }

            Control.Text = stringBuilder.ToString();
        }

        public IByteArray Data { get; }
        public int BytesPerRow { get; }
    }
}
