using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Rigging;
using CommonLib.SGL;
using CommonLib.ThirdParty.TexturePacker;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Files.X8PC;
using SF3.Models.Structs.Shared.SGL;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared.SGL;
using SF3.Models.Tables.X8PC;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Models.Structs.X8PC {
    public class PolyChar : Struct, ITableContainer, ITextureMetaCollection, ISGL_ModelMetaCollection, ISGL_ModelCollection, IDisposable, IPCAttackAnimBoneKeyframesTableContainer {
        public PolyChar(IByteData data, int id, string name, int address, ScenarioType scenario)
        : base(data, id, name, address, 0 /* not applicable */) {
            Scenario = scenario;

            var tables = new List<ITable>();

            // Build the first header, which is the chunk table.
            var hasAttackAnims = Scenario >= ScenarioType.Scenario2;
            Header = new PCHeader(Data, 0, nameof(PCHeader), Address, hasAttackAnims);

            // Build all chunks.
            Chunks = new ChunkData[Header.ChunkDefTable.Count];
            for (int i = 0; i < Chunks.Length; i++) {
                var isCompressed = (i == 1);
                var def = Header.ChunkDefTable[i];
                Chunks[i] = new ChunkData(new ByteArray(Data.Data.GetDataCopyAt(Address + (int) def.Offset, (int) def.DataSize)),
                    isCompressed ? CompressionType.LZSS : CompressionType.None, i);
                Chunks[i].DecompressedData.IsModifiedChanged += (s, e) => Data.IsModified |= ((IByteData) s).IsModified;
            }

            if (hasAttackAnims) {
                AttackAnimChunks = new ChunkData[Header.AttackAnimChunkDefTable.Count];
                for (int i = 0; i < AttackAnimChunks.Length; i++) {
                    var def = Header.AttackAnimChunkDefTable[i];
                    AttackAnimChunks[i] = new ChunkData(new ByteArray(Data.Data.GetDataCopyAt(Address + (int) def.Offset, (int) def.DataSize)),
                        CompressionType.AttackAnim, i);
                    AttackAnimChunks[i].DecompressedData.IsModifiedChanged += (s, e) => Data.IsModified |= ((IByteData) s).IsModified;
                }
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

            var rigFact = new PCModelRigFactory(Scenario);
            Rig = rigFact.CreateModelRig(ModelChunk.DecompressedData, (int) ModelChunkHeader.RigOffset);
            BoneTable = PCBoneWrapperTable.Create("BoneNodes", Rig.RootBone, this);

            if (XPDataTables.Length > 0) {
                var xpdataTable = XPDataTables[0];
                foreach (var xpdata in xpdataTable)
                    xpdata.AssociateWithRig(Rig);
            }

            var ngc = new NameGetterContext(Scenario);

            AnimationChunkHeader = new PCAnimationChunkHeader(AnimationChunk.DecompressedData, 0, nameof(ModelChunkHeader), 0, this, ngc);
            BoneKeyframesTable   = PCBoneKeyframesTable.Create(
                AnimationChunk.DecompressedData, nameof(BoneKeyframesTable), (int) AnimationChunkHeader.BoneKeyframesTableOffset,
                Scenario < ScenarioType.Scenario1
            );

            // BoneKeyframesTables from attack animation chunks
            if (AttackAnimChunks != null) {
                AttackAnimBoneKeyframesTables = AttackAnimChunks.Select((x, i) => {
                    var animDef        = AnimationChunkHeader.AnimationDefTable.FirstOrDefault(y => y.AttackAnimChunkIdx == i);
                    var animIdProperty = animDef.GetType().GetProperty(nameof(animDef.AnimID));
                    string animName    = (animDef == null) ? "Unknown" : animDef.GetPropertyValueName(animIdProperty, ngc);
                    return PCBoneKeyframesTable.Create(
                        x.DecompressedData, $"AttackAnimBoneKeyframes_{i:D2}_{animName}", (int) x.DecompressedData.GetUInt32(0), Scenario < ScenarioType.Scenario1
                    );
                }).ToArray();
            }

            // Create all colors as a color palette that can be easily modified.
            var attrsByModelThenColor = GetATTRsByModelThenColor();
            Palette = new PCPalette(attrsByModelThenColor.Values.SelectMany(x => x.Select(y => y.Value)).ToArray());

            // Create a big texture atlas that can be used to change all textures at once.
            TextureAtlas = new PCTextureAtlas(GetTextureAtlasesByModelID(), Palette);

            tables.AddRange(Header.Tables);
            tables.AddRange(AnimationChunkHeader.Tables);

            tables.Add(TextureTable);

            tables.Add(XPDataListTable);
            tables.AddRange(XPDataTables);
            tables.AddRange(VertexTablesByOffset.Values);
            tables.AddRange(PolygonTablesByOffset.Values);
            tables.AddRange(AttrTablesByOffset.Values);
            tables.AddRange(VertexNormalTablesByOffset.Values);
            tables.Add(BoneTable);

            tables.Add(BoneKeyframesTable);
            tables.AddRange(BoneKeyframesTable.SelectMany(x => x.Tables).ToArray());

            Tables = tables.ToArray();
        }

        private Dictionary<int, TextureAtlas> GetTextureAtlasesByModelID() {
            // First, get a list of all textures used by ATTR lists.
            var texturedAttrsWithTableOffsets = AttrTablesByOffset
                .SelectMany(x => x.Value.Select(y => (Offset: x.Key, Attr: y)))
                .Where(x => x.Attr.UseTexture)
                .GroupBy(x => x.Offset * 10000 + x.Attr.TextureNo)
                .Select(x => x.First())
                .Select(x => (x.Offset, x.Attr.TextureNo))
                .ToArray();

            // Get a list of all XPDATAs.
            var xpdatas = XPDataTables.SelectMany(x => x).ToArray();

            // This is terrifying -- for each texture ID, get the first model that has it in its ATTR list.
            var texturesByModelId = texturedAttrsWithTableOffsets
                .Select(x => (xpdatas.FirstOrDefault(y => y.AttributesOffset == x.Offset)?.ModelID, x.TextureNo))
                .Where(x => x.ModelID.HasValue)
                // Paranoid filtering to make sure we only have 1 texture ID and it belongs to the first model (sorted).
                .OrderBy(x => x.ModelID.Value)
                .ThenBy(x => x.TextureNo)
                .GroupBy(x => x.TextureNo)
                .Select(x => x.First())
                // All sorted -- now let's bundle them into a dictionary of textures by model ID.
                .GroupBy(x => x.ModelID.Value)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Select(y => TextureTable[y.TextureNo]).Cast<ITexture>().OrderBy(y => y.TextureID));

            // This code below is for paranoid checks that all textures are used. Seems to always work!
#if false
            var allTextureIdsUsed = texturesByModelId.SelectMany(x => x.Value).Select(x => x.TextureID).OrderBy(x => x).ToArray();
            var actualTextureIds = TextureTable.Select(x => x.TextureID).OrderBy(x => x).ToArray();
            if (!Enumerable.SequenceEqual(allTextureIdsUsed, actualTextureIds))
                ;
#endif

            // Generate TextureAtlas's for each model.
            var atlasesByModelId = texturesByModelId
                .ToDictionary(x => x.Key, x => new TextureAtlas(x.Value));

            return atlasesByModelId;
        }

        private Dictionary<int, Dictionary<ushort, AttrStruct[]>> GetATTRsByModelThenColor() {
            int GetModelKey(int offset) {
                var tableIdx = XPDataTables
                    .Select((x, i) => (Table: x, Idx: i))
                    .First(x => x.Table.Any(y => y.AttributesOffset == offset))
                    .Idx;

                // Only separate group 1, because 0 and 2 are the model + (if it exists) unarmed model (an arm for monks)
                return tableIdx == 1 ? 1 : 0;
            }

            var allAttrs = AttrTablesByOffset
                .SelectMany(x => x.Value
                    .Where(y => !y.UseTexture)
                    .Select(y => (ModelKey: GetModelKey(x.Key), Attr: y))
                )
                .OrderBy(x => x.ModelKey)
                .ThenBy(x => x.Attr.ID)
                .ToArray();

            return allAttrs
                .GroupBy(x => x.ModelKey)
                .ToDictionary(
                    x => x.Key,
                    x => x
                        .GroupBy(y => y.Attr.ColorNo)
                        .ToDictionary(
                            y => y.Key,
                            y => y.Select(z => z.Attr).ToArray()
                        )
                );
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

        private struct ChunkWithDef {
            public ChunkWithDef(ChunkData data, PCChunkDef def) {
                Chunk = data;
                Def  = def;
            }

            public readonly ChunkData Chunk;
            public readonly PCChunkDef Def;
        };

        public bool UpdateAndCommitChunks(bool neverShrinkChunks) {
            // Rebuild the entire chunk table.
            uint nextOffset = 0x800;
            uint polyCharOffset = (uint) Address;

            var allChunksWithDefs = Chunks
                .Select((x, i) => new ChunkWithDef(x, Header.ChunkDefTable[x.Index]))
                .Concat(AttackAnimChunks?.Select((x, i) => new ChunkWithDef(x, Header.AttackAnimChunkDefTable[x.Index]))?.ToArray() ?? new ChunkWithDef[0])
                .ToArray();

            foreach (var chunkWithDef in allChunksWithDefs) {
                var chunk = chunkWithDef.Chunk;
                var def   = chunkWithDef.Def;

                // Recompress if necessary.
                if (TexDataChunk.NeedsRecompression)
                    if (!TexDataChunk.Recompress())
                        return false;

                // Update the chunk table entry.
                def.Offset = nextOffset;
                def.DataSize = (uint) chunk.Data.Length;

                var newChunkSize = (uint) ((chunk.Data.Length + 0x7FF) / 0x800) * 0x800;
                def.ChunkSize = neverShrinkChunks ? Math.Max(def.ChunkSize, newChunkSize) : newChunkSize;

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
        public ModelRig Rig { get; }
        public PCBoneWrapperTable BoneTable { get; }

        public PCAnimationChunkHeader AnimationChunkHeader { get; }
        public PCBoneKeyframesTable BoneKeyframesTable { get; }
        public IReadOnlyList<PCBoneKeyframesTable> AttackAnimBoneKeyframesTables { get; }

        public ChunkData[] Chunks { get; }
        public ChunkData[] AttackAnimChunks { get; }

        public ChunkData TexDefChunk { get; }
        public ChunkData TexDataChunk { get; }
        public ChunkData ModelChunk { get; }
        public ChunkData AnimationChunk { get; }

        public PCPalette Palette { get; }
        public PCTextureAtlas TextureAtlas { get; }

        private readonly Dictionary<int, ISGL_Model> _modelsById;

        private bool disposedValue;
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing)
                    TextureAtlas?.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        // TODO: Support more than one model collection perhaps?
        public ISGL_ModelCollection GetModelCollection(int mcId) => (mcId == 0) ? this : null;
    }
}
