using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Main;

namespace SF3.Win.Views.MPD {
    public class ModelSwitchGroupView : TabView {
        public ModelSwitchGroupView(string name, IMPD_File mpdFile, ModelSwitchGroup switchGroup, INameGetterContext nameGetterContext) : base(name) {
            MPD_File = mpdFile;
            _switchGroup = switchGroup;
            NameGetterContext = nameGetterContext;

            HeaderView = new DataModelView("Header", switchGroup, nameGetterContext, typeof(ModelSwitchGroup));
            ModelsVisibleWhenFlagOffView = new ModelIDTableView("Models Visible when Flag Off", MPD_File, switchGroup?.ModelInstancesVisibleWhenOffTable, nameGetterContext);
            ModelsVisibleWhenFlagOnView = new ModelIDTableView("Models Visible when Flag On", MPD_File, switchGroup?.ModelInstancesVisibleWhenOnTable,  nameGetterContext);
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

        public IMPD_File MPD_File { get; }
        public INameGetterContext NameGetterContext { get; }

        private ModelSwitchGroup _switchGroup = null;
        public ModelSwitchGroup SwitchGroup {
            get => _switchGroup;
            set {
                if (_switchGroup != value) {
                    _switchGroup = value;
                    HeaderView.Model = _switchGroup;
                    ModelsVisibleWhenFlagOffView.Table = _switchGroup?.ModelInstancesVisibleWhenOffTable;
                    ModelsVisibleWhenFlagOnView.Table = _switchGroup?.ModelInstancesVisibleWhenOnTable;
                }
            }
        }

        public DataModelView HeaderView { get; }
        public ModelIDTableView ModelsVisibleWhenFlagOffView { get; }
        public ModelIDTableView ModelsVisibleWhenFlagOnView { get; }
    }
}
