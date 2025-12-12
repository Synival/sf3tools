using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;

namespace SF3.Models.Tables.MPD.Model {
    public class HeaderModelInstanceTable : TerminatedTable<HeaderModelInstance> {
        protected HeaderModelInstanceTable(IByteData data, CollectionType collection, string name, int address)
        : base(data, name, address, 4, null) {
            Collection = collection;
        }

        public static HeaderModelInstanceTable Create(IByteData data, CollectionType collection, string name, int address)
            => Create(() => new HeaderModelInstanceTable(data, collection, name, address));

        public override bool Load() {
            return Load(
                (id, address) => new HeaderModelInstance(Data, Collection, id, "ModelInstance" + id.ToString("D2"), address),
                (rows, lastRow) => lastRow.PData0 != 0x00,
                false);
        }

        public CollectionType Collection { get; }
    }
}
