using System.Collections.Generic;
using SF3.ModelLoaders;
using SF3.Models.Files;
using SF3.Models.Files.X019;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using static SF3.Win.Extensions.TabControlExtensions;

namespace SF3.X019_Editor.Forms {
    public partial class frmX019_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.16 (2025-02-26 dev)";

        public IX019_File File => base.FileLoader.Model as IX019_File;

        public frmX019_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter
            => ((Scenario == ScenarioType.PremiumDisk)
                    ? "SF3 Data (X019.BIN;X044.BIN)|X019.BIN;X044.BIN|"
                    : "SF3 Data (X019.BIN)|X019.BIN|")
                + base.FileDialogFilter;

        protected override IBaseFile CreateModel(IModelFileLoader loader)
            => X019_File.Create(loader.ByteData, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabMonsterTab1, olvMonsterTab1, File.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab2, olvMonsterTab2, File.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab3, olvMonsterTab3, File.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab4, olvMonsterTab4, File.MonsterTable),
                new PopulateOLVTabConfig(tabMonsterTab5, olvMonsterTab5, File.MonsterTable),
            });
        }
    }
}
