using System;
using System.Windows.Forms;
using SF3.Editor.Forms;
using SF3.Win;
using static SF3.Win.Utils.ObjectListViewUtils;

namespace SF3.Editor {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Fetch saved application state
            AppState.RetrieveAppState("SF3 Editor");

            // Add some special handles types to ObjectListView 
            RegisterNamedValues();

            Application.Run(new SF3EditorForm());
        }
    }
}