using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.TextureAnimation;

namespace SF3.Models.Files.MPD {
    public class TextureAnimationFrameChunk : TableFile {
        protected TextureAnimationFrameChunk(IByteData data, INameGetterContext nameContext, int address, string name, Dictionary<int, UniqueTextureAnimationFrameInfo> infoByOffset, IMPD_File mpdFile)
        : base(data, nameContext) {
            Address      = address;
            Name         = name;
            InfoByOffset = infoByOffset;
            MPD_File     = mpdFile;
        }

        public static TextureAnimationFrameChunk Create(IByteData data, INameGetterContext nameContext, int address, string name, Dictionary<int, UniqueTextureAnimationFrameInfo> infoByOffset, IMPD_File mpdFile) {
            var newFile = new TextureAnimationFrameChunk(data, nameContext, address, name, infoByOffset, mpdFile);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (UniqueTextureAnimationFrameTable = UniqueTextureAnimationFrameTable.Create(Data, nameof(UniqueTextureAnimationFrameTable), 0, InfoByOffset, MPD_File))
            };
        }

        [BulkCopyRowName]
        public string Name { get; }
        public Dictionary<int, UniqueTextureAnimationFrameInfo> InfoByOffset { get; }
        public IMPD_File MPD_File { get; }
        public int Address { get; }
        public UniqueTextureAnimationFrameTable UniqueTextureAnimationFrameTable { get; private set; }
    }
}
