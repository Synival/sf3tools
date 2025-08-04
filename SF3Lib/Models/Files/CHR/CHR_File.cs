using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.CHR;
using SF3.Models.Tables;
using SF3.Models.Tables.CHR;
using SF3.Types;

namespace SF3.Models.Files.CHR {
    public class CHR_File : ScenarioTableFile, ICHR_File {
        public override int RamAddress => 0x00210000;
        public override int RamAddressLimit => 0x00290000;

        protected CHR_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario, int startId, uint dataOffset, bool isInCHP) : base(data, nameContext, scenario) {
            StartID = startId;
            DataOffset = dataOffset;
            IsInCHP = isInCHP;
        }

        public static CHR_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new CHR_File(data, nameContext, scenario, 0, 0, false);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public static CHR_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario, int startId, uint dataOffset) {
            var newFile = new CHR_File(data, nameContext, scenario, startId, dataOffset, true);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new ITable[] {
                (SpriteTable = SpriteTable.Create(Data, nameof(SpriteTable), (int) DataOffset, StartID, DataOffset, NameGetterContext, IsInCHP))
            };
        }

        public CHR_Def ToCHR_Def() {
            // Collect a list of frames with duplicates. We'll need this to force certain
            // duplicate frames to have their own location.
            var framesWithDuplicates = new HashSet<string>(
                SpriteTable
                    .Where(x => x.FrameTable != null)
                    .SelectMany(x => x.FrameTable)
                    .GroupBy(x => x.TextureHash)
                    .Where(x => x.Count() > 1 && !x.All(y => y.TextureOffset != x.First().TextureOffset))
                    .Select(x => x.Key)
            );

            // XBTL127.CHR has 6 random '0x0009' words between the frame tables and the frame images.
            // Let's account for anomalies like that.
            byte[] junkAfterFrameTable = null;

            var firstFrameTableOffset = (SpriteTable != null && SpriteTable.Length > 0)
                ? (int) SpriteTable[0].Header.FrameTableOffset
                : (int?) null;

            var firstTextureOffset = SpriteTable
                .Where(x => x.FrameTable != null)
                .SelectMany(x => x.FrameTable)
                .OrderBy(x => x.TextureOffset)
                .Select(x => (int?) x.TextureOffset)
                .FirstOrDefault();

            var frameImagesAreBeforeTables = firstTextureOffset.HasValue && firstFrameTableOffset.HasValue && firstTextureOffset.Value < firstFrameTableOffset.Value;
            if (frameImagesAreBeforeTables)
                ;

            var lastFrameTablePosition = SpriteTable
                .Where(x => x.FrameTable != null)
                .Select(x => x.FrameTable)
                .OrderByDescending(x => x.Address)
                .Select(x => (int?) x.Address + x.SizeInBytesPlusTerminator + 4)
                .FirstOrDefault();

            if (firstTextureOffset.HasValue && lastFrameTablePosition.HasValue && firstTextureOffset >= lastFrameTablePosition) {
                var spaceBetween = firstTextureOffset.Value - lastFrameTablePosition.Value;
                if (spaceBetween > 0)
                    junkAfterFrameTable = Data.GetDataCopyAt(lastFrameTablePosition.Value, spaceBetween);
            }

            return new CHR_Def() {
                WriteFrameImagesBeforeTables = frameImagesAreBeforeTables ? true : (bool?) null,
                JunkAfterFrameTables = junkAfterFrameTable,
                Sprites = SpriteTable.Select(x => x.ToCHR_SpriteDef(framesWithDuplicates)).ToArray()
            };
        }

        public int GetSize() {
            if (SpriteTable == null || SpriteTable.Length == 0)
                return 0x18 + 0x04; // Length of zero-sprite header plus EOF padding

            var lastImageEndPos = SpriteTable
                .SelectMany(x => x.FrameTable.Select(y => (int) y.TextureEndOffset))
                .Concat(new int[] { 0 })
                .Max(x => x)
                + 0x04; // Extra padding

            var lastFrameTableEndPos = (int) SpriteTable
                .SelectMany(x => x.FrameTable.Select(y => y.Address + y.Size - (int) DataOffset))
                .Concat(new int[] { 0 })
                .Max(x => x)
                + 0x04; // Terminating frame

            var size = (lastFrameTableEndPos >= lastImageEndPos)
                ? lastFrameTableEndPos + 0x08 // Tons of extra padding in this case for some reason.
                : lastImageEndPos;
            if (size % 0x04 != 0)
                size += 0x04 - (size % 0x04);

            return size;
        }

        public int StartID { get; }
        public uint DataOffset { get; }
        public bool IsInCHP { get; }
        public SpriteTable SpriteTable { get; private set; }
    }
}
