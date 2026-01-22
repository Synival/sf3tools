using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Types;
using SF3.NamedValues;
using SF3.MPD;
using SF3.Models.Structs.MPD.Main;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.MPD.Main;
using SF3.Models.Tables.MPD.Animation;
using CommonLib.Imaging;
using CommonLib.Geometry;
using CommonLib.Utils;
using CommonLib;

namespace SF3.Models.Files.MPD {
    public partial class MPD_File : ScenarioTableFile, IMPD_File {
        public override int RamAddress => c_RamAddress;
        public override int RamAddressLimit => 0x002D0000;

        private const int c_RamAddress = 0x00290000;

        protected MPD_File(IByteData data, Dictionary<ScenarioType, INameGetterContext> nameContexts, ScenarioType? fallbackScenario = null)
        : base(data, GetNameGetterContext(nameContexts, DetectScenario(data) ?? fallbackScenario ?? ScenarioType.Prototype), DetectScenario(data) ?? fallbackScenario ?? ScenarioType.Prototype) {
            DetermineChunkIndices();
            Lighting = new LightingClass(this);
        }

        private static INameGetterContext GetNameGetterContext(Dictionary<ScenarioType, INameGetterContext> nameContexts, ScenarioType scenario) {
            if (nameContexts == null)
                return null;
            else if (nameContexts.Count == 1)
                return nameContexts.Values.First();
            else if (scenario == ScenarioType.Scenario3 && nameContexts.ContainsKey(ScenarioType.PremiumDisk))
                return nameContexts[ScenarioType.PremiumDisk];
            else if (nameContexts.ContainsKey(scenario))
                return nameContexts[scenario];
            else
                return null;
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

        // TODO: UpdatePlaneImages() shouldn't be necessary!!
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
        public IMPD_BinaryReproductionFlags BinaryReproductionFlags { get; private set; }
        public IMPD_Surface Surface { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<MPD_CollectionType, IMPD_ModelCollection> ModelCollections { get; } = new Dictionary<MPD_CollectionType, IMPD_ModelCollection>();

        public Palette TexturePalette => TexturePaletteColorTable?.Palette;

        private class LightingClass : IMPD_Lighting {
            public LightingClass(MPD_File mpdFile) {
                MPD_File = mpdFile;
            }

            public Palette Palette => MPD_File.LightPaletteColorTable?.Palette;

            public float Pitch {
                get => MathHelpers.ActualMod((MPD_File.LightPosition?.Pitch ?? 0) + 180.0f, 360.0f) - 180.0f;
                set {
                    if (MPD_File.LightPosition != null)
                        MPD_File.LightPosition.Pitch = MathHelpers.ActualMod(value + 180.0f, 360.0f) - 180.0f;
                }
            }

            public float Yaw {
                get => MathHelpers.ActualMod((MPD_File.LightPosition?.Yaw ?? 0) + 180.0f, 360.0f) - 180.0f;
                set {
                    if (MPD_File.LightPosition != null)
                        MPD_File.LightPosition.Yaw = MathHelpers.ActualMod(value + 180.0f, 360.0f) - 180.0f;
                }
            }

            public MPD_File MPD_File { get; }
        }

        public IMPD_Lighting Lighting { get; }
        public IIndexedEnumerableWithLength<IMPD_ModelSwitchGroup> ModelSwitchGroups => ModelSwitchGroupsTable;
        public IMPD_Planes Planes { get; private set; }
        public IMPD_Collisions Collisions { get; private set; }
        public IRectangleShort CameraBoundaries => (BoundariesTable?.Length >= 1) ? BoundariesTable[0] : null;
        public IRectangleShort BattleCursorBoundaries => (BoundariesTable?.Length >= 2) ? BoundariesTable[1] : null;
        public IMPD_Gradient Gradient => (GradientTable?.Length > 0) ? GradientTable[0] : null;

        public IIndexedEnumerableWithLength<byte> GroundAnimation => GroundAnimationTable;
        public IIndexedEnumerableWithLength<ushort> Scenario1UnknownTable1 => Unknown1Table;
        public IIndexedEnumerableWithLength<ushort> Scenario1UnknownTable2 => Unknown2Table;
        public IIndexedEnumerableWithLength<byte> UnreferencedDataAfterPaletteAdjustment => UnreferencedDataAfterPaletteAdjustmentTable;

        [BulkCopyRecurse]
        public MPD_Header MPDHeader { get; private set; }

        [BulkCopyRecurse]
        public ChunkLocationTable ChunkLocations { get; private set; }

        [BulkCopyRecurse]
        public ColorTable LightPaletteColorTable { get; private set; }

        [BulkCopyRecurse]
        public LightPosition LightPosition { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt16Table Unknown1Table { get; private set; }

        [BulkCopyRecurse]
        public PaletteAdjustment PaletteAdjustment { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt8Table UnreferencedDataAfterPaletteAdjustmentTable { get; private set; }

        [BulkCopyRecurse]
        public ModelSwitchGroupsTable ModelSwitchGroupsTable { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt8Table GroundAnimationTable { get; private set; }

        [BulkCopyRecurse]
        public IgnoredTextureTable IgnoredTextureTable { get; private set; }

        [BulkCopyRecurse]
        public ColorTable GroundPaletteColorTable { get; private set; }

        [BulkCopyRecurse]
        public ColorTable SkyPaletteColorTable { get; private set; }

        [BulkCopyRecurse]
        public ColorTable TexturePaletteColorTable { get; private set; }

        [BulkCopyRecurse]
        public IndexedTextureTable IndexedTextureTable { get; private set; }

        [BulkCopyRecurse]
        public AnimationTable Animations { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt16Table Unknown2Table { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt16Table Unknown3Table { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt16Table Unknown4Table { get; private set; }

        [BulkCopyRecurse]
        public GradientTable GradientTable { get; private set; }

        [BulkCopyRecurse]
        public BoundaryTable BoundariesTable { get; private set; }

        [BulkCopyRecurse]
        public SurfaceModelChunk SurfaceModelChunk { get; private set; }

        public HashSet<int> ModelChunkIndices { get; private set; } = null;

        public AnimationFrameChunk AnimationFrameChunk { get; private set; }

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
