using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Tables.X019;

namespace SF3.Win.Views.X019 {
    public class MonstersView : TabView {
        public MonstersView(
            string name, MonsterTable model, INameGetterContext nameGetterContext,
            bool lazyLoad = true, TabAlignment tabAlignment = TabAlignment.Top
        ) : base(name, lazyLoad, tabAlignment) {
            Model = model;
            NameGetterContext = nameGetterContext;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(new TableView("Page 1", Model, NameGetterContext, displayGroups: ["Metadata", "Page1"]));
            CreateChild(new TableView("Page 2", Model, NameGetterContext, displayGroups: ["Metadata", "Page2"]));
            CreateChild(new TableView("Page 3", Model, NameGetterContext, displayGroups: ["Metadata", "Page3"]));
            CreateChild(new TableView("Page 4", Model, NameGetterContext, displayGroups: ["Metadata", "Page4"]));
            CreateChild(new TableView("Page 5", Model, NameGetterContext, displayGroups: ["Metadata", "Page5"]));

            return Control;
        }

        public MonsterTable Model { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
