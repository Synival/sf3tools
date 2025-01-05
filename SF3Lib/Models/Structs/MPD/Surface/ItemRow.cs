using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Surface {
    public class ItemRow : Struct {
        private readonly int[] xAddress = new int[64];

        public ItemRow(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 64) {
            for (var i = 0; i < xAddress.Length; i++)
                xAddress[i] = Address + i;
        }

        public byte this[int x] {
            get => (byte) Data.GetByte(xAddress[x]);
            set => Data.SetByte(xAddress[x], value);
        }

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            public TileMetadataAttribute(int x) : base(displayName: "X" + x.ToString("D2"), displayOrder: x, displayFormat: "X2") { }
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 64 individual properties.
        [TileMetadata(0)] public byte X0Tile { get => (byte) Data.GetByte(xAddress[0]); set => Data.SetByte(xAddress[0], value); }
        [TileMetadata(1)] public byte X1Tile { get => (byte) Data.GetByte(xAddress[1]); set => Data.SetByte(xAddress[1], value); }
        [TileMetadata(2)] public byte X2Tile { get => (byte) Data.GetByte(xAddress[2]); set => Data.SetByte(xAddress[2], value); }
        [TileMetadata(3)] public byte X3Tile { get => (byte) Data.GetByte(xAddress[3]); set => Data.SetByte(xAddress[3], value); }
        [TileMetadata(4)] public byte X4Tile { get => (byte) Data.GetByte(xAddress[4]); set => Data.SetByte(xAddress[4], value); }
        [TileMetadata(5)] public byte X5Tile { get => (byte) Data.GetByte(xAddress[5]); set => Data.SetByte(xAddress[5], value); }
        [TileMetadata(6)] public byte X6Tile { get => (byte) Data.GetByte(xAddress[6]); set => Data.SetByte(xAddress[6], value); }
        [TileMetadata(7)] public byte X7Tile { get => (byte) Data.GetByte(xAddress[7]); set => Data.SetByte(xAddress[7], value); }
        [TileMetadata(8)] public byte X8Tile { get => (byte) Data.GetByte(xAddress[8]); set => Data.SetByte(xAddress[8], value); }
        [TileMetadata(9)] public byte X9Tile { get => (byte) Data.GetByte(xAddress[9]); set => Data.SetByte(xAddress[9], value); }
        [TileMetadata(10)] public byte X10Tile { get => (byte) Data.GetByte(xAddress[10]); set => Data.SetByte(xAddress[10], value); }
        [TileMetadata(11)] public byte X11Tile { get => (byte) Data.GetByte(xAddress[11]); set => Data.SetByte(xAddress[11], value); }
        [TileMetadata(12)] public byte X12Tile { get => (byte) Data.GetByte(xAddress[12]); set => Data.SetByte(xAddress[12], value); }
        [TileMetadata(13)] public byte X13Tile { get => (byte) Data.GetByte(xAddress[13]); set => Data.SetByte(xAddress[13], value); }
        [TileMetadata(14)] public byte X14Tile { get => (byte) Data.GetByte(xAddress[14]); set => Data.SetByte(xAddress[14], value); }
        [TileMetadata(15)] public byte X15Tile { get => (byte) Data.GetByte(xAddress[15]); set => Data.SetByte(xAddress[15], value); }
        [TileMetadata(16)] public byte X16Tile { get => (byte) Data.GetByte(xAddress[16]); set => Data.SetByte(xAddress[16], value); }
        [TileMetadata(17)] public byte X17Tile { get => (byte) Data.GetByte(xAddress[17]); set => Data.SetByte(xAddress[17], value); }
        [TileMetadata(18)] public byte X18Tile { get => (byte) Data.GetByte(xAddress[18]); set => Data.SetByte(xAddress[18], value); }
        [TileMetadata(19)] public byte X19Tile { get => (byte) Data.GetByte(xAddress[19]); set => Data.SetByte(xAddress[19], value); }
        [TileMetadata(20)] public byte X20Tile { get => (byte) Data.GetByte(xAddress[20]); set => Data.SetByte(xAddress[20], value); }
        [TileMetadata(21)] public byte X21Tile { get => (byte) Data.GetByte(xAddress[21]); set => Data.SetByte(xAddress[21], value); }
        [TileMetadata(22)] public byte X22Tile { get => (byte) Data.GetByte(xAddress[22]); set => Data.SetByte(xAddress[22], value); }
        [TileMetadata(23)] public byte X23Tile { get => (byte) Data.GetByte(xAddress[23]); set => Data.SetByte(xAddress[23], value); }
        [TileMetadata(24)] public byte X24Tile { get => (byte) Data.GetByte(xAddress[24]); set => Data.SetByte(xAddress[24], value); }
        [TileMetadata(25)] public byte X25Tile { get => (byte) Data.GetByte(xAddress[25]); set => Data.SetByte(xAddress[25], value); }
        [TileMetadata(26)] public byte X26Tile { get => (byte) Data.GetByte(xAddress[26]); set => Data.SetByte(xAddress[26], value); }
        [TileMetadata(27)] public byte X27Tile { get => (byte) Data.GetByte(xAddress[27]); set => Data.SetByte(xAddress[27], value); }
        [TileMetadata(28)] public byte X28Tile { get => (byte) Data.GetByte(xAddress[28]); set => Data.SetByte(xAddress[28], value); }
        [TileMetadata(29)] public byte X29Tile { get => (byte) Data.GetByte(xAddress[29]); set => Data.SetByte(xAddress[29], value); }
        [TileMetadata(30)] public byte X30Tile { get => (byte) Data.GetByte(xAddress[30]); set => Data.SetByte(xAddress[30], value); }
        [TileMetadata(31)] public byte X31Tile { get => (byte) Data.GetByte(xAddress[31]); set => Data.SetByte(xAddress[31], value); }
        [TileMetadata(32)] public byte X32Tile { get => (byte) Data.GetByte(xAddress[32]); set => Data.SetByte(xAddress[32], value); }
        [TileMetadata(33)] public byte X33Tile { get => (byte) Data.GetByte(xAddress[33]); set => Data.SetByte(xAddress[33], value); }
        [TileMetadata(34)] public byte X34Tile { get => (byte) Data.GetByte(xAddress[34]); set => Data.SetByte(xAddress[34], value); }
        [TileMetadata(35)] public byte X35Tile { get => (byte) Data.GetByte(xAddress[35]); set => Data.SetByte(xAddress[35], value); }
        [TileMetadata(36)] public byte X36Tile { get => (byte) Data.GetByte(xAddress[36]); set => Data.SetByte(xAddress[36], value); }
        [TileMetadata(37)] public byte X37Tile { get => (byte) Data.GetByte(xAddress[37]); set => Data.SetByte(xAddress[37], value); }
        [TileMetadata(38)] public byte X38Tile { get => (byte) Data.GetByte(xAddress[38]); set => Data.SetByte(xAddress[38], value); }
        [TileMetadata(39)] public byte X39Tile { get => (byte) Data.GetByte(xAddress[39]); set => Data.SetByte(xAddress[39], value); }
        [TileMetadata(40)] public byte X40Tile { get => (byte) Data.GetByte(xAddress[40]); set => Data.SetByte(xAddress[40], value); }
        [TileMetadata(41)] public byte X41Tile { get => (byte) Data.GetByte(xAddress[41]); set => Data.SetByte(xAddress[41], value); }
        [TileMetadata(42)] public byte X42Tile { get => (byte) Data.GetByte(xAddress[42]); set => Data.SetByte(xAddress[42], value); }
        [TileMetadata(43)] public byte X43Tile { get => (byte) Data.GetByte(xAddress[43]); set => Data.SetByte(xAddress[43], value); }
        [TileMetadata(44)] public byte X44Tile { get => (byte) Data.GetByte(xAddress[44]); set => Data.SetByte(xAddress[44], value); }
        [TileMetadata(45)] public byte X45Tile { get => (byte) Data.GetByte(xAddress[45]); set => Data.SetByte(xAddress[45], value); }
        [TileMetadata(46)] public byte X46Tile { get => (byte) Data.GetByte(xAddress[46]); set => Data.SetByte(xAddress[46], value); }
        [TileMetadata(47)] public byte X47Tile { get => (byte) Data.GetByte(xAddress[47]); set => Data.SetByte(xAddress[47], value); }
        [TileMetadata(48)] public byte X48Tile { get => (byte) Data.GetByte(xAddress[48]); set => Data.SetByte(xAddress[48], value); }
        [TileMetadata(49)] public byte X49Tile { get => (byte) Data.GetByte(xAddress[49]); set => Data.SetByte(xAddress[49], value); }
        [TileMetadata(50)] public byte X50Tile { get => (byte) Data.GetByte(xAddress[50]); set => Data.SetByte(xAddress[50], value); }
        [TileMetadata(51)] public byte X51Tile { get => (byte) Data.GetByte(xAddress[51]); set => Data.SetByte(xAddress[51], value); }
        [TileMetadata(52)] public byte X52Tile { get => (byte) Data.GetByte(xAddress[52]); set => Data.SetByte(xAddress[52], value); }
        [TileMetadata(53)] public byte X53Tile { get => (byte) Data.GetByte(xAddress[53]); set => Data.SetByte(xAddress[53], value); }
        [TileMetadata(54)] public byte X54Tile { get => (byte) Data.GetByte(xAddress[54]); set => Data.SetByte(xAddress[54], value); }
        [TileMetadata(55)] public byte X55Tile { get => (byte) Data.GetByte(xAddress[55]); set => Data.SetByte(xAddress[55], value); }
        [TileMetadata(56)] public byte X56Tile { get => (byte) Data.GetByte(xAddress[56]); set => Data.SetByte(xAddress[56], value); }
        [TileMetadata(57)] public byte X57Tile { get => (byte) Data.GetByte(xAddress[57]); set => Data.SetByte(xAddress[57], value); }
        [TileMetadata(58)] public byte X58Tile { get => (byte) Data.GetByte(xAddress[58]); set => Data.SetByte(xAddress[58], value); }
        [TileMetadata(59)] public byte X59Tile { get => (byte) Data.GetByte(xAddress[59]); set => Data.SetByte(xAddress[59], value); }
        [TileMetadata(60)] public byte X60Tile { get => (byte) Data.GetByte(xAddress[60]); set => Data.SetByte(xAddress[60], value); }
        [TileMetadata(61)] public byte X61Tile { get => (byte) Data.GetByte(xAddress[61]); set => Data.SetByte(xAddress[61], value); }
        [TileMetadata(62)] public byte X62Tile { get => (byte) Data.GetByte(xAddress[62]); set => Data.SetByte(xAddress[62], value); }
        [TileMetadata(63)] public byte X63Tile { get => (byte) Data.GetByte(xAddress[63]); set => Data.SetByte(xAddress[63], value); }
    }
}
