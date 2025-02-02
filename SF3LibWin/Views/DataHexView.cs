using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;

namespace SF3.Win.Views {
    public class DataHexView : ControlView<TextBox> {
        public DataHexView(string name, IByteArray data) : base(name) {
            Data = data;
        }

        public override Control Create() {
            var rval = base.Create();
            if (rval == null)
                return rval;

            Control.Font = new Font("Courier New", 12.0f);
            Control.ReadOnly = true;
            Control.Multiline = true;
            Control.ScrollBars = ScrollBars.Both;

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
                stringBuilder.Append(b.ToString("X2"));
                stringBuilder.Append(pos == 15 ? "\r\n" : " ");
                pos = (pos + 1) % 16;
            }

            Control.Text = stringBuilder.ToString();
        }

        public IByteArray Data { get; }
    }
}
