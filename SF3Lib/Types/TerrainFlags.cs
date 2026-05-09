using CommonLib.Attributes;

namespace SF3.Types {
    // Just one flag! No others seen in any other MPD anywhere.
    public enum TerrainFlags : byte {
        [EnumDisplayName("Ignore Height Cost")]
        IgnoreHeightCost = 0x04
    }
}
