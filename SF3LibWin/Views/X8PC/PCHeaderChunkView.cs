using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCHeaderChunkView : TabView {
        public PCHeaderChunkView(string name, PolyChar model, INameGetterContext ngc) : base(name) {
            NameGetterContext = ngc;
            ChunkDefView      = new TableView("Chunks", model?.Header?.ChunkDefTable, ngc, modelType: typeof(PCChunkDef));
            AttackAnimChunkDefView = new TableView("Attack Anim. Chunks", model?.Header?.AttackAnimChunkDefTable, ngc, modelType: typeof(PCChunkDef));
            Model             = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(ChunkDefView);
            CreateChild(AttackAnimChunkDefView);

            foreach (var tabObj in TabControl.TabPages) {
                var tab = tabObj as TabPage;
                if (tab != null) {
                    if (tab.Text == "Chunks")
                        _chunkDefViewTab = tab;
                    else if (tab.Text == "Attack Anim. Chunks")
                        _attackAnimChunkDefViewTab = tab;
                }
            }

            ShowHideTables();

            return Control;
        }

        private void ShowHideTables() {
            if (TabControl == null || TabControl.TabPages == null)
                return;

            var showAttackAnims = _model?.Header?.AttackAnimChunkDefTable != null;
            if (showAttackAnims && !TabControl.TabPages.Contains(_attackAnimChunkDefViewTab))
                TabControl.TabPages.Insert(TabControl.TabPages.IndexOf(_chunkDefViewTab) + 1, _attackAnimChunkDefViewTab);
            else if (!showAttackAnims && TabControl.TabPages.Contains(_attackAnimChunkDefViewTab))
                TabControl.TabPages.Remove(_attackAnimChunkDefViewTab);
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    ChunkDefView.Table           = _model?.Header?.ChunkDefTable;
                    AttackAnimChunkDefView.Table = _model?.Header?.AttackAnimChunkDefTable;

                    ShowHideTables();
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }
        public TableView ChunkDefView { get; }
        public TableView AttackAnimChunkDefView { get; }

        public TabPage _chunkDefViewTab;
        public TabPage _attackAnimChunkDefViewTab;
    }
}
