using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class CharacterTargetUnknownTable : FixedSizeTable<CharacterTargetUnknown> {
        protected CharacterTargetUnknownTable(IByteData data, string name, int address) : base(data, name, address, 0x40) {
        }

        public static CharacterTargetUnknownTable Create(IByteData data, string name, int address) {
            var newTable = new CharacterTargetUnknownTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new CharacterTargetUnknown(Data, id, "CharacterUnknown" + id.ToString("D2"), address));
    }
}
