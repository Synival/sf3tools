using System.Linq;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;

namespace SF3.Models.Tables.MPD.Model {
    public class PDataTable : AddressedTable<PDataStruct> {
        public struct PDataRef {
            public int Address;
            public MPD_CollectionType Collection;
            public int? ChunkIndex;
            public int Index;
            public int RefCount;
        }

        protected PDataTable(IByteData data, string name, IMPD_File mpdFile, PDataRef[] refs)
        : base(data, name, refs.Select(x => x.Address).ToArray()) {
            MPD_File = mpdFile;
            Refs = refs;
        }

        public static PDataTable Create(IByteData data, string name, IMPD_File mpdFile, PDataRef[] refs)
            => Create(() => new PDataTable(data, name, mpdFile, refs));

        public override bool Load() {
            return Load((id, address) => {
                var r = Refs[id];
                return new PDataStruct(
                    Data, id, "PDATA_" + r.Collection.ToString() + "_" + id.ToString("D4"), address,
                    r.Collection, MPD_File, r.ChunkIndex, r.Index, r.RefCount
               );
            });
        }

        public IMPD_File MPD_File { get; }
        private PDataRef[] Refs { get; }
    }
}
