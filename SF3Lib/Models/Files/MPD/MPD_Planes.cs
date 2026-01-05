using System.Linq;
using CommonLib.Imaging;
using SF3.Imaging;
using SF3.Models.Structs.MPD.Plane;
using SF3.Models.Tables.Shared;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class MPD_Planes : IMPD_Planes {
        private static readonly Palette c_fakePalette = new Palette(0x100);

        public MPD_Planes(IMPD_File mpdFile) {
            MPD_File = mpdFile;
            UpdateImages();
        }

        public void UpdateImages() {
            // Set the ground plane.
            ITextureData groundImage = null;
            if (MPD_File.GroundImageChunkDatas?.Any() == true) {
                try {
                    groundImage = new MultiChunkTextureIndexed(
                        MPD_File.GroundImageChunkDatas.Select(x => x.DecompressedData).ToArray(),
                        TexturePixelFormat.Palette1,
                        () => MPD_File.GetPalette(TexturePixelFormat.Palette1) ?? c_fakePalette,
                        palette => TrySetPalette(TexturePixelFormat.Palette1, palette),
                        zeroIsTransparent: false, isTiled: false
                    );
                }
                catch {
                    // TODO: what to do here??
                }
            }
            GroundImage = groundImage;

            // Set the tiled ground plane.
            IMPD_TiledPlane groundTiledImage = null;
            if (MPD_File.GroundTilesetChunkDatas?.Any() == true && MPD_File.GroundTileAssignmentChunks?.Length == 2) {
                if ((groundTiledImage = GroundTiledImage) != null)
                    ((TiledImagePlane) GroundTiledImage).UpdateImages();
                else {
                    try {
                        groundTiledImage = new TiledImagePlane(
                            MPD_File.GroundTilesetChunkDatas[0].DecompressedData,
                            MPD_File.GroundTilesetChunkDatas[1].DecompressedData,
                            new MPD_GroundPlaneTileAssignment(
                                MPD_File.GroundTileAssignmentChunks[0].PlaneTileTextureRowTable,
                                MPD_File.GroundTileAssignmentChunks[1].PlaneTileTextureRowTable
                            ),
                            TexturePixelFormat.Palette1,
                            () => MPD_File.GetPalette(TexturePixelFormat.Palette1) ?? c_fakePalette,
                            palette => TrySetPalette(TexturePixelFormat.Palette1, palette),
                            zeroIsTransparent: false
                        );
                    }
                    catch {
                        // TODO: what to do here??
                    }
                }
            }
            GroundTiledImage = groundTiledImage;

            // Set the background image (Ishahakat's room).
            ITextureData backgroundImage = null;
            if (MPD_File.BackgroundChunkDatas?.Any() == true) {
                try {
                    backgroundImage = new MultiChunkTextureIndexed(
                        MPD_File.BackgroundChunkDatas.Select(x => x.DecompressedData).ToArray(),
                        TexturePixelFormat.Palette1,
                        () => MPD_File.GetPalette(TexturePixelFormat.Palette1) ?? c_fakePalette,
                        palette => TrySetPalette(TexturePixelFormat.Palette1, palette),
                        zeroIsTransparent: false, isTiled: false
                    );
                }
                catch {
                    // TODO: what to do here??
                }
            }
            BackgroundImage = backgroundImage;

            // Set the cutscene/battle sky.
            ITextureData skyImage = null;
            if (MPD_File.SkyChunkDatas?.Any() == true) {
                try {
                    skyImage = new MultiChunkTextureIndexed(
                        MPD_File.SkyChunkDatas.Select(x => x.DecompressedData).ToArray(),
                        TexturePixelFormat.Palette2,
                        () => MPD_File.GetPalette(TexturePixelFormat.Palette2) ?? c_fakePalette,
                        palette => TrySetPalette(TexturePixelFormat.Palette2, palette),
                        zeroIsTransparent: false, isTiled: false
                    );
                }
                catch {
                    // TODO: what to do here??
                }
            }
            SkyImage = skyImage;

            // Set the foreground image (Ishahakat).
            IMPD_TiledPlane foregroundTiledImage = null;
            if (MPD_File.ForegroundTileChunkDatas?.Any() == true && MPD_File.ForegroundTileAssignmentChunk != null) {
                if ((foregroundTiledImage = ForegroundTiledImage) != null)
                    ((TiledImagePlane) ForegroundTiledImage).UpdateImages();
                else {
                    try {
                        foregroundTiledImage = new TiledImagePlane(
                            MPD_File.ForegroundTileChunkDatas[0].DecompressedData,
                            MPD_File.ForegroundTileChunkDatas[1].DecompressedData,
                            new MPD_ForegroundPlaneTileAssignment(MPD_File.ForegroundTileAssignmentChunk.PlaneTileTextureRowTable),
                            TexturePixelFormat.Palette2,
                            () => MPD_File.GetPalette(TexturePixelFormat.Palette2) ?? c_fakePalette,
                            palette => TrySetPalette(TexturePixelFormat.Palette2, palette),
                            zeroIsTransparent: true
                        );
                    }
                    catch {
                        // TODO: what to do here??
                    }
                }
            }
            ForegroundTiledImage = foregroundTiledImage;
        }

        private bool TrySetPalette(TexturePixelFormat pixelFormat, Palette palette) {
            ColorTable table = null;
            switch (pixelFormat) {
                case TexturePixelFormat.Palette1: table = MPD_File.GroundPaletteColorTable;  break;
                case TexturePixelFormat.Palette2: table = MPD_File.SkyPaletteColorTable;     break;
                case TexturePixelFormat.Palette3: table = MPD_File.TexturePaletteColorTable; break;
            }
            if (table == null)
                return false;

            table.Palette = palette;
            return true;
        }

        public IMPD_File MPD_File { get; }

        public ITextureData GroundImage { get; private set; }
        public IMPD_TiledPlane GroundTiledImage { get; private set; }
        public ITextureData BackgroundImage { get; private set; }

        public ITextureData SkyImage { get; private set; }
        public IMPD_TiledPlane ForegroundTiledImage { get; private set; }

        public short GroundX {
            get => MPD_File.MPDHeader.GroundX;
            set => MPD_File.MPDHeader.GroundX = value;
        }

        public short GroundY {
            get => MPD_File.MPDHeader.GroundY;
            set => MPD_File.MPDHeader.GroundY = value;
        }

        public short GroundZ {
            get => MPD_File.MPDHeader.GroundZ;
            set => MPD_File.MPDHeader.GroundZ = value;
        }

        public float GroundXRotation {
            get => MPD_File.MPDHeader.GroundXRotation;
            set => MPD_File.MPDHeader.GroundXRotation = value;
        }

        public short BackgroundX {
            get => MPD_File.MPDHeader.BackgroundX;
            set => MPD_File.MPDHeader.BackgroundX = value;
        }

        public short BackgroundY {
            get => MPD_File.MPDHeader.BackgroundY;
            set => MPD_File.MPDHeader.BackgroundY = value;
        }

        public Palette GroundPalette => MPD_File.GroundPaletteColorTable?.Palette;
        public Palette SkyPalette => MPD_File.SkyPaletteColorTable?.Palette;
    }
}
