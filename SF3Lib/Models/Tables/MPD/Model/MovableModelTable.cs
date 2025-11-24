using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;

namespace SF3.Models.Tables.MPD.Model {
    public class MovableModelTable : TerminatedTable<MovableModel> {
        protected MovableModelTable(IByteData data, CollectionType collection, string name, int address)
        : base(data, name, address, 4, null) {
            Collection = collection;
        }

        public static MovableModelTable Create(IByteData data, CollectionType collection, string name, int address)
            => Create(() => new MovableModelTable(data, collection, name, address));

        public override bool Load() {
            return Load(
                (id, address) => new MovableModel(Data, Collection, id, "MovableModel" + id.ToString("D2"), address),
                (rows, lastRow) => lastRow.PData0 != 0x00,
                false);
        }

        public CollectionType Collection { get; }
    }
}
