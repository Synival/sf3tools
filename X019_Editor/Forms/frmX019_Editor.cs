using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.Editors;
using SF3.Editors.X019;
using SF3.Loaders;
using SF3.NamedValues;
using SF3.Types;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X019_Editor.Forms {
    public partial class frmX019_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.16";

        public IX019_Editor Editor => base.FileLoader.Editor as IX019_Editor;

        public frmX019_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter
            => ((Scenario == ScenarioType.PremiumDisk)
                    ? "SF3 Data (X019.BIN;X044.BIN)|X019.BIN;X044.BIN|"
                    : "SF3 Data (X019.BIN)|X019.BIN|")
                + base.FileDialogFilter;

        protected override IBaseEditor MakeEditor(IFileLoader loader)
            => Editors.X019.X019_Editor.Create(loader.RawEditor, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabMonsterTab1, olvMonsterTab1, Editor.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab2, olvMonsterTab2, Editor.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab3, olvMonsterTab3, Editor.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab4, olvMonsterTab4, Editor.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab5, olvMonsterTab5, Editor.MonsterTable),
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);
    }
}
