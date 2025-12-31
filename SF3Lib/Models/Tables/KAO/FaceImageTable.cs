using SF3.ByteData;
using SF3.Models.Structs.KAO;

namespace SF3.Models.Tables.MPD.Animation {
    public class FaceImageTable : FixedSizeTable<FaceImage> {
        private static readonly string[] _names = new string[] {
            "Blink (Still)",
            "Blink 1",
            "Blink 2, 4",
            "Blink 3",
            "Talk 1",
            "Talk 2",
            "Talk 3",
            "Talk 4",
            "Talk 5",
            "Talk (Still)",
        };

        protected FaceImageTable(IByteData data, string name, FaceChunk chunk)
        : base(data, name, /* addresss not applicable! */ address: 0, 10) {
            Chunk = chunk;
        }

        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;

        public static FaceImageTable Create(IByteData data, string name, FaceChunk chunk)
            => Create(() => new FaceImageTable(data, name, chunk));

        public override bool Load() {
            return Load((id, address) => {
                var layer = (id == 0) ? 0 : (id < 4) ? 1 : 2;
                var index = (id == 0) ? 0 : (id < 4) ? (id - 1) : (id - 4);
                return new FaceImage(Data, id, layer, index, _names[id], Chunk);
            });
        }

        public FaceChunk Chunk { get; }
    }
}
