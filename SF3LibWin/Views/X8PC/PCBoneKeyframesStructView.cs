using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCBoneKeyframesStructView : TabView {
        public PCBoneKeyframesStructView(string name, PCBoneKeyframesStruct model, INameGetterContext ngc) : base(name) {
            NameGetterContext = ngc;

            HeaderView = new DataModelView("Header",       model,             ngc, typeof(PCBoneKeyframesStruct));
            PosView    = new TableView    ("Translations", model?.PosTable,   ngc, typeof(PCBoneKeyframePosStruct));
            RotView    = new TableView    ("Rotations",    model?.RotTable,   ngc, typeof(PCBoneKeyframeRotStruct));
            ScaleView  = new TableView    ("Scales",       model?.ScaleTable, ngc, typeof(PCBoneKeyframeScaleStruct));

            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = NameGetterContext;

            CreateChild(HeaderView);
            CreateChild(PosView);
            CreateChild(RotView);
            CreateChild(ScaleView);

            return Control;
        }

        private PCBoneKeyframesStruct _model = null;
        public PCBoneKeyframesStruct Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;

                    HeaderView.Model = _model;
                    PosView.Table    = _model?.PosTable;
                    RotView.Table    = _model?.RotTable;
                    ScaleView.Table  = _model?.ScaleTable;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }
        public DataModelView HeaderView { get; }
        public TableView PosView { get; }
        public TableView RotView { get; }
        public TableView ScaleView { get; }
    }
}
