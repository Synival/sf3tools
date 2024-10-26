using System;
using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.IconPointerEditor.Forms {
    public partial class frmIconPointerEditor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.11";

        new public IIconPointerFileEditor FileEditor => base.FileEditor as IIconPointerFileEditor;

        private bool _isX026 = false;

        public bool IsX026 {
            get => _isX026;
            set {
                _isX026 = value;
                tsmiScenario_X026Toggle.Checked = _isX026;
                UpdateTitle();
            }
        }

        public frmIconPointerEditor() {
            InitializeComponent();
            InitializeEditor(menuStrip1);
            IsX026 = false;
        }

        protected override string FileDialogFilter
            => (IsX026 ? "SF3 Data (X026.BIN)|X026.BIN|" : "SF3 Data (X011.BIN;X021.BIN)|X011.BIN;X021.BIN|") + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new IconPointerFileEditor(Scenario, IsX026);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>() {
                new PopulateTabConfig(tabSpellIcons, olvSpellIcons, FileEditor.SpellIconList),
                new PopulateTabConfig(tabItemIcons, olvItemIcons, FileEditor.ItemIconList)
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

        private void tsmiScenario_X026Toggle_Click(object sender, EventArgs e) => IsX026 = !IsX026;
    }
}
