using SF3.ByteData;
using SF3.Models.Structs.X014;

namespace SF3.Models.Tables.X014 {
    public class Sc1BattleSceneFileIdTable : ResourceTable<BattleSceneFileIdSc1> {
        protected Sc1BattleSceneFileIdTable(IByteData data, string name, string resourceFile, int address, int size, int? globalIdStart)
        : base(data, name, resourceFile, address, size) {
            GlobalIdStart = globalIdStart;
        }

        public static Sc1BattleSceneFileIdTable Create(IByteData data, string name, string resourceFile, int address, int size, int? globalIdStart)
            => Create(() => new Sc1BattleSceneFileIdTable(data, name, resourceFile, address, size, globalIdStart));

        public override bool Load()
            => Load((id, name, address) => new BattleSceneFileIdSc1(Data, id, name, address, GlobalIdStart.HasValue ? (GlobalIdStart + id) : null));

        public int? GlobalIdStart { get; }
    }
}
