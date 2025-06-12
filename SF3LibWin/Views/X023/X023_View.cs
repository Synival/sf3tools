using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.X023;
using SF3.Models.Tables.X023;
using SF3.Types;

namespace SF3.Win.Views.X023 {
    public class X023_View : TabView {
        public X023_View(string name, IX023_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            if (Model.ShopItemsPointerTable != null)
                CreateChild(new TableView("Shop Item Pointers", Model.ShopItemsPointerTable, ngc));
            if (Model.ShopAutoDealsPointerTable != null)
                CreateChild(new TableView("Shop Auto Deal Pointers" + (Model.Scenario >= ScenarioType.Scenario3 ? " + Flags" : ""), Model.ShopAutoDealsPointerTable, ngc));
            if (Model.ShopHagglesPointerTable != null)
                CreateChild(new TableView("Shop Haggle Pointers", Model.ShopHagglesPointerTable, ngc));

            if (Model.ShopItemTablesByAddress?.Count > 0)
                CreateChild(new TableArrayView<ShopItemTable>("Shop Items", Model.ShopItemTablesByAddress.Values.ToArray(), ngc));
            if (Model.ShopAutoDealTablesByAddress?.Count > 0)
                CreateChild(new TableArrayView<ShopAutoDealTable>("Shop Auto Deals", Model.ShopAutoDealTablesByAddress.Values.ToArray(), ngc));
            if (Model.ShopHaggleTablesByAddress?.Count > 0)
                CreateChild(new TableArrayView<ShopHaggleTable>("Shop Haggles", Model.ShopHaggleTablesByAddress.Values.ToArray(), ngc));

            if (Model.BlacksmithTable != null)
                CreateChild(new TableView("Blacksmith", Model.BlacksmithTable, ngc));

            return Control;
        }

        public IX023_File Model { get; }
    }
}
