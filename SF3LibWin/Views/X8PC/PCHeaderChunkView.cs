using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCHeaderChunkView : TabView {
        public PCHeaderChunkView(string name, PolyChar model, INameGetterContext ngc) : base(name) {
            NameGetterContext = ngc;
            ChunkDefView      = new TableView("Chunks", model?.Header?.ChunkDefTable, ngc, modelType: typeof(PCChunkDef));
            ExtraAnimChunkDefView = new TableView("Extra Anim. Chunks", model?.Header?.ExtraAnimChunkDefTable, ngc, modelType: typeof(PCChunkDef));
            Model             = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(ChunkDefView);
            CreateChild(ExtraAnimChunkDefView);

            foreach (var tabObj in TabControl.TabPages) {
                var tab = tabObj as TabPage;
                if (tab != null) {
                    if (tab.Text == "Chunks")
                        _chunkDefViewTab = tab;
                    else if (tab.Text == "Extra Anim. Chunks")
                        _extraAnimChunkDefViewTab = tab;
                }
            }

            ShowHideTables();

            return Control;
        }

        private void ShowHideTables() {
            if (TabControl == null || TabControl.TabPages == null)
                return;

            var showExtraAnims = _model?.Header?.ExtraAnimChunkDefTable != null;
            if (showExtraAnims && !TabControl.TabPages.Contains(_extraAnimChunkDefViewTab))
                TabControl.TabPages.Insert(TabControl.TabPages.IndexOf(_chunkDefViewTab) + 1, _extraAnimChunkDefViewTab);
            else if (!showExtraAnims && TabControl.TabPages.Contains(_extraAnimChunkDefViewTab))
                TabControl.TabPages.Remove(_extraAnimChunkDefViewTab);
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    ChunkDefView.Table          = _model?.Header?.ChunkDefTable;
                    ExtraAnimChunkDefView.Table = _model?.Header?.ExtraAnimChunkDefTable;

                    ShowHideTables();
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }
        public TableView ChunkDefView { get; }
        public TableView ExtraAnimChunkDefView { get; }

        public TabPage _chunkDefViewTab;
        public TabPage _extraAnimChunkDefViewTab;
    }
}
