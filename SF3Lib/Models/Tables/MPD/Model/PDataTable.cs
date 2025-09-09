using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;

namespace SF3.Models.Tables.MPD.Model {
    public class PDataTable : AddressedTable<PDataModel> {
        public struct PDataRef {
            public int Address;
            public ModelCollectionType Collection;
            public int? ChunkIndex;
            public int Index;
            public int RefCount;
        }

        protected PDataTable(IByteData data, string name, PDataRef[] refs) : base(data, name, refs.Select(x => x.Address).ToArray()) {
            Refs = refs;
        }

        public static PDataTable Create(IByteData data, string name, PDataRef[] refs)
            => CreateBase(() => new PDataTable(data, name, refs));

        public override bool Load() {
            return Load((id, address) => new PDataModel(
                Data, id, "PDATA_" + Refs[id].Collection.ToString() + "_" + id.ToString("D4"), address,
                Refs[id].Collection, Refs[id].ChunkIndex, Refs[id].Index, Refs[id].RefCount
            ));
        }

        private PDataRef[] Refs { get; }
    }
}
