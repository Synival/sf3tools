using CommonLib.Attributes;
using SF3.RawEditors;

namespace SF3.Models.MPD {
    public class TileSurfaceHeightmapRow : Model {
        private readonly int[] xAddress = new int[64];

        public TileSurfaceHeightmapRow(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 256) {
            for (var i = 0; i < xAddress.Length; i++)
                xAddress[i] = Address + i * 4;
        }

        private class TileMetadataAttribute : DataViewModelColumnAttribute {
            public TileMetadataAttribute(int x) : base(displayName: "X" + x.ToString("D2"), displayOrder: x, displayFormat: "X8", minWidth: 75) { }
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 64 individual properties.
        [TileMetadata(0)] public uint X0Tile { get => (uint) Editor.GetDouble(xAddress[0]); set => Editor.SetDouble(xAddress[0], (int) value); }
        [TileMetadata(1)] public uint X1Tile { get => (uint) Editor.GetDouble(xAddress[1]); set => Editor.SetDouble(xAddress[1], (int) value); }
        [TileMetadata(2)] public uint X2Tile { get => (uint) Editor.GetDouble(xAddress[2]); set => Editor.SetDouble(xAddress[2], (int) value); }
        [TileMetadata(3)] public uint X3Tile { get => (uint) Editor.GetDouble(xAddress[3]); set => Editor.SetDouble(xAddress[3], (int) value); }
        [TileMetadata(4)] public uint X4Tile { get => (uint) Editor.GetDouble(xAddress[4]); set => Editor.SetDouble(xAddress[4], (int) value); }
        [TileMetadata(5)] public uint X5Tile { get => (uint) Editor.GetDouble(xAddress[5]); set => Editor.SetDouble(xAddress[5], (int) value); }
        [TileMetadata(6)] public uint X6Tile { get => (uint) Editor.GetDouble(xAddress[6]); set => Editor.SetDouble(xAddress[6], (int) value); }
        [TileMetadata(7)] public uint X7Tile { get => (uint) Editor.GetDouble(xAddress[7]); set => Editor.SetDouble(xAddress[7], (int) value); }
        [TileMetadata(8)] public uint X8Tile { get => (uint) Editor.GetDouble(xAddress[8]); set => Editor.SetDouble(xAddress[8], (int) value); }
        [TileMetadata(9)] public uint X9Tile { get => (uint) Editor.GetDouble(xAddress[9]); set => Editor.SetDouble(xAddress[9], (int) value); }
        [TileMetadata(10)] public uint X10Tile { get => (uint) Editor.GetDouble(xAddress[10]); set => Editor.SetDouble(xAddress[10], (int) value); }
        [TileMetadata(11)] public uint X11Tile { get => (uint) Editor.GetDouble(xAddress[11]); set => Editor.SetDouble(xAddress[11], (int) value); }
        [TileMetadata(12)] public uint X12Tile { get => (uint) Editor.GetDouble(xAddress[12]); set => Editor.SetDouble(xAddress[12], (int) value); }
        [TileMetadata(13)] public uint X13Tile { get => (uint) Editor.GetDouble(xAddress[13]); set => Editor.SetDouble(xAddress[13], (int) value); }
        [TileMetadata(14)] public uint X14Tile { get => (uint) Editor.GetDouble(xAddress[14]); set => Editor.SetDouble(xAddress[14], (int) value); }
        [TileMetadata(15)] public uint X15Tile { get => (uint) Editor.GetDouble(xAddress[15]); set => Editor.SetDouble(xAddress[15], (int) value); }
        [TileMetadata(16)] public uint X16Tile { get => (uint) Editor.GetDouble(xAddress[16]); set => Editor.SetDouble(xAddress[16], (int) value); }
        [TileMetadata(17)] public uint X17Tile { get => (uint) Editor.GetDouble(xAddress[17]); set => Editor.SetDouble(xAddress[17], (int) value); }
        [TileMetadata(18)] public uint X18Tile { get => (uint) Editor.GetDouble(xAddress[18]); set => Editor.SetDouble(xAddress[18], (int) value); }
        [TileMetadata(19)] public uint X19Tile { get => (uint) Editor.GetDouble(xAddress[19]); set => Editor.SetDouble(xAddress[19], (int) value); }
        [TileMetadata(20)] public uint X20Tile { get => (uint) Editor.GetDouble(xAddress[20]); set => Editor.SetDouble(xAddress[20], (int) value); }
        [TileMetadata(21)] public uint X21Tile { get => (uint) Editor.GetDouble(xAddress[21]); set => Editor.SetDouble(xAddress[21], (int) value); }
        [TileMetadata(22)] public uint X22Tile { get => (uint) Editor.GetDouble(xAddress[22]); set => Editor.SetDouble(xAddress[22], (int) value); }
        [TileMetadata(23)] public uint X23Tile { get => (uint) Editor.GetDouble(xAddress[23]); set => Editor.SetDouble(xAddress[23], (int) value); }
        [TileMetadata(24)] public uint X24Tile { get => (uint) Editor.GetDouble(xAddress[24]); set => Editor.SetDouble(xAddress[24], (int) value); }
        [TileMetadata(25)] public uint X25Tile { get => (uint) Editor.GetDouble(xAddress[25]); set => Editor.SetDouble(xAddress[25], (int) value); }
        [TileMetadata(26)] public uint X26Tile { get => (uint) Editor.GetDouble(xAddress[26]); set => Editor.SetDouble(xAddress[26], (int) value); }
        [TileMetadata(27)] public uint X27Tile { get => (uint) Editor.GetDouble(xAddress[27]); set => Editor.SetDouble(xAddress[27], (int) value); }
        [TileMetadata(28)] public uint X28Tile { get => (uint) Editor.GetDouble(xAddress[28]); set => Editor.SetDouble(xAddress[28], (int) value); }
        [TileMetadata(29)] public uint X29Tile { get => (uint) Editor.GetDouble(xAddress[29]); set => Editor.SetDouble(xAddress[29], (int) value); }
        [TileMetadata(30)] public uint X30Tile { get => (uint) Editor.GetDouble(xAddress[30]); set => Editor.SetDouble(xAddress[30], (int) value); }
        [TileMetadata(31)] public uint X31Tile { get => (uint) Editor.GetDouble(xAddress[31]); set => Editor.SetDouble(xAddress[31], (int) value); }
        [TileMetadata(32)] public uint X32Tile { get => (uint) Editor.GetDouble(xAddress[32]); set => Editor.SetDouble(xAddress[32], (int) value); }
        [TileMetadata(33)] public uint X33Tile { get => (uint) Editor.GetDouble(xAddress[33]); set => Editor.SetDouble(xAddress[33], (int) value); }
        [TileMetadata(34)] public uint X34Tile { get => (uint) Editor.GetDouble(xAddress[34]); set => Editor.SetDouble(xAddress[34], (int) value); }
        [TileMetadata(35)] public uint X35Tile { get => (uint) Editor.GetDouble(xAddress[35]); set => Editor.SetDouble(xAddress[35], (int) value); }
        [TileMetadata(36)] public uint X36Tile { get => (uint) Editor.GetDouble(xAddress[36]); set => Editor.SetDouble(xAddress[36], (int) value); }
        [TileMetadata(37)] public uint X37Tile { get => (uint) Editor.GetDouble(xAddress[37]); set => Editor.SetDouble(xAddress[37], (int) value); }
        [TileMetadata(38)] public uint X38Tile { get => (uint) Editor.GetDouble(xAddress[38]); set => Editor.SetDouble(xAddress[38], (int) value); }
        [TileMetadata(39)] public uint X39Tile { get => (uint) Editor.GetDouble(xAddress[39]); set => Editor.SetDouble(xAddress[39], (int) value); }
        [TileMetadata(40)] public uint X40Tile { get => (uint) Editor.GetDouble(xAddress[40]); set => Editor.SetDouble(xAddress[40], (int) value); }
        [TileMetadata(41)] public uint X41Tile { get => (uint) Editor.GetDouble(xAddress[41]); set => Editor.SetDouble(xAddress[41], (int) value); }
        [TileMetadata(42)] public uint X42Tile { get => (uint) Editor.GetDouble(xAddress[42]); set => Editor.SetDouble(xAddress[42], (int) value); }
        [TileMetadata(43)] public uint X43Tile { get => (uint) Editor.GetDouble(xAddress[43]); set => Editor.SetDouble(xAddress[43], (int) value); }
        [TileMetadata(44)] public uint X44Tile { get => (uint) Editor.GetDouble(xAddress[44]); set => Editor.SetDouble(xAddress[44], (int) value); }
        [TileMetadata(45)] public uint X45Tile { get => (uint) Editor.GetDouble(xAddress[45]); set => Editor.SetDouble(xAddress[45], (int) value); }
        [TileMetadata(46)] public uint X46Tile { get => (uint) Editor.GetDouble(xAddress[46]); set => Editor.SetDouble(xAddress[46], (int) value); }
        [TileMetadata(47)] public uint X47Tile { get => (uint) Editor.GetDouble(xAddress[47]); set => Editor.SetDouble(xAddress[47], (int) value); }
        [TileMetadata(48)] public uint X48Tile { get => (uint) Editor.GetDouble(xAddress[48]); set => Editor.SetDouble(xAddress[48], (int) value); }
        [TileMetadata(49)] public uint X49Tile { get => (uint) Editor.GetDouble(xAddress[49]); set => Editor.SetDouble(xAddress[49], (int) value); }
        [TileMetadata(50)] public uint X50Tile { get => (uint) Editor.GetDouble(xAddress[50]); set => Editor.SetDouble(xAddress[50], (int) value); }
        [TileMetadata(51)] public uint X51Tile { get => (uint) Editor.GetDouble(xAddress[51]); set => Editor.SetDouble(xAddress[51], (int) value); }
        [TileMetadata(52)] public uint X52Tile { get => (uint) Editor.GetDouble(xAddress[52]); set => Editor.SetDouble(xAddress[52], (int) value); }
        [TileMetadata(53)] public uint X53Tile { get => (uint) Editor.GetDouble(xAddress[53]); set => Editor.SetDouble(xAddress[53], (int) value); }
        [TileMetadata(54)] public uint X54Tile { get => (uint) Editor.GetDouble(xAddress[54]); set => Editor.SetDouble(xAddress[54], (int) value); }
        [TileMetadata(55)] public uint X55Tile { get => (uint) Editor.GetDouble(xAddress[55]); set => Editor.SetDouble(xAddress[55], (int) value); }
        [TileMetadata(56)] public uint X56Tile { get => (uint) Editor.GetDouble(xAddress[56]); set => Editor.SetDouble(xAddress[56], (int) value); }
        [TileMetadata(57)] public uint X57Tile { get => (uint) Editor.GetDouble(xAddress[57]); set => Editor.SetDouble(xAddress[57], (int) value); }
        [TileMetadata(58)] public uint X58Tile { get => (uint) Editor.GetDouble(xAddress[58]); set => Editor.SetDouble(xAddress[58], (int) value); }
        [TileMetadata(59)] public uint X59Tile { get => (uint) Editor.GetDouble(xAddress[59]); set => Editor.SetDouble(xAddress[59], (int) value); }
        [TileMetadata(60)] public uint X60Tile { get => (uint) Editor.GetDouble(xAddress[60]); set => Editor.SetDouble(xAddress[60], (int) value); }
        [TileMetadata(61)] public uint X61Tile { get => (uint) Editor.GetDouble(xAddress[61]); set => Editor.SetDouble(xAddress[61], (int) value); }
        [TileMetadata(62)] public uint X62Tile { get => (uint) Editor.GetDouble(xAddress[62]); set => Editor.SetDouble(xAddress[62], (int) value); }
        [TileMetadata(63)] public uint X63Tile { get => (uint) Editor.GetDouble(xAddress[63]); set => Editor.SetDouble(xAddress[63], (int) value); }
    }
}
