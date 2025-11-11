using System.Windows.Forms;
using System.ComponentModel;

namespace SF3.Win.Controls {

    public class ByteUpDownControl : NumericUpDown {
        public ByteUpDownControl() {
            this.DecimalPlaces = 0;
            this.Minimum = byte.MinValue;
            this.Maximum = byte.MaxValue;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public byte Value {
            get => decimal.ToByte(base.Value);
            set => base.Value = new decimal(value);
        }
    }
}