using System;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace CommonLib.Win.Controls {
    public class DarkModeTabPage : TabPage {
        public DarkModeTabPage() {}
        public DarkModeTabPage(string text) : base(text) {}

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<TabPage>(this);
                DarkModeContext.Init();
            }
        }

        private DarkModeControlContext<TabPage> DarkModeContext { get; set; }
    }
}
