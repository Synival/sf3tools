using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCAnimationChunkView : TabView {
        public PCAnimationChunkView(string name, PolyChar model, INameGetterContext ngc) : base(name) {
            NameGetterContext = ngc;

            AnimationFrames   = new PCAnimationFrameView("Animation Frames", model, model?.AnimationFramesTable, ngc);

            HeaderView        = new DataModelView("Header", model?.AnimationChunkHeader, ngc, typeof(PCAnimationChunkHeader));
            BoneKeyframesView = new TableView("Bone Keyframes", model?.BoneKeyframeTable, ngc, typeof(PCBoneKeyframeStruct));

            PosView           = new BaseModelTablesView<PCBoneKeyframePosStruct, PCBoneKeyframePosTable>("Translations", model, ngc, x => x?.BoneKeyframePosTables);
            RotView           = new BaseModelTablesView<PCBoneKeyframeRotStruct, PCBoneKeyframeRotTable>("Rotations", model, ngc, x => x?.BoneKeyframeRotTables);
            ScaleView         = new BaseModelTablesView<PCBoneKeyframeScaleStruct, PCBoneKeyframeScaleTable>("Scales", model, ngc, x => x?.BoneKeyframeScaleTables);

            Model             = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = NameGetterContext;

            CreateChild(AnimationFrames);

            CreateChild(HeaderView);
            CreateChild(BoneKeyframesView);

            CreateChild(PosView);
            CreateChild(RotView);
            CreateChild(ScaleView);

            return Control;
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;

                    AnimationFrames.SetTable(_model, _model?.AnimationFramesTable);

                    HeaderView.Model        = _model?.AnimationChunkHeader;
                    BoneKeyframesView.Table = _model?.BoneKeyframeTable;

                    PosView.Model           = _model;
                    RotView.Model           = _model;
                    ScaleView.Model         = _model;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }

        public PCAnimationFrameView AnimationFrames { get; }
        public DataModelView HeaderView { get; }
        public TableView BoneKeyframesView { get; }
        public BaseModelTablesView<PCBoneKeyframePosStruct, PCBoneKeyframePosTable> PosView { get; }
        public BaseModelTablesView<PCBoneKeyframeRotStruct, PCBoneKeyframeRotTable> RotView { get; }
        public BaseModelTablesView<PCBoneKeyframeScaleStruct, PCBoneKeyframeScaleTable> ScaleView { get; }
    }
}
