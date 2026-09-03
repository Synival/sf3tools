using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables;

namespace SF3.Win.Views.X8PC {
    public class PCAnimationChunkView : TabView {
        public PCAnimationChunkView(string name, PolyChar model, INameGetterContext ngc) : base(name) {
            NameGetterContext = ngc;

            HeaderView        = new DataModelView("Header", model?.AnimationChunkHeader, ngc, typeof(PCAnimationChunkHeader));
            BoneKeyframesView = new TableView("Bone Keyframes", model?.BoneKeyframeTable, ngc, typeof(PCBoneKeyframeStruct));

            PosFramesView     = new BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<short>>("PosFrames", model, ngc, x => x?.BoneKeyframePosFrameTablesById?.Values?.ToArray());
            RotFramesView     = new BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<short>>("RotFrames", model, ngc, x => x?.BoneKeyframeRotFrameTablesById?.Values?.ToArray());
            ScaleFramesView   = new BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<short>>("ScaleFames", model, ngc, x => x?.BoneKeyframeScaleFrameTablesById?.Values?.ToArray());

            PosXView          = new BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<float>>("PosX", model, ngc, x => x?.BoneKeyframePosXTablesById?.Values?.ToArray());
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

            CreateChild(PosFramesView);
            CreateChild(RotFramesView);
            CreateChild(ScaleFramesView);

            CreateChild(PosXView);
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

                    PosFramesView.Model     = _model;
                    RotFramesView.Model     = _model;
                    ScaleFramesView.Model   = _model;

                    PosXView.Model          = _model;
                    RotWView.Model          = _model;
                    ScaleXView.Model        = _model;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }
        public DataModelView HeaderView { get; }
        public TableView BoneKeyframesView { get; }
        public BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<short>> PosFramesView { get; }
        public BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<short>> RotFramesView { get; }
        public BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<short>> ScaleFramesView { get; }
        public BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<float>> PosXView { get; }
        public BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<float>> RotWView { get; }
        public BaseModelTablesView<GenericDataStruct<float>, GenericFixedSizeTable<float>> ScaleXView { get; }
    }
}
