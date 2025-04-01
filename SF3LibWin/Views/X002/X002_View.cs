using System.Windows.Forms;
using SF3.Models.Files.X002;

namespace SF3.Win.Views.X002 {
    public class X002_View : TabView {
        public X002_View(string name, IX002_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.ItemTable != null)
                CreateChild(new ItemsView("Items", Model.ItemTable, ngc));
            if (Model.SpellTable != null)
                CreateChild(new TableView("Spells", Model.SpellTable, ngc));
            if (Model.WeaponSpellTable != null)
                CreateChild(new TableView("Weapon Spells", Model.WeaponSpellTable, ngc));
            if (Model.LoadingTable != null)
                CreateChild(new TableView("Loading", Model.LoadingTable, ngc));
            if (Model.LoadedOverrideTable != null)
                CreateChild(new TableView("Loading Overrides", Model.LoadedOverrideTable, ngc));
            if (Model.StatBoostTable != null)
                CreateChild(new TableView("Stat Boosts", Model.StatBoostTable, ngc));
            if (Model.WeaponRankTable != null)
                CreateChild(new TableView("Weapon Rank Attack", Model.WeaponRankTable, ngc));
            if (Model.AttackResist != null)
                CreateChild(new DataModelView("Attack/Resist", Model.AttackResist, ngc));
            if (Model.WarpTable != null)
                CreateChild(new TableView("Warps (Scn1)", Model.WarpTable, ngc));

            return Control;
        }

        public IX002_File Model { get; }
    }
}
