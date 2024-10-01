using BrightIdeasSoftware;
using SF3.Values;
using System;
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
                (Object model, OLVColumn column, Object value) => Editor.Utils.MakeNamedValueComboBox(CharacterClassValue.ComboBoxValues)
            );

            // TODO: generic method to prevent copy + paste
            ObjectListView.EditorRegistry.Register(
                typeof(SexValue),
                (Object model, OLVColumn column, Object value) => Editor.Utils.MakeNamedValueComboBox(SexValue.ComboBoxValues)
            );

            // TODO: generic method to prevent copy + paste
            ObjectListView.EditorRegistry.Register(
                typeof(WeaponTypeValue),
                (Object model, OLVColumn column, Object value) => Editor.Utils.MakeNamedValueComboBox(WeaponTypeValue.ComboBoxValues)
            );

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.frmMain());
        }
    }
}
