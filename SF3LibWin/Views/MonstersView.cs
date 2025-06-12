using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Tables.Shared;

namespace SF3.Win.Views {
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

            CreateChild(new TableView("Stats (1/2)", Model, NameGetterContext, displayGroups: ["Metadata", "Stats1"]));
            CreateChild(new TableView("Stats (2/2)", Model, NameGetterContext, displayGroups: ["Metadata", "Stats2"]));
            CreateChild(new TableView("Magic Res", Model, NameGetterContext, displayGroups: ["Metadata", "MagicRes"]));
            CreateChild(new TableView("Spells", Model, NameGetterContext, displayGroups: ["Metadata", "Spells"]));
            CreateChild(new TableView("Eq / Items", Model, NameGetterContext, displayGroups: ["Metadata", "Items"]));
            CreateChild(new TableView("Specials", Model, NameGetterContext, displayGroups: ["Metadata", "Specials"]));
            CreateChild(new TableView("(Unknowns 1)", Model, NameGetterContext, displayGroups: ["Metadata", "Unknown"]));
            CreateChild(new TableView("(Unknowns 2)", Model, NameGetterContext, displayGroups: ["Metadata", "LastPage"]));

            return Control;
        }

        public MonsterTable Model { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
