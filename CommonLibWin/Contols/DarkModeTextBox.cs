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
                        if (DarkModeContext.Enabled) {
                            // Must use BeginInvoke() to prevent call to RecreateHandle() that breaks everything
                            BeginInvoke(() => this.BorderStyle = BorderStyle.FixedSingle);
                        }
                        else {
                            // Must use BeginInvoke() to prevent call to RecreateHandle() that breaks everything
                            BeginInvoke(() => this.BorderStyle = OriginalBorderStyle);
                        }
                    };
                }

                DarkModeContext.Init();
            }
        }

        private DarkModeControlContext<DarkModeTextBox> DarkModeContext { get; set; }
        public BorderStyle OriginalBorderStyle { get; private set; }
    }
}
