﻿using System.Windows.Forms;

namespace SF3.Win.Controls {

    public class ByteUpDownControl : NumericUpDown {
        public ByteUpDownControl() {
            this.DecimalPlaces = 0;
            this.Minimum = byte.MinValue;
            this.Maximum = byte.MaxValue;
        }

        new public byte Value {
            get => decimal.ToByte(base.Value);
            set => base.Value = new decimal(value);
        }
    }
}