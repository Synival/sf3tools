using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X023;
using SF3.Types;

namespace SF3.Models.Files.X023 {
    public class X023_File : ScenarioTableFile, IX023_File {
        public int RamAddress => 0x06078000;

        protected X023_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X023_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X023_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        private int GetShopItemPointersAddr() {
            switch (Scenario) {
                case ScenarioType.Scenario1:   return Data.GetDouble(0x2c5c) - RamAddress;
                case ScenarioType.Scenario2:   return Data.GetDouble(0x2dc0) - RamAddress;
                case ScenarioType.Scenario3:   return Data.GetDouble(0x2f70) - RamAddress;
                case ScenarioType.PremiumDisk: return Data.GetDouble(0x2f78) - RamAddress;
                default: return 0;
            }
        }

        private int GetShopAutoDealPointersAddr() {
            switch (Scenario) {
                case ScenarioType.Scenario1:   return Data.GetDouble(0x0f68) - RamAddress;
                case ScenarioType.Scenario2:   return Data.GetDouble(0x0f80) - RamAddress; // Same in both versions
                case ScenarioType.Scenario3:   return Data.GetDouble(0x10c8) - RamAddress;
                case ScenarioType.PremiumDisk: return Data.GetDouble(0x10d0) - RamAddress;
                default: return 0;
            }
        }

        private int GetShopDealPointersAddr() {
            switch (Scenario) {
                case ScenarioType.Scenario1:   return Data.GetDouble(0x1024) - RamAddress;
                case ScenarioType.Scenario2:   return Data.GetDouble(0x103c) - RamAddress; // Same in both versions
                case ScenarioType.Scenario3:   return Data.GetDouble(0x1184) - RamAddress;
                case ScenarioType.PremiumDisk: return Data.GetDouble(0x118c) - RamAddress;
                default: return 0;
            }
        }

        private int GetBlacksmithTableAddr() {
            switch (Scenario) {
                case ScenarioType.Scenario2:   return Data.GetDouble(0x4cb4) - RamAddress; // Same in both versions
                case ScenarioType.Scenario3:   return Data.GetDouble(0x4e24) - RamAddress;
                case ScenarioType.PremiumDisk: return Data.GetDouble(0x4e2c) - RamAddress;
                default: return 0;
            }
        }

        public override IEnumerable<ITable> MakeTables() {            
            var tables = new List<ITable>();

            var shopItemPtrsAddr = GetShopItemPointersAddr();
            if (shopItemPtrsAddr > 0) {
                tables.Add(ShopItemsPointerTable = ShopItemsPointerTable.Create(Data, nameof(ShopItemsPointerTable), shopItemPtrsAddr));
                ShopItemTablesByAddress = ShopItemsPointerTable
                    .Select(x => x.ShopItems)
                    .Distinct()
                    .OrderBy(x => x)
                    .Select((x, i) => ShopItemTable.Create(Data, $"{nameof(ShopItemTable)}{i:D2} (@0x{x:X8})", (int) x - RamAddress))
                    .ToDictionary(x => x.Address + RamAddress, x => x);
                tables.AddRange(ShopItemTablesByAddress.Values);
            }

            var shopAutoDealPtrsAddr = GetShopAutoDealPointersAddr();
            if (shopAutoDealPtrsAddr > 0) {
                var flagOffset = (Scenario >= ScenarioType.Scenario3) ? RamAddress : (int?) null;
                tables.Add(ShopAutoDealsPointerTable = ShopAutoDealsPointerTable.Create(Data, nameof(ShopAutoDealsPointerTable), shopAutoDealPtrsAddr, flagOffset));
                ShopAutoDealTablesByAddress = ShopAutoDealsPointerTable
                    .Select(x => x.ShopAutoDeals)
                    .Distinct()
                    .OrderBy(x => x)
                    .Select((x, i) => ShopAutoDealTable.Create(Data, $"{nameof(ShopAutoDealTable)}{i:D2} (@0x{x:X8})", (int) x - RamAddress + (flagOffset.HasValue ? 0x04 : 0x00), !flagOffset.HasValue))
                    .ToDictionary(x => x.Address + RamAddress, x => x);
                tables.AddRange(ShopAutoDealTablesByAddress.Values);
            }

            var shopDealPtrsAddr = GetShopDealPointersAddr();
            if (shopDealPtrsAddr > 0) {
                // For some reason, there's always a normal shop in this list... Let's filter it out.
                var shopPtrs = ShopItemsPointerTable?.Select(x => x.ShopItems)?.Distinct()?.ToArray() ?? new uint[] {};
                var shopPtrSet = new HashSet<uint>(shopPtrs);

                tables.Add(ShopDealsPointerTable = ShopDealsPointerTable.Create(Data, nameof(ShopDealsPointerTable), shopDealPtrsAddr));
                ShopDealTablesByAddress = ShopDealsPointerTable
                    .Select(x => x.ShopDeals)
                    .Where(x => !shopPtrSet.Contains(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .Select((x, i) => ShopDealTable.Create(Data, $"{nameof(ShopDealTable)}{i:D2} (@0x{x:X8})", (int) x - RamAddress))
                    .ToDictionary(x => x.Address + RamAddress, x => x);
                tables.AddRange(ShopDealTablesByAddress.Values);
            }

            var blacksmithAddr = GetBlacksmithTableAddr();
            if (blacksmithAddr > 0)
                tables.Add(BlacksmithTable = BlacksmithTable.Create(Data, nameof(BlacksmithTable), blacksmithAddr));

            return tables;
        }

        public ShopItemsPointerTable ShopItemsPointerTable { get; private set; }
        public Dictionary<int, ShopItemTable> ShopItemTablesByAddress { get; private set; }
        public ShopAutoDealsPointerTable ShopAutoDealsPointerTable { get; private set; }
        public Dictionary<int, ShopAutoDealTable> ShopAutoDealTablesByAddress { get; private set; }
        public ShopDealsPointerTable ShopDealsPointerTable { get; private set; }
        public Dictionary<int, ShopDealTable> ShopDealTablesByAddress { get; private set; }
        public BlacksmithTable BlacksmithTable { get; private set; }
    }
}
