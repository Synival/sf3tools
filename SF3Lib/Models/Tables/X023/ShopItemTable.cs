﻿using System;
using SF3.ByteData;
using SF3.Models.Structs.X023;

namespace SF3.Models.Tables.X023 {
    public class ShopItemTable : TerminatedTable<ShopItem> {
        protected ShopItemTable(IByteData data, string name, int address) : base(data, name, address, 4, 300) {
        }

        public static ShopItemTable Create(IByteData data, string name, int address) {
            var newTable = new ShopItemTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ShopItem(Data, id, $"{nameof(ShopItem)}{id:D2}", address),
                (rows, prev) => prev.Item != 0,
                false);
    }
}
