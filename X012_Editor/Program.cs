using System;
using System.Windows.Forms;
using SF3.Win;

namespace SF3.X012_Editor {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Fetch saved application state
            AppState.RetrieveAppState("X012 Editor");

            Application.Run(new Forms.frmX012_Editor());
        }
    }
}
