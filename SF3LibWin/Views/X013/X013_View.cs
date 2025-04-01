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
            if (Model.SpecialsTable != null)
                CreateChild(new TableView("Specials", Model.SpecialsTable, ngc));
            if (Model.SpecialEffectTable != null)
                CreateChild(new TableView("Special Effects (Scn3+)", Model.SpecialEffectTable, ngc));
            if (Model.FriendshipExp != null)
                CreateChild(new DataModelView("Friendship Exp", Model.FriendshipExp, ngc));
            if (Model.SoulmateTable != null)
                CreateChild(new TableView("Soulmate Chances", Model.SoulmateTable, ngc));
            if (Model.SoulFail != null)
                CreateChild(new DataModelView("Soulmate Chance Fail Exp Loss", Model.SoulFail, ngc));
            if (Model.CritMod != null)
                CreateChild(new DataModelView("Crit Vantages", Model.CritMod, ngc));
            if (Model.SpecialChances != null)
                CreateChild(new DataModelView("Special Chances", Model.SpecialChances, ngc));
            if (Model.ExpLimit != null)
                CreateChild(new DataModelView("Exp Limit", Model.ExpLimit, ngc));
            if (Model.HealExp != null)
                CreateChild(new DataModelView("Heal Exp", Model.HealExp, ngc));
            if (Model.SupportTypeTable != null)
                CreateChild(new TableView("Friendship Support Types", Model.SupportTypeTable, ngc));
            if (Model.SupportStatsTable != null)
                CreateChild(new TableView("Support Stats", Model.SupportStatsTable, ngc));
            if (Model.MagicBonusTable != null)
                CreateChild(new TableView("Magic Bonuses", Model.MagicBonusTable, ngc));
            if (Model.CritrateTable != null)
                CreateChild(new TableView("Crit/Count Rates", Model.CritrateTable, ngc));
            if (Model.WeaponSpellRankTable != null)
                CreateChild(new TableView("Weapon Rank Magic Bonuses", Model.WeaponSpellRankTable, ngc));
            if (Model.StatusEffectTable != null)
                CreateChild(new TableView("Status Effect Chances", Model.StatusEffectTable, ngc));

            return Control;
        }

        public IX013_File Model { get; }
    }
}
