using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using SF3.Types;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X019_Editor.Forms {
    public partial class frmX019_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.14";

        public new IX019_FileEditor FileEditor => base.FileEditor as IX019_FileEditor;

        public frmX019_Editor() {
            InitializeComponent();
            InitializeEditor();
        }

        protected override string FileDialogFilter
            => ((Scenario == ScenarioType.PremiumDisk)
                    ? "SF3 Data (X019.BIN;X044.BIN)|X019.BIN;X044.BIN|"
                    : "SF3 Data (X019.BIN)|X019.BIN|")
                + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new X019_FileEditor(Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabMonsterTab1, olvMonsterTab1, FileEditor.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab2, olvMonsterTab2, FileEditor.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab3, olvMonsterTab3, FileEditor.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab4, olvMonsterTab4, FileEditor.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab5, olvMonsterTab5, FileEditor.MonsterTable),
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);
    }
}
