using System;
using System.Windows.Forms;

namespace SF3.X1_Editor {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.frmX1_Editor());
        }
    }
}
