using System.Linq;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;

namespace SF3.Models.Tables.MPD.Model {
    public class MPD_SGL_Model_PDataTable : AddressedTable<MPD_SGL_Model_PDataStruct> {
        public struct PDataRef {
            public int Address;
            public MPD_CollectionType Collection;
            public int? ChunkIndex;
            public int ModelID;
            public int LevelOfDetail;
            public int RefCount;
        }

        protected MPD_SGL_Model_PDataTable(IByteData data, string name, IMPD_File mpdFile, PDataRef[] refs)
        : base(data, name, refs.Select(x => x.Address).ToArray()) {
            MPD_File = mpdFile;
            Refs = refs;
        }

        public static MPD_SGL_Model_PDataTable Create(IByteData data, string name, IMPD_File mpdFile, PDataRef[] refs)
            => Create(() => new MPD_SGL_Model_PDataTable(data, name, mpdFile, refs));

        public override bool Load() {
            return Load((id, address) => {
                var r = Refs[id];
                return new MPD_SGL_Model_PDataStruct(
                    Data, id, "PDATA_" + r.Collection.ToString() + "_" + id.ToString("D4"), address,
                    r.Collection, MPD_File, r.ChunkIndex, r.ModelID, r.LevelOfDetail, r.RefCount
               );
            });
        }

        public IMPD_File MPD_File { get; }
        private PDataRef[] Refs { get; }
    }
}
