using System.Windows.Forms;

namespace SF3.Win.Controls {

    public class SByteUpDownControl : NumericUpDown
    {
        public SByteUpDownControl() {
            this.DecimalPlaces = 0;
            this.Minimum = sbyte.MinValue;
            this.Maximum = sbyte.MaxValue;
        }

        new public sbyte Value {
            get => decimal.ToSByte(base.Value);
            set => base.Value = new decimal(value);
        }
    }
}