using System;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class Tile {
        public byte TextureID {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetTextureID(X) : (byte) 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetTextureID(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte TextureFlags {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetTextureFlags(X) : (byte) 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetTextureFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureFlipType TextureFlip {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetFlip(X) : 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetFlip(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureRotateType TextureRotate {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetRotate(X) : 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetRotate(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
