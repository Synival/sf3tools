using System;
using System.Windows.Forms;
using SF3.Win.Extensions;

namespace SF3.IconPointerEditor {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.frmIconPointerEditor());
        }
    }
}
