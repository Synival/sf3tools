using SF3.ByteData;
using SF3.Models.Structs.X014;

namespace SF3.Models.Tables.X014 {
    public class SpecialModelAnimationSubstitutionTable : FixedSizeTable<SpecialModelAnimationSubstitution> {
        public SpecialModelAnimationSubstitutionTable(IByteData data, string name, int address) : base(data, name, address, 0x7a) {
        }

        public static SpecialModelAnimationSubstitutionTable Create(IByteData data, string name, int address)
            => Create(() => new SpecialModelAnimationSubstitutionTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new SpecialModelAnimationSubstitution(Data, id, "SpecModelAniSub" + id.ToString("D2"), address));
    }
}
