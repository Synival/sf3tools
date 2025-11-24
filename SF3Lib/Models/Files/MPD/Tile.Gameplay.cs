using System;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class Tile {
        public TerrainType TerrainType {
            get => (MPD_File.SurfaceData != null) ? MPD_File.SurfaceData.HeightTerrainRowTable[Y].GetTerrainType(X) : 0;
            set {
                if (MPD_File.SurfaceData?.HeightTerrainRowTable == null)
                    return;
                MPD_File.SurfaceData.HeightTerrainRowTable[Y].SetTerrainType(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TerrainFlags TerrainFlags {
            get => (MPD_File.SurfaceData != null) ? MPD_File.SurfaceData.HeightTerrainRowTable[Y].GetTerrainFlags(X) : 0;
            set {
                if (MPD_File.SurfaceData?.HeightTerrainRowTable == null)
                    return;
                MPD_File.SurfaceData.HeightTerrainRowTable[Y].SetTerrainFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte EventID {
            get => (MPD_File.SurfaceData != null) ? MPD_File.SurfaceData.EventIDRowTable[Y][X] : (byte) 0;
            set {
                if (MPD_File.SurfaceData?.EventIDRowTable == null)
                    return;
                MPD_File.SurfaceData.EventIDRowTable[Y][X] = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
