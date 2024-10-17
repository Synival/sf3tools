using BrightIdeasSoftware;
using SF3.Values;
using System;
using System.Windows.Forms;

namespace SF3.X002_Editor
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
                typeof(WeaponTypeValue),
                (Object model, OLVColumn column, Object value) => Editor.Utils.MakeNamedValueComboBox(WeaponTypeValue.ComboBoxValues)
            );

            // TODO: generic method to prevent copy + paste
            ObjectListView.EditorRegistry.Register(
                typeof(SpellValue),
                (Object model, OLVColumn column, Object value) => Editor.Utils.MakeNamedValueComboBox(SpellValue.ComboBoxValues)
            );

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.frmX002_Editor());
        }
    }
}
