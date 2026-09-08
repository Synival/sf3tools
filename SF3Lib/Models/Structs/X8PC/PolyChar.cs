using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CommonLib.Arrays;
using CommonLib.Imaging;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Structs.Shared.SGL;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared.SGL;
using SF3.Models.Tables.X8PC;
using SF3.Types;
using SF3.X8PC;

namespace SF3.Models.Structs.X8PC {
    public class PolyChar : Struct, ITableContainer, ITextureMetaCollection, ISGL_ModelCollection {
        public PolyChar(IByteData data, int id, string name, int address, ScenarioType scenario)
        : base(data, id, name, address, 0 /* not applicable */) {
            Scenario = scenario;

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
            _modelsById       = XPDataTables.SelectMany(x => x).ToDictionary(x => x.ModelID, x => (ISGL_Model) x);

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

            var skelFact = new SkeletonFactory(Scenario);
            Skeleton = skelFact.CreateSkeleton(ModelChunk.DecompressedData, (int) ModelChunkHeader.SkeletonOffset);
            BoneTable = PCBoneWrapperTable.Create("BoneNodes", Skeleton.RootBone, this);

            if (XPDataTables.Length > 0) {
                var xpdataTable = XPDataTables[0];
                foreach (var xpdata in xpdataTable)
                    xpdata.AssociateWithSkeleton(Skeleton);
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
                        (int) x.RotFramesOffset, (int) x.RotXPtr, (int) x.RotYPtr, (int) x.RotZPtr, (int) x.RotWPtr, Scenario < ScenarioType.Scenario1
                    )
                ).ToArray();

            BoneKeyframeScaleTables = BoneKeyframeTable
                .Select(x => PCBoneKeyframeScaleTable.Create(
                    AnimationChunk.DecompressedData, $"Bone{x.ID:D2}_KeyframeScale", x.ID, (int) x.NumScaleKeyFrames,
                        (int) x.ScaleFramesOffset, (int) x.ScaleXPtr, (int) x.ScaleYPtr, (int) x.ScaleZPtr
                    )
                ).ToArray();

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

        public struct KeyframeInfo {
            public int IndexA, IndexB;
            public float? FramesLeft;
            public float Mix;

            public override string ToString() => $"{{ A={IndexA}, B={IndexB}) Frames={FramesLeft}, Mix={Mix} }}";
        }

        private KeyframeInfo GetAnimationKeyframe<T>(T[] list, Func<T, int> frameGetter, float frame) {
            int max = list.Length;
            var lastF = 0;

            for (int i = 0; i < max; i++) {
                var element = list[i];
                var f = frameGetter(element);
                if ((frame >= lastF && frame < f) || i == max - 1) {
                    if (i == 0)
                        return new KeyframeInfo() { IndexA = 0, IndexB = 0, FramesLeft = f - frame, Mix = 0.0f };
                    else if (i == max - 1)
                        return new KeyframeInfo() { IndexA = i, IndexB = i, FramesLeft = null, Mix = 1.0f };
                    else
                        return new KeyframeInfo() { IndexA = i - 1, IndexB = i, FramesLeft = f - frame, Mix = (frame - lastF) / (f - lastF) };
                }
                lastF = f;
            }
            return new KeyframeInfo() { IndexA = 0, IndexB = 0, Mix = 0.0f };
        }

        public struct BoneKeyframeInfo {
            public KeyframeInfo Pos;
            public KeyframeInfo Rot;
            public KeyframeInfo Scale;
        }

        public BoneKeyframeInfo[] GetAnimationBoneKeyframes(float frame) {
            var pos = BoneKeyframePosTables.Select(x => GetAnimationKeyframe(x.AsArray(), y => y.Frame, frame)).ToArray();
            var rot = BoneKeyframeRotTables.Select(x => GetAnimationKeyframe(x.AsArray(), y => y.Frame, frame)).ToArray();
            var scale = BoneKeyframeScaleTables.Select(x => GetAnimationKeyframe(x.AsArray(), y => y.Frame, frame)).ToArray();

            var numBones = Math.Min(BoneKeyframePosTables.Length, Math.Min(BoneKeyframeRotTables.Length, BoneKeyframeScaleTables.Length));
            var boneKeyframes = new BoneKeyframeInfo[numBones];
            for (int i = 0; i < numBones; i++)
                boneKeyframes[i] = new BoneKeyframeInfo { Pos = pos[i], Rot = rot[i], Scale = scale[i] };

            return boneKeyframes;
        }

        public Matrix4x4 GetModelInstanceMatrixInAnimation(ISGL_ModelInstance modelInstance, IBone bone, BoneKeyframeInfo[] keyframeInfo) {
            var matrix = Matrix4x4.Identity;

            void ApplyMatrices(IBone b) {
                if (b.BoneID.HasValue) {
                    var bId = b.BoneID.Value;

                    var boneFrame  = keyframeInfo[bId];
                    var posFrame   = boneFrame.Pos;
                    var rotFrame   = boneFrame.Rot;
                    var scaleFrame = boneFrame.Scale;

                    var pos1   = BoneKeyframePosTables[bId][posFrame.IndexA].CreateVector();
                    var rot1   = BoneKeyframeRotTables[bId][rotFrame.IndexA].CreateQuaternion();
                    var scale1 = BoneKeyframeScaleTables[bId][scaleFrame.IndexA].CreateVector();

                    var pos2   = BoneKeyframePosTables[bId][posFrame.IndexB].CreateVector();
                    var rot2   = BoneKeyframeRotTables[bId][rotFrame.IndexB].CreateQuaternion();
                    var scale2 = BoneKeyframeScaleTables[bId][scaleFrame.IndexB].CreateVector();

                    matrix *= IBoneExtensions.CreateMatrix(
                        pos1,   pos2,   posFrame.Mix,
                        rot1,   rot2,   rotFrame.Mix,
                        scale1, scale2, scaleFrame.Mix
                    );
                }
                else if (b.Tag == 0x30 || b.Tag == 0x81)
                    matrix *= b.CreateMatrix();

                if (b.Parent != null)
                    ApplyMatrices(b.Parent);
            }

            ApplyMatrices(bone);

            return matrix;
        }

        private Dictionary<int, IAnimatableTexture> _animatableTextureDictionary;
        public Dictionary<int, IAnimatableTexture> GetAnimatableTexturesByModelCollectionID(int mcId)
            => _animatableTextureDictionary;

        public ISGL_Model GetModel(int id, int lod) => _modelsById.TryGetValue(id, out var model) ? model : null;
        public IEnumerator<ISGL_Model> GetEnumerator() => _modelsById.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<ITable> Tables { get; }

        public ScenarioType Scenario { get; }

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

        public ChunkData[] Chunks { get; }
        public ChunkData TexDefChunk { get; }
        public ChunkData TexDataChunk { get; }
        public ChunkData ModelChunk { get; }
        public ChunkData AnimationChunk { get; }

        private Dictionary<int, ISGL_Model> _modelsById;
    }
}
