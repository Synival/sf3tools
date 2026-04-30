using SF3.ByteData;
using SF3.Models.Structs.X1;

namespace SF3.Models.Tables.X1.Battle {
    public class CharacterAttackScoreBonusTable : FixedSizeTable<CharacterAttackScoreBonus> {
        protected CharacterAttackScoreBonusTable(IByteData data, string name, int address) : base(data, name, address, 0x40) {
        }

        public static CharacterAttackScoreBonusTable Create(IByteData data, string name, int address)
            => Create(() => new CharacterAttackScoreBonusTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new CharacterAttackScoreBonus(Data, id, "CharacterAttackScoreBonus" + id.ToString("D2"), address));
    }
}
