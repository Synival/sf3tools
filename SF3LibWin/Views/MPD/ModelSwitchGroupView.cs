using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.Main;

namespace SF3.Win.Views.MPD {
    public class ModelSwitchGroupView : TabView {
        public ModelSwitchGroupView(string name, ModelSwitchGroup switchGroup, INameGetterContext nameGetterContext) : base(name) {
            _switchGroup = switchGroup;
            NameGetterContext = nameGetterContext;

            HeaderView = new DataModelView("Header", switchGroup, nameGetterContext, typeof(ModelSwitchGroup));
            ModelsVisibleWhenFlagOffView = new TableView("Models Visible when Flag Off", switchGroup?.ModelsVisibleWhenOff, nameGetterContext, typeof(ModelIDStruct));
            ModelsVisibleWhenFlagOnView = new TableView("Models Visible when Flag On",  switchGroup?.ModelsVisibleWhenOn,  nameGetterContext, typeof(ModelIDStruct));
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = NameGetterContext;
            CreateChild(HeaderView);
            CreateChild(ModelsVisibleWhenFlagOffView);
            CreateChild(ModelsVisibleWhenFlagOnView);
  
            return Control;
        }

        private ModelSwitchGroup _switchGroup = null;
        public ModelSwitchGroup SwitchGroup {
            get => _switchGroup;
            set {
                if (_switchGroup != value) {
                    _switchGroup = value;
                    HeaderView.Model = _switchGroup;
                    ModelsVisibleWhenFlagOffView.Table = _switchGroup?.ModelsVisibleWhenOff;
                    ModelsVisibleWhenFlagOnView.Table = _switchGroup?.ModelsVisibleWhenOn;
                }
            }
        }
        public INameGetterContext NameGetterContext { get; }

        public DataModelView HeaderView { get; }
        public TableView ModelsVisibleWhenFlagOffView { get; }
        public TableView ModelsVisibleWhenFlagOnView { get; }
    }
}
