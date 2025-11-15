using System;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace CommonLib.Win.Controls {
    /// <summary>
    /// TextBox with dark mode support.
    /// </summary>
    public class DarkModeTextBox : TextBox {
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {                
                DarkModeContext = new DarkModeControlContext<DarkModeTextBox>(this);

                OriginalBorderStyle = BorderStyle;
                if (OriginalBorderStyle != BorderStyle.None && BorderStyle != BorderStyle.FixedSingle) {
                    DarkModeContext.EnabledChanged += (s, e) => {
                        if (DarkModeContext.Enabled)
                            BeginInvoke(() => this.BorderStyle = BorderStyle.FixedSingle);
                        else
                            BeginInvoke(() => this.BorderStyle = OriginalBorderStyle);
                    };
                }

                DarkModeContext.Init();
            }
        }

        private DarkModeControlContext<DarkModeTextBox> DarkModeContext { get; set; }
        public BorderStyle OriginalBorderStyle { get; private set; }
    }
}
