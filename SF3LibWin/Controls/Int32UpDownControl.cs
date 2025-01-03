using System.Windows.Forms;

namespace SF3.Win.Controls {

    public class Int32UpDownControl : NumericUpDown {
        public Int32UpDownControl() {
            this.DecimalPlaces = 0;
            this.Minimum = int.MinValue;
            this.Maximum = int.MaxValue;
        }

        new public int Value {
            get => decimal.ToInt32(base.Value);
            set => base.Value = new decimal(value);
        }
    }
}