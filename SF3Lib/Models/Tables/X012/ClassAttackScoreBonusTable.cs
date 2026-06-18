using SF3.ByteData;
using SF3.Models.Structs.X012;

namespace SF3.Models.Tables.X012 {
    public class ClassAttackScoreBonusTable : TerminatedTable<ClassAttackScoreBonus> {
        protected ClassAttackScoreBonusTable(IByteData data, string name, int address)
        : base(data, name, address, 1, null) {
        }

        public static ClassAttackScoreBonusTable Create(IByteData data, string name, int address)
            => Create(() => new ClassAttackScoreBonusTable(data, name, address));

        public override bool Load() {
            return Load(
                (id, address) => new ClassAttackScoreBonus(Data, id, "ClassAtkScoreBonus" + id.ToString("D2"), address),
                (rows, model) => model.ClassID != 0xFF,
                false
            );
        }
    }
}
