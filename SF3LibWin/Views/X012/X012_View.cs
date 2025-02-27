using System.Windows.Forms;
using SF3.Models.Files.X012;
using SF3.Models.Tables;
using SF3.Models.Tables.X012;

namespace SF3.Win.Views.X012 {
    public class X012_View : TabView {
        public X012_View(string name, IX012_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.ClassTargetPriorityTables != null)
                CreateChild(new TableArrayView<ClassTargetPriorityTable>("Class Target Priorities", Model.ClassTargetPriorityTables, ngc));
            if (Model.UnknownPriorityTables != null)
                CreateChild(new TableArrayView<UnknownUInt8Table>("Unknown Tables", Model.UnknownPriorityTables, ngc));

            return Control;
        }

        public IX012_File Model { get; }
    }
}
