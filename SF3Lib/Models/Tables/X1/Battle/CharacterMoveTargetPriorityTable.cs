using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class CharacterMoveTargetPriorityTable : FixedSizeTable<CharacterMoveTargetPriority> {
        protected CharacterMoveTargetPriorityTable(IByteData data, string name, int address) : base(data, name, address, 0x40) {
        }

        public static CharacterMoveTargetPriorityTable Create(IByteData data, string name, int address)
            => Create(() => new CharacterMoveTargetPriorityTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new CharacterMoveTargetPriority(Data, id, "MoveTargetPriority" + id.ToString("D2"), address));
    }
}
