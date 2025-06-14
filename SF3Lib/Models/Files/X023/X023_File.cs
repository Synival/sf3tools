using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X023;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X023 {
    public class X023_File : ScenarioTableFile, IX023_File {
        public override int RamAddress => 0x06078000;
        public override int RamAddressLimit => 0x06080000;

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

        private int GetShopHagglePointersAddr() {
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
                tables.Add(ShopItemsPointerTable = ShopItemsPointerTable.Create(Data, nameof(ShopItemsPointerTable), ResourceFileForScenario(Scenario, "Shops.xml"), shopItemPtrsAddr));
                var namesByAddr = ShopItemsPointerTable
                    .GroupBy(x => x.ShopItems)
                    .ToDictionary(x => x.Key, x => string.Join(", ", x.OrderBy(y => y.ID).Select(y => y.Name)));

                ShopItemTablesByAddress = namesByAddr
                    .Select(x => x.Key)
                    .Select((x, i) => ShopItemTable.Create(Data, $"0x{x:X8}: {namesByAddr[x]}", (int) x - RamAddress))
                    .ToDictionary(x => x.Address + RamAddress, x => x);
                tables.AddRange(ShopItemTablesByAddress.Values);
            }

            var shopAutoDealPtrsAddr = GetShopAutoDealPointersAddr();
            if (shopAutoDealPtrsAddr > 0) {
                var flagOffset = (Scenario >= ScenarioType.Scenario3) ? RamAddress : (int?) null;
                tables.Add(ShopAutoDealsPointerTable = ShopAutoDealsPointerTable.Create(Data, nameof(ShopAutoDealsPointerTable), ResourceFileForScenario(Scenario, "ShopDeals.xml"), shopAutoDealPtrsAddr, flagOffset));
                var namesByAddr = ShopAutoDealsPointerTable
                    .GroupBy(x => x.ShopAutoDeals)
                    .ToDictionary(x => x.Key, x => string.Join(", ", x.OrderBy(y => y.ID).Select(y => y.Name)));

                ShopAutoDealTablesByAddress = namesByAddr
                    .Select(x => x.Key)
                    .Select((x, i) => ShopAutoDealTable.Create(Data, $"0x{x:X8}: {namesByAddr[x]}", (int) x - RamAddress + (flagOffset.HasValue ? 0x04 : 0x00), !flagOffset.HasValue))
                    .ToDictionary(x => x.Address + RamAddress, x => x);
                tables.AddRange(ShopAutoDealTablesByAddress.Values);
            }

            var shopHagglePtrsAddr = GetShopHagglePointersAddr();
            if (shopHagglePtrsAddr > 0) {
                tables.Add(ShopHagglesPointerTable = ShopHagglesPointerTable.Create(Data, nameof(ShopHagglesPointerTable), ResourceFileForScenario(Scenario, "ShopDeals.xml"), shopHagglePtrsAddr));
                var namesByAddr = ShopHagglesPointerTable
                    .GroupBy(x => x.ShopHaggles)
                    .ToDictionary(x => x.Key, x => string.Join(", ", x.OrderBy(y => y.ID).Select(y => y.Name)));

                ShopHaggleTablesByAddress = namesByAddr
                    .Select(x => x.Key)
                    .Select((x, i) => ShopHaggleTable.Create(Data, $"0x{x:X8}: {namesByAddr[x]}", (int) x - RamAddress))
                    .ToDictionary(x => x.Address + RamAddress, x => x);
                tables.AddRange(ShopHaggleTablesByAddress.Values);
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
        public ShopHagglesPointerTable ShopHagglesPointerTable { get; private set; }
        public Dictionary<int, ShopHaggleTable> ShopHaggleTablesByAddress { get; private set; }
        public BlacksmithTable BlacksmithTable { get; private set; }
    }
}
