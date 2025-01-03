using System.Windows.Forms;
using BrightIdeasSoftware;

namespace SF3.X1_Editor.Controls {
    public partial class BattleEditorControl : UserControl {
        public BattleEditorControl() {
            InitializeComponent();
        }

        public TabControl Tabs => this.tabMain;

        public TabPage TabHeader => this.tabHeader;
        public TabPage TabSlotTab1 => this.tabSlotTab1;
        public TabPage TabSlotTab2 => this.tabSlotTab2;
        public TabPage TabSlotTab3 => this.tabSlotTab3;
        public TabPage TabSlotTab4 => this.tabSlotTab4;
        public TabPage TabSpawnZones => this.tabSpawnZones;
        public TabPage TabAITargetPosition => this.tabAITargetPosition;
        public TabPage TabScriptedMovement => this.tabScriptedMovement;

        public ObjectListView OLVHeader => this.olvHeader;
        public ObjectListView OLVSlotTab1 => this.olvSlotTab1;
        public ObjectListView OLVSlotTab2 => this.olvSlotTab2;
        public ObjectListView OLVSlotTab3 => this.olvSlotTab3;
        public ObjectListView OLVSlotTab4 => this.olvSlotTab4;
        public ObjectListView OLVSpawnZones => this.olvSpawnZones;
        public ObjectListView OLVAITargetPosition => this.olvAITargetPosition;
        public ObjectListView OLVScriptedMovement => this.olvScriptedMovement;
    }
}
