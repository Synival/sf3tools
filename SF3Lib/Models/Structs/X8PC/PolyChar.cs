using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Imaging;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Structs.Shared.SGL;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared.SGL;
using SF3.Models.Tables.X8PC;
using SF3.X8PC;

namespace SF3.Models.Structs.X8PC {
    public class PolyChar : Struct, ITableContainer, ITextureMetaCollection {
        public PolyChar(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0 /* not applicable */) {
            var tables = new List<ITable>();

            // Build the first header, which is the chunk table.
            Header = new PCHeader(Data, 0, nameof(PCHeader), Address);

            // Build all chunks.
            Chunks = new ChunkData[Header.ChunkDefTable.Count];
            for (int i = 0; i < Chunks.Length; i++) {
                var isCompressed = (i == 1);
                var def = Header.ChunkDefTable[i];
                Chunks[i] = new ChunkData(new ByteArray(Data.Data.GetDataCopyAt(Address + (int) def.Offset, (int) def.DataSize)), isCompressed, i);
                Chunks[i].DecompressedData.IsModifiedChanged += (s, e) => Data.IsModified |= ((IByteData) s).IsModified;
            }

            // Store references to chunks by name as well as index.
            TexDefChunk    = Chunks[0];
            TexDataChunk   = Chunks[1];
            ModelChunk     = Chunks[2];
            AnimationChunk = Chunks[3];

            // Initialize structs and tables in chunks.
            TexDefChunkHeader = new PCTexDefChunkHeader(TexDefChunk.DecompressedData, 0, nameof(TexDefChunkHeader), 0);
            TextureTable      = PCTextureTable.Create(TexDefChunk.DecompressedData, TexDataChunk.DecompressedData.Data, "Textures", (int) TexDefChunkHeader.TexDefsOffset, (int) TexDefChunkHeader.NumTextures);
            _animatableTextureDictionary = TextureTable.ToDictionary(x => x.ID, x => (IAnimatableTexture) x);

            ModelChunkHeader  = new PCModelChunkHeader(ModelChunk.DecompressedData, 0, nameof(ModelChunkHeader), 0);
            XPDataListTable   = PC_XPDataListTable.Create(ModelChunk.DecompressedData, "XPDATA_Lists", (int) ModelChunkHeader.ModelsOffset, this);
            XPDataTables      = XPDataListTable.Select((x, i) => PC_XPDataTable.Create(ModelChunk.DecompressedData, $"XPDATAs_{x.ID}", x.XPDataListOffset, this, i * 1000)).ToArray();

            WeaponXPData = (XPDataTables.Length >= 2 && XPDataTables[1].Count >= 1) ? XPDataTables[1][0] : null;

            VertexTablesByOffset = FetchTablesByOffset(
                XPDataTables,
                x => (x.VertexCount, (int) x.VerticesOffset),
                (count, offset, index) => VertexTable.Create(ModelChunk.DecompressedData, $"{nameof(VertexTable)}_{index:D3} @{offset:X4}", offset, count)
            );

            PolygonTablesByOffset = FetchTablesByOffset(
                XPDataTables,
                x => (x.FaceCount, (int) x.PolygonsOffset),
                (count, offset, index) => PolygonTable.Create(ModelChunk.DecompressedData, $"{nameof(PolygonTable)}_{index:D3} @{offset:X4}", offset, count)
            );

            AttrTablesByOffset = FetchTablesByOffset(
                XPDataTables,
                x => (x.FaceCount, (int) x.AttributesOffset),
                (count, offset, index) => AttrTable.Create(ModelChunk.DecompressedData, $"{nameof(AttrTable)}_{index:D3} @{offset:X4}", offset, count)
            );

            VertexNormalTablesByOffset = FetchTablesByOffset(
                XPDataTables,
                x => (x.VertexCount, (int) x.VertexNormalsOffset),
                (count, offset, index) => VertexNormalTable.Create(ModelChunk.DecompressedData, $"{nameof(VertexNormalTable)}_{index:D3} @{offset:X4}", offset, count)
            );

            var skelFact = new SkeletonFactory();
            var skeleton = skelFact.CreateSkeleton(ModelChunk.DecompressedData, (int) ModelChunkHeader.SkeletonOffset);
            BoneTable = PCBoneWrapperTable.Create("BoneNodes", skeleton.RootBone, this);

            if (XPDataTables.Length > 0) {
                var xpdataTable = XPDataTables[0];
                foreach (var xpdata in xpdataTable)
                    xpdata.AssociateWithSkeleton(skeleton);
            }

            AnimationChunkHeader = new PCAnimationChunkHeader(AnimationChunk.DecompressedData, 0, nameof(ModelChunkHeader), 0);
            BoneKeyframeTable    = PCBoneKeyframeTable.Create(AnimationChunk.DecompressedData, "BoneKeyframeTable", (int) AnimationChunkHeader.BoneKeyframesTableOffset);

            BoneKeyframePosTables = BoneKeyframeTable
                .Select(x => PCBoneKeyframePosTable.Create(
                    AnimationChunk.DecompressedData, $"Bone{x.ID:D2}_KeyframePos", x.ID, (int) x.NumPosKeyFrames,
                        (int) x.PosFramesOffset, (int) x.PosXPtr, (int) x.PosYPtr, (int) x.PosZPtr
                    )
                ).ToArray();

            BoneKeyframeRotTables = BoneKeyframeTable
                .Select(x => PCBoneKeyframeRotTable.Create(
                    AnimationChunk.DecompressedData, $"Bone{x.ID:D2}_KeyframeRot", x.ID, (int) x.NumRotKeyFrames,
                        (int) x.RotFramesOffset, (int) x.RotXPtr, (int) x.RotYPtr, (int) x.RotZPtr, (int) x.RotWPtr
                    )
                ).ToArray();

            BoneKeyframeScaleTables = BoneKeyframeTable
                .Select(x => PCBoneKeyframeScaleTable.Create(
                    AnimationChunk.DecompressedData, $"Bone{x.ID:D2}_KeyframeScale", x.ID, (int) x.NumScaleKeyFrames,
                        (int) x.ScaleFramesOffset, (int) x.ScaleXPtr, (int) x.ScaleYPtr, (int) x.ScaleZPtr
                    )
                ).ToArray();

            AnimationFramesTable = PCAnimationFrameTable.Create("AnimationFrames", this);

            tables.AddRange(Header.Tables);
            tables.Add(TextureTable);

            tables.Add(XPDataListTable);
            tables.AddRange(XPDataTables);
            tables.AddRange(VertexTablesByOffset.Values);
            tables.AddRange(PolygonTablesByOffset.Values);
            tables.AddRange(AttrTablesByOffset.Values);
            tables.AddRange(VertexNormalTablesByOffset.Values);
            tables.Add(BoneTable);

            tables.Add(BoneKeyframeTable);
            tables.AddRange(BoneKeyframePosTables);
            tables.AddRange(BoneKeyframeRotTables);
            tables.AddRange(BoneKeyframeScaleTables);
            tables.Add(AnimationFramesTable);

            Tables = tables.ToArray();
        }

