using System;
using CommonLib.Imaging;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_TiledPlaneTextureDataSource : ITextureDataSource {
        public MPD_TiledPlaneTextureDataSource(ITextureData tileset, IMPD_PlaneTileAssignment tileAssignment) {
            Tileset        = tileset;
            TileAssignment = tileAssignment;
        }

        public byte[,] FetchImageData8Bit(ITextureData tex)
            => MPD_TiledPlane.CreateTiledImageData(Tileset, TileAssignment);

        public object ConvertImageDataToStorageData8Bit(ITextureData tex, byte[,] data, IPalette palette, out int? storageSize) {
            // These images aren't stored, but generated based on inputs.
            storageSize = null;
            return null;
        }

        public void StoreImageData(ITextureData data, object storageData) {
            // These images aren't stored, but generated based on inputs.
        }

        // 16-bits not supported.
        public ushort[,] FetchImageData16Bit(ITextureData tex) => throw new NotSupportedException();
        public object ConvertImageDataToStorageData16Bit(ITextureData tex, ushort[,] data, out int? storageSize) => throw new NotSupportedException();

        public ITextureData Tileset { get; }
        public IMPD_PlaneTileAssignment TileAssignment { get; }

        public int? StoredImageDataSize => null;
    }
}
