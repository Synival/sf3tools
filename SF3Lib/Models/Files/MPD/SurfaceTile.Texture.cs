using System;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class SurfaceTile {
        public byte TextureID {
            get => (MPD_File.SurfaceModelChunk != null) ? MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].GetTextureID(X) : (byte) 0xFF;
            set {
                MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].SetTextureID(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte TextureFlags {
            get => (MPD_File.SurfaceModelChunk != null) ? MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].GetTextureFlags(X) : (byte) 0;
            set {
                MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].SetTextureFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureFlipType TextureFlip {
            get => (MPD_File.SurfaceModelChunk != null) ? MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].GetFlip(X) : 0;
            set {
                MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].SetFlip(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureRotateType TextureRotate {
            get => (MPD_File.SurfaceModelChunk != null) ? MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].GetRotate(X) : 0;
            set {
                MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].SetRotate(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte UnknownTextureFlags {
            get {
                if (MPD_File.SurfaceModelChunk == null)
                    return 0;

                var row = MPD_File.SurfaceModelChunk.TileTextureRowTable[Y];
                var hasRotate = row.HasRotation;
                var flags = row.GetTextureFlags(X);

                return (byte) (flags & ~(hasRotate ? 0xB3 : 0xB0));
            }
            set {}
        }
    }
}
