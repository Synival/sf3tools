using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.ModelLoaders;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win.Views;
using SF3.Win.Views.X1;
using static CommonLib.Win.Utils.MessageUtils;
using static SF3.Win.Extensions.ObjectListViewExtensions;

namespace SF3Editor {
    public partial class frmSF3Editor : Form {
        private readonly Dictionary<ScenarioType, INameGetterContext> c_nameGetterContexts = Enum.GetValues<ScenarioType>()
            .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

        private readonly string FileDialogFilter =
            "All Supported Files|X1*.BIN;X002.BIN;X005.BIN;X012.BIN;X013.BIN;X014.BIN;X019.BIN;X031.BIN;X033.BIN;*.MPD"
            + "|X1 Files (X1*.BIN)|X1*.BIN"
            + "|X002 File (X002.BIN)|X002.BIN"
            + "|X005 File (X005.BIN)|X005.BIN"
            + "|X013 File (X013.BIN)|X013.BIN"
            + "|X014 File (X014.BIN)|X014.BIN"
            + "|X019 File (X019.BIN)|X019.BIN"
            + "|X031 File (X031.BIN)|X031.BIN"
            + "|X033 File (X033.BIN)|X033.BIN"
            + "|MPD File (*.MPD)|*.MPD"
            + "|All Files (*.*)|*.*"
            ;

        public frmSF3Editor() {
            RegisterNamedValues();

            SuspendLayout();
            InitializeComponent();

            FileContainerView = new TabView("File Container");
            _ = FileContainerView.Create();
            var fileContainerControl = FileContainerView.TabControl;
            Controls.Add(fileContainerControl);
            fileContainerControl.Dock = DockStyle.Fill;
            fileContainerControl.BringToFront(); // If this isn't in the front, the menu is placed behind it (eep)

            ResumeLayout();
        }

        /// <summary>
        /// Creates an "Open" dialog and, if a file was chosen, opens it, processes its data, and loads it.
        /// </summary>
        public bool OpenFileDialog() {
            var openfile = new OpenFileDialog {
                Filter = FileDialogFilter
            };
            if (openfile.ShowDialog() != DialogResult.OK)
                return false;

            return LoadFile(openfile.FileName);
        }

        private bool LoadFile(string filename) {
            try {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    return LoadFile(filename, stream);
            }
            catch (Exception e) {
                ErrorMessage("Error trying to load file:\n\n" + e.Message);
                return false;
            }
        }

        private bool LoadFile(string filename, Stream stream) {
            var fileLoader = new ModelFileLoader();
            bool success = fileLoader.LoadFile(filename, loader => {
                // TODO: more than just X1 files.
                // TODO: get scenario.
                return X1_File.Create(loader.ByteData, c_nameGetterContexts[ScenarioType.Scenario1], ScenarioType.Scenario1, false);
            });

            if (!success) {
                // Wrong file was selected.
                ErrorMessage("Data in '" + filename + "' appears corrupt or invalid.\n" +
                             "Is this the correct type of file?");
                fileLoader.Close();
                return false;
            }

            // Create a view for the file.
            // TODO: not just for an X1 file!!
            var view = (IView) new X1_View(fileLoader.ShortFilename, (X1_File) fileLoader.Model);
            FileContainerView.CreateChild(view, control => {
                // Focus the first control. Drill downward through control containers
                // to find the bottom-most control.
                var focusView = view;
                var focusControl = control;
                while (focusControl != null && focusView is IContainerView cc) {
                    var firstChild = cc.ChildViews.FirstOrDefault();
                    var firstChildControl = firstChild?.Control;
                    if (firstChildControl == null)
                        break;

                    focusView = firstChild;
                    focusControl = firstChildControl;
                }
                if (focusControl == null)
                    focusControl = this;

                focusControl.Focus();
            });

            return true;
        }

        private void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();
        private void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();

        private TabView FileContainerView { get; }
    }
}
