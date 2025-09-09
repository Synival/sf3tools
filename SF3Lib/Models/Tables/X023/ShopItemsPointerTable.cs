using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.X023;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Models.Tables.X023 {
    public class ShopItemsPointerTable : TerminatedTable<ShopItemsPointer> {
        protected ShopItemsPointerTable(IByteData data, string name, string resourceFile, int address) : base(data, name, address, 4, 300) {
            Resources = GetValueNameDictionaryFromXML(resourceFile);
        }

        public static ShopItemsPointerTable Create(IByteData data, string name, string resourceFile, int address)
            => Create(() => new ShopItemsPointerTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, address) => new ShopItemsPointer(Data, id, Resources.TryGetValue(id, out var name) ? name : $"{nameof(ShopItemsPointer)}{id:D2}", address),
                (rows, prev) => prev.ShopItems != 0,
                false);

        private Dictionary<int, string> Resources { get; }
    }
}
