using System;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;
using DFRLib.Win.Forms;

namespace DFRTool.GUI {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DarkModeContext.Observable = new StandaloneDarkModeObservable(true);
            Application.Run(new frmDFRTool());
        }
    }
}