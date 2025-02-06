using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class MPDHeaderModel : Struct {
        private readonly int mapFlagsAddress;             // int16  Unknown. Might be map id.
        private readonly int padding1Address;             // int16  Always zero
        private readonly int offsetLightPaletteAddress;   // int32  Always 0x0c. Pointer to 32 values for light palette. See (#header-offset-1).
        private readonly int offsetLightPosAddress;       // int32  Always 0x4c. Pointer to light position. See (#header-offset-2).
        private readonly int offsetUnknown1Address;       // int32  Always 0x50. pointer to 0x20 unknown int16s at the start of the file. Mostly zero or 0x8000. (#header-offset-3)
        private readonly int viewDistanceAddress;         // int16  Something like a view distance for meshes from the models chunk.
        private readonly int padding2Address;             // int16  Always zero
        private readonly int offsetModelSwitchGroupsAddr; // int32  Pointer to model switch group list.
        private readonly int offsetTextureAnimationsAddress; // int32 Offset to list of texture groups. See (#texture-groups)
        private readonly int offsetUnknown2Address;       // int32  Pointer to unknown list. Only used in RAIL1.MPD. 5 values.
        private readonly int offsetScrollScreenAnimationAddr; // int32  Pointer to list of KA table for scroll screen animation.
        private readonly int offsetMesh1Address;          // int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int offsetMesh2Address;          // int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int offsetMesh3Address;          // int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int modelsPreYRotation;          // ANGLE  mostly 0x8000. The meshes from the models chunk are pre-rotated by this angle.
        private readonly int modelsViewAngleMin;          // ANGLE  mostly 0xb334. Has something to do with the view angle. more research necessary.
        private readonly int modelsViewAngleMax;          // ANGLE  mostly 0x4ccc. Has something to do with the view angle. more research necessary.
        private readonly int padding3Address;             // int16  Always zero
        private readonly int offsetTextureAnimAltAddress; // int32  Pointer to a list of texture indices. These textures are the same images as the "real" texture animations, but these textures are from the normal texture block (and doesn't seems to be used). See (#texture-animation-alternatives)
        private readonly int offsetPal1Address;           // int32  Pointer to 256 rgb16 colors. May be null.
        private readonly int offsetPal2Address;           // int32  Pointer to 256 rgb16 colors. May be null.

        // vvv Screnario 3+ only
        private readonly int offsetPal3Address;           // (scn3+pd) int32  Pointer to 256 rgb16 colors. May be null.
        private readonly int offsetIndexedTextures;       // (scn3+pd) int32  Pointer to unknown data.
        // ^^^ Screnario 3+ only

        private readonly int scrollScreenXAddress;        // int16  X Pos of scroll screen
        private readonly int scrollScreenYAddress;        // int16  Y Pos of scroll screen
        private readonly int scrollScreenZAddress;        // int16  Z Pos of scroll screen
        private readonly int scrollScreenAngleAddress;    // ANGLE  Yaw angle (X) of scroll screen
        private readonly int unknown1Address;             // int16  Unknown. Looks like a map coordinate.
        private readonly int backgroundScrollXAddress;    // int16  X coordinate for background scroll screen.
        private readonly int backgroundScrollYAddress;    // int16  Y coordinate for background scroll screen.
        private readonly int padding4Address;             // int16  Always zero
        private readonly int offsetBoundariesAddress;     // int32  Pointer to camera and battle boundaries in real-world coordinates.

        public MPDHeaderModel(IByteData data, int id, string name, int address, ScenarioType scenario)
        : base(data, id, name, address, 0x58) {
            Scenario = scenario;

            mapFlagsAddress             = Address;        // 2 bytes
            padding1Address             = Address + 0x02; // 2 bytes
            offsetLightPaletteAddress   = Address + 0x04; // 4 bytes
            offsetLightPosAddress       = Address + 0x08; // 4 bytes
            offsetUnknown1Address       = Address + 0x0C; // 4 bytes
            viewDistanceAddress         = Address + 0x10; // 2 bytes
            padding2Address             = Address + 0x12; // 2 bytes
            offsetModelSwitchGroupsAddr = Address + 0x14; // 4 bytes
            offsetTextureAnimationsAddress = Address + 0x18; // 4 bytes
            offsetUnknown2Address       = Address + 0x1C; // 4 bytes
            offsetScrollScreenAnimationAddr = Address + 0x20; // 4 bytes

            int addressNext;
            if (Scenario >= ScenarioType.Scenario1) {
                offsetMesh1Address = Address + 0x24; // 4 bytes
                offsetMesh2Address = Address + 0x28; // 4 bytes
                offsetMesh3Address = Address + 0x2C; // 4 bytes
                addressNext = Address + 0x30;
            }
            else {
                offsetMesh1Address = -1;
                offsetMesh2Address = -1;
                offsetMesh3Address = -1;
                addressNext = Address + 0x24;
            }

            modelsPreYRotation          = addressNext + 0x00; // 2 bytes
            modelsViewAngleMin          = addressNext + 0x02; // 2 bytes
            modelsViewAngleMax          = addressNext + 0x04; // 2 bytes
            padding3Address             = addressNext + 0x06; // 2 bytes
            offsetTextureAnimAltAddress = addressNext + 0x08; // 4 bytes
            offsetPal1Address           = addressNext + 0x0C; // 4 bytes
            offsetPal2Address           = addressNext + 0x10; // 4 bytes

            if (HasPalette3) {
                offsetPal3Address = addressNext + 0x14; // 4 bytes
                offsetIndexedTextures = addressNext + 0x18; // 4 bytes
                addressNext = addressNext + 0x1C;
            }
            else {
                offsetPal3Address = -1;
                offsetIndexedTextures = -1; // 4 bytes
                addressNext = addressNext + 0x14;
            }

            scrollScreenXAddress        = addressNext + 0x00; // 2 bytes
            scrollScreenYAddress        = addressNext + 0x02; // 2 bytes
            scrollScreenZAddress        = addressNext + 0x04; // 2 bytes
            scrollScreenAngleAddress    = addressNext + 0x06; // 2 bytes
            unknown1Address             = addressNext + 0x08; // 2 bytes
            backgroundScrollXAddress    = addressNext + 0x0A; // 2 bytes
            backgroundScrollYAddress    = addressNext + 0x0C; // 2 bytes
            padding4Address             = addressNext + 0x0E; // 2 bytes
            offsetBoundariesAddress     = addressNext + 0x10; // 4 bytes

            Size = (offsetBoundariesAddress - Address) + 0x04;
        }

        public ScenarioType Scenario { get; }

        public bool HasMeshes => Scenario >= ScenarioType.Scenario1;
        public bool HasPalette3 => Scenario >= ScenarioType.Scenario3;
        public bool HasIndexedTextures => Scenario >= ScenarioType.Scenario3;

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, displayFormat: "X4")]
        public ushort MapFlags {
            get => (ushort) Data.GetWord(mapFlagsAddress);
            set => Data.SetWord(mapFlagsAddress, value);
        }

        [TableViewModelColumn(displayOrder: 0.1f, displayName: "HasChunk19Model (Scn1)")]
        public bool HasChunk19Model {
            get => Scenario == ScenarioType.Scenario1 && (MapFlags & 0x0080) == 0x0080;
            set {
                if (Scenario == ScenarioType.Scenario1)
                    MapFlags = (ushort) ((MapFlags & ~0x0080) | (value ? 0x0080 : 0));
            }
        }

        [TableViewModelColumn(displayOrder: 0.20f, displayName: "Image Chunks Type")]
        public MPDImageChunksType ImageChunksType {
            get => MPDImageChunksTypeExtensions.FromMapFlags(MapFlags, Scenario);
            set => MapFlags = (ushort) ((MapFlags & ~MPDImageChunksTypeExtensions.ApplicableMapFlags(Scenario)) | value.ToMapFlags(Scenario));
        }

        [TableViewModelColumn(displayOrder: 0.21f, displayName: "Repeating Ground")]
        public bool HasRepeatingGround => ImageChunksType.HasRepeatingGround();

        [TableViewModelColumn(displayOrder: 0.22f, displayName: "Tiled Ground")]
        public bool HasTiledGround => ImageChunksType.HasTiledGround();

        [TableViewModelColumn(displayOrder: 0.23f, displayName: "Sky Box")]
        public bool HasSkyBox => ImageChunksType.HasSkyBox();

        [TableViewModelColumn(displayOrder: 0.24f, displayName: "Background")]
        public bool HasBackground => ImageChunksType.HasBackground();

        [TableViewModelColumn(displayOrder: 0.3f, displayName: "UseNewLighting (Scn2+)")]
        public bool UseNewLighting {
            get => Scenario >= ScenarioType.Scenario2 && (MapFlags & 0x2000) == 0x2000;
            set {
                if (Scenario == ScenarioType.Scenario2)
                    MapFlags = (ushort) ((MapFlags & ~0x2000) | (value ? 0x2000 : 0));
            }
        }

        [BulkCopy]
        public ushort Padding1 {
            get => (ushort) Data.GetWord(padding1Address);
            set => Data.SetWord(padding1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, isPointer: true)]
        public int OffsetLightPalette {
            get => Data.GetDouble(offsetLightPaletteAddress);
            set => Data.SetDouble(offsetLightPaletteAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 3, isPointer: true)]
        public int OffsetLightPosition {
            get => Data.GetDouble(offsetLightPosAddress);
            set => Data.SetDouble(offsetLightPosAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 4, isPointer: true)]
        public int OffsetUnknown1 {
            get => Data.GetDouble(offsetUnknown1Address);
            set => Data.SetDouble(offsetUnknown1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 5, displayFormat: "X2")]
        public ushort ViewDistance {
            get => (ushort) Data.GetWord(viewDistanceAddress);
            set => Data.SetWord(viewDistanceAddress, value);
        }

        [BulkCopy]
        public ushort Padding2 {
            get => (ushort) Data.GetWord(padding2Address);
            set => Data.SetWord(padding2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 7, isPointer: true)]
        public int OffsetModelSwitchGroups {
            get => Data.GetDouble(offsetModelSwitchGroupsAddr);
            set => Data.SetDouble(offsetModelSwitchGroupsAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 8, isPointer: true)]
        public int OffsetTextureAnimations {
            get => Data.GetDouble(offsetTextureAnimationsAddress);
            set => Data.SetDouble(offsetTextureAnimationsAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 9, isPointer: true)]
        public int OffsetUnknown2 {
            get => Data.GetDouble(offsetUnknown2Address);
            set => Data.SetDouble(offsetUnknown2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 10, isPointer: true)]
        public int OffsetScrollScreenAnimation {
            get => Data.GetDouble(offsetScrollScreenAnimationAddr);
            set => Data.SetDouble(offsetScrollScreenAnimationAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 11, isPointer: true)]
        public int OffsetMesh1 {
            get => HasMeshes ? Data.GetDouble(offsetMesh1Address) : 0;
            set {
                if (HasMeshes)
                    Data.SetDouble(offsetMesh1Address, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 12, isPointer: true)]
        public int OffsetMesh2 {
            get => HasMeshes ? Data.GetDouble(offsetMesh2Address) : 0;
            set {
                if (HasMeshes)
                    Data.SetDouble(offsetMesh2Address, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 13, isPointer: true)]
        public int OffsetMesh3 {
            get => HasMeshes ? Data.GetDouble(offsetMesh3Address) : 0;
            set {
                if (HasMeshes)
                    Data.SetDouble(offsetMesh3Address, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 14, displayFormat: "X4")]
        public ushort ModelsPreYRotation {
            get => (ushort) Data.GetWord(modelsPreYRotation);
            set => Data.SetWord(modelsPreYRotation, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 14.5f, displayFormat: "X4")]
        public ushort ModelsViewAngleMin {
            get => (ushort) Data.GetWord(modelsViewAngleMin);
            set => Data.SetWord(modelsViewAngleMin, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 15, displayFormat: "X4")]
        public ushort ModelsViewAngleMax {
            get => (ushort) Data.GetWord(modelsViewAngleMax);
            set => Data.SetWord(modelsViewAngleMax, value);
        }

        [BulkCopy]
        public ushort Padding3 {
            get => (ushort) Data.GetWord(padding3Address);
            set => Data.SetWord(padding3Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 16, isPointer: true)]
        public int OffsetTextureAnimAlt {
            get => Data.GetDouble(offsetTextureAnimAltAddress);
            set => Data.SetDouble(offsetTextureAnimAltAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 17, isPointer: true)]
        public int OffsetPal1 {
            get => Data.GetDouble(offsetPal1Address);
            set => Data.SetDouble(offsetPal1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 18, isPointer: true)]
        public int OffsetPal2 {
            get => Data.GetDouble(offsetPal2Address);
            set => Data.SetDouble(offsetPal2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: nameof(OffsetPal3) + " (Scn3+PD)", displayOrder: 19, isPointer: true)]
        public int OffsetPal3 {
            get => HasPalette3 ? Data.GetDouble(offsetPal3Address) : 0;
            set {
                if (HasPalette3)
                    Data.SetDouble(offsetPal3Address, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: nameof(OffsetIndexedTextures) + " (Scn3+PD)", displayOrder: 19.5f, isPointer: true)]
        public int OffsetIndexedTextures {
            get => HasIndexedTextures ? Data.GetDouble(offsetIndexedTextures) : 0;
            set {
                if (HasIndexedTextures)
                    Data.SetDouble(offsetIndexedTextures, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 20, displayFormat: "X4")]
        public ushort ScrollScreenX {
            get => (ushort) Data.GetWord(scrollScreenXAddress);
            set => Data.SetWord(scrollScreenXAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 20.5f, displayFormat: "X4")]
        public ushort ScrollScreenY {
            get => (ushort) Data.GetWord(scrollScreenYAddress);
            set => Data.SetWord(scrollScreenYAddress, value);
        }
        [BulkCopy]
        [TableViewModelColumn(displayOrder: 21, displayFormat: "X4")]
        public ushort ScrollScreenZ {
            get => (ushort) Data.GetWord(scrollScreenZAddress);
            set => Data.SetWord(scrollScreenZAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 21.5f, displayFormat: "X4")]
        public ushort ScrollScreenAngle {
            get => (ushort) Data.GetWord(scrollScreenAngleAddress);
            set => Data.SetWord(scrollScreenAngleAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 22)]
        public short Unknown1 {
            get => (short) Data.GetWord(unknown1Address);
            set => Data.SetWord(unknown1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 22.5f, displayFormat: "X4")]
        public ushort BackgroundScrollX {
            get => (ushort) Data.GetWord(backgroundScrollXAddress);
            set => Data.SetWord(backgroundScrollXAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 23, displayFormat: "X4")]
        public ushort BackgroundScrollY {
            get => (ushort) Data.GetWord(backgroundScrollYAddress);
            set => Data.SetWord(backgroundScrollYAddress, value);
        }

        [BulkCopy]
        public ushort Padding4 {
            get => (ushort) Data.GetWord(padding4Address);
            set => Data.SetWord(padding4Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 24, isPointer: true)]
        public int OffsetBoundaries {
            get => Data.GetDouble(offsetBoundariesAddress);
            set => Data.SetDouble(offsetBoundariesAddress, value);
        }
    }
}
