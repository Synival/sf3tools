using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawEditors;

namespace SF3.Models.Structs.MPD {
    public class TileSurfaceCharacterRow : Struct {
        private readonly int[] xAddress = new int[64];

        public TileSurfaceCharacterRow(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 128) {
            for (var i = 0; i < xAddress.Length; i++) {
                var block = i / 4;
                var x = i % 4;
                xAddress[i] = Address + (block * 16 + x) * 2;
            }
        }

        public ushort[] Tiles {
            get {
                // TODO: somehow cache this?
                var tiles = new ushort[64];
                for (var i = 0; i < tiles.Length; i++)
                    tiles[i] = (ushort) Editor.GetWord(xAddress[i]);
                return tiles;
            }
        }

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            public TileMetadataAttribute(int x) : base(displayName: "X" + x.ToString("D2"), displayOrder: x, displayFormat: "X4", minWidth: 50) { }
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 64 individual properties.
        [TileMetadata(0)] public int X0Tile { get => Editor.GetWord(xAddress[0]); set => Editor.SetWord(xAddress[0], value); }
        [TileMetadata(1)] public int X1Tile { get => Editor.GetWord(xAddress[1]); set => Editor.SetWord(xAddress[1], value); }
        [TileMetadata(2)] public int X2Tile { get => Editor.GetWord(xAddress[2]); set => Editor.SetWord(xAddress[2], value); }
        [TileMetadata(3)] public int X3Tile { get => Editor.GetWord(xAddress[3]); set => Editor.SetWord(xAddress[3], value); }
        [TileMetadata(4)] public int X4Tile { get => Editor.GetWord(xAddress[4]); set => Editor.SetWord(xAddress[4], value); }
        [TileMetadata(5)] public int X5Tile { get => Editor.GetWord(xAddress[5]); set => Editor.SetWord(xAddress[5], value); }
        [TileMetadata(6)] public int X6Tile { get => Editor.GetWord(xAddress[6]); set => Editor.SetWord(xAddress[6], value); }
        [TileMetadata(7)] public int X7Tile { get => Editor.GetWord(xAddress[7]); set => Editor.SetWord(xAddress[7], value); }
        [TileMetadata(8)] public int X8Tile { get => Editor.GetWord(xAddress[8]); set => Editor.SetWord(xAddress[8], value); }
        [TileMetadata(9)] public int X9Tile { get => Editor.GetWord(xAddress[9]); set => Editor.SetWord(xAddress[9], value); }
        [TileMetadata(10)] public int X10Tile { get => Editor.GetWord(xAddress[10]); set => Editor.SetWord(xAddress[10], value); }
        [TileMetadata(11)] public int X11Tile { get => Editor.GetWord(xAddress[11]); set => Editor.SetWord(xAddress[11], value); }
        [TileMetadata(12)] public int X12Tile { get => Editor.GetWord(xAddress[12]); set => Editor.SetWord(xAddress[12], value); }
        [TileMetadata(13)] public int X13Tile { get => Editor.GetWord(xAddress[13]); set => Editor.SetWord(xAddress[13], value); }
        [TileMetadata(14)] public int X14Tile { get => Editor.GetWord(xAddress[14]); set => Editor.SetWord(xAddress[14], value); }
        [TileMetadata(15)] public int X15Tile { get => Editor.GetWord(xAddress[15]); set => Editor.SetWord(xAddress[15], value); }
        [TileMetadata(16)] public int X16Tile { get => Editor.GetWord(xAddress[16]); set => Editor.SetWord(xAddress[16], value); }
        [TileMetadata(17)] public int X17Tile { get => Editor.GetWord(xAddress[17]); set => Editor.SetWord(xAddress[17], value); }
        [TileMetadata(18)] public int X18Tile { get => Editor.GetWord(xAddress[18]); set => Editor.SetWord(xAddress[18], value); }
        [TileMetadata(19)] public int X19Tile { get => Editor.GetWord(xAddress[19]); set => Editor.SetWord(xAddress[19], value); }
        [TileMetadata(20)] public int X20Tile { get => Editor.GetWord(xAddress[20]); set => Editor.SetWord(xAddress[20], value); }
        [TileMetadata(21)] public int X21Tile { get => Editor.GetWord(xAddress[21]); set => Editor.SetWord(xAddress[21], value); }
        [TileMetadata(22)] public int X22Tile { get => Editor.GetWord(xAddress[22]); set => Editor.SetWord(xAddress[22], value); }
        [TileMetadata(23)] public int X23Tile { get => Editor.GetWord(xAddress[23]); set => Editor.SetWord(xAddress[23], value); }
        [TileMetadata(24)] public int X24Tile { get => Editor.GetWord(xAddress[24]); set => Editor.SetWord(xAddress[24], value); }
        [TileMetadata(25)] public int X25Tile { get => Editor.GetWord(xAddress[25]); set => Editor.SetWord(xAddress[25], value); }
        [TileMetadata(26)] public int X26Tile { get => Editor.GetWord(xAddress[26]); set => Editor.SetWord(xAddress[26], value); }
        [TileMetadata(27)] public int X27Tile { get => Editor.GetWord(xAddress[27]); set => Editor.SetWord(xAddress[27], value); }
        [TileMetadata(28)] public int X28Tile { get => Editor.GetWord(xAddress[28]); set => Editor.SetWord(xAddress[28], value); }
        [TileMetadata(29)] public int X29Tile { get => Editor.GetWord(xAddress[29]); set => Editor.SetWord(xAddress[29], value); }
        [TileMetadata(30)] public int X30Tile { get => Editor.GetWord(xAddress[30]); set => Editor.SetWord(xAddress[30], value); }
        [TileMetadata(31)] public int X31Tile { get => Editor.GetWord(xAddress[31]); set => Editor.SetWord(xAddress[31], value); }
        [TileMetadata(32)] public int X32Tile { get => Editor.GetWord(xAddress[32]); set => Editor.SetWord(xAddress[32], value); }
        [TileMetadata(33)] public int X33Tile { get => Editor.GetWord(xAddress[33]); set => Editor.SetWord(xAddress[33], value); }
        [TileMetadata(34)] public int X34Tile { get => Editor.GetWord(xAddress[34]); set => Editor.SetWord(xAddress[34], value); }
        [TileMetadata(35)] public int X35Tile { get => Editor.GetWord(xAddress[35]); set => Editor.SetWord(xAddress[35], value); }
        [TileMetadata(36)] public int X36Tile { get => Editor.GetWord(xAddress[36]); set => Editor.SetWord(xAddress[36], value); }
        [TileMetadata(37)] public int X37Tile { get => Editor.GetWord(xAddress[37]); set => Editor.SetWord(xAddress[37], value); }
        [TileMetadata(38)] public int X38Tile { get => Editor.GetWord(xAddress[38]); set => Editor.SetWord(xAddress[38], value); }
        [TileMetadata(39)] public int X39Tile { get => Editor.GetWord(xAddress[39]); set => Editor.SetWord(xAddress[39], value); }
        [TileMetadata(40)] public int X40Tile { get => Editor.GetWord(xAddress[40]); set => Editor.SetWord(xAddress[40], value); }
        [TileMetadata(41)] public int X41Tile { get => Editor.GetWord(xAddress[41]); set => Editor.SetWord(xAddress[41], value); }
        [TileMetadata(42)] public int X42Tile { get => Editor.GetWord(xAddress[42]); set => Editor.SetWord(xAddress[42], value); }
        [TileMetadata(43)] public int X43Tile { get => Editor.GetWord(xAddress[43]); set => Editor.SetWord(xAddress[43], value); }
        [TileMetadata(44)] public int X44Tile { get => Editor.GetWord(xAddress[44]); set => Editor.SetWord(xAddress[44], value); }
        [TileMetadata(45)] public int X45Tile { get => Editor.GetWord(xAddress[45]); set => Editor.SetWord(xAddress[45], value); }
        [TileMetadata(46)] public int X46Tile { get => Editor.GetWord(xAddress[46]); set => Editor.SetWord(xAddress[46], value); }
        [TileMetadata(47)] public int X47Tile { get => Editor.GetWord(xAddress[47]); set => Editor.SetWord(xAddress[47], value); }
        [TileMetadata(48)] public int X48Tile { get => Editor.GetWord(xAddress[48]); set => Editor.SetWord(xAddress[48], value); }
        [TileMetadata(49)] public int X49Tile { get => Editor.GetWord(xAddress[49]); set => Editor.SetWord(xAddress[49], value); }
        [TileMetadata(50)] public int X50Tile { get => Editor.GetWord(xAddress[50]); set => Editor.SetWord(xAddress[50], value); }
        [TileMetadata(51)] public int X51Tile { get => Editor.GetWord(xAddress[51]); set => Editor.SetWord(xAddress[51], value); }
        [TileMetadata(52)] public int X52Tile { get => Editor.GetWord(xAddress[52]); set => Editor.SetWord(xAddress[52], value); }
        [TileMetadata(53)] public int X53Tile { get => Editor.GetWord(xAddress[53]); set => Editor.SetWord(xAddress[53], value); }
        [TileMetadata(54)] public int X54Tile { get => Editor.GetWord(xAddress[54]); set => Editor.SetWord(xAddress[54], value); }
        [TileMetadata(55)] public int X55Tile { get => Editor.GetWord(xAddress[55]); set => Editor.SetWord(xAddress[55], value); }
        [TileMetadata(56)] public int X56Tile { get => Editor.GetWord(xAddress[56]); set => Editor.SetWord(xAddress[56], value); }
        [TileMetadata(57)] public int X57Tile { get => Editor.GetWord(xAddress[57]); set => Editor.SetWord(xAddress[57], value); }
        [TileMetadata(58)] public int X58Tile { get => Editor.GetWord(xAddress[58]); set => Editor.SetWord(xAddress[58], value); }
        [TileMetadata(59)] public int X59Tile { get => Editor.GetWord(xAddress[59]); set => Editor.SetWord(xAddress[59], value); }
        [TileMetadata(60)] public int X60Tile { get => Editor.GetWord(xAddress[60]); set => Editor.SetWord(xAddress[60], value); }
        [TileMetadata(61)] public int X61Tile { get => Editor.GetWord(xAddress[61]); set => Editor.SetWord(xAddress[61], value); }
        [TileMetadata(62)] public int X62Tile { get => Editor.GetWord(xAddress[62]); set => Editor.SetWord(xAddress[62], value); }
        [TileMetadata(63)] public int X63Tile { get => Editor.GetWord(xAddress[63]); set => Editor.SetWord(xAddress[63], value); }
    }
}
