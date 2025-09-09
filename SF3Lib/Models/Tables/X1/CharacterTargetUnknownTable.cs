using SF3.ByteData;
using SF3.Models.Structs.X1;

namespace SF3.Models.Tables.X1 {
    public class CharacterTargetUnknownTable : FixedSizeTable<CharacterTargetUnknown> {
        protected CharacterTargetUnknownTable(IByteData data, string name, int address) : base(data, name, address, 0x40) {
        }

        public static CharacterTargetUnknownTable Create(IByteData data, string name, int address)
            => Create(() => new CharacterTargetUnknownTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new CharacterTargetUnknown(Data, id, "CharacterUnknown" + id.ToString("D2"), address));
    }
}
