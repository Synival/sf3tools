using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.X023;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Models.Tables.X023 {
    public class ShopAutoDealsPointerTable : TerminatedTable<ShopAutoDealsPointer> {
        protected ShopAutoDealsPointerTable(IByteData data, string name, string resourceFile, int address, int? hasFlagOffset) : base(data, name, address, 4, 300) {
            Resources = GetValueNameDictionaryFromXML(resourceFile);
            HasFlagOffset = hasFlagOffset;
        }

        public static ShopAutoDealsPointerTable Create(IByteData data, string name, string resourceFile, int address, int? hasFlagOffset)
            => CreateBase(() => new ShopAutoDealsPointerTable(data, name, resourceFile, address, hasFlagOffset));

        public override bool Load()
            => Load((id, address) => new ShopAutoDealsPointer(Data, id, Resources.TryGetValue(id, out var name) ? name : $"{nameof(ShopAutoDealsPointer)}{id:D2}", address, HasFlagOffset),
                (rows, prev) => prev.ShopAutoDeals != 0,
                false);

        public int? HasFlagOffset { get; }
        private Dictionary<int, string> Resources { get; }
    }
}