        private static Dictionary<int, T> FetchTablesByOffset<T>(PC_XPDataTable[] tables, Func<XPDataStruct, (int Count, int Offset)> countOffsetFetcher, Func<int, int, int, T> tableMaker) {
            return tables
                .SelectMany(x => x.Select(y => countOffsetFetcher(y)))
                .OrderBy(x => x.Offset)
                .ThenByDescending(x => x.Count)
                .GroupBy(x => x.Offset)
                .Select((x, i) => (Model: x.First(), Index: i))
                .ToDictionary(
                    x => x.Model.Offset,
                    x => tableMaker(x.Model.Count, x.Model.Offset, x.Index)
                );
        }

        public bool UpdateAndCommitChunks() {
            // Rebuild the entire chunk table.
            uint nextOffset = 0x800;
            uint polyCharOffset = (uint) Address;
            foreach (var chunk in Chunks) {
                var def = Header.ChunkDefTable[chunk.Index];

                if (!chunk.DecompressedData.IsModified && !chunk.IsModified) {
                    nextOffset += def.ChunkSize;
                    continue;
                }

                // Recompress if necessary.
                if (TexDataChunk.NeedsRecompression)
                    if (!TexDataChunk.Recompress())
                        return false;

                // Update the chunk table entry.
                def.Offset = nextOffset;
                def.DataSize = (uint) chunk.Data.Length;
                def.ChunkSize = (uint) ((chunk.Data.Length + 0x7FF) / 0x800) * 0x800;

                // Copy to the actual data.
                Data.Data.SetDataAtTo((int) (polyCharOffset + def.Offset), (int) def.DataSize, chunk.Data.GetDataCopyOrReference());

                // Pad the rest of the chunk with 0xFF.
                var paddingBytes = new byte[(int) def.ChunkSize - def.DataSize];
                paddingBytes.AsSpan().Fill(0xFF);
                Data.Data.SetDataAtTo((int) (polyCharOffset + def.Offset + def.DataSize), paddingBytes.Length, paddingBytes);

                nextOffset += def.ChunkSize;
            }

            return true;
        }

        private Dictionary<int, IAnimatableTexture> _animatableTextureDictionary;
        public Dictionary<int, IAnimatableTexture> GetAnimatableTexturesByModelCollectionID(int mcId)
            => _animatableTextureDictionary;

        public IEnumerable<ITable> Tables { get; }

        public PCHeader Header { get; }

        public PCTexDefChunkHeader TexDefChunkHeader { get; }
        public PCTextureTable TextureTable { get; }

        public PCModelChunkHeader ModelChunkHeader { get; }
        public PC_XPDataListTable XPDataListTable { get; }
        public PC_XPDataStruct WeaponXPData { get; }
        public PC_XPDataTable[] XPDataTables { get; }
        public Dictionary<int, VertexTable> VertexTablesByOffset { get; }
        public Dictionary<int, PolygonTable> PolygonTablesByOffset { get; }
        public Dictionary<int, AttrTable> AttrTablesByOffset { get; }
        public Dictionary<int, VertexNormalTable> VertexNormalTablesByOffset { get; }
        public Skeleton Skeleton { get; }
        public PCBoneWrapperTable BoneTable { get; }

        public PCAnimationChunkHeader AnimationChunkHeader { get; }
        public PCBoneKeyframeTable BoneKeyframeTable { get; }
        public PCBoneKeyframePosTable[] BoneKeyframePosTables { get; }
        public PCBoneKeyframeRotTable[] BoneKeyframeRotTables { get; }
        public PCBoneKeyframeScaleTable[] BoneKeyframeScaleTables { get; }
        public PCAnimationFrameTable AnimationFramesTable { get; }

        public ChunkData[] Chunks { get; }
        public ChunkData TexDefChunk { get; }
        public ChunkData TexDataChunk { get; }
        public ChunkData ModelChunk { get; }
        public ChunkData AnimationChunk { get; }
    }
}
