﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.X019_Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.frmX019_Editor());
        }
    }
}
