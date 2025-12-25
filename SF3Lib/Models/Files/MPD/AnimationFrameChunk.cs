using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.Animation;

namespace SF3.Models.Files.MPD {
    public class AnimationFrameChunk : TableFile {
        protected AnimationFrameChunk(IByteData data, INameGetterContext nameContext, int address, string name, Dictionary<int, UniqueAnimationFrameInfo> infoByOffset, IMPD_File mpdFile)
        : base(data, nameContext) {
            Address      = address;
            Name         = name;
            InfoByOffset = infoByOffset;
            MPD_File     = mpdFile;
        }

        public static AnimationFrameChunk Create(IByteData data, INameGetterContext nameContext, int address, string name, Dictionary<int, UniqueAnimationFrameInfo> infoByOffset, IMPD_File mpdFile) {
            var newFile = new AnimationFrameChunk(data, nameContext, address, name, infoByOffset, mpdFile);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (UniqueAnimationFrameTable = UniqueAnimationFrameTable.Create(Data, nameof(UniqueAnimationFrameTable), 0, InfoByOffset, MPD_File))
            };
        }

        [BulkCopyRowName]
        public string Name { get; }
        public Dictionary<int, UniqueAnimationFrameInfo> InfoByOffset { get; }
        public IMPD_File MPD_File { get; }
        public int Address { get; }
        public UniqueAnimationFrameTable UniqueAnimationFrameTable { get; private set; }
    }
}
