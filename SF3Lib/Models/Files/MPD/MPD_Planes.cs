using System.Linq;
using SF3.Images;
using SF3.Models.Structs.MPD.Plane;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class MPD_Planes : IMPD_Planes {
        public MPD_Planes(IMPD_File mpdFile) {
            MPD_File = mpdFile;
            UpdateImages();
        }

        public void UpdateImages() {
            // Set the ground plane.
            ITextureData groundImage = null;
            if (MPD_File.GroundImageChunkDatas?.Any() == true) {
                try {
                    groundImage = new MultiChunkTextureIndexed(MPD_File.GroundImageChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, MPD_File.CreatePalette(0));
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
                            () => MPD_File.CreatePalette(0)
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
                    backgroundImage = new MultiChunkTextureIndexed(MPD_File.BackgroundChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, MPD_File.CreatePalette(0));
                }
                catch {
                    // TODO: what to do here??
                }
            }
            BackgroundImage = backgroundImage;

            // Set the cutscene/battle skybox.
            ITextureData skyBoxImage = null;
            if (MPD_File.SkyBoxChunkDatas?.Any() == true) {
                try {
                    skyBoxImage = new MultiChunkTextureIndexed(MPD_File.SkyBoxChunkDatas.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette2, MPD_File.CreatePalette(1));
                }
                catch {
                    // TODO: what to do here??
                }
            }
            SkyBoxImage = skyBoxImage;

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
                            () => MPD_File.CreatePalette(1)
                        );
                    }
                    catch {
                        // TODO: what to do here??
                    }
                }
            }
            ForegroundTiledImage = foregroundTiledImage;
        }

        public IMPD_File MPD_File { get; }

        public ITextureData GroundImage { get; private set; }
        public IMPD_TiledPlane GroundTiledImage { get; private set; }
        public ITextureData BackgroundImage { get; private set; }

        public ITextureData SkyBoxImage { get; private set; }
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
    }
}
