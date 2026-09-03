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
            BoneTable = PC_BoneWrapperTable.Create("BoneNodes", skeleton.RootBone, this);

            if (XPDataTables.Length > 0) {
                var xpdataTable = XPDataTables[0];
                foreach (var xpdata in xpdataTable)
                    xpdata.AssociateWithSkeleton(skeleton);
            }

            AnimationChunkHeader = new PCAnimationChunkHeader(AnimationChunk.DecompressedData, 0, nameof(ModelChunkHeader), 0);
            BoneKeyframeTable    = PCBoneKeyframeTable.Create(AnimationChunk.DecompressedData, "BoneKeyframeTable", (int) AnimationChunkHeader.BoneKeyframesTableOffset);

            BoneKeyframePosXTablesById = BoneKeyframeTable.ToDictionary(x => x.ID, x => GenericFixedSizeTable<float>.Create(AnimationChunk.DecompressedData, $"BoneKeyframePosXTable_{x.ID:D2}", $"Bone_{x.ID:D2}_PosX", (int) x.PosXPtr, (int) x.NumPosKeyFrames, 4, _fixedGetter, _fixedSetter));
            BoneKeyframePosYTablesById = BoneKeyframeTable.ToDictionary(x => x.ID, x => GenericFixedSizeTable<float>.Create(AnimationChunk.DecompressedData, $"BoneKeyframePosYTable_{x.ID:D2}", $"Bone_{x.ID:D2}_PosY", (int) x.PosYPtr, (int) x.NumPosKeyFrames, 4, _fixedGetter, _fixedSetter));
            BoneKeyframePosZTablesById = BoneKeyframeTable.ToDictionary(x => x.ID, x => GenericFixedSizeTable<float>.Create(AnimationChunk.DecompressedData, $"BoneKeyframePosZTable_{x.ID:D2}", $"Bone_{x.ID:D2}_PosZ", (int) x.PosZPtr, (int) x.NumPosKeyFrames, 4, _fixedGetter, _fixedSetter));

            BoneKeyframeRotXTablesById = BoneKeyframeTable.ToDictionary(x => x.ID, x => GenericFixedSizeTable<float>.Create(AnimationChunk.DecompressedData, $"BoneKeyframeRotXTable_{x.ID:D2}", $"Bone_{x.ID:D2}_RotX", (int) x.RotXPtr, (int) x.NumRotKeyFrames, 2, _compressedFixedGetter, _compressedFixedSetter));
            BoneKeyframeRotYTablesById = BoneKeyframeTable.ToDictionary(x => x.ID, x => GenericFixedSizeTable<float>.Create(AnimationChunk.DecompressedData, $"BoneKeyframeRotYTable_{x.ID:D2}", $"Bone_{x.ID:D2}_RotY", (int) x.RotYPtr, (int) x.NumRotKeyFrames, 2, _compressedFixedGetter, _compressedFixedSetter));
            BoneKeyframeRotZTablesById = BoneKeyframeTable.ToDictionary(x => x.ID, x => GenericFixedSizeTable<float>.Create(AnimationChunk.DecompressedData, $"BoneKeyframeRotZTable_{x.ID:D2}", $"Bone_{x.ID:D2}_RotZ", (int) x.RotZPtr, (int) x.NumRotKeyFrames, 2, _compressedFixedGetter, _compressedFixedSetter));
            BoneKeyframeRotWTablesById = BoneKeyframeTable.ToDictionary(x => x.ID, x => GenericFixedSizeTable<float>.Create(AnimationChunk.DecompressedData, $"BoneKeyframeRotWTable_{x.ID:D2}", $"Bone_{x.ID:D2}_RotW", (int) x.RotWPtr, (int) x.NumRotKeyFrames, 2, _compressedFixedGetter, _compressedFixedSetter));

            BoneKeyframeScaleXTablesById = BoneKeyframeTable.ToDictionary(x => x.ID, x => GenericFixedSizeTable<float>.Create(AnimationChunk.DecompressedData, $"BoneKeyframeScaleXTable_{x.ID:D2}", $"Bone_{x.ID:D2}_ScaleX", (int) x.ScaleXPtr, (int) x.NumScaleKeyFrames, 4, _fixedGetter, _fixedSetter));
            BoneKeyframeScaleYTablesById = BoneKeyframeTable.ToDictionary(x => x.ID, x => GenericFixedSizeTable<float>.Create(AnimationChunk.DecompressedData, $"BoneKeyframeScaleYTable_{x.ID:D2}", $"Bone_{x.ID:D2}_ScaleY", (int) x.ScaleYPtr, (int) x.NumScaleKeyFrames, 4, _fixedGetter, _fixedSetter));
            BoneKeyframeScaleZTablesById = BoneKeyframeTable.ToDictionary(x => x.ID, x => GenericFixedSizeTable<float>.Create(AnimationChunk.DecompressedData, $"BoneKeyframeScaleZTable_{x.ID:D2}", $"Bone_{x.ID:D2}_ScaleZ", (int) x.ScaleZPtr, (int) x.NumScaleKeyFrames, 4, _fixedGetter, _fixedSetter));

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

        private static Func<IByteData, int /*addr*/, float> _fixedGetter             = (d, a) => d.GetFIXED(a).Float;
        private static Func<IByteData, int /*addr*/, float> _compressedFixedGetter   = (d, a) => d.GetCompressedFIXED(a, 14).Float;
        private static Action<IByteData, int /*addr*/, float> _fixedSetter           = (d, a, v) => d.SetFIXED(a, new FIXED(v, 0));
        private static Action<IByteData, int /*addr*/, float> _compressedFixedSetter = (d, a, v) => d.SetCompressedFIXED(a, new CompressedFIXED(v, 14, 0));

        private Dictionary<int, IAnimatableTexture> _animatableTextureDictionary;
        public Dictionary<int, IAnimatableTexture> GetAnimatableTexturesByModelCollectionID(int mcId)
            => _animatableTextureDictionary;

        public IEnumerable<ITable> Tables { get; private set; }

        public PCHeader Header { get; private set; }

        public PCTexDefChunkHeader TexDefChunkHeader { get; private set; }
        public PCTextureTable TextureTable { get; private set; }

        public PCModelChunkHeader ModelChunkHeader { get; private set; }
        public PC_XPDataListTable XPDataListTable { get; private set; }
        public PC_XPDataTable[] XPDataTables { get; private set; }
        public Dictionary<int, VertexTable> VertexTablesByOffset { get; }
        public Dictionary<int, PolygonTable> PolygonTablesByOffset { get; }
        public Dictionary<int, AttrTable> AttrTablesByOffset { get; }
        public Dictionary<int, VertexNormalTable> VertexNormalTablesByOffset { get; }
        public Skeleton Skeleton { get; }
        public PC_BoneWrapperTable BoneTable { get; }

        public PCAnimationChunkHeader AnimationChunkHeader { get; }
        public PCBoneKeyframeTable BoneKeyframeTable { get; }
        public Dictionary<int, GenericFixedSizeTable<float>> BoneKeyframePosXTablesById { get; }
        public Dictionary<int, GenericFixedSizeTable<float>> BoneKeyframePosYTablesById { get; }
        public Dictionary<int, GenericFixedSizeTable<float>> BoneKeyframePosZTablesById { get; }
        public Dictionary<int, GenericFixedSizeTable<float>> BoneKeyframeRotXTablesById { get; }
        public Dictionary<int, GenericFixedSizeTable<float>> BoneKeyframeRotYTablesById { get; }
        public Dictionary<int, GenericFixedSizeTable<float>> BoneKeyframeRotZTablesById { get; }
        public Dictionary<int, GenericFixedSizeTable<float>> BoneKeyframeRotWTablesById { get; }
        public Dictionary<int, GenericFixedSizeTable<float>> BoneKeyframeScaleXTablesById { get; }
        public Dictionary<int, GenericFixedSizeTable<float>> BoneKeyframeScaleYTablesById { get; }
        public Dictionary<int, GenericFixedSizeTable<float>> BoneKeyframeScaleZTablesById { get; }

        public ChunkData[] Chunks { get; private set; }
        public ChunkData TexDefChunk { get; private set; }
        public ChunkData TexDataChunk { get; private set; }
        public ChunkData ModelChunk { get; private set; }
        public ChunkData AnimationChunk { get; private set; }
    }
}
