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
                CreateChild(new TableView("Shop Pointers", Model.ShopItemsPointerTable, ngc));
            if (Model.ShopAutoDealsPointerTable != null)
                CreateChild(new TableView("Shop Auto Deal Pointers" + (Model.Scenario >= ScenarioType.Scenario3 ? " + Flag" : ""), Model.ShopAutoDealsPointerTable, ngc));
            if (Model.ShopDealsPointerTable != null)
                CreateChild(new TableView("Shop Deal Pointers", Model.ShopDealsPointerTable, ngc));

            if (Model.ShopItemTablesByAddress?.Count > 0)
                CreateChild(new TableArrayView<ShopItemTable>("Shop Items", Model.ShopItemTablesByAddress.Values.ToArray(), ngc));
            if (Model.ShopAutoDealTablesByAddress?.Count > 0)
                CreateChild(new TableArrayView<ShopAutoDealTable>("Shop Auto Deals", Model.ShopAutoDealTablesByAddress.Values.ToArray(), ngc));
            if (Model.ShopDealTablesByAddress?.Count > 0)
                CreateChild(new TableArrayView<ShopDealTable>("Shop Deals", Model.ShopDealTablesByAddress.Values.ToArray(), ngc));

            if (Model.BlacksmithTable != null)
                CreateChild(new TableView("Blacksmith", Model.BlacksmithTable, ngc));

            return Control;
        }

        public IX023_File Model { get; }
    }
}
