using SF3.ByteData;
using SF3.Models.Structs.X014;

namespace SF3.Models.Tables.X014 {
    public class SpellModelAnimationSubstitutionTable : FixedSizeTable<SpellModelAnimationSubstitution> {
        public SpellModelAnimationSubstitutionTable(IByteData data, string name, int address) : base(data, name, address, 0x06) {
        }

        public static SpellModelAnimationSubstitutionTable Create(IByteData data, string name, int address)
            => Create(() => new SpellModelAnimationSubstitutionTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new SpellModelAnimationSubstitution(Data, id, "SpellModelAniSub" + id.ToString("D2"), address));
    }
}
