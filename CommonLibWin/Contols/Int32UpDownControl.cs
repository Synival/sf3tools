using System.ComponentModel;

namespace CommonLib.Win.Controls {

    public class Int32UpDownControl : DarkModeNumericUpDown {
        public Int32UpDownControl() {
            this.DecimalPlaces = 0;
            this.Minimum = int.MinValue;
            this.Maximum = int.MaxValue;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public int Value {
            get => decimal.ToInt32(base.Value);
            set => base.Value = new decimal(value);
        }
    }
}