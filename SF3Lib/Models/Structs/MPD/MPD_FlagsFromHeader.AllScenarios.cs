using CommonLib.Attributes;

namespace SF3.Models.Structs.MPD {
    public partial class MPD_FlagsFromHeader {
        public bool CanSet_0x0001_Unknown => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0001f, displayName: "(0x0001) Unknown", displayGroup: "Flags")]
        public bool Bit_0x0001_Unknown {
            get => (MapFlags & 0x0001) == 0x0001;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0001) : (ushort) (MapFlags & ~0x0001);
        }

        public bool CanSet_0x0004_AddDotProductBasedNoiseToStandardLightmap => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0004f, displayName: "(0x0004) AddDotProductBasedNoiseToStandardLightmap", displayGroup: "Flags")]
        public bool Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap {
            get => (MapFlags & 0x0004) == 0x0004;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0004) : (ushort) (MapFlags & ~0x0004);
        }

        public bool CanSet_0x0008_KeepTexturelessFlatTiles => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0008f, displayName: "(0x0008) KeepTexturelessFlatTiles", displayGroup: "Flags")]
        public bool Bit_0x0008_KeepTexturelessFlatTiles {
            get => (MapFlags & 0x0008) == 0x0008;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0008) : (ushort) (MapFlags & ~0x0008);
        }

        public bool CanSet_0x0010_HasTileBasedForegroundImage => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0010f, displayName: "(0x0010) HasTileBasedForegroundImage", minWidth: 100, displayGroup: "Flags")]
        public bool Bit_0x0010_HasTileBasedForegroundImage {
            get => (MapFlags & 0x0010) == 0x0010;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0010) : (ushort) (MapFlags & ~0x0010);
        }

        public bool CanSet_0x0020_Unknown => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0020f, displayName: "(0x0020) Unknown", displayGroup: "Flags")]
        public bool Bit_0x0020_Unknown {
            get => (MapFlags & 0x0020) == 0x0020;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0020) : (ushort) (MapFlags & ~0x0020);
        }

        public bool CanSet_0x0040_HasBackgroundImage => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0040f, displayName: "(0x0040) HasBackgroundImage", minWidth: 100, displayGroup: "Flags")]
        public bool Bit_0x0040_HasBackgroundImage {
            get => (MapFlags & 0x0040) == 0x0040;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0040) : (ushort) (MapFlags & ~0x0040);
        }

        public bool CanSet_0x0100_HasModels => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0100f, displayName: "(0x0100) HasModels", displayGroup: "Flags")]
        public bool Bit_0x0100_HasModels {
            get => (MapFlags & 0x0100) == 0x0100;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0100) : (ushort) (MapFlags & ~0x0100);
        }

        public bool CanSet_0x0200_HasSurfaceModel => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0200f, displayName: "(0x0200) HasSurfaceModel", displayGroup: "Flags")]
        public bool Bit_0x0200_HasSurfaceModel {
            get => (MapFlags & 0x0200) == 0x0200;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0200) : (ushort) (MapFlags & ~0x0200);
        }

        public bool CanSet_0x0400_HasGroundImage => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0400f, displayName: "(0x0400) HasGroundImage", minWidth: 100, displayGroup: "Flags")]
        public bool Bit_0x0400_HasGroundImage {
            get => (MapFlags & 0x0400) == 0x0400;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0400) : (ushort) (MapFlags & ~0x0400);
        }

        public bool CanSet_0x1000_HasTileBasedGroundImage => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.1000f, displayName: "(0x1000) HasTileBasedGroundImage", minWidth: 100, displayGroup: "Flags")]
        public bool Bit_0x1000_HasTileBasedGroundImage {
            get => (MapFlags & 0x1000) == 0x1000;
            set => MapFlags = value ? (ushort) (MapFlags | 0x1000) : (ushort) (MapFlags & ~0x1000);
        }

        public bool CanSet_0x8000_ModelsAreStillLowMemoryWithSurfaceModel => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.8000f, displayName: "(0x8000) ModelsAreStillLowMemoryWithSurfaceModel", displayGroup: "Flags")]
        public bool Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel {
            get => (MapFlags & 0x8000) == 0x8000;
            set => MapFlags = value ? (ushort) (MapFlags | 0x8000) : (ushort) (MapFlags & ~0x8000);
        }
    }
}
