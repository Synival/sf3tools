using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.MPD {
    public class TileItemRow : Struct {
        private readonly int[] xAddress = new int[64];

        public TileItemRow(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 64) {
            for (var i = 0; i < xAddress.Length; i++)
                xAddress[i] = Address + i;
        }

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            public TileMetadataAttribute(int x) : base(displayName: "X" + x.ToString("D2"), displayOrder: x, displayFormat: "X2") { }
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 64 individual properties.
        [TileMetadata(0)] public int X0Tile { get => Data.GetByte(xAddress[0]); set => Data.SetByte(xAddress[0], (byte) value); }
        [TileMetadata(1)] public int X1Tile { get => Data.GetByte(xAddress[1]); set => Data.SetByte(xAddress[1], (byte) value); }
        [TileMetadata(2)] public int X2Tile { get => Data.GetByte(xAddress[2]); set => Data.SetByte(xAddress[2], (byte) value); }
        [TileMetadata(3)] public int X3Tile { get => Data.GetByte(xAddress[3]); set => Data.SetByte(xAddress[3], (byte) value); }
        [TileMetadata(4)] public int X4Tile { get => Data.GetByte(xAddress[4]); set => Data.SetByte(xAddress[4], (byte) value); }
        [TileMetadata(5)] public int X5Tile { get => Data.GetByte(xAddress[5]); set => Data.SetByte(xAddress[5], (byte) value); }
        [TileMetadata(6)] public int X6Tile { get => Data.GetByte(xAddress[6]); set => Data.SetByte(xAddress[6], (byte) value); }
        [TileMetadata(7)] public int X7Tile { get => Data.GetByte(xAddress[7]); set => Data.SetByte(xAddress[7], (byte) value); }
        [TileMetadata(8)] public int X8Tile { get => Data.GetByte(xAddress[8]); set => Data.SetByte(xAddress[8], (byte) value); }
        [TileMetadata(9)] public int X9Tile { get => Data.GetByte(xAddress[9]); set => Data.SetByte(xAddress[9], (byte) value); }
        [TileMetadata(10)] public int X10Tile { get => Data.GetByte(xAddress[10]); set => Data.SetByte(xAddress[10], (byte) value); }
        [TileMetadata(11)] public int X11Tile { get => Data.GetByte(xAddress[11]); set => Data.SetByte(xAddress[11], (byte) value); }
        [TileMetadata(12)] public int X12Tile { get => Data.GetByte(xAddress[12]); set => Data.SetByte(xAddress[12], (byte) value); }
        [TileMetadata(13)] public int X13Tile { get => Data.GetByte(xAddress[13]); set => Data.SetByte(xAddress[13], (byte) value); }
        [TileMetadata(14)] public int X14Tile { get => Data.GetByte(xAddress[14]); set => Data.SetByte(xAddress[14], (byte) value); }
        [TileMetadata(15)] public int X15Tile { get => Data.GetByte(xAddress[15]); set => Data.SetByte(xAddress[15], (byte) value); }
        [TileMetadata(16)] public int X16Tile { get => Data.GetByte(xAddress[16]); set => Data.SetByte(xAddress[16], (byte) value); }
        [TileMetadata(17)] public int X17Tile { get => Data.GetByte(xAddress[17]); set => Data.SetByte(xAddress[17], (byte) value); }
        [TileMetadata(18)] public int X18Tile { get => Data.GetByte(xAddress[18]); set => Data.SetByte(xAddress[18], (byte) value); }
        [TileMetadata(19)] public int X19Tile { get => Data.GetByte(xAddress[19]); set => Data.SetByte(xAddress[19], (byte) value); }
        [TileMetadata(20)] public int X20Tile { get => Data.GetByte(xAddress[20]); set => Data.SetByte(xAddress[20], (byte) value); }
        [TileMetadata(21)] public int X21Tile { get => Data.GetByte(xAddress[21]); set => Data.SetByte(xAddress[21], (byte) value); }
        [TileMetadata(22)] public int X22Tile { get => Data.GetByte(xAddress[22]); set => Data.SetByte(xAddress[22], (byte) value); }
        [TileMetadata(23)] public int X23Tile { get => Data.GetByte(xAddress[23]); set => Data.SetByte(xAddress[23], (byte) value); }
        [TileMetadata(24)] public int X24Tile { get => Data.GetByte(xAddress[24]); set => Data.SetByte(xAddress[24], (byte) value); }
        [TileMetadata(25)] public int X25Tile { get => Data.GetByte(xAddress[25]); set => Data.SetByte(xAddress[25], (byte) value); }
        [TileMetadata(26)] public int X26Tile { get => Data.GetByte(xAddress[26]); set => Data.SetByte(xAddress[26], (byte) value); }
        [TileMetadata(27)] public int X27Tile { get => Data.GetByte(xAddress[27]); set => Data.SetByte(xAddress[27], (byte) value); }
        [TileMetadata(28)] public int X28Tile { get => Data.GetByte(xAddress[28]); set => Data.SetByte(xAddress[28], (byte) value); }
        [TileMetadata(29)] public int X29Tile { get => Data.GetByte(xAddress[29]); set => Data.SetByte(xAddress[29], (byte) value); }
        [TileMetadata(30)] public int X30Tile { get => Data.GetByte(xAddress[30]); set => Data.SetByte(xAddress[30], (byte) value); }
        [TileMetadata(31)] public int X31Tile { get => Data.GetByte(xAddress[31]); set => Data.SetByte(xAddress[31], (byte) value); }
        [TileMetadata(32)] public int X32Tile { get => Data.GetByte(xAddress[32]); set => Data.SetByte(xAddress[32], (byte) value); }
        [TileMetadata(33)] public int X33Tile { get => Data.GetByte(xAddress[33]); set => Data.SetByte(xAddress[33], (byte) value); }
        [TileMetadata(34)] public int X34Tile { get => Data.GetByte(xAddress[34]); set => Data.SetByte(xAddress[34], (byte) value); }
        [TileMetadata(35)] public int X35Tile { get => Data.GetByte(xAddress[35]); set => Data.SetByte(xAddress[35], (byte) value); }
        [TileMetadata(36)] public int X36Tile { get => Data.GetByte(xAddress[36]); set => Data.SetByte(xAddress[36], (byte) value); }
        [TileMetadata(37)] public int X37Tile { get => Data.GetByte(xAddress[37]); set => Data.SetByte(xAddress[37], (byte) value); }
        [TileMetadata(38)] public int X38Tile { get => Data.GetByte(xAddress[38]); set => Data.SetByte(xAddress[38], (byte) value); }
        [TileMetadata(39)] public int X39Tile { get => Data.GetByte(xAddress[39]); set => Data.SetByte(xAddress[39], (byte) value); }
        [TileMetadata(40)] public int X40Tile { get => Data.GetByte(xAddress[40]); set => Data.SetByte(xAddress[40], (byte) value); }
        [TileMetadata(41)] public int X41Tile { get => Data.GetByte(xAddress[41]); set => Data.SetByte(xAddress[41], (byte) value); }
        [TileMetadata(42)] public int X42Tile { get => Data.GetByte(xAddress[42]); set => Data.SetByte(xAddress[42], (byte) value); }
        [TileMetadata(43)] public int X43Tile { get => Data.GetByte(xAddress[43]); set => Data.SetByte(xAddress[43], (byte) value); }
        [TileMetadata(44)] public int X44Tile { get => Data.GetByte(xAddress[44]); set => Data.SetByte(xAddress[44], (byte) value); }
        [TileMetadata(45)] public int X45Tile { get => Data.GetByte(xAddress[45]); set => Data.SetByte(xAddress[45], (byte) value); }
        [TileMetadata(46)] public int X46Tile { get => Data.GetByte(xAddress[46]); set => Data.SetByte(xAddress[46], (byte) value); }
        [TileMetadata(47)] public int X47Tile { get => Data.GetByte(xAddress[47]); set => Data.SetByte(xAddress[47], (byte) value); }
        [TileMetadata(48)] public int X48Tile { get => Data.GetByte(xAddress[48]); set => Data.SetByte(xAddress[48], (byte) value); }
        [TileMetadata(49)] public int X49Tile { get => Data.GetByte(xAddress[49]); set => Data.SetByte(xAddress[49], (byte) value); }
        [TileMetadata(50)] public int X50Tile { get => Data.GetByte(xAddress[50]); set => Data.SetByte(xAddress[50], (byte) value); }
        [TileMetadata(51)] public int X51Tile { get => Data.GetByte(xAddress[51]); set => Data.SetByte(xAddress[51], (byte) value); }
        [TileMetadata(52)] public int X52Tile { get => Data.GetByte(xAddress[52]); set => Data.SetByte(xAddress[52], (byte) value); }
        [TileMetadata(53)] public int X53Tile { get => Data.GetByte(xAddress[53]); set => Data.SetByte(xAddress[53], (byte) value); }
        [TileMetadata(54)] public int X54Tile { get => Data.GetByte(xAddress[54]); set => Data.SetByte(xAddress[54], (byte) value); }
        [TileMetadata(55)] public int X55Tile { get => Data.GetByte(xAddress[55]); set => Data.SetByte(xAddress[55], (byte) value); }
        [TileMetadata(56)] public int X56Tile { get => Data.GetByte(xAddress[56]); set => Data.SetByte(xAddress[56], (byte) value); }
        [TileMetadata(57)] public int X57Tile { get => Data.GetByte(xAddress[57]); set => Data.SetByte(xAddress[57], (byte) value); }
        [TileMetadata(58)] public int X58Tile { get => Data.GetByte(xAddress[58]); set => Data.SetByte(xAddress[58], (byte) value); }
        [TileMetadata(59)] public int X59Tile { get => Data.GetByte(xAddress[59]); set => Data.SetByte(xAddress[59], (byte) value); }
        [TileMetadata(60)] public int X60Tile { get => Data.GetByte(xAddress[60]); set => Data.SetByte(xAddress[60], (byte) value); }
        [TileMetadata(61)] public int X61Tile { get => Data.GetByte(xAddress[61]); set => Data.SetByte(xAddress[61], (byte) value); }
        [TileMetadata(62)] public int X62Tile { get => Data.GetByte(xAddress[62]); set => Data.SetByte(xAddress[62], (byte) value); }
        [TileMetadata(63)] public int X63Tile { get => Data.GetByte(xAddress[63]); set => Data.SetByte(xAddress[63], (byte) value); }
    }
}
