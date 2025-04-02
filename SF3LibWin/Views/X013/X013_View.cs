using System.Windows.Forms;
using SF3.Models.Files.X013;

namespace SF3.Win.Views.X013 {
    public class X013_View : TabView {
        public X013_View(string name, IX013_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.SignificantValues != null)
                CreateChild(new DataModelView("Significant Values", Model.SignificantValues, ngc));
            if (Model.SpecialsTable != null)
                CreateChild(new TableView("Specials", Model.SpecialsTable, ngc));
            if (Model.SpecialEffectTable != null)
                CreateChild(new TableView("Special Status Effects (Scn3+)", Model.SpecialEffectTable, ngc));
            if (Model.SoulmateTable != null)
                CreateChild(new TableView("Soulmate Chances", Model.SoulmateTable, ngc));
            if (Model.SupportTypeTable != null)
                CreateChild(new TableView("Friendship Support Types", Model.SupportTypeTable, ngc));
            if (Model.SupportStatsTable != null)
                CreateChild(new TableView("Support Stats", Model.SupportStatsTable, ngc));
            if (Model.MagicBonusTable != null)
                CreateChild(new TableView("Magic Bonuses", Model.MagicBonusTable, ngc));
            if (Model.CritrateTable != null)
                CreateChild(new TableView("Crit/Counter Rates", Model.CritrateTable, ngc));
            if (Model.WeaponSpellRankTable != null)
                CreateChild(new TableView("Weapon Rank Magic Bonuses", Model.WeaponSpellRankTable, ngc));
            if (Model.StatusEffectTable != null)
                CreateChild(new TableView("Status Effect Chances", Model.StatusEffectTable, ngc));

            return Control;
        }

        public IX013_File Model { get; }
    }
}
