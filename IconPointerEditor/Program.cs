﻿using SF3.Editor.Extensions;
using System;
using System.Windows.Forms;

namespace SF3.IconPointerEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ObjectListViewExtensions.RegisterSF3Values();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.frmIconPointerEditor());
        }
    }
}
