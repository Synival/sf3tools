using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class MPDHeaderModel : Struct {
        private readonly int _mapFlagsAddr;                // int16  Unknown. Might be map id.
        private readonly int _padding1Addr;                // int16  Always zero
        private readonly int _offsetLightPaletteAddr;      // int32  Always 0x0c. Pointer to 32 values for light palette. See (#header-offset-1).
        private readonly int _offsetLightPosAddr;          // int32  Always 0x4c. Pointer to light position. See (#header-offset-2).
        private readonly int _offsetUnknown1Addr;          // int32  Always 0x50. pointer to 0x20 unknown int16s at the start of the file. Mostly zero or 0x8000. (#header-offset-3)
        private readonly int _offsetLightAdjustmentAddr;   // int32  Always 0x50. Replaces Scenario1 'unknown 1' table. A single structure with adjustments to overall lighting.
        private readonly int _viewDistanceAddr;            // int16  Something like a view distance for meshes from the models chunk.
        private readonly int _padding2Addr;                // int16  Always zero
        private readonly int _offsetModelSwitchGroupsAddr; // int32  Pointer to model switch group list.
        private readonly int _offsetTextureAnimationsAddr; // int32 Offset to list of texture groups. See (#texture-groups)
        private readonly int _offsetUnknown2Addr;          // int32  Pointer to unknown list. Only used in RAIL1.MPD. 5 values.
        private readonly int _offsetGradientAddr;          // int32  Pointer to gradient table that replaces Scenario 1 "unknown2" table.
        private readonly int _offsetGroundAnimationAddr;   // int32  Pointer to list of KA table for ground model animation.
        private readonly int _offsetMesh1Addr;             // int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int _offsetMesh2Addr;             // int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int _otherUnknownAddr;            // int32  Unknown value only present in 'Other' MPD files
        private readonly int _offsetMesh3Addr;             // int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int _modelsPreYRotationAddr;      // ANGLE  mostly 0x8000. The meshes from the models chunk are pre-rotated by this angle.
        private readonly int _modelsViewAngleMinAddr;      // ANGLE  mostly 0xb334. Has something to do with the view angle. more research necessary.
        private readonly int _modelsViewAngleMaxAddr;      // ANGLE  mostly 0x4ccc. Has something to do with the view angle. more research necessary.
        private readonly int _padding3Addr;                // int16  Always zero
        private readonly int _offsetTextureAnimAltAddr;    // int32  Pointer to a list of texture indices. These textures are the same images as the "real" texture animations, but these textures are from the normal texture block (and doesn't seems to be used). See (#texture-animation-alternatives)
        private readonly int _offsetPal1Addr;              // int32  Pointer to 256 rgb16 colors. May be null.
        private readonly int _offsetPal2Addr;              // int32  Pointer to 256 rgb16 colors. May be null.

        // vvv Screnario 3+ only
        private readonly int _offsetPal3Addr;              // (scn3+pd) int32  Pointer to 256 rgb16 colors. May be null.
        private readonly int _offsetIndexedTexturesAddr;   // (scn3+pd) int32  Pointer to unknown data.
        // ^^^ Screnario 3+ only

        private readonly int _groundXAddr;                 // int16  X Pos of ground plane
        private readonly int _groundYAddr;                 // int16  Y Pos of ground plane
        private readonly int _groundZAddr;                 // int16  Z Pos of ground plane
        private readonly int _groundAngleAddr;             // ANGLE  Yaw angle (X) of scroll screen
        private readonly int _unknown1Addr;                // int16  Unknown. Looks like a map coordinate.
        private readonly int _backgroundXAddr;             // int16  X coordinate for background
        private readonly int _backgroundYAddr;             // int16  Y coordinate for background
        private readonly int _padding4Addr;                // int16  Always zero
        private readonly int _offsetBoundariesAddr;        // int32  Pointer to camera and battle boundaries in real-world coordinates.

        public MPDHeaderModel(IByteData data, int id, string name, int address, ScenarioType scenario)
        : base(data, id, name, address, 0x58) {
            Scenario = scenario;

            _mapFlagsAddr           = Address;        // 2 bytes
            _padding1Addr           = Address + 0x02; // 2 bytes
            _offsetLightPaletteAddr = Address + 0x04; // 4 bytes
            _offsetLightPosAddr     = Address + 0x08; // 4 bytes

            if (Scenario >= ScenarioType.Scenario2) {
                _offsetUnknown1Addr        = -1;
                _offsetLightAdjustmentAddr = Address + 0x0C; // 4 bytes
            }
            else {
                _offsetUnknown1Addr        = Address + 0x0C; // 4 bytes
                _offsetLightAdjustmentAddr = -1;
            }

            _viewDistanceAddr            = Address + 0x10; // 2 bytes
            _padding2Addr                = Address + 0x12; // 2 bytes
            _offsetModelSwitchGroupsAddr = Address + 0x14; // 4 bytes
            _offsetTextureAnimationsAddr = Address + 0x18; // 4 bytes

            if (Scenario >= ScenarioType.Scenario2) {
                _offsetGradientAddr = Address + 0x1C; // 4 bytes
                _offsetUnknown2Addr = -1;
            }
            else {
                _offsetGradientAddr = -1;
                _offsetUnknown2Addr = Address + 0x1C; // 4 bytes
            }

            _offsetGroundAnimationAddr = Address + 0x20; // 4 bytes
            _offsetMesh1Addr           = Address + 0x24; // 4 bytes
            _offsetMesh2Addr           = Address + 0x28; // 4 bytes

            int addressNext;
            if (Scenario >= ScenarioType.Scenario1) {
                _offsetMesh3Addr  = Address + 0x2C; // 4 bytes
                _otherUnknownAddr = -1;
                addressNext = Address + 0x30;
            }
            else if (Scenario == ScenarioType.Other) {
                _offsetMesh3Addr  = -1;
                _otherUnknownAddr = Address + 0x2C; // 4 bytes
                addressNext = Address + 0x30;
            }
            else {
                _otherUnknownAddr = -1;
                _offsetMesh3Addr  = -1;
                addressNext = Address + 0x2C;
            }

            if (Scenario != ScenarioType.Other) {
                _modelsPreYRotationAddr   = addressNext + 0x00; // 2 bytes
                _modelsViewAngleMinAddr   = addressNext + 0x02; // 2 bytes
                _modelsViewAngleMaxAddr   = addressNext + 0x04; // 2 bytes
                _padding3Addr             = addressNext + 0x06; // 2 bytes
                _offsetTextureAnimAltAddr = addressNext + 0x08; // 4 bytes
                addressNext += 0x0C;
            }
            else {
                _modelsPreYRotationAddr   = -1;
                _modelsViewAngleMinAddr   = -1;
                _modelsViewAngleMaxAddr   = -1;
                _padding3Addr             = -1;
                _offsetTextureAnimAltAddr = -1;
                // TODO: missing 4-byte value
                addressNext += 0x04;
            }

            _offsetPal1Addr = addressNext + 0x00; // 4 bytes
            _offsetPal2Addr = addressNext + 0x04; // 4 bytes

            if (HasPalette3) {
                _offsetPal3Addr = addressNext + 0x08; // 4 bytes
                addressNext += 0x0C;
            }
            else {
                _offsetPal3Addr = -1;
                addressNext += 0x08;
            }

            if (HasIndexedTextures) {
                _offsetIndexedTexturesAddr = addressNext; // 4 bytes
                addressNext += 0x04;
            }
            else
                _offsetIndexedTexturesAddr = -1; // 4 bytes

            _groundXAddr          = addressNext + 0x00; // 2 bytes
            _groundYAddr          = addressNext + 0x02; // 2 bytes
            _groundZAddr          = addressNext + 0x04; // 2 bytes
            _groundAngleAddr      = addressNext + 0x06; // 2 bytes
            _unknown1Addr         = addressNext + 0x08; // 2 bytes
            _backgroundXAddr      = addressNext + 0x0A; // 2 bytes
            _backgroundYAddr      = addressNext + 0x0C; // 2 bytes
            _padding4Addr         = addressNext + 0x0E; // 2 bytes
            _offsetBoundariesAddr = addressNext + 0x10; // 4 bytes

            Size = (_offsetBoundariesAddr - Address) + 0x04;
        }

        public ScenarioType Scenario { get; }

        public bool IsScenario1 => Scenario == ScenarioType.Scenario1;
        public bool IsScenario1OrEarlier => Scenario <= ScenarioType.Scenario1;
        public bool IsScenario1OrLater => Scenario >= ScenarioType.Scenario1;
        public bool IsScenario2OrEarlier => Scenario <= ScenarioType.Scenario2;
        public bool IsScenario2OrLater => Scenario >= ScenarioType.Scenario2;
        public bool IsScenario3OrLater => Scenario >= ScenarioType.Scenario3;

        public bool HasUnknown1Table => IsScenario1OrEarlier;
        public bool HasLightAdjustmentTable => IsScenario2OrLater;
        public bool HasUnknown2Table => IsScenario1OrEarlier;
        public bool HasGradientTable => IsScenario2OrLater;
        public bool HasMesh3 => IsScenario1OrLater;
        public bool HasModelsInfo => Scenario != ScenarioType.Other;
        public bool HasPalette3 => IsScenario3OrLater || Scenario == ScenarioType.Other;
        public bool HasIndexedTextures => IsScenario3OrLater;

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_mapFlagsAddr), displayOrder: 0, displayFormat: "X4", displayGroup: "Main")]
        public ushort MapFlags {
            get => (ushort) Data.GetWord(_mapFlagsAddr);
            set => Data.SetWord(_mapFlagsAddr, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0001f, displayName: "(0x0001) " + nameof(UnknownMapFlag0x0001), displayGroup: "Flags")]
        public bool UnknownMapFlag0x0001 {
            get => (MapFlags & 0x0001) == 0x0001;
            set => MapFlags = value ? (ushort) (MapFlags & ~0x0001) : (ushort) (MapFlags | 0x0001);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0002f, displayName: "(0x0002) " + nameof(UnknownMapFlag0x0002) + " (Scn1,2)", visibilityProperty: nameof(IsScenario2OrEarlier), displayGroup: "Flags")]
        public bool UnknownMapFlag0x0002 {
            get => Scenario < ScenarioType.Scenario3 && (MapFlags & 0x0002) == 0x0002;
            set {
                if (Scenario < ScenarioType.Scenario3)
                    MapFlags = value ? (ushort) (MapFlags & ~0x0002) : (ushort) (MapFlags | 0x0002);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.00021f, displayName: "(0x0002) " + nameof(HasSurfaceTextureRotation) + " (Scn3+)", visibilityProperty: nameof(IsScenario3OrLater), displayGroup: "Flags")]
        public bool HasSurfaceTextureRotation {
            get => Scenario >= ScenarioType.Scenario3 && (MapFlags & 0x0002) == 0x0002;
            set {
                if (Scenario >= ScenarioType.Scenario3)
                    MapFlags = value ? (ushort) (MapFlags & ~0x0002) : (ushort) (MapFlags | 0x0002);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0004f, displayName: "(0x0004) " + nameof(AddDotProductBasedNoiseToStandardLightmap), displayGroup: "Flags")]
        public bool AddDotProductBasedNoiseToStandardLightmap {
            get => (MapFlags & 0x0004) == 0x0004;
            set => MapFlags = value ? (ushort) (MapFlags & ~0x0004) : (ushort) (MapFlags | 0x0004);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0008f, displayName: "(0x0008) " + nameof(UnknownFlatTileFlag0x0008), displayGroup: "Flags")]
        public bool UnknownFlatTileFlag0x0008 {
            get => (MapFlags & 0x0008) == 0x0008;
            set => MapFlags = value ? (ushort) (MapFlags & ~0x0008) : (ushort) (MapFlags | 0x0008);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0010f, displayName: "(0x0010 | 40) " + nameof(BackgroundImageType), minWidth: 100, displayGroup: "Flags")]
        public BackgroundImageType BackgroundImageType {
            get => BackgroundImageTypeExtensions.FromMapFlags(MapFlags);
            set => MapFlags = (ushort) ((MapFlags & ~BackgroundImageTypeExtensions.ApplicableMapFlags) | value.ToMapFlags());
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0020f, displayName: "(0x0020) " + nameof(UnknownMapFlag0x0020), displayGroup: "Flags")]
        public bool UnknownMapFlag0x0020 {
            get => (MapFlags & 0x0020) == 0x0020;
            set => MapFlags = value ? (ushort) (MapFlags & ~0x0020) : (ushort) (MapFlags | 0x0020);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0080f, displayName: "(0x0080) " + nameof(HasChunk19Model) + " (Scn1)", visibilityProperty: nameof(IsScenario1), displayGroup: "Flags")]
        public bool HasChunk19Model {
            get => IsScenario1 && (MapFlags & 0x0080) == 0x0080;
            set {
                if (IsScenario1)
                    MapFlags = (ushort) ((MapFlags & ~0x0080) | (value ? 0x0080 : 0));
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0081f, displayName: "(0x0080) " + nameof(ModifyPalette1ForGradient) + " (Redundant) (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool ModifyPalette1ForGradient {
            get => Scenario >= ScenarioType.Scenario2 && (MapFlags & 0x0080) == 0x0080;
            set {
                if (Scenario >= ScenarioType.Scenario2)
                    MapFlags = value ? (ushort) (MapFlags & ~0x0080) : (ushort) (MapFlags | 0x0080);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0100f, displayName: "(0x0100) " + nameof(HasModels), displayGroup: "Flags")]
        public bool HasModels {
            get => (MapFlags & 0x0100) == 0x0100;
            set => MapFlags = value ? (ushort) (MapFlags & ~0x0100) : (ushort) (MapFlags | 0x0100);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0200f, displayName: "(0x0200) " + nameof(HasSurfaceModel), displayGroup: "Flags")]
        public bool HasSurfaceModel {
            get => (MapFlags & 0x0200) == 0x0200;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0200) : (ushort) (MapFlags & ~0x0200);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0400f, displayName: "(0x0400 | 1000): Ground Image Type", minWidth: 100, displayGroup: "Flags")]
        public GroundImageType GroundImageType {
            get => GroundImageTypeExtensions.FromMapFlags(MapFlags);
            set => MapFlags = (ushort) ((MapFlags & ~GroundImageTypeExtensions.ApplicableMapFlags) | value.ToMapFlags());
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0801f, displayName: "(0x0800) " + nameof(HasCutsceneSkyBox) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool HasCutsceneSkyBox {
            get => IsScenario2OrLater ? (MapFlags & 0x0800) == 0x0800 : false;
            set {
                if (IsScenario2OrLater)
                    MapFlags = value ? (ushort) (MapFlags | 0x0800) : (ushort) (MapFlags & ~0x0800);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.2000f, displayName: "(0x2000) " + nameof(HasBattleSkyBox) + " (Scn1)", visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool HasBattleSkyBox {
            get => IsScenario1OrEarlier ? (MapFlags & 0x2000) == 0x2000 : false;
            set {
                if (IsScenario1OrEarlier)
                    MapFlags = value ? (ushort) (MapFlags | 0x2000) : (ushort) (MapFlags & ~0x2000);
            }
        }
 
        [TableViewModelColumn(addressField: null, displayOrder: 0.2001f, displayName: "(0x2000) " + nameof(NarrowAngleBasedLightmap) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool NarrowAngleBasedLightmap {
            get => IsScenario2OrLater && (MapFlags & 0x2000) == 0x2000;
            set {
                if (IsScenario2OrLater)
                    MapFlags = (ushort) ((MapFlags & ~0x2000) | (value ? 0x2000 : 0));
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.4001f, displayName: "(0x4000) " + nameof(HasExtraChunk1ModelWithChunk21Textures) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool HasExtraChunk1ModelWithChunk21Textures {
            get => IsScenario2OrLater && (MapFlags & 0x4000) == 0x4000;
            set {
                if (IsScenario2OrLater)
                    MapFlags = (ushort) (value ? MapFlags | 0x4000 : MapFlags & ~0x4000);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.8000f, displayName: "(0x8000) " + nameof(Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists) + " (Scn1)", visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists {
            get => IsScenario1OrEarlier ? (MapFlags & 0x8000) == 0x8000 : false;
            set {
                if (IsScenario1OrEarlier)
                    MapFlags = value ? (ushort) (MapFlags | 0x8000) : (ushort) (MapFlags & ~0x8000);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.8010f, displayName: "(Derived) " + nameof(Chunk1IsLoadedFromLowMemory), visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Chunk1IsLoadedFromLowMemory
            => HasModels && (IsScenario1OrEarlier ? (!HasSurfaceModel || Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists) : Chunk1IsModels);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8011f, displayName: "(Derived) " + nameof(Chunk1IsLoadedFromHighMemory), visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Chunk1IsLoadedFromHighMemory
            => HasModels && IsScenario1OrEarlier && HasSurfaceModel && !Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists;

        [TableViewModelColumn(addressField: null, displayOrder: 0.8012f, displayName: "(0x8000) " + nameof(Chunk20IsSurfaceModelIfExists) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk20IsSurfaceModelIfExists {
            get => IsScenario2OrLater ? (MapFlags & 0x8000) == 0x8000 : false;
            set {
                if (IsScenario2OrLater)
                    MapFlags = value ? (ushort) (MapFlags | 0x8000) : (ushort) (MapFlags & ~0x8000);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.8013f, displayName: "(Derived) " + nameof(Chunk1IsModels) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk1IsModels
            => HasModels && (IsScenario1OrEarlier || (!HasSurfaceModel || Chunk20IsSurfaceModelIfExists || HasExtraChunk1ModelWithChunk21Textures));

        [TableViewModelColumn(addressField: null, displayOrder: 0.8014f, displayName: "(Derived) " + nameof(Chunk2IsSurfaceModel) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk2IsSurfaceModel
            => HasSurfaceModel && (IsScenario1OrEarlier || !Chunk20IsSurfaceModelIfExists);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8015f, displayName: "(Derived) " + nameof(Chunk20IsSurfaceModel) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk20IsSurfaceModel
            => IsScenario2OrLater && HasSurfaceModel && Chunk20IsSurfaceModelIfExists;

        [TableViewModelColumn(addressField: null, displayOrder: 0.8016f, displayName: "(Derived) " + nameof(Chunk20IsModels) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk20IsModels
            => IsScenario2OrLater && HasSurfaceModel && !Chunk20IsSurfaceModelIfExists;

        [TableViewModelColumn(addressField: null, displayOrder: 0.8017f, displayName: "(Derived) " + nameof(HighMemoryHasModels), displayGroup: "Flags")]
        public bool HighMemoryHasModels => HasModels && (IsScenario1OrEarlier ? Chunk1IsLoadedFromHighMemory : Chunk20IsModels);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8018f, displayName: "(Derived) " + nameof(HighMemoryHasSurfaceModel) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool HighMemoryHasSurfaceModel => HasSurfaceModel && IsScenario2OrLater && Chunk20IsSurfaceModel;

        public bool HasAnySkyBox => HasCutsceneSkyBox || HasBattleSkyBox;

        [BulkCopy]
        public ushort Padding1 {
            get => (ushort) Data.GetWord(_padding1Addr);
            set => Data.SetWord(_padding1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetLightPaletteAddr), displayOrder: 2, isPointer: true, displayGroup: "Main")]
        public int OffsetLightPalette {
            get => Data.GetDouble(_offsetLightPaletteAddr);
            set => Data.SetDouble(_offsetLightPaletteAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetLightPosAddr), displayOrder: 3, isPointer: true, displayGroup: "Main")]
        public int OffsetLightPosition {
            get => Data.GetDouble(_offsetLightPosAddr);
            set => Data.SetDouble(_offsetLightPosAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetUnknown1Addr), displayOrder: 4, isPointer: true, displayName: nameof(OffsetUnknown1) + " (Scn1)", visibilityProperty: nameof(HasUnknown1Table), displayGroup: "Main")]
        public int OffsetUnknown1 {
            get => HasUnknown1Table ? Data.GetDouble(_offsetUnknown1Addr) : 0;
            set {
                if (HasUnknown1Table)
                    Data.SetDouble(_offsetUnknown1Addr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetLightAdjustmentAddr), displayOrder: 4, isPointer: true, displayName: nameof(OffsetLightAdjustment) + " (Scn2+)", visibilityProperty: nameof(HasLightAdjustmentTable), displayGroup: "Main")]
        public int OffsetLightAdjustment {
            get => HasLightAdjustmentTable ? Data.GetDouble(_offsetLightAdjustmentAddr) : 0;
            set {
                if (HasLightAdjustmentTable)
                    Data.SetDouble(_offsetLightAdjustmentAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_viewDistanceAddr), displayOrder: 5, displayFormat: "X2", displayGroup: "Main")]
        public ushort ViewDistance {
            get => (ushort) Data.GetWord(_viewDistanceAddr);
            set => Data.SetWord(_viewDistanceAddr, value);
        }

        [BulkCopy]
        public ushort Padding2 {
            get => (ushort) Data.GetWord(_padding2Addr);
            set => Data.SetWord(_padding2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetModelSwitchGroupsAddr), displayOrder: 7, isPointer: true, displayGroup: "Main")]
        public int OffsetModelSwitchGroups {
            get => Data.GetDouble(_offsetModelSwitchGroupsAddr);
            set => Data.SetDouble(_offsetModelSwitchGroupsAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetTextureAnimationsAddr), displayOrder: 8, isPointer: true, displayGroup: "Main")]
        public int OffsetTextureAnimations {
            get => Data.GetDouble(_offsetTextureAnimationsAddr);
            set => Data.SetDouble(_offsetTextureAnimationsAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetUnknown2Addr), displayOrder: 9, isPointer: true, displayName: nameof(OffsetUnknown2) + " (Scn1)", visibilityProperty: nameof(HasUnknown2Table), displayGroup: "Main")]
        public int OffsetUnknown2 {
            get => HasUnknown2Table ? Data.GetDouble(_offsetUnknown2Addr) : 0;
            set {
                if (HasUnknown2Table)
                    Data.SetDouble(_offsetUnknown2Addr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetGradientAddr), displayOrder: 9.5f, isPointer: true, displayName: nameof(OffsetGradient) + " (Scn2+)", visibilityProperty: nameof(HasGradientTable), displayGroup: "Main")]
        public int OffsetGradient {
            get => HasGradientTable ? Data.GetDouble(_offsetGradientAddr) : 0;
            set {
                if (HasGradientTable)
                    Data.SetDouble(_offsetGradientAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetGroundAnimationAddr), displayOrder: 10, isPointer: true, displayGroup: "Main")]
        public int OffsetGroundAnimation {
            get => Data.GetDouble(_offsetGroundAnimationAddr);
            set => Data.SetDouble(_offsetGroundAnimationAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetMesh1Addr), displayOrder: 11, isPointer: true, displayGroup: "Main")]
        public int OffsetMesh1 {
            get => Data.GetDouble(_offsetMesh1Addr);
            set => Data.SetDouble(_offsetMesh1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetMesh2Addr), displayOrder: 12, isPointer: true, displayGroup: "Main")]
        public int OffsetMesh2 {
            get => Data.GetDouble(_offsetMesh2Addr);
            set => Data.SetDouble(_offsetMesh2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetMesh3Addr), displayOrder: 13, isPointer: true, visibilityProperty: nameof(HasMesh3), displayGroup: "Main")]
        public int OffsetMesh3 {
            get => HasMesh3 ? Data.GetDouble(_offsetMesh3Addr) : 0;
            set {
                if (HasMesh3)
                    Data.SetDouble(_offsetMesh3Addr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_modelsPreYRotationAddr), displayOrder: 14, visibilityProperty: nameof(HasModelsInfo), displayGroup: "Main")]
        public float ModelsPreYRotation {
            get => HasModelsInfo ? Data.GetCompressedFIXED(_modelsPreYRotationAddr).Float : -1;
            set {
                if (HasModelsInfo)
                    Data.SetCompressedFIXED(_modelsPreYRotationAddr, new CompressedFIXED(value, 0));
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_modelsViewAngleMinAddr), displayOrder: 14.5f, visibilityProperty: nameof(HasModelsInfo), displayGroup: "Main")]
        public float ModelsViewAngleMin {
            get => HasModelsInfo ? Data.GetCompressedFIXED(_modelsViewAngleMinAddr).Float : -0.6f;
            set {
                if (HasModelsInfo)
                    Data.SetCompressedFIXED(_modelsViewAngleMinAddr, new CompressedFIXED(value, 0));
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_modelsViewAngleMaxAddr), displayOrder: 15, visibilityProperty: nameof(HasModelsInfo), displayGroup: "Main")]
        public float ModelsViewAngleMax {
            get => HasModelsInfo ? Data.GetCompressedFIXED(_modelsViewAngleMaxAddr).Float : 0.6f;
            set {
                if (HasModelsInfo)
                    Data.SetCompressedFIXED(_modelsViewAngleMaxAddr, new CompressedFIXED(value, 0));
            }
        }

        [BulkCopy]
        public ushort Padding3 {
            get => HasModelsInfo ? (ushort) Data.GetWord(_padding3Addr) : (ushort) 0;
            set {
                if (HasModelsInfo)
                    Data.SetWord(_padding3Addr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetTextureAnimAltAddr), displayOrder: 16, isPointer: true, visibilityProperty: nameof(HasModelsInfo), displayGroup: "Main")]
        public int OffsetTextureAnimAlt {
            // TODO: Create HasOffsetTexturesAnimAlt for this case
            get => HasModelsInfo ? Data.GetDouble(_offsetTextureAnimAltAddr) : 0;
            set {
                if (HasModelsInfo)
                    Data.SetDouble(_offsetTextureAnimAltAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetPal1Addr), displayOrder: 17, isPointer: true, displayGroup: "Main")]
        public int OffsetPal1 {
            get => Data.GetDouble(_offsetPal1Addr);
            set => Data.SetDouble(_offsetPal1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetPal2Addr), displayOrder: 18, isPointer: true, displayGroup: "Main")]
        public int OffsetPal2 {
            get => Data.GetDouble(_offsetPal2Addr);
            set => Data.SetDouble(_offsetPal2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetPal3Addr), displayName: nameof(OffsetPal3) + " (Scn3+)", displayOrder: 19, isPointer: true, visibilityProperty: nameof(HasPalette3), displayGroup: "Main")]
        public int OffsetPal3 {
            get => HasPalette3 ? Data.GetDouble(_offsetPal3Addr) : 0;
            set {
                if (HasPalette3)
                    Data.SetDouble(_offsetPal3Addr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetIndexedTexturesAddr), displayName: nameof(OffsetIndexedTextures) + " (Scn3+)", displayOrder: 19.5f, isPointer: true, visibilityProperty: nameof(HasIndexedTextures), displayGroup: "Main")]
        public int OffsetIndexedTextures {
            get => HasIndexedTextures ? Data.GetDouble(_offsetIndexedTexturesAddr) : 0;
            set {
                if (HasIndexedTextures)
                    Data.SetDouble(_offsetIndexedTexturesAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundXAddr), displayOrder: 20, displayGroup: "Main")]
        public short GroundX {
            get => (short) Data.GetWord(_groundXAddr);
            set => Data.SetWord(_groundXAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundYAddr), displayOrder: 20.5f, displayGroup: "Main")]
        public short GroundY {
            get => (short) Data.GetWord(_groundYAddr);
            set => Data.SetWord(_groundYAddr, value);
        }
        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundZAddr), displayOrder: 21, displayGroup: "Main")]
        public short GroundZ {
            get => (short) Data.GetWord(_groundZAddr);
            set => Data.SetWord(_groundZAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundAngleAddr), displayOrder: 21.5f, displayGroup: "Main")]
        public float GroundAngle {
            get => Data.GetCompressedFIXED(_groundAngleAddr).Float;
            set => Data.SetCompressedFIXED(_groundAngleAddr, new CompressedFIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_unknown1Addr), displayOrder: 22, displayGroup: "Main")]
        public short Unknown1 {
            get => (short) Data.GetWord(_unknown1Addr);
            set => Data.SetWord(_unknown1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_backgroundXAddr), displayOrder: 22.5f, displayGroup: "Main")]
        public short BackgroundX {
            get => (short) Data.GetWord(_backgroundXAddr);
            set => Data.SetWord(_backgroundXAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_backgroundYAddr), displayOrder: 23, displayGroup: "Main")]
        public short BackgroundY {
            get => (short) Data.GetWord(_backgroundYAddr);
            set => Data.SetWord(_backgroundYAddr, value);
        }

        [BulkCopy]
        public ushort Padding4 {
            get => (ushort) Data.GetWord(_padding4Addr);
            set => Data.SetWord(_padding4Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_offsetBoundariesAddr), displayOrder: 24, isPointer: true, displayGroup: "Main")]
        public int OffsetBoundaries {
            get => Data.GetDouble(_offsetBoundariesAddr);
            set => Data.SetDouble(_offsetBoundariesAddr, value);
        }
    }
}
