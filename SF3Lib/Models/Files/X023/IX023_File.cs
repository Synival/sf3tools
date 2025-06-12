using System.Collections.Generic;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X023;

namespace SF3.Models.Files.X023 {
    public interface IX023_File : IScenarioTableFile {
        ShopItemsPointerTable ShopItemsPointerTable { get; }
        Dictionary<int, ShopItemTable> ShopItemTablesByAddress { get; }
        ShopAutoDealsPointerTable ShopAutoDealsPointerTable { get; }
        Dictionary<int, ShopAutoDealTable> ShopAutoDealTablesByAddress { get; }
        ShopHagglesPointerTable ShopHagglesPointerTable { get; }
        Dictionary<int, ShopHaggleTable> ShopHaggleTablesByAddress { get; }
        BlacksmithTable BlacksmithTable { get; }
    }
}
