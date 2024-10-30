using System;
using System.Windows.Forms;
using SF3.Editor.Extensions;

namespace SF3.X019_Editor {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            ObjectListViewExtensions.RegisterNamedValues();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.frmX019_Editor());
        }
    }
}
