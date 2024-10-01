using BrightIdeasSoftware;
using SF3.Editor;
using SF3.Editor.Values;
using SF3.X033_X031_Editor.Models.Items;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.X033_X031_Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // TODO: generic method to prevent copy + paste
            ObjectListView.EditorRegistry.Register(
                typeof(CharacterClassValue),
                (Object model, OLVColumn column, Object value) => Utils.MakeNamedValueComboBox(CharacterClassValue.ComboBoxValues)
            );

            // TODO: generic method to prevent copy + paste
            ObjectListView.EditorRegistry.Register(
                typeof(SexValue),
                (Object model, OLVColumn column, Object value) => Utils.MakeNamedValueComboBox(SexValue.ComboBoxValues)
            );

            // TODO: generic method to prevent copy + paste
            ObjectListView.EditorRegistry.Register(
                typeof(WeaponTypeValue),
                (Object model, OLVColumn column, Object value) => Utils.MakeNamedValueComboBox(WeaponTypeValue.ComboBoxValues)
            );

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.frmMain());
        }
    }
}
