using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class MPDHeaderModel : Struct {
        private readonly int mapFlagsAddress;             // int16  Unknown. Might be map id.
        private readonly int padding1Address;             // int16  Always zero
        private readonly int offsetLightPaletteAddress;   // int32  Always 0x0c. Pointer to 32 values for light palette. See (#header-offset-1).
        private readonly int offsetLightPosAddress;       // int32  Always 0x4c. Pointer to light position. See (#header-offset-2).
        private readonly int offsetUnknown1Address;       // int32  Always 0x50. pointer to 0x20 unknown int16s at the start of the file. Mostly zero or 0x8000. (#header-offset-3)
        private readonly int offsetLightAdjustmentAddr;   // int32  Always 0x50. Replaces Scenario1 'unknown 1' table. A single structure with adjustments to overall lighting.
        private readonly int viewDistanceAddress;         // int16  Something like a view distance for meshes from the models chunk.
        private readonly int padding2Address;             // int16  Always zero
        private readonly int offsetModelSwitchGroupsAddr; // int32  Pointer to model switch group list.
        private readonly int offsetTextureAnimationsAddress; // int32 Offset to list of texture groups. See (#texture-groups)
        private readonly int offsetUnknown2Address;       // int32  Pointer to unknown list. Only used in RAIL1.MPD. 5 values.
        private readonly int offsetGradientAddress;       // int32  Pointer to gradient table that replaces Scenario 1 "unknown2" table.
        private readonly int offsetGroundAnimationAddr;   // int32  Pointer to list of KA table for ground model animation.
        private readonly int offsetMesh1Address;          // int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int offsetMesh2Address;          // int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int otherUnknownAddr;            // int32  Unknown value only present in 'Other' MPD files
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

        private readonly int groundXAddress;              // int16  X Pos of ground plane
        private readonly int groundYAddress;              // int16  Y Pos of ground plane
        private readonly int groundZAddress;              // int16  Z Pos of ground plane
        private readonly int groundAngleAddress;          // ANGLE  Yaw angle (X) of scroll screen
        private readonly int unknown1Address;             // int16  Unknown. Looks like a map coordinate.
        private readonly int backgroundXAddress;          // int16  X coordinate for background
        private readonly int backgroundYAddress;          // int16  Y coordinate for background
        private readonly int padding4Address;             // int16  Always zero
        private readonly int offsetBoundariesAddress;     // int32  Pointer to camera and battle boundaries in real-world coordinates.

        public MPDHeaderModel(IByteData data, int id, string name, int address, ScenarioType scenario)
        : base(data, id, name, address, 0x58) {
            Scenario = scenario;

            mapFlagsAddress             = Address;        // 2 bytes
            padding1Address             = Address + 0x02; // 2 bytes
            offsetLightPaletteAddress   = Address + 0x04; // 4 bytes
            offsetLightPosAddress       = Address + 0x08; // 4 bytes

            if (Scenario >= ScenarioType.Scenario2) {
                offsetUnknown1Address     = -1;
                offsetLightAdjustmentAddr = Address + 0x0C; // 4 bytes
            }
            else {
                offsetUnknown1Address     = Address + 0x0C; // 4 bytes
                offsetLightAdjustmentAddr = -1;
            }

            viewDistanceAddress         = Address + 0x10; // 2 bytes
            padding2Address             = Address + 0x12; // 2 bytes
            offsetModelSwitchGroupsAddr = Address + 0x14; // 4 bytes
            offsetTextureAnimationsAddress = Address + 0x18; // 4 bytes

            if (Scenario >= ScenarioType.Scenario2) {
                offsetGradientAddress = Address + 0x1C; // 4 bytes
                offsetUnknown2Address = -1;
            }
            else {
                offsetGradientAddress = -1;
                offsetUnknown2Address = Address + 0x1C; // 4 bytes
            }

            offsetGroundAnimationAddr   = Address + 0x20; // 4 bytes
            offsetMesh1Address          = Address + 0x24; // 4 bytes
            offsetMesh2Address          = Address + 0x28; // 4 bytes

            int addressNext;
            if (Scenario >= ScenarioType.Scenario1) {
                offsetMesh3Address = Address + 0x2C; // 4 bytes
                otherUnknownAddr   = -1;
                addressNext = Address + 0x30;
            }
            else if (Scenario == ScenarioType.Other) {
                offsetMesh3Address = -1;
                otherUnknownAddr   = Address + 0x2C; // 4 bytes
                addressNext = Address + 0x30;
            }
            else {
                otherUnknownAddr   = -1;
                offsetMesh3Address = -1;
                addressNext = Address + 0x2C;
            }

            if (Scenario != ScenarioType.Other) {
                modelsPreYRotation = addressNext + 0x00; // 2 bytes
                modelsViewAngleMin = addressNext + 0x02; // 2 bytes
                modelsViewAngleMax = addressNext + 0x04; // 2 bytes
                padding3Address    = addressNext + 0x06; // 2 bytes
                offsetTextureAnimAltAddress = addressNext + 0x08; // 4 bytes
                addressNext += 0x0C;
            }
            else {
                modelsPreYRotation = -1;
                modelsViewAngleMin = -1;
                modelsViewAngleMax = -1;
                padding3Address    = -1;
                offsetTextureAnimAltAddress = -1;
                // TODO: missing 4-byte value
                addressNext += 0x04;
            }

            offsetPal1Address           = addressNext + 0x00; // 4 bytes
            offsetPal2Address           = addressNext + 0x04; // 4 bytes

            if (HasPalette3) {
                offsetPal3Address = addressNext + 0x08; // 4 bytes
                addressNext += 0x0C;
            }
            else {
                offsetPal3Address = -1;
                addressNext += 0x08;
            }

            if (HasIndexedTextures) {
                offsetIndexedTextures = addressNext; // 4 bytes
                addressNext += 0x04;
            }
            else
                offsetIndexedTextures = -1; // 4 bytes

            groundXAddress              = addressNext + 0x00; // 2 bytes
            groundYAddress              = addressNext + 0x02; // 2 bytes
            groundZAddress              = addressNext + 0x04; // 2 bytes
            groundAngleAddress          = addressNext + 0x06; // 2 bytes
            unknown1Address             = addressNext + 0x08; // 2 bytes
            backgroundXAddress          = addressNext + 0x0A; // 2 bytes
            backgroundYAddress          = addressNext + 0x0C; // 2 bytes
            padding4Address             = addressNext + 0x0E; // 2 bytes
            offsetBoundariesAddress     = addressNext + 0x10; // 4 bytes

            Size = (offsetBoundariesAddress - Address) + 0x04;
        }

        public ScenarioType Scenario { get; }

        public bool HasUnknown1Table => Scenario <= ScenarioType.Scenario1;
        public bool HasLightAdjustmentTable => Scenario >= ScenarioType.Scenario2;
        public bool HasUnknown2Table => Scenario <= ScenarioType.Scenario1;
        public bool HasGradientTable => Scenario >= ScenarioType.Scenario2;
        public bool HasMesh3 => Scenario >= ScenarioType.Scenario1;
        public bool HasModelsInfo => Scenario != ScenarioType.Other;
        public bool HasPalette3 => Scenario >= ScenarioType.Scenario3 || Scenario == ScenarioType.Other;
        public bool HasIndexedTextures => Scenario >= ScenarioType.Scenario3;

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, displayFormat: "X4")]
        public ushort MapFlags {
            get => (ushort) Data.GetWord(mapFlagsAddress);
            set => Data.SetWord(mapFlagsAddress, value);
        }

        [TableViewModelColumn(displayOrder: 0x025f, displayName: "HasSurfaceTextureRotation (Scn3+)")]
        public bool HasSurfaceTextureRotation {
            get => Scenario >= ScenarioType.Scenario3 && (MapFlags & 0x0002) == 0x0002;
            set {
                if (Scenario >= ScenarioType.Scenario3)
                    MapFlags = value ? (ushort) (MapFlags & ~0x0002) : (ushort) (MapFlags | 0x0002);
            }
        }

        [TableViewModelColumn(displayOrder: 0.05f)]
        public bool HasSurfaceModel {
            get => (MapFlags & 0x0200) == 0x0200;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0200) : (ushort) (MapFlags & ~0x0200);
        }

        [TableViewModelColumn(displayOrder: 0.055f, displayName: "HasHighMemoryChunk1 (Scn1)")]
        public bool HasHighMemoryChunk1
            => Scenario <= ScenarioType.Scenario1 && HasSurfaceModel;

        [TableViewModelColumn(displayOrder: 0.06f, displayName: "Chunk20IsSurfaceModelIfExists (Scn2+)")]
        public bool Chunk20IsSurfaceModelIfExists {
            get => Scenario >= ScenarioType.Scenario2 ? (MapFlags & 0x8000) == 0x8000 : false;
            set {
                if (Scenario >= ScenarioType.Scenario2)
                    MapFlags = value ? (ushort) (MapFlags | 0x8000) : (ushort) (MapFlags & ~0x8000);
            }
        }

        [TableViewModelColumn(displayOrder: 0.07f, displayName: "Chunk20IsModelsIfExists (Scn2+)")]
        public bool Chunk20IsModels
            => Scenario >= ScenarioType.Scenario2 && HasSurfaceModel && !Chunk20IsSurfaceModelIfExists;

        [TableViewModelColumn(displayOrder: 0.1f, displayName: "HasChunk19Model (Scn1)")]
        public bool HasChunk19Model {
            get => Scenario == ScenarioType.Scenario1 && (MapFlags & 0x0080) == 0x0080;
            set {
                if (Scenario == ScenarioType.Scenario1)
                    MapFlags = (ushort) ((MapFlags & ~0x0080) | (value ? 0x0080 : 0));
            }
        }

        [TableViewModelColumn(displayOrder: 0.11f, displayName: "HasChunk19Model (Scn1)")]
        public bool HasExtraChunk1ModelWithChunk21Textures {
            get => Scenario >= ScenarioType.Scenario2 && (MapFlags & 0x4000) == 0x4000;
            set {
                if (Scenario >= ScenarioType.Scenario2)
                    MapFlags = (ushort) (value ? MapFlags | 0x4000 : MapFlags & ~0x4000);
            }
        }

        [TableViewModelColumn(displayOrder: 0.20f, displayName: "Background Image Type")]
        public BackgroundImageType BackgroundImageType {
            get => BackgroundImageTypeExtensions.FromMapFlags(MapFlags);
            set => MapFlags = (ushort) ((MapFlags & ~BackgroundImageTypeExtensions.ApplicableMapFlags) | value.ToMapFlags());
        }

        [TableViewModelColumn(displayOrder: 0.21f, displayName: "Ground Image Type")]
        public GroundImageType GroundImageType {
            get => GroundImageTypeExtensions.FromMapFlags(MapFlags);
            set => MapFlags = (ushort) ((MapFlags & ~GroundImageTypeExtensions.ApplicableMapFlags) | value.ToMapFlags());
        }

        private int SkyBoxFlag => (Scenario == ScenarioType.Scenario1) ? 0x2000 : 0x0800;

        [TableViewModelColumn(displayOrder: 0.22f, displayName: "Sky Box")]
        public bool HasSkyBox {
            get => (MapFlags & SkyBoxFlag) != 0;
            set => MapFlags = (ushort) ((MapFlags & ~SkyBoxFlag) | (value ? SkyBoxFlag : 0));
        }
 
        [TableViewModelColumn(displayOrder: 0.3f, displayName: "Outdoor Lighting (Scn2+)")]
        public bool OutdoorLighting {
            get => Scenario >= ScenarioType.Scenario2 && (MapFlags & 0x2000) == 0x2000;
            set {
                if (Scenario >= ScenarioType.Scenario2)
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
            get => HasUnknown1Table ? Data.GetDouble(offsetUnknown1Address) : 0;
            set {
                if (HasUnknown1Table)
                    Data.SetDouble(offsetUnknown1Address, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 4, isPointer: true)]
        public int OffsetLightAdjustment {
            get => HasLightAdjustmentTable ? Data.GetDouble(offsetLightAdjustmentAddr) : 0;
            set {
                if (HasLightAdjustmentTable)
                    Data.SetDouble(offsetLightAdjustmentAddr, value);
            }
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
            get => HasUnknown2Table ? Data.GetDouble(offsetUnknown2Address) : 0;
            set {
                if (HasUnknown2Table)
                    Data.SetDouble(offsetUnknown2Address, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 9.5f, isPointer: true)]
        public int OffsetGradient {
            get => HasGradientTable ? Data.GetDouble(offsetGradientAddress) : 0;
            set {
                if (HasGradientTable)
                    Data.SetDouble(offsetGradientAddress, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 10, isPointer: true)]
        public int OffsetGroundAnimation {
            get => Data.GetDouble(offsetGroundAnimationAddr);
            set => Data.SetDouble(offsetGroundAnimationAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 11, isPointer: true)]
        public int OffsetMesh1 {
            get => Data.GetDouble(offsetMesh1Address);
            set => Data.SetDouble(offsetMesh1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 12, isPointer: true)]
        public int OffsetMesh2 {
            get => Data.GetDouble(offsetMesh2Address);
            set => Data.SetDouble(offsetMesh2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 13, isPointer: true)]
        public int OffsetMesh3 {
            get => HasMesh3 ? Data.GetDouble(offsetMesh3Address) : 0;
            set {
                if (HasMesh3)
                    Data.SetDouble(offsetMesh3Address, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 14, displayFormat: "X4")]
        public ushort ModelsPreYRotation {
            get => HasModelsInfo ? (ushort) Data.GetWord(modelsPreYRotation) : (ushort) 0;
            set {
                if (HasModelsInfo)
                    Data.SetWord(modelsPreYRotation, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 14.5f, displayFormat: "X4")]
        public ushort ModelsViewAngleMin {
            get => HasModelsInfo ? (ushort) Data.GetWord(modelsViewAngleMin) : (ushort) 0;
            set {
                if (HasModelsInfo)
                    Data.SetWord(modelsViewAngleMin, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 15, displayFormat: "X4")]
        public ushort ModelsViewAngleMax {
            get => HasModelsInfo ? (ushort) Data.GetWord(modelsViewAngleMax) : (ushort) 0;
            set {
                if (HasModelsInfo)
                    Data.SetWord(modelsViewAngleMax, value);
            }
        }

        [BulkCopy]
        public ushort Padding3 {
            get => HasModelsInfo ? (ushort) Data.GetWord(padding3Address) : (ushort) 0;
            set {
                if (HasModelsInfo)
                    Data.SetWord(padding3Address, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 16, isPointer: true)]
        public int OffsetTextureAnimAlt {
            // TODO: Create HasOffsetTexturesAnimAlt for this case
            get => HasModelsInfo ? Data.GetDouble(offsetTextureAnimAltAddress) : 0;
            set {
                if (HasModelsInfo)
                    Data.SetDouble(offsetTextureAnimAltAddress, value);
            }
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
        [TableViewModelColumn(displayOrder: 20)]
        public short GroundX {
            get => (short) Data.GetWord(groundXAddress);
            set => Data.SetWord(groundXAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 20.5f)]
        public short GroundY {
            get => (short) Data.GetWord(groundYAddress);
            set => Data.SetWord(groundYAddress, value);
        }
        [BulkCopy]
        [TableViewModelColumn(displayOrder: 21)]
        public short GroundZ {
            get => (short) Data.GetWord(groundZAddress);
            set => Data.SetWord(groundZAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 21.5f)]
        public float GroundAngle {
            get => Data.GetCompressedFIXED(groundAngleAddress).Float;
            set => Data.SetCompressedFIXED(groundAngleAddress, new CompressedFIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 22)]
        public short Unknown1 {
            get => (short) Data.GetWord(unknown1Address);
            set => Data.SetWord(unknown1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 22.5f)]
        public short BackgroundX {
            get => (short) Data.GetWord(backgroundXAddress);
            set => Data.SetWord(backgroundXAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 23)]
        public short BackgroundY {
            get => (short) Data.GetWord(backgroundYAddress);
            set => Data.SetWord(backgroundYAddress, value);
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
