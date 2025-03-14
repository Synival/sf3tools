using System;
using System.Windows.Forms;
using SF3.Win;

namespace SF3.X014_Editor {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            if (Environment.OSVersion.Version.Major >= 6)
                if (!SetProcessDPIAware())
                    throw new NotSupportedException("SetProcessDPIAware()");

            AppState.RetrieveAppState("X014 Editor");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.frmX014_Editor());
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
