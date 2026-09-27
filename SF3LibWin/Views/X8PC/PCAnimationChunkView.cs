using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCAnimationChunkView : TabView {
        public PCAnimationChunkView(string name, PolyChar model, INameGetterContext ngc) : base(name) {
            NameGetterContext = ngc;

            HeaderView        = new DataModelView("Header", model?.AnimationChunkHeader, ngc, typeof(PCAnimationChunkHeader));
            AnimationDefView  = new TableView("Animations", model?.AnimationChunkHeader?.AnimationDefTable, ngc, typeof(PCAnimationDefStruct));
            BoneKeyframesView = new PCBoneKeyframesTableView("Bone Keyframes", model?.BoneKeyframesTable, ngc);

            Model             = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = NameGetterContext;

            CreateChild(HeaderView);
            CreateChild(AnimationDefView);
            CreateChild(BoneKeyframesView);

            return Control;
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;

                    HeaderView.Model        = _model?.AnimationChunkHeader;
                    AnimationDefView.Table  = _model?.AnimationChunkHeader?.AnimationDefTable;
                    BoneKeyframesView.Table = _model?.BoneKeyframesTable;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }

        public DataModelView HeaderView { get; }
        public TableView AnimationDefView { get; }
        public PCBoneKeyframesTableView BoneKeyframesView { get; }
    }
}
