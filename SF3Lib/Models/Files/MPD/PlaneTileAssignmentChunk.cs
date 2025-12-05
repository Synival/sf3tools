using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.Plane;

namespace SF3.Models.Files.MPD {
    public class PlaneTileAssignmentChunk : TableFile {
        protected PlaneTileAssignmentChunk(IByteData data, INameGetterContext nameContext, int address, string name, int? chunkIndex, int startY, int blockCountX)
        : base(data, nameContext) {
            Address     = address;
            Name        = name;
            ChunkIndex  = chunkIndex;
            StartY      = startY;
            BlockCountX = blockCountX;
            RowCount    = (int) Math.Ceiling((double) data.Length / (0x40 /* blockTileWidth* */ * blockCountX) / 2 /* bytes-per-value */);
        }

        public static PlaneTileAssignmentChunk Create(IByteData data, INameGetterContext nameContext, int address, string name, int? chunkIndex, int startY, int blockCountX) {
            var newFile = new PlaneTileAssignmentChunk(data, nameContext, address, name, chunkIndex, startY, blockCountX);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (PlaneTileTextureRowTable = PlaneTileTextureRowTable.Create(Data, nameof(PlaneTileTextureRowTable), 0x0000, StartY, BlockCountX, RowCount)),
            };
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }
        public int? ChunkIndex { get; }
        public int StartY { get; }
        public int BlockCountX { get; }
        public int RowCount { get; }

        [BulkCopyRecurse]
        public PlaneTileTextureRowTable PlaneTileTextureRowTable { get; private set; }
    }
}
