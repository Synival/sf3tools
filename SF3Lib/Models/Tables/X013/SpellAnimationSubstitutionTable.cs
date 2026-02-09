using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SpellAnimationSubstitutionTable : FixedSizeTable<SpellAnimationSubstitution> {
        protected SpellAnimationSubstitutionTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static SpellAnimationSubstitutionTable Create(IByteData data, string name, int address, int size)
            => Create(() => new SpellAnimationSubstitutionTable(data, name, address, size));

        public override bool Load()
            => Load((id, address) => new SpellAnimationSubstitution(Data, id, $"SpellAniSub{id:D2}", address));
    }
}
