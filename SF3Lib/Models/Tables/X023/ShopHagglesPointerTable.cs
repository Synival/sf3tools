﻿using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.X023;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Models.Tables.X023 {
    public class ShopHagglesPointerTable : TerminatedTable<ShopHagglesPointer> {
        protected ShopHagglesPointerTable(IByteData data, string name, string resourceFile, int address) : base(data, name, address, 4, 300) {
            Resources = GetValueNameDictionaryFromXML(resourceFile);
        }

        public static ShopHagglesPointerTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new ShopHagglesPointerTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ShopHagglesPointer(Data, id, Resources.TryGetValue(id, out var name) ? name : $"{nameof(ShopHagglesPointer)}{id:D2}", address),
                (rows, prev) => prev.ShopHaggles != 0,
                false);

        private Dictionary<int, string> Resources { get; }
    }
}
