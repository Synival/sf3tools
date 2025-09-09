using SF3.ByteData;
using SF3.Models.Structs.X014;

namespace SF3.Models.Tables.X014 {
    public class MPDBattleSceneIdTable : TerminatedTable<MPDBattleSceneId> {
        protected MPDBattleSceneIdTable(IByteData data, string name, int address) : base(data, name, address, 2, null) {
        }

        public static MPDBattleSceneIdTable Create(IByteData data, string name, int address)
            => Create(() => new MPDBattleSceneIdTable(data, name, address));

        public override bool Load() {
            return Load(
                (id, address) => new MPDBattleSceneId(Data, id, $"{nameof(MPDBattleSceneId)}{id:D2}", address),
                (rows, lastRow) => (uint) lastRow.MPDFileID != 0xFFFFFFFFu,
                false
            );
        }
    }
}
