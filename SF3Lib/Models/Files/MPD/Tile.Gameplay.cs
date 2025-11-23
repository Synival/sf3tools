using System;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class Tile {
        public TerrainType TerrainType {
            get => (MPD_File.Surface != null) ? MPD_File.Surface.HeightTerrainRowTable[Y].GetTerrainType(X) : 0;
            set {
                if (MPD_File.Surface?.HeightTerrainRowTable == null)
                    return;
                MPD_File.Surface.HeightTerrainRowTable[Y].SetTerrainType(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TerrainFlags TerrainFlags {
            get => (MPD_File.Surface != null) ? MPD_File.Surface.HeightTerrainRowTable[Y].GetTerrainFlags(X) : 0;
            set {
                if (MPD_File.Surface?.HeightTerrainRowTable == null)
                    return;
                MPD_File.Surface.HeightTerrainRowTable[Y].SetTerrainFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte EventID {
            get => (MPD_File.Surface != null) ? MPD_File.Surface.EventIDRowTable[Y][X] : (byte) 0;
            set {
                if (MPD_File.Surface?.EventIDRowTable == null)
                    return;
                MPD_File.Surface.EventIDRowTable[Y][X] = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
