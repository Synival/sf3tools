using System.ComponentModel;

namespace SF3.Win.Controls {

    public class SByteUpDownControl : DarkModeNumericUpDown {
        public SByteUpDownControl() {
            this.DecimalPlaces = 0;
            this.Minimum = sbyte.MinValue;
            this.Maximum = sbyte.MaxValue;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public sbyte Value {
            get => decimal.ToSByte(base.Value);
            set => base.Value = new decimal(value);
        }
    }
}