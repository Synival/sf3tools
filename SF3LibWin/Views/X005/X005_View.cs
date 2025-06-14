using System.Windows.Forms;
using SF3.Models.Files.X005;
using SF3.Win.Views.X1;

namespace SF3.Win.Views.X005 {
    public class X005_View : TabView {
        public X005_View(string name, IX005_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.CameraSettings != null)
                CreateChild(new DataModelView("Camera Settings", Model.CameraSettings, ngc));

            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX005_File Model { get; }
    }
}
