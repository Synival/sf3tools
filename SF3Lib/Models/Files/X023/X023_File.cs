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

        private int GetShopPointersAddr() {
            switch (Scenario) {
                case ScenarioType.Scenario1:   return Data.GetDouble(0x2c5c) - RamAddress;
                case ScenarioType.Scenario2:   return Data.GetDouble(0x2dc0) - RamAddress;
                case ScenarioType.Scenario3:   return Data.GetDouble(0x2f70) - RamAddress;
                case ScenarioType.PremiumDisk: return Data.GetDouble(0x2f78) - RamAddress;
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

            var shopPtrsAddr = GetShopPointersAddr();
            if (shopPtrsAddr > 0) {
                tables.Add(ShopPointerTable = ShopPointerTable.Create(Data, nameof(ShopPointerTable), shopPtrsAddr));
                ShopItemTablesByAddress = ShopPointerTable
                    .Select(x => x.Shop)
                    .Distinct()
                    .OrderBy(x => x)
                    .Select((x, i) => ShopItemTable.Create(Data, $"{nameof(ShopItemTable)}{i:D2} (@0x{x:X8})", (int) x - RamAddress))
                    .ToDictionary(x => x.Address + RamAddress, x => x);
                tables.AddRange(ShopItemTablesByAddress.Values);
            }

            var blacksmithAddr = GetBlacksmithTableAddr();
            if (blacksmithAddr > 0)
                tables.Add(BlacksmithTable = BlacksmithTable.Create(Data, nameof(BlacksmithTable), blacksmithAddr));

            return tables;
        }

        public ShopPointerTable ShopPointerTable { get; private set; }
        public Dictionary<int, ShopItemTable> ShopItemTablesByAddress { get; private set; }
        public BlacksmithTable BlacksmithTable { get; private set; }
    }
}
