using System.Collections.Generic;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X023;

namespace SF3.Models.Files.X023 {
    public interface IX023_File : IScenarioTableFile {
        ShopPointerTable ShopPointerTable { get; }
        Dictionary<int, ShopItemTable> ShopItemTablesByAddress { get; }
        BlacksmithTable BlacksmithTable { get; }
    }
}
