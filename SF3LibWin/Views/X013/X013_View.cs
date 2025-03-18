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
            if (Model.FriendshipExpTable != null)
                CreateChild(new TableView("Friendship Exp", Model.FriendshipExpTable, ngc));
            if (Model.SupportTypeTable != null)
                CreateChild(new TableView("Support Types", Model.SupportTypeTable, ngc));
            if (Model.SupportStatsTable != null)
                CreateChild(new TableView("Support Stats", Model.SupportStatsTable, ngc));
            if (Model.SoulmateTable != null)
                CreateChild(new TableView("Soulmate Chances", Model.SoulmateTable, ngc));
            if (Model.SoulfailTable != null)
                CreateChild(new TableView("Soulmate Chance Fail", Model.SoulfailTable, ngc));
            if (Model.MagicBonusTable != null)
                CreateChild(new TableView("Magic Bonus", Model.MagicBonusTable, ngc));
            if (Model.CritModTable != null)
                CreateChild(new TableView("Crit Modifiers", Model.CritModTable, ngc));
            if (Model.SpecialChanceTable != null)
                CreateChild(new TableView("Special Chances", Model.SpecialChanceTable, ngc));
            if (Model.ExpLimitTable != null)
                CreateChild(new TableView("Exp Limit", Model.ExpLimitTable, ngc));
            if (Model.HealExpTable != null)
                CreateChild(new TableView("Heal Exp", Model.HealExpTable, ngc));
            if (Model.WeaponSpellRankTable != null)
                CreateChild(new TableView("Weapon Spell Ranks", Model.WeaponSpellRankTable, ngc));
            if (Model.StatusEffectTable != null)
                CreateChild(new TableView("Status Effects", Model.StatusEffectTable, ngc));

            return Control;
        }

        public IX013_File Model { get; }
    }
}
