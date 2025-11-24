using System;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class Tile {
        public TerrainType TerrainType {
            get => (MPD_File.SurfaceDataChunk != null) ? MPD_File.SurfaceDataChunk.HeightTerrainRowTable[Y].GetTerrainType(X) : 0;
            set {
                if (MPD_File.SurfaceDataChunk?.HeightTerrainRowTable == null)
                    return;
                MPD_File.SurfaceDataChunk.HeightTerrainRowTable[Y].SetTerrainType(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TerrainFlags TerrainFlags {
            get => (MPD_File.SurfaceDataChunk != null) ? MPD_File.SurfaceDataChunk.HeightTerrainRowTable[Y].GetTerrainFlags(X) : 0;
            set {
                if (MPD_File.SurfaceDataChunk?.HeightTerrainRowTable == null)
                    return;
                MPD_File.SurfaceDataChunk.HeightTerrainRowTable[Y].SetTerrainFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte EventID {
            get => (MPD_File.SurfaceDataChunk != null) ? MPD_File.SurfaceDataChunk.EventIDRowTable[Y][X] : (byte) 0;
            set {
                if (MPD_File.SurfaceDataChunk?.EventIDRowTable == null)
                    return;
                MPD_File.SurfaceDataChunk.EventIDRowTable[Y][X] = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
