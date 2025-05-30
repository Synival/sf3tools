﻿using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class CollisionBlockTable : FixedSizeTable<CollisionBlockRow> {
        protected CollisionBlockTable(IByteData data, string name, int address) : base(data, name, address, 16) {
        }

        public static CollisionBlockTable Create(IByteData data, string name, int address) {
            var newTable = new CollisionBlockTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new CollisionBlockRow(Data, id, "Y" + id.ToString("D2"), address));
    }
}
