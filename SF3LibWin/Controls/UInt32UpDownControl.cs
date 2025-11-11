using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace SF3.Win.Controls {
    public class UInt32UpDownControl : NumericUpDown {
        public UInt32UpDownControl() {
            this.DecimalPlaces = 0;
            this.Minimum = uint.MinValue;
            this.Maximum = uint.MaxValue;
        }

        /// <summary>
        /// Base validation does some work to set the value, and it sets the value completely wrong for
        /// values >= 0x8000,0000. Undo whatever silly validation it did if the value is indeed a valid one.
        /// </summary>
        protected override void ValidateEditText() {
            uint? valueFromValidation = null;
            if (Hexadecimal) {
                if (uint.TryParse(Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value))
                    valueFromValidation = value;
            }
            else {
                if (uint.TryParse(Text, out uint value))
                    valueFromValidation = value;
            }

            var oldText = Text;
            base.ValidateEditText();
            if (valueFromValidation != null && oldText != Text)
                base.Value = valueFromValidation.Value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public uint Value {
            get => (uint) decimal.ToUInt64(base.Value);
            set => base.Value = new decimal(value);
        }
    }
}