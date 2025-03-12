using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Files.IconPointer;
using SF3.Models.Files.MPD;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win.Views;
using SF3.Win.Views.IconPointer;
using SF3.Win.Views.MPD;
using SF3.Win.Views.X1;
using static SF3.Win.Extensions.ObjectListViewExtensions;

namespace SF3Editor {
    public partial class frmSF3Editor : Form {
        private readonly Dictionary<ScenarioType, INameGetterContext> c_nameGetterContexts = Enum.GetValues<ScenarioType>()
            .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

        public frmSF3Editor() {
            RegisterNamedValues();

            SuspendLayout();
            InitializeComponent();

            var fileContainer = new TabView("File Container");
            _ = fileContainer.Create();
            var fileContainerControl = fileContainer.TabControl;
            fileContainerControl.Dock = DockStyle.Fill;
            Controls.Add(fileContainerControl);

            ResumeLayout();
        }
    }
}
