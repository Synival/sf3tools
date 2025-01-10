using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Surface {
    public class HeightTerrainRow : Struct {
        private readonly int[] xAddress = new int[64];

        public HeightTerrainRow(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 128) {
            for (var i = 0; i < xAddress.Length; i++)
                xAddress[i] = Address + i * 2;
        }

        public int this[int index] {
            get => Data.GetWord(xAddress[index]);
            set => Data.SetWord(xAddress[index], value);
        }

        public float GetHeight(int x)
            => ((this[x] >> 8) & 0xFF) / 16f;
        public void SetHeight(int x, float value)
            => this[x] = (this[x] & 0xFF) + (((int) (value * 16f)) << 8);

        public TerrainType GetTerrainType(int x)
            => (TerrainType) (this[x] & 0x0F);
        public void SetTerrainType(int x, TerrainType value)
            => this[x] = (this[x] & 0xFFF0) + (byte) ((byte) value & 0x0F);

        public TerrainFlags GetTerrainFlags(int x)
            => (TerrainFlags) (this[x] & 0xF0);
        public void SetTerrainFlags(int x, TerrainFlags flags)
            => this[x] = (this[x] & 0xFF0F) + (byte) ((byte) flags & 0xF0);

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            public TileMetadataAttribute(int x) : base(displayName: "X" + x.ToString("D2"), displayOrder: x, displayFormat: "X4", minWidth: 50) { }
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 64 individual properties.
        [TileMetadata(0)] public int X0Tile { get => Data.GetWord(xAddress[0]); set => Data.SetWord(xAddress[0], value); }
        [TileMetadata(1)] public int X1Tile { get => Data.GetWord(xAddress[1]); set => Data.SetWord(xAddress[1], value); }
        [TileMetadata(2)] public int X2Tile { get => Data.GetWord(xAddress[2]); set => Data.SetWord(xAddress[2], value); }
        [TileMetadata(3)] public int X3Tile { get => Data.GetWord(xAddress[3]); set => Data.SetWord(xAddress[3], value); }
        [TileMetadata(4)] public int X4Tile { get => Data.GetWord(xAddress[4]); set => Data.SetWord(xAddress[4], value); }
        [TileMetadata(5)] public int X5Tile { get => Data.GetWord(xAddress[5]); set => Data.SetWord(xAddress[5], value); }
        [TileMetadata(6)] public int X6Tile { get => Data.GetWord(xAddress[6]); set => Data.SetWord(xAddress[6], value); }
        [TileMetadata(7)] public int X7Tile { get => Data.GetWord(xAddress[7]); set => Data.SetWord(xAddress[7], value); }
        [TileMetadata(8)] public int X8Tile { get => Data.GetWord(xAddress[8]); set => Data.SetWord(xAddress[8], value); }
        [TileMetadata(9)] public int X9Tile { get => Data.GetWord(xAddress[9]); set => Data.SetWord(xAddress[9], value); }
        [TileMetadata(10)] public int X10Tile { get => Data.GetWord(xAddress[10]); set => Data.SetWord(xAddress[10], value); }
        [TileMetadata(11)] public int X11Tile { get => Data.GetWord(xAddress[11]); set => Data.SetWord(xAddress[11], value); }
        [TileMetadata(12)] public int X12Tile { get => Data.GetWord(xAddress[12]); set => Data.SetWord(xAddress[12], value); }
        [TileMetadata(13)] public int X13Tile { get => Data.GetWord(xAddress[13]); set => Data.SetWord(xAddress[13], value); }
        [TileMetadata(14)] public int X14Tile { get => Data.GetWord(xAddress[14]); set => Data.SetWord(xAddress[14], value); }
        [TileMetadata(15)] public int X15Tile { get => Data.GetWord(xAddress[15]); set => Data.SetWord(xAddress[15], value); }
        [TileMetadata(16)] public int X16Tile { get => Data.GetWord(xAddress[16]); set => Data.SetWord(xAddress[16], value); }
        [TileMetadata(17)] public int X17Tile { get => Data.GetWord(xAddress[17]); set => Data.SetWord(xAddress[17], value); }
        [TileMetadata(18)] public int X18Tile { get => Data.GetWord(xAddress[18]); set => Data.SetWord(xAddress[18], value); }
        [TileMetadata(19)] public int X19Tile { get => Data.GetWord(xAddress[19]); set => Data.SetWord(xAddress[19], value); }
        [TileMetadata(20)] public int X20Tile { get => Data.GetWord(xAddress[20]); set => Data.SetWord(xAddress[20], value); }
        [TileMetadata(21)] public int X21Tile { get => Data.GetWord(xAddress[21]); set => Data.SetWord(xAddress[21], value); }
        [TileMetadata(22)] public int X22Tile { get => Data.GetWord(xAddress[22]); set => Data.SetWord(xAddress[22], value); }
        [TileMetadata(23)] public int X23Tile { get => Data.GetWord(xAddress[23]); set => Data.SetWord(xAddress[23], value); }
        [TileMetadata(24)] public int X24Tile { get => Data.GetWord(xAddress[24]); set => Data.SetWord(xAddress[24], value); }
        [TileMetadata(25)] public int X25Tile { get => Data.GetWord(xAddress[25]); set => Data.SetWord(xAddress[25], value); }
        [TileMetadata(26)] public int X26Tile { get => Data.GetWord(xAddress[26]); set => Data.SetWord(xAddress[26], value); }
        [TileMetadata(27)] public int X27Tile { get => Data.GetWord(xAddress[27]); set => Data.SetWord(xAddress[27], value); }
        [TileMetadata(28)] public int X28Tile { get => Data.GetWord(xAddress[28]); set => Data.SetWord(xAddress[28], value); }
        [TileMetadata(29)] public int X29Tile { get => Data.GetWord(xAddress[29]); set => Data.SetWord(xAddress[29], value); }
        [TileMetadata(30)] public int X30Tile { get => Data.GetWord(xAddress[30]); set => Data.SetWord(xAddress[30], value); }
        [TileMetadata(31)] public int X31Tile { get => Data.GetWord(xAddress[31]); set => Data.SetWord(xAddress[31], value); }
        [TileMetadata(32)] public int X32Tile { get => Data.GetWord(xAddress[32]); set => Data.SetWord(xAddress[32], value); }
        [TileMetadata(33)] public int X33Tile { get => Data.GetWord(xAddress[33]); set => Data.SetWord(xAddress[33], value); }
        [TileMetadata(34)] public int X34Tile { get => Data.GetWord(xAddress[34]); set => Data.SetWord(xAddress[34], value); }
        [TileMetadata(35)] public int X35Tile { get => Data.GetWord(xAddress[35]); set => Data.SetWord(xAddress[35], value); }
        [TileMetadata(36)] public int X36Tile { get => Data.GetWord(xAddress[36]); set => Data.SetWord(xAddress[36], value); }
        [TileMetadata(37)] public int X37Tile { get => Data.GetWord(xAddress[37]); set => Data.SetWord(xAddress[37], value); }
        [TileMetadata(38)] public int X38Tile { get => Data.GetWord(xAddress[38]); set => Data.SetWord(xAddress[38], value); }
        [TileMetadata(39)] public int X39Tile { get => Data.GetWord(xAddress[39]); set => Data.SetWord(xAddress[39], value); }
        [TileMetadata(40)] public int X40Tile { get => Data.GetWord(xAddress[40]); set => Data.SetWord(xAddress[40], value); }
        [TileMetadata(41)] public int X41Tile { get => Data.GetWord(xAddress[41]); set => Data.SetWord(xAddress[41], value); }
        [TileMetadata(42)] public int X42Tile { get => Data.GetWord(xAddress[42]); set => Data.SetWord(xAddress[42], value); }
        [TileMetadata(43)] public int X43Tile { get => Data.GetWord(xAddress[43]); set => Data.SetWord(xAddress[43], value); }
        [TileMetadata(44)] public int X44Tile { get => Data.GetWord(xAddress[44]); set => Data.SetWord(xAddress[44], value); }
        [TileMetadata(45)] public int X45Tile { get => Data.GetWord(xAddress[45]); set => Data.SetWord(xAddress[45], value); }
        [TileMetadata(46)] public int X46Tile { get => Data.GetWord(xAddress[46]); set => Data.SetWord(xAddress[46], value); }
        [TileMetadata(47)] public int X47Tile { get => Data.GetWord(xAddress[47]); set => Data.SetWord(xAddress[47], value); }
        [TileMetadata(48)] public int X48Tile { get => Data.GetWord(xAddress[48]); set => Data.SetWord(xAddress[48], value); }
        [TileMetadata(49)] public int X49Tile { get => Data.GetWord(xAddress[49]); set => Data.SetWord(xAddress[49], value); }
        [TileMetadata(50)] public int X50Tile { get => Data.GetWord(xAddress[50]); set => Data.SetWord(xAddress[50], value); }
        [TileMetadata(51)] public int X51Tile { get => Data.GetWord(xAddress[51]); set => Data.SetWord(xAddress[51], value); }
        [TileMetadata(52)] public int X52Tile { get => Data.GetWord(xAddress[52]); set => Data.SetWord(xAddress[52], value); }
        [TileMetadata(53)] public int X53Tile { get => Data.GetWord(xAddress[53]); set => Data.SetWord(xAddress[53], value); }
        [TileMetadata(54)] public int X54Tile { get => Data.GetWord(xAddress[54]); set => Data.SetWord(xAddress[54], value); }
        [TileMetadata(55)] public int X55Tile { get => Data.GetWord(xAddress[55]); set => Data.SetWord(xAddress[55], value); }
        [TileMetadata(56)] public int X56Tile { get => Data.GetWord(xAddress[56]); set => Data.SetWord(xAddress[56], value); }
        [TileMetadata(57)] public int X57Tile { get => Data.GetWord(xAddress[57]); set => Data.SetWord(xAddress[57], value); }
        [TileMetadata(58)] public int X58Tile { get => Data.GetWord(xAddress[58]); set => Data.SetWord(xAddress[58], value); }
        [TileMetadata(59)] public int X59Tile { get => Data.GetWord(xAddress[59]); set => Data.SetWord(xAddress[59], value); }
        [TileMetadata(60)] public int X60Tile { get => Data.GetWord(xAddress[60]); set => Data.SetWord(xAddress[60], value); }
        [TileMetadata(61)] public int X61Tile { get => Data.GetWord(xAddress[61]); set => Data.SetWord(xAddress[61], value); }
        [TileMetadata(62)] public int X62Tile { get => Data.GetWord(xAddress[62]); set => Data.SetWord(xAddress[62], value); }
        [TileMetadata(63)] public int X63Tile { get => Data.GetWord(xAddress[63]); set => Data.SetWord(xAddress[63], value); }
    }
}
