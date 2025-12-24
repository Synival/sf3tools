using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Types;
using SF3.NamedValues;
using SF3.MPD;

namespace SF3.Models.Files.MPD {
    public partial class MPD_File : ScenarioTableFile, IMPD_File {
        public override int RamAddress => c_RamAddress;
        public override int RamAddressLimit => 0x002D0000;

        private const int c_RamAddress = 0x00290000;

        protected MPD_File(IByteData data, Dictionary<ScenarioType, INameGetterContext> nameContexts, ScenarioType? fallbackScenario = null)
        : base(data, nameContexts?[DetectScenario(data) ?? fallbackScenario ?? ScenarioType.Other], DetectScenario(data) ?? fallbackScenario ?? ScenarioType.Other) {
            DetermineChunkIndices();
        }

        public static MPD_File Create(IByteData data, INameGetterContext nameContext, ScenarioType fallbackScenario)
            => Create(data, new Dictionary<ScenarioType, INameGetterContext>() { { fallbackScenario, nameContext } }, fallbackScenario);

        public static MPD_File Create(IByteData data, NameGetterContext nameContext)
            => Create(data, new Dictionary<ScenarioType, INameGetterContext>() { { nameContext.Scenario, nameContext } }, nameContext.Scenario);

        public static MPD_File Create(IByteData data, Dictionary<ScenarioType, INameGetterContext> nameContexts, ScenarioType? fallbackScenario) {
            var newFile = new MPD_File(data, nameContexts, fallbackScenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize MPD_File");
            return newFile;
        }

        public override bool OnFinish() {
            // Recompress any chunk waiting for it.
            RecompressChunks(onlyModified: true);

            // Make sure the chunk table matches the chunks, in the expected order.
            if (RebuildChunkTableOnFinish)
                RebuildChunkTable();

            // Update the content of the file.
            CommitChunks();

            // Always return success.
            return true;
        }

        public override void Dispose() {
            base.Dispose();
            if (ChunkData != null) {
                foreach (var cd in ChunkData.Where(x => x != null))
                    cd.Dispose();
            }
        }

        public void UpdatePlaneImages() => ((MPD_Planes) Planes).UpdateImages();

        public override bool IsModified {
            get => base.IsModified | ChunkData.Any(x => x != null && x.IsModified);
            set {
                base.IsModified = value;
                foreach (var ce in ChunkData.Where(x => x != null))
                    ce.IsModified = value;
            }
        }

        public IMPD_AllFlags Flags { get; private set; }
        public IMPD_Settings Settings { get; private set; }
        public IMPD_Surface Surface { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<CollectionType, IMPD_ModelCollection> ModelCollections { get; } = new Dictionary<CollectionType, IMPD_ModelCollection>();

        public IMPD_Planes Planes { get; private set; }
        public IMPD_Collisions Collisions { get; private set; }

        [BulkCopyRecurse]
        public MPD_Header MPDHeader { get; private set; }

        [BulkCopyRecurse]
        public ChunkLocationTable ChunkLocations { get; private set; }

        [BulkCopyRecurse]
        public ColorTable LightPalette { get; private set; }

        [BulkCopyRecurse]
        public LightPosition LightPosition { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt16Table Unknown1Table { get; private set; }

        [BulkCopyRecurse]
        public LightAdjustment LightAdjustment { get; private set; }

        [BulkCopyRecurse]
        public ModelSwitchGroupsTable ModelSwitchGroupsTable { get; private set; }

        public Dictionary<int, ModelIDTable> VisibleModelsWhenFlagOffByAddr { get; private set; }
        public Dictionary<int, ModelIDTable> VisibleModelsWhenFlagOnByAddr { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt8Table GroundAnimationTable { get; private set; }

        [BulkCopyRecurse]
        public TextureIDTable SkipTextures { get; private set; }

        [BulkCopyRecurse]
        public ColorTable[] PaletteTables { get; private set; }

        [BulkCopyRecurse]
        public TextureIDTable IndexedTextureTable { get; private set; }

        [BulkCopyRecurse]
        public TextureAnimationTable TextureAnimations { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt16Table Unknown2Table { get; private set; }

        [BulkCopyRecurse]
        public GradientTable GradientTable { get; private set; }

        [BulkCopyRecurse]
        public BoundaryTable BoundariesTable { get; private set; }

        public int? SurfaceModelChunkIndex { get; private set; } = null;

        [BulkCopyRecurse]
        public SurfaceModelChunk SurfaceModelChunk { get; private set; }

        public int[] ModelChunkIndices { get; private set; } = null;

        public TextureAnimationFrameChunk TextureAnimationFrameChunk { get; private set; }

        [BulkCopyRecurse]
        public SurfaceDataChunk SurfaceDataChunk { get; private set; }

        [BulkCopyRecurse]
        public TextureChunk[] TextureChunks { get; private set; }

        [BulkCopyRecurse]
        public PlaneTileAssignmentChunk[] GroundTileAssignmentChunks { get; private set; }

        [BulkCopyRecurse]
        public PlaneTileAssignmentChunk ForegroundTileAssignmentChunk { get; private set; }

        public static bool UpdateChunkTableOnChunkResize { get; set; } = true;
        public static bool RebuildChunkTableOnFinish { get; set; } = true;

        public EventHandler ModelsUpdated { get; set; }
    }
}
