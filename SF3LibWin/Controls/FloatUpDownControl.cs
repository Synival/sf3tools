using System.ComponentModel;
using System.Windows.Forms;

namespace SF3.Win.Controls {

    public class FloatUpDownControl : NumericUpDown {
        public FloatUpDownControl() {
            this.DecimalPlaces = 7;
            this.Maximum = int.MaxValue;
            this.Minimum = -int.MaxValue;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public float Value {
            get => (float) base.Value;
            set => base.Value = new decimal(value);
        }
    }
}