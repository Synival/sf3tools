using SF3.FileEditors;

namespace SF3.Models.MPD {
    public class TileSurfaceCharacterRow : Model {
        private readonly int[] xAddress = new int[64];

        public TileSurfaceCharacterRow(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 128) {
            for (var i = 0; i < xAddress.Length; i++) {
                int block = i / 4;
                int x = i % 4;
                xAddress[i] = Address + ((block * 16) + x) * 2;
            }
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 64 individual properties.
        public int X0Tile { get => Editor.GetWord(xAddress[0]); set => Editor.SetWord(xAddress[0], value); }
        public int X1Tile { get => Editor.GetWord(xAddress[1]); set => Editor.SetWord(xAddress[1], value); }
        public int X2Tile { get => Editor.GetWord(xAddress[2]); set => Editor.SetWord(xAddress[2], value); }
        public int X3Tile { get => Editor.GetWord(xAddress[3]); set => Editor.SetWord(xAddress[3], value); }
        public int X4Tile { get => Editor.GetWord(xAddress[4]); set => Editor.SetWord(xAddress[4], value); }
        public int X5Tile { get => Editor.GetWord(xAddress[5]); set => Editor.SetWord(xAddress[5], value); }
        public int X6Tile { get => Editor.GetWord(xAddress[6]); set => Editor.SetWord(xAddress[6], value); }
        public int X7Tile { get => Editor.GetWord(xAddress[7]); set => Editor.SetWord(xAddress[7], value); }
        public int X8Tile { get => Editor.GetWord(xAddress[8]); set => Editor.SetWord(xAddress[8], value); }
        public int X9Tile { get => Editor.GetWord(xAddress[9]); set => Editor.SetWord(xAddress[9], value); }
        public int X10Tile { get => Editor.GetWord(xAddress[10]); set => Editor.SetWord(xAddress[10], value); }
        public int X11Tile { get => Editor.GetWord(xAddress[11]); set => Editor.SetWord(xAddress[11], value); }
        public int X12Tile { get => Editor.GetWord(xAddress[12]); set => Editor.SetWord(xAddress[12], value); }
        public int X13Tile { get => Editor.GetWord(xAddress[13]); set => Editor.SetWord(xAddress[13], value); }
        public int X14Tile { get => Editor.GetWord(xAddress[14]); set => Editor.SetWord(xAddress[14], value); }
        public int X15Tile { get => Editor.GetWord(xAddress[15]); set => Editor.SetWord(xAddress[15], value); }
        public int X16Tile { get => Editor.GetWord(xAddress[16]); set => Editor.SetWord(xAddress[16], value); }
        public int X17Tile { get => Editor.GetWord(xAddress[17]); set => Editor.SetWord(xAddress[17], value); }
        public int X18Tile { get => Editor.GetWord(xAddress[18]); set => Editor.SetWord(xAddress[18], value); }
        public int X19Tile { get => Editor.GetWord(xAddress[19]); set => Editor.SetWord(xAddress[19], value); }
        public int X20Tile { get => Editor.GetWord(xAddress[20]); set => Editor.SetWord(xAddress[20], value); }
        public int X21Tile { get => Editor.GetWord(xAddress[21]); set => Editor.SetWord(xAddress[21], value); }
        public int X22Tile { get => Editor.GetWord(xAddress[22]); set => Editor.SetWord(xAddress[22], value); }
        public int X23Tile { get => Editor.GetWord(xAddress[23]); set => Editor.SetWord(xAddress[23], value); }
        public int X24Tile { get => Editor.GetWord(xAddress[24]); set => Editor.SetWord(xAddress[24], value); }
        public int X25Tile { get => Editor.GetWord(xAddress[25]); set => Editor.SetWord(xAddress[25], value); }
        public int X26Tile { get => Editor.GetWord(xAddress[26]); set => Editor.SetWord(xAddress[26], value); }
        public int X27Tile { get => Editor.GetWord(xAddress[27]); set => Editor.SetWord(xAddress[27], value); }
        public int X28Tile { get => Editor.GetWord(xAddress[28]); set => Editor.SetWord(xAddress[28], value); }
        public int X29Tile { get => Editor.GetWord(xAddress[29]); set => Editor.SetWord(xAddress[29], value); }
        public int X30Tile { get => Editor.GetWord(xAddress[30]); set => Editor.SetWord(xAddress[30], value); }
        public int X31Tile { get => Editor.GetWord(xAddress[31]); set => Editor.SetWord(xAddress[31], value); }
        public int X32Tile { get => Editor.GetWord(xAddress[32]); set => Editor.SetWord(xAddress[32], value); }
        public int X33Tile { get => Editor.GetWord(xAddress[33]); set => Editor.SetWord(xAddress[33], value); }
        public int X34Tile { get => Editor.GetWord(xAddress[34]); set => Editor.SetWord(xAddress[34], value); }
        public int X35Tile { get => Editor.GetWord(xAddress[35]); set => Editor.SetWord(xAddress[35], value); }
        public int X36Tile { get => Editor.GetWord(xAddress[36]); set => Editor.SetWord(xAddress[36], value); }
        public int X37Tile { get => Editor.GetWord(xAddress[37]); set => Editor.SetWord(xAddress[37], value); }
        public int X38Tile { get => Editor.GetWord(xAddress[38]); set => Editor.SetWord(xAddress[38], value); }
        public int X39Tile { get => Editor.GetWord(xAddress[39]); set => Editor.SetWord(xAddress[39], value); }
        public int X40Tile { get => Editor.GetWord(xAddress[40]); set => Editor.SetWord(xAddress[40], value); }
        public int X41Tile { get => Editor.GetWord(xAddress[41]); set => Editor.SetWord(xAddress[41], value); }
        public int X42Tile { get => Editor.GetWord(xAddress[42]); set => Editor.SetWord(xAddress[42], value); }
        public int X43Tile { get => Editor.GetWord(xAddress[43]); set => Editor.SetWord(xAddress[43], value); }
        public int X44Tile { get => Editor.GetWord(xAddress[44]); set => Editor.SetWord(xAddress[44], value); }
        public int X45Tile { get => Editor.GetWord(xAddress[45]); set => Editor.SetWord(xAddress[45], value); }
        public int X46Tile { get => Editor.GetWord(xAddress[46]); set => Editor.SetWord(xAddress[46], value); }
        public int X47Tile { get => Editor.GetWord(xAddress[47]); set => Editor.SetWord(xAddress[47], value); }
        public int X48Tile { get => Editor.GetWord(xAddress[48]); set => Editor.SetWord(xAddress[48], value); }
        public int X49Tile { get => Editor.GetWord(xAddress[49]); set => Editor.SetWord(xAddress[49], value); }
        public int X50Tile { get => Editor.GetWord(xAddress[50]); set => Editor.SetWord(xAddress[50], value); }
        public int X51Tile { get => Editor.GetWord(xAddress[51]); set => Editor.SetWord(xAddress[51], value); }
        public int X52Tile { get => Editor.GetWord(xAddress[52]); set => Editor.SetWord(xAddress[52], value); }
        public int X53Tile { get => Editor.GetWord(xAddress[53]); set => Editor.SetWord(xAddress[53], value); }
        public int X54Tile { get => Editor.GetWord(xAddress[54]); set => Editor.SetWord(xAddress[54], value); }
        public int X55Tile { get => Editor.GetWord(xAddress[55]); set => Editor.SetWord(xAddress[55], value); }
        public int X56Tile { get => Editor.GetWord(xAddress[56]); set => Editor.SetWord(xAddress[56], value); }
        public int X57Tile { get => Editor.GetWord(xAddress[57]); set => Editor.SetWord(xAddress[57], value); }
        public int X58Tile { get => Editor.GetWord(xAddress[58]); set => Editor.SetWord(xAddress[58], value); }
        public int X59Tile { get => Editor.GetWord(xAddress[59]); set => Editor.SetWord(xAddress[59], value); }
        public int X60Tile { get => Editor.GetWord(xAddress[60]); set => Editor.SetWord(xAddress[60], value); }
        public int X61Tile { get => Editor.GetWord(xAddress[61]); set => Editor.SetWord(xAddress[61], value); }
        public int X62Tile { get => Editor.GetWord(xAddress[62]); set => Editor.SetWord(xAddress[62], value); }
        public int X63Tile { get => Editor.GetWord(xAddress[63]); set => Editor.SetWord(xAddress[63], value); }
    }
}
