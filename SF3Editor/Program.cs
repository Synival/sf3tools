using System;
using System.Windows.Forms;
using SF3.Win;

namespace SF3Editor {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            AppState.RetrieveAppState("SF3 Editor");

            Application.Run(new frmSF3Editor());
        }
    }
}