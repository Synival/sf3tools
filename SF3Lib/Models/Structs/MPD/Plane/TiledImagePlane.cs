using System;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.MPD.Interfaces;
using SF3.MPD.Project;

namespace SF3.Models.Structs.MPD.Plane {
    public class TiledImagePlane : IMPD_TiledPlane, IDisposable {
        public TiledImagePlane(
            IByteData tilesetData1,
            IByteData tilesetData2,
            IMPD_PlaneTileAssignment tileAssignment,
            IPalette palette,
            bool zeroIsTransparent
        ) {
            TilesetDatas   = new IByteData[] { tilesetData1, tilesetData2 };
            TileAssignment = tileAssignment;
            Palette        = palette;
            ZeroIsTransparent = zeroIsTransparent;

            Tileset = new MultiChunkTextureData(TilesetDatas, isTiled: true, Palette, ZeroIsTransparent, ImageDataCanSet.CanSet8Bit, IndexedColorUpdateStrategy.UpdateExistingPalette);
            TiledImage = new MPD_TiledPlaneTextureData(Tileset, TileAssignment, Palette, ZeroIsTransparent);

            Tileset.Invalidated += InvalidateTileset;
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing)
                    Tileset.Invalidated -= InvalidateTileset;
                _disposedValue = true;
            }
        }

        private void InvalidateTileset(object sender, EventArgs ags)
            => ((MPD_TiledPlaneTextureData) TiledImage).Invalidate();

        public void Invalidate() {
            ((MultiChunkTextureData) Tileset)?.Invalidate();
            ((MPD_TiledPlaneTextureData) TiledImage)?.Invalidate();
        }

        public IByteData[] TilesetDatas { get; }
        public IMPD_PlaneTileAssignment TileAssignment { get; private set; }
        public IPalette Palette { get; }
        public bool ZeroIsTransparent { get; }

        public ITextureData Tileset { get; private set; }
        public ITextureData TiledImage { get; private set; }

        private bool _disposedValue;
    }
}
