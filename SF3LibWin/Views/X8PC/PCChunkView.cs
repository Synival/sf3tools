using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCChunkView : TabView {
        public PCChunkView(string name, PolyChar model, INameGetterContext ngc) : base(name) {
            NameGetterContext = ngc;

            HeaderChunkView      = new PCHeaderChunkView("Header Chunk", model, NameGetterContext);
            TextureChunkView     = new PCTexChunkView("Texture Chunk", model, NameGetterContext);
            ModelChunkView       = new PCModelChunkView("Model Chunk", model, NameGetterContext);
            AnimationChunkView   = new PCAnimationChunkView("Animation Chunk", model, NameGetterContext);
            AttackAnimChunksView = new PCAttackAnimChunksView("Attack Anim Chunks", model, NameGetterContext);

            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(HeaderChunkView);
            CreateChild(TextureChunkView);
            CreateChild(ModelChunkView);
            CreateChild(AnimationChunkView);
            CreateChild(AttackAnimChunksView);

            foreach (var tabObj in TabControl.TabPages) {
                var tab = tabObj as TabPage;
                if (tab != null) {
                    if (tab.Text == "Animation Chunk")
                        _animationChunkViewTab = tab;
                    else if (tab.Text == "Attack Anim Chunks")
                        _attackAnimChunksViewTab = tab;
                }
            }

            ShowHideTables();

            return Control;
        }

        private void ShowHideTables() {
            if (TabControl == null || TabControl.TabPages == null)
                return;

            var showAttackAnims = _model?.AttackAnimBoneKeyframesTables != null;
            if (showAttackAnims && !TabControl.TabPages.Contains(_attackAnimChunksViewTab))
                TabControl.TabPages.Insert(TabControl.TabPages.IndexOf(_animationChunkViewTab) + 1, _attackAnimChunksViewTab);
            else if (!showAttackAnims && TabControl.TabPages.Contains(_attackAnimChunksViewTab))
                TabControl.TabPages.Remove(_attackAnimChunksViewTab);
        }

        private PolyChar _model = null;

        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;

                    HeaderChunkView.Model      = _model;
                    TextureChunkView.Model     = _model;
                    ModelChunkView.Model       = _model;
                    AnimationChunkView.Model   = _model;
                    AttackAnimChunksView.Model = _model;
                }
            }
        }

        private TabPage _animationChunkViewTab;
        private TabPage _attackAnimChunksViewTab;

        public INameGetterContext NameGetterContext { get; }

        public PCHeaderChunkView HeaderChunkView { get; }
        public PCTexChunkView TextureChunkView { get; }
        public PCModelChunkView ModelChunkView { get; }
        public PCAnimationChunkView AnimationChunkView { get; }
        public PCAttackAnimChunksView AttackAnimChunksView { get; }
    }
}
