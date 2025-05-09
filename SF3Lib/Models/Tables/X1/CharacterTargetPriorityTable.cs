﻿using System;
using SF3.ByteData;
using SF3.Models.Structs.X1;

namespace SF3.Models.Tables.X1 {
    public class CharacterTargetPriorityTable : FixedSizeTable<CharacterTargetPriority> {
        protected CharacterTargetPriorityTable(IByteData data, string name, int address) : base(data, name, address, 0x40) {
        }

        public static CharacterTargetPriorityTable Create(IByteData data, string name, int address) {
            var newTable = new CharacterTargetPriorityTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new CharacterTargetPriority(Data, id, "CharacterTarget" + id.ToString("D2"), address));
    }
}
