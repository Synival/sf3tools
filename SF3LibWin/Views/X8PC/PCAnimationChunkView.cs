using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCAnimationChunkView : TabView {
        public PCAnimationChunkView(string name, PolyChar model, INameGetterContext ngc) : base(name) {
            NameGetterContext = ngc;

            HeaderView        = new DataModelView("Header", model?.AnimationChunkHeader, ngc, typeof(PCAnimationChunkHeader));
            BoneKeyframesView = new TableView("Bone Keyframes", model?.BoneKeyframeTable, ngc, typeof(PCBoneKeyframeStruct));

            PosView           = new BaseModelTablesView<PCBoneKeyframePosStruct, PCBoneKeyframePosTable>("Pos", model, ngc, x => x?.BoneKeyframePosTable?.Values?.ToArray());

            RotFramesView     = new BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<short>>("RotFrames", model, ngc, x => x?.BoneKeyframeRotFrameTablesById?.Values?.ToArray());
            ScaleFramesView   = new BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<short>>("ScaleFames", model, ngc, x => x?.BoneKeyframeScaleFrameTablesById?.Values?.ToArray());

            RotWView          = new BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<float>>("RotW", model, ngc, x => x?.BoneKeyframeRotWTablesById?.Values?.ToArray());
            ScaleXView        = new BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<float>>("ScaleX", model, ngc, x => x?.BoneKeyframeScaleXTablesById?.Values?.ToArray());

            Model             = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = NameGetterContext;

            CreateChild(HeaderView);
            CreateChild(BoneKeyframesView);

            CreateChild(PosView);

            CreateChild(RotFramesView);
            CreateChild(ScaleFramesView);

            CreateChild(RotWView);
            CreateChild(ScaleXView);

            return Control;
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    HeaderView.Model        = _model?.AnimationChunkHeader;
                    BoneKeyframesView.Table = _model?.BoneKeyframeTable;

                    PosView.Model           = _model;

                    RotFramesView.Model     = _model;
                    ScaleFramesView.Model   = _model;

                    RotWView.Model          = _model;
                    ScaleXView.Model        = _model;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }
        public DataModelView HeaderView { get; }
        public TableView BoneKeyframesView { get; }
        public BaseModelTablesView<PCBoneKeyframePosStruct, PCBoneKeyframePosTable> PosView { get; }
        public BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<short>> RotFramesView { get; }
        public BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<short>> ScaleFramesView { get; }
        public BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<float>> RotWView { get; }
        public BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<float>> ScaleXView { get; }
    }
}
