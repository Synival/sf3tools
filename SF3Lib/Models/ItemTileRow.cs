using SF3.FileEditors;

namespace SF3.Models {
    public class ItemTileRow : Model {
        private readonly int[] xAddress = new int[64];

        public ItemTileRow(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 64) {
            for (int i = 0; i < xAddress.Length; i++)
                xAddress[i] = Address + i;
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 64 individual properties.
        public int X0Treasure  { get => Editor.GetByte(xAddress[ 0]); set => Editor.SetByte(xAddress[ 0], (byte) value); }
        public int X1Treasure  { get => Editor.GetByte(xAddress[ 1]); set => Editor.SetByte(xAddress[ 1], (byte) value); }
        public int X2Treasure  { get => Editor.GetByte(xAddress[ 2]); set => Editor.SetByte(xAddress[ 2], (byte) value); }
        public int X3Treasure  { get => Editor.GetByte(xAddress[ 3]); set => Editor.SetByte(xAddress[ 3], (byte) value); }
        public int X4Treasure  { get => Editor.GetByte(xAddress[ 4]); set => Editor.SetByte(xAddress[ 4], (byte) value); }
        public int X5Treasure  { get => Editor.GetByte(xAddress[ 5]); set => Editor.SetByte(xAddress[ 5], (byte) value); }
        public int X6Treasure  { get => Editor.GetByte(xAddress[ 6]); set => Editor.SetByte(xAddress[ 6], (byte) value); }
        public int X7Treasure  { get => Editor.GetByte(xAddress[ 7]); set => Editor.SetByte(xAddress[ 7], (byte) value); }
        public int X8Treasure  { get => Editor.GetByte(xAddress[ 8]); set => Editor.SetByte(xAddress[ 8], (byte) value); }
        public int X9Treasure  { get => Editor.GetByte(xAddress[ 9]); set => Editor.SetByte(xAddress[ 9], (byte) value); }
        public int X10Treasure { get => Editor.GetByte(xAddress[10]); set => Editor.SetByte(xAddress[10], (byte) value); }
        public int X11Treasure { get => Editor.GetByte(xAddress[11]); set => Editor.SetByte(xAddress[11], (byte) value); }
        public int X12Treasure { get => Editor.GetByte(xAddress[12]); set => Editor.SetByte(xAddress[12], (byte) value); }
        public int X13Treasure { get => Editor.GetByte(xAddress[13]); set => Editor.SetByte(xAddress[13], (byte) value); }
        public int X14Treasure { get => Editor.GetByte(xAddress[14]); set => Editor.SetByte(xAddress[14], (byte) value); }
        public int X15Treasure { get => Editor.GetByte(xAddress[15]); set => Editor.SetByte(xAddress[15], (byte) value); }
        public int X16Treasure { get => Editor.GetByte(xAddress[16]); set => Editor.SetByte(xAddress[16], (byte) value); }
        public int X17Treasure { get => Editor.GetByte(xAddress[17]); set => Editor.SetByte(xAddress[17], (byte) value); }
        public int X18Treasure { get => Editor.GetByte(xAddress[18]); set => Editor.SetByte(xAddress[18], (byte) value); }
        public int X19Treasure { get => Editor.GetByte(xAddress[19]); set => Editor.SetByte(xAddress[19], (byte) value); }
        public int X20Treasure { get => Editor.GetByte(xAddress[20]); set => Editor.SetByte(xAddress[20], (byte) value); }
        public int X21Treasure { get => Editor.GetByte(xAddress[21]); set => Editor.SetByte(xAddress[21], (byte) value); }
        public int X22Treasure { get => Editor.GetByte(xAddress[22]); set => Editor.SetByte(xAddress[22], (byte) value); }
        public int X23Treasure { get => Editor.GetByte(xAddress[23]); set => Editor.SetByte(xAddress[23], (byte) value); }
        public int X24Treasure { get => Editor.GetByte(xAddress[24]); set => Editor.SetByte(xAddress[24], (byte) value); }
        public int X25Treasure { get => Editor.GetByte(xAddress[25]); set => Editor.SetByte(xAddress[25], (byte) value); }
        public int X26Treasure { get => Editor.GetByte(xAddress[26]); set => Editor.SetByte(xAddress[26], (byte) value); }
        public int X27Treasure { get => Editor.GetByte(xAddress[27]); set => Editor.SetByte(xAddress[27], (byte) value); }
        public int X28Treasure { get => Editor.GetByte(xAddress[28]); set => Editor.SetByte(xAddress[28], (byte) value); }
        public int X29Treasure { get => Editor.GetByte(xAddress[29]); set => Editor.SetByte(xAddress[29], (byte) value); }
        public int X30Treasure { get => Editor.GetByte(xAddress[30]); set => Editor.SetByte(xAddress[30], (byte) value); }
        public int X31Treasure { get => Editor.GetByte(xAddress[31]); set => Editor.SetByte(xAddress[31], (byte) value); }
        public int X32Treasure { get => Editor.GetByte(xAddress[32]); set => Editor.SetByte(xAddress[32], (byte) value); }
        public int X33Treasure { get => Editor.GetByte(xAddress[33]); set => Editor.SetByte(xAddress[33], (byte) value); }
        public int X34Treasure { get => Editor.GetByte(xAddress[34]); set => Editor.SetByte(xAddress[34], (byte) value); }
        public int X35Treasure { get => Editor.GetByte(xAddress[35]); set => Editor.SetByte(xAddress[35], (byte) value); }
        public int X36Treasure { get => Editor.GetByte(xAddress[36]); set => Editor.SetByte(xAddress[36], (byte) value); }
        public int X37Treasure { get => Editor.GetByte(xAddress[37]); set => Editor.SetByte(xAddress[37], (byte) value); }
        public int X38Treasure { get => Editor.GetByte(xAddress[38]); set => Editor.SetByte(xAddress[38], (byte) value); }
        public int X39Treasure { get => Editor.GetByte(xAddress[39]); set => Editor.SetByte(xAddress[39], (byte) value); }
        public int X40Treasure { get => Editor.GetByte(xAddress[40]); set => Editor.SetByte(xAddress[40], (byte) value); }
        public int X41Treasure { get => Editor.GetByte(xAddress[41]); set => Editor.SetByte(xAddress[41], (byte) value); }
        public int X42Treasure { get => Editor.GetByte(xAddress[42]); set => Editor.SetByte(xAddress[42], (byte) value); }
        public int X43Treasure { get => Editor.GetByte(xAddress[43]); set => Editor.SetByte(xAddress[43], (byte) value); }
        public int X44Treasure { get => Editor.GetByte(xAddress[44]); set => Editor.SetByte(xAddress[44], (byte) value); }
        public int X45Treasure { get => Editor.GetByte(xAddress[45]); set => Editor.SetByte(xAddress[45], (byte) value); }
        public int X46Treasure { get => Editor.GetByte(xAddress[46]); set => Editor.SetByte(xAddress[46], (byte) value); }
        public int X47Treasure { get => Editor.GetByte(xAddress[47]); set => Editor.SetByte(xAddress[47], (byte) value); }
        public int X48Treasure { get => Editor.GetByte(xAddress[48]); set => Editor.SetByte(xAddress[48], (byte) value); }
        public int X49Treasure { get => Editor.GetByte(xAddress[49]); set => Editor.SetByte(xAddress[49], (byte) value); }
        public int X50Treasure { get => Editor.GetByte(xAddress[50]); set => Editor.SetByte(xAddress[50], (byte) value); }
        public int X51Treasure { get => Editor.GetByte(xAddress[51]); set => Editor.SetByte(xAddress[51], (byte) value); }
        public int X52Treasure { get => Editor.GetByte(xAddress[52]); set => Editor.SetByte(xAddress[52], (byte) value); }
        public int X53Treasure { get => Editor.GetByte(xAddress[53]); set => Editor.SetByte(xAddress[53], (byte) value); }
        public int X54Treasure { get => Editor.GetByte(xAddress[54]); set => Editor.SetByte(xAddress[54], (byte) value); }
        public int X55Treasure { get => Editor.GetByte(xAddress[55]); set => Editor.SetByte(xAddress[55], (byte) value); }
        public int X56Treasure { get => Editor.GetByte(xAddress[56]); set => Editor.SetByte(xAddress[56], (byte) value); }
        public int X57Treasure { get => Editor.GetByte(xAddress[57]); set => Editor.SetByte(xAddress[57], (byte) value); }
        public int X58Treasure { get => Editor.GetByte(xAddress[58]); set => Editor.SetByte(xAddress[58], (byte) value); }
        public int X59Treasure { get => Editor.GetByte(xAddress[59]); set => Editor.SetByte(xAddress[59], (byte) value); }
        public int X60Treasure { get => Editor.GetByte(xAddress[60]); set => Editor.SetByte(xAddress[60], (byte) value); }
        public int X61Treasure { get => Editor.GetByte(xAddress[61]); set => Editor.SetByte(xAddress[61], (byte) value); }
        public int X62Treasure { get => Editor.GetByte(xAddress[62]); set => Editor.SetByte(xAddress[62], (byte) value); }
        public int X63Treasure { get => Editor.GetByte(xAddress[63]); set => Editor.SetByte(xAddress[63], (byte) value); }
    }
}
