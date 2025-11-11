using System.ComponentModel;
using System.Windows.Forms;

namespace SF3.Win.Controls {

    public class Int16UpDownControl : NumericUpDown {
        public Int16UpDownControl() {
            this.DecimalPlaces = 0;
            this.Minimum = short.MinValue;
            this.Maximum = short.MaxValue;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public short Value {
            get => decimal.ToInt16(base.Value);
            set => base.Value = new decimal(value);
        }
    }
}