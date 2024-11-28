using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.MPD {
    public class TileSurfaceHeightmapRow : Struct {
        private readonly int[] xAddress = new int[64];

        public TileSurfaceHeightmapRow(IRawData data, int id, string name, int address)
        : base(data, id, name, address, 256) {
            for (var i = 0; i < xAddress.Length; i++)
                xAddress[i] = Address + i * 4;
        }

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            public TileMetadataAttribute(int x) : base(displayName: "X" + x.ToString("D2"), displayOrder: x, displayFormat: "X8", minWidth: 75) { }
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 64 individual properties.
        [TileMetadata(0)] public uint X0Tile { get => (uint) Data.GetDouble(xAddress[0]); set => Data.SetDouble(xAddress[0], (int) value); }
        [TileMetadata(1)] public uint X1Tile { get => (uint) Data.GetDouble(xAddress[1]); set => Data.SetDouble(xAddress[1], (int) value); }
        [TileMetadata(2)] public uint X2Tile { get => (uint) Data.GetDouble(xAddress[2]); set => Data.SetDouble(xAddress[2], (int) value); }
        [TileMetadata(3)] public uint X3Tile { get => (uint) Data.GetDouble(xAddress[3]); set => Data.SetDouble(xAddress[3], (int) value); }
        [TileMetadata(4)] public uint X4Tile { get => (uint) Data.GetDouble(xAddress[4]); set => Data.SetDouble(xAddress[4], (int) value); }
        [TileMetadata(5)] public uint X5Tile { get => (uint) Data.GetDouble(xAddress[5]); set => Data.SetDouble(xAddress[5], (int) value); }
        [TileMetadata(6)] public uint X6Tile { get => (uint) Data.GetDouble(xAddress[6]); set => Data.SetDouble(xAddress[6], (int) value); }
        [TileMetadata(7)] public uint X7Tile { get => (uint) Data.GetDouble(xAddress[7]); set => Data.SetDouble(xAddress[7], (int) value); }
        [TileMetadata(8)] public uint X8Tile { get => (uint) Data.GetDouble(xAddress[8]); set => Data.SetDouble(xAddress[8], (int) value); }
        [TileMetadata(9)] public uint X9Tile { get => (uint) Data.GetDouble(xAddress[9]); set => Data.SetDouble(xAddress[9], (int) value); }
        [TileMetadata(10)] public uint X10Tile { get => (uint) Data.GetDouble(xAddress[10]); set => Data.SetDouble(xAddress[10], (int) value); }
        [TileMetadata(11)] public uint X11Tile { get => (uint) Data.GetDouble(xAddress[11]); set => Data.SetDouble(xAddress[11], (int) value); }
        [TileMetadata(12)] public uint X12Tile { get => (uint) Data.GetDouble(xAddress[12]); set => Data.SetDouble(xAddress[12], (int) value); }
        [TileMetadata(13)] public uint X13Tile { get => (uint) Data.GetDouble(xAddress[13]); set => Data.SetDouble(xAddress[13], (int) value); }
        [TileMetadata(14)] public uint X14Tile { get => (uint) Data.GetDouble(xAddress[14]); set => Data.SetDouble(xAddress[14], (int) value); }
        [TileMetadata(15)] public uint X15Tile { get => (uint) Data.GetDouble(xAddress[15]); set => Data.SetDouble(xAddress[15], (int) value); }
        [TileMetadata(16)] public uint X16Tile { get => (uint) Data.GetDouble(xAddress[16]); set => Data.SetDouble(xAddress[16], (int) value); }
        [TileMetadata(17)] public uint X17Tile { get => (uint) Data.GetDouble(xAddress[17]); set => Data.SetDouble(xAddress[17], (int) value); }
        [TileMetadata(18)] public uint X18Tile { get => (uint) Data.GetDouble(xAddress[18]); set => Data.SetDouble(xAddress[18], (int) value); }
        [TileMetadata(19)] public uint X19Tile { get => (uint) Data.GetDouble(xAddress[19]); set => Data.SetDouble(xAddress[19], (int) value); }
        [TileMetadata(20)] public uint X20Tile { get => (uint) Data.GetDouble(xAddress[20]); set => Data.SetDouble(xAddress[20], (int) value); }
        [TileMetadata(21)] public uint X21Tile { get => (uint) Data.GetDouble(xAddress[21]); set => Data.SetDouble(xAddress[21], (int) value); }
        [TileMetadata(22)] public uint X22Tile { get => (uint) Data.GetDouble(xAddress[22]); set => Data.SetDouble(xAddress[22], (int) value); }
        [TileMetadata(23)] public uint X23Tile { get => (uint) Data.GetDouble(xAddress[23]); set => Data.SetDouble(xAddress[23], (int) value); }
        [TileMetadata(24)] public uint X24Tile { get => (uint) Data.GetDouble(xAddress[24]); set => Data.SetDouble(xAddress[24], (int) value); }
        [TileMetadata(25)] public uint X25Tile { get => (uint) Data.GetDouble(xAddress[25]); set => Data.SetDouble(xAddress[25], (int) value); }
        [TileMetadata(26)] public uint X26Tile { get => (uint) Data.GetDouble(xAddress[26]); set => Data.SetDouble(xAddress[26], (int) value); }
        [TileMetadata(27)] public uint X27Tile { get => (uint) Data.GetDouble(xAddress[27]); set => Data.SetDouble(xAddress[27], (int) value); }
        [TileMetadata(28)] public uint X28Tile { get => (uint) Data.GetDouble(xAddress[28]); set => Data.SetDouble(xAddress[28], (int) value); }
        [TileMetadata(29)] public uint X29Tile { get => (uint) Data.GetDouble(xAddress[29]); set => Data.SetDouble(xAddress[29], (int) value); }
        [TileMetadata(30)] public uint X30Tile { get => (uint) Data.GetDouble(xAddress[30]); set => Data.SetDouble(xAddress[30], (int) value); }
        [TileMetadata(31)] public uint X31Tile { get => (uint) Data.GetDouble(xAddress[31]); set => Data.SetDouble(xAddress[31], (int) value); }
        [TileMetadata(32)] public uint X32Tile { get => (uint) Data.GetDouble(xAddress[32]); set => Data.SetDouble(xAddress[32], (int) value); }
        [TileMetadata(33)] public uint X33Tile { get => (uint) Data.GetDouble(xAddress[33]); set => Data.SetDouble(xAddress[33], (int) value); }
        [TileMetadata(34)] public uint X34Tile { get => (uint) Data.GetDouble(xAddress[34]); set => Data.SetDouble(xAddress[34], (int) value); }
        [TileMetadata(35)] public uint X35Tile { get => (uint) Data.GetDouble(xAddress[35]); set => Data.SetDouble(xAddress[35], (int) value); }
        [TileMetadata(36)] public uint X36Tile { get => (uint) Data.GetDouble(xAddress[36]); set => Data.SetDouble(xAddress[36], (int) value); }
        [TileMetadata(37)] public uint X37Tile { get => (uint) Data.GetDouble(xAddress[37]); set => Data.SetDouble(xAddress[37], (int) value); }
        [TileMetadata(38)] public uint X38Tile { get => (uint) Data.GetDouble(xAddress[38]); set => Data.SetDouble(xAddress[38], (int) value); }
        [TileMetadata(39)] public uint X39Tile { get => (uint) Data.GetDouble(xAddress[39]); set => Data.SetDouble(xAddress[39], (int) value); }
        [TileMetadata(40)] public uint X40Tile { get => (uint) Data.GetDouble(xAddress[40]); set => Data.SetDouble(xAddress[40], (int) value); }
        [TileMetadata(41)] public uint X41Tile { get => (uint) Data.GetDouble(xAddress[41]); set => Data.SetDouble(xAddress[41], (int) value); }
        [TileMetadata(42)] public uint X42Tile { get => (uint) Data.GetDouble(xAddress[42]); set => Data.SetDouble(xAddress[42], (int) value); }
        [TileMetadata(43)] public uint X43Tile { get => (uint) Data.GetDouble(xAddress[43]); set => Data.SetDouble(xAddress[43], (int) value); }
        [TileMetadata(44)] public uint X44Tile { get => (uint) Data.GetDouble(xAddress[44]); set => Data.SetDouble(xAddress[44], (int) value); }
        [TileMetadata(45)] public uint X45Tile { get => (uint) Data.GetDouble(xAddress[45]); set => Data.SetDouble(xAddress[45], (int) value); }
        [TileMetadata(46)] public uint X46Tile { get => (uint) Data.GetDouble(xAddress[46]); set => Data.SetDouble(xAddress[46], (int) value); }
        [TileMetadata(47)] public uint X47Tile { get => (uint) Data.GetDouble(xAddress[47]); set => Data.SetDouble(xAddress[47], (int) value); }
        [TileMetadata(48)] public uint X48Tile { get => (uint) Data.GetDouble(xAddress[48]); set => Data.SetDouble(xAddress[48], (int) value); }
        [TileMetadata(49)] public uint X49Tile { get => (uint) Data.GetDouble(xAddress[49]); set => Data.SetDouble(xAddress[49], (int) value); }
        [TileMetadata(50)] public uint X50Tile { get => (uint) Data.GetDouble(xAddress[50]); set => Data.SetDouble(xAddress[50], (int) value); }
        [TileMetadata(51)] public uint X51Tile { get => (uint) Data.GetDouble(xAddress[51]); set => Data.SetDouble(xAddress[51], (int) value); }
        [TileMetadata(52)] public uint X52Tile { get => (uint) Data.GetDouble(xAddress[52]); set => Data.SetDouble(xAddress[52], (int) value); }
        [TileMetadata(53)] public uint X53Tile { get => (uint) Data.GetDouble(xAddress[53]); set => Data.SetDouble(xAddress[53], (int) value); }
        [TileMetadata(54)] public uint X54Tile { get => (uint) Data.GetDouble(xAddress[54]); set => Data.SetDouble(xAddress[54], (int) value); }
        [TileMetadata(55)] public uint X55Tile { get => (uint) Data.GetDouble(xAddress[55]); set => Data.SetDouble(xAddress[55], (int) value); }
        [TileMetadata(56)] public uint X56Tile { get => (uint) Data.GetDouble(xAddress[56]); set => Data.SetDouble(xAddress[56], (int) value); }
        [TileMetadata(57)] public uint X57Tile { get => (uint) Data.GetDouble(xAddress[57]); set => Data.SetDouble(xAddress[57], (int) value); }
        [TileMetadata(58)] public uint X58Tile { get => (uint) Data.GetDouble(xAddress[58]); set => Data.SetDouble(xAddress[58], (int) value); }
        [TileMetadata(59)] public uint X59Tile { get => (uint) Data.GetDouble(xAddress[59]); set => Data.SetDouble(xAddress[59], (int) value); }
        [TileMetadata(60)] public uint X60Tile { get => (uint) Data.GetDouble(xAddress[60]); set => Data.SetDouble(xAddress[60], (int) value); }
        [TileMetadata(61)] public uint X61Tile { get => (uint) Data.GetDouble(xAddress[61]); set => Data.SetDouble(xAddress[61], (int) value); }
        [TileMetadata(62)] public uint X62Tile { get => (uint) Data.GetDouble(xAddress[62]); set => Data.SetDouble(xAddress[62], (int) value); }
        [TileMetadata(63)] public uint X63Tile { get => (uint) Data.GetDouble(xAddress[63]); set => Data.SetDouble(xAddress[63], (int) value); }
    }
}
