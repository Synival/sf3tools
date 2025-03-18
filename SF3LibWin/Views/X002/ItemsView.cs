using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Tables.X002;

namespace SF3.Win.Views.X002 {
    public class ItemsView : TabView {
        public ItemsView(
            string name, ItemTable model, INameGetterContext nameGetterContext,
            bool lazyLoad = true, TabAlignment tabAlignment = TabAlignment.Top
        ) : base(name, lazyLoad, tabAlignment) {
            Model = model;
            NameGetterContext = nameGetterContext;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(new TableView("Stats",   Model, NameGetterContext, displayGroups: ["Metadata", "Stats"]));
            CreateChild(new TableView("Bonuses", Model, NameGetterContext, displayGroups: ["Metadata", "Bonuses"]));
            CreateChild(new TableView("Flags",   Model, NameGetterContext, displayGroups: ["Metadata", "Flags"]));

            return Control;
        }

        public ItemTable Model { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
