using SF3.ByteData;
using SF3.Models.Structs.KAO;

namespace SF3.Models.Tables.MPD.Animation {
    public class FaceCompositeImageTable : FixedSizeTable<FaceCompositeImage> {
        protected FaceCompositeImageTable(IByteData data, string name, FaceChunk chunk)
        : base(data, name, /* addresss not applicable! */ address: 0, 9) {
            Chunk = chunk;
        }

        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;

        public static FaceCompositeImageTable Create(IByteData data, string name, FaceChunk chunk)
            => Create(() => new FaceCompositeImageTable(data, name, chunk));

        public override bool Load() {
            return Load((id, address) => {
                var layer = (id < 3) ? 1 : 2;
                var index = (id < 3) ? id : (id - 3);
                return new FaceCompositeImage(Data, id, layer, index, $"{nameof(FaceCompositeImage)}_{layer}_{index}", Chunk);
            });
        }

        public FaceChunk Chunk { get; }
    }
}
