using System.ComponentModel;
using System.Windows.Forms;

namespace SF3.Win.Controls {

    public class UInt16UpDownControl : NumericUpDown {
        public UInt16UpDownControl() {
            this.DecimalPlaces = 0;
            this.Minimum = ushort.MinValue;
            this.Maximum = ushort.MaxValue;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public ushort Value {
            get => decimal.ToUInt16(base.Value);
            set => base.Value = new decimal(value);
        }
    }
}