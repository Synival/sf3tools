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

        public ushort this[int index] {
            get => Data.GetUInt16(xAddress[index]);
            set => Data.SetUInt16(xAddress[index], value);
        }

        public byte GetHeight(int x)
            => (byte) ((this[x] >> 8) & 0xFF);
        public void SetHeight(int x, byte value)
            => this[x] = (ushort) ((this[x] & 0xFF) + (value << 8));

        public TerrainType GetTerrainType(int x)
            => (TerrainType) (this[x] & 0x0F);
        public void SetTerrainType(int x, TerrainType value)
            => this[x] = (ushort) ((this[x] & 0xFFF0) + (byte) ((byte) value & 0x0F));

        public TerrainFlags GetTerrainFlags(int x)
            => (TerrainFlags) ((this[x] & 0xF0) >> 4);
        public void SetTerrainFlags(int x, TerrainFlags flags)
            => this[x] = (ushort) ((this[x] & 0xFF0F) + (byte) (((byte) flags & 0x0F) << 4));

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            // TODO: address!
            public TileMetadataAttribute(int x) : base(addressField: null, displayName: "X" + x.ToString("D2"), displayOrder: x, displayFormat: "X4", minWidth: 50) { }
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 64 individual properties.
        [TileMetadata(0)] public ushort X0Tile { get => Data.GetUInt16(xAddress[0]); set => Data.SetUInt16(xAddress[0], value); }
        [TileMetadata(1)] public ushort X1Tile { get => Data.GetUInt16(xAddress[1]); set => Data.SetUInt16(xAddress[1], value); }
        [TileMetadata(2)] public ushort X2Tile { get => Data.GetUInt16(xAddress[2]); set => Data.SetUInt16(xAddress[2], value); }
        [TileMetadata(3)] public ushort X3Tile { get => Data.GetUInt16(xAddress[3]); set => Data.SetUInt16(xAddress[3], value); }
        [TileMetadata(4)] public ushort X4Tile { get => Data.GetUInt16(xAddress[4]); set => Data.SetUInt16(xAddress[4], value); }
        [TileMetadata(5)] public ushort X5Tile { get => Data.GetUInt16(xAddress[5]); set => Data.SetUInt16(xAddress[5], value); }
        [TileMetadata(6)] public ushort X6Tile { get => Data.GetUInt16(xAddress[6]); set => Data.SetUInt16(xAddress[6], value); }
        [TileMetadata(7)] public ushort X7Tile { get => Data.GetUInt16(xAddress[7]); set => Data.SetUInt16(xAddress[7], value); }
        [TileMetadata(8)] public ushort X8Tile { get => Data.GetUInt16(xAddress[8]); set => Data.SetUInt16(xAddress[8], value); }
        [TileMetadata(9)] public ushort X9Tile { get => Data.GetUInt16(xAddress[9]); set => Data.SetUInt16(xAddress[9], value); }
        [TileMetadata(10)] public ushort X10Tile { get => Data.GetUInt16(xAddress[10]); set => Data.SetUInt16(xAddress[10], value); }
        [TileMetadata(11)] public ushort X11Tile { get => Data.GetUInt16(xAddress[11]); set => Data.SetUInt16(xAddress[11], value); }
        [TileMetadata(12)] public ushort X12Tile { get => Data.GetUInt16(xAddress[12]); set => Data.SetUInt16(xAddress[12], value); }
        [TileMetadata(13)] public ushort X13Tile { get => Data.GetUInt16(xAddress[13]); set => Data.SetUInt16(xAddress[13], value); }
        [TileMetadata(14)] public ushort X14Tile { get => Data.GetUInt16(xAddress[14]); set => Data.SetUInt16(xAddress[14], value); }
        [TileMetadata(15)] public ushort X15Tile { get => Data.GetUInt16(xAddress[15]); set => Data.SetUInt16(xAddress[15], value); }
        [TileMetadata(16)] public ushort X16Tile { get => Data.GetUInt16(xAddress[16]); set => Data.SetUInt16(xAddress[16], value); }
        [TileMetadata(17)] public ushort X17Tile { get => Data.GetUInt16(xAddress[17]); set => Data.SetUInt16(xAddress[17], value); }
        [TileMetadata(18)] public ushort X18Tile { get => Data.GetUInt16(xAddress[18]); set => Data.SetUInt16(xAddress[18], value); }
        [TileMetadata(19)] public ushort X19Tile { get => Data.GetUInt16(xAddress[19]); set => Data.SetUInt16(xAddress[19], value); }
        [TileMetadata(20)] public ushort X20Tile { get => Data.GetUInt16(xAddress[20]); set => Data.SetUInt16(xAddress[20], value); }
        [TileMetadata(21)] public ushort X21Tile { get => Data.GetUInt16(xAddress[21]); set => Data.SetUInt16(xAddress[21], value); }
        [TileMetadata(22)] public ushort X22Tile { get => Data.GetUInt16(xAddress[22]); set => Data.SetUInt16(xAddress[22], value); }
        [TileMetadata(23)] public ushort X23Tile { get => Data.GetUInt16(xAddress[23]); set => Data.SetUInt16(xAddress[23], value); }
        [TileMetadata(24)] public ushort X24Tile { get => Data.GetUInt16(xAddress[24]); set => Data.SetUInt16(xAddress[24], value); }
        [TileMetadata(25)] public ushort X25Tile { get => Data.GetUInt16(xAddress[25]); set => Data.SetUInt16(xAddress[25], value); }
        [TileMetadata(26)] public ushort X26Tile { get => Data.GetUInt16(xAddress[26]); set => Data.SetUInt16(xAddress[26], value); }
        [TileMetadata(27)] public ushort X27Tile { get => Data.GetUInt16(xAddress[27]); set => Data.SetUInt16(xAddress[27], value); }
        [TileMetadata(28)] public ushort X28Tile { get => Data.GetUInt16(xAddress[28]); set => Data.SetUInt16(xAddress[28], value); }
        [TileMetadata(29)] public ushort X29Tile { get => Data.GetUInt16(xAddress[29]); set => Data.SetUInt16(xAddress[29], value); }
        [TileMetadata(30)] public ushort X30Tile { get => Data.GetUInt16(xAddress[30]); set => Data.SetUInt16(xAddress[30], value); }
        [TileMetadata(31)] public ushort X31Tile { get => Data.GetUInt16(xAddress[31]); set => Data.SetUInt16(xAddress[31], value); }
        [TileMetadata(32)] public ushort X32Tile { get => Data.GetUInt16(xAddress[32]); set => Data.SetUInt16(xAddress[32], value); }
        [TileMetadata(33)] public ushort X33Tile { get => Data.GetUInt16(xAddress[33]); set => Data.SetUInt16(xAddress[33], value); }
        [TileMetadata(34)] public ushort X34Tile { get => Data.GetUInt16(xAddress[34]); set => Data.SetUInt16(xAddress[34], value); }
        [TileMetadata(35)] public ushort X35Tile { get => Data.GetUInt16(xAddress[35]); set => Data.SetUInt16(xAddress[35], value); }
        [TileMetadata(36)] public ushort X36Tile { get => Data.GetUInt16(xAddress[36]); set => Data.SetUInt16(xAddress[36], value); }
        [TileMetadata(37)] public ushort X37Tile { get => Data.GetUInt16(xAddress[37]); set => Data.SetUInt16(xAddress[37], value); }
        [TileMetadata(38)] public ushort X38Tile { get => Data.GetUInt16(xAddress[38]); set => Data.SetUInt16(xAddress[38], value); }
        [TileMetadata(39)] public ushort X39Tile { get => Data.GetUInt16(xAddress[39]); set => Data.SetUInt16(xAddress[39], value); }
        [TileMetadata(40)] public ushort X40Tile { get => Data.GetUInt16(xAddress[40]); set => Data.SetUInt16(xAddress[40], value); }
        [TileMetadata(41)] public ushort X41Tile { get => Data.GetUInt16(xAddress[41]); set => Data.SetUInt16(xAddress[41], value); }
        [TileMetadata(42)] public ushort X42Tile { get => Data.GetUInt16(xAddress[42]); set => Data.SetUInt16(xAddress[42], value); }
        [TileMetadata(43)] public ushort X43Tile { get => Data.GetUInt16(xAddress[43]); set => Data.SetUInt16(xAddress[43], value); }
        [TileMetadata(44)] public ushort X44Tile { get => Data.GetUInt16(xAddress[44]); set => Data.SetUInt16(xAddress[44], value); }
        [TileMetadata(45)] public ushort X45Tile { get => Data.GetUInt16(xAddress[45]); set => Data.SetUInt16(xAddress[45], value); }
        [TileMetadata(46)] public ushort X46Tile { get => Data.GetUInt16(xAddress[46]); set => Data.SetUInt16(xAddress[46], value); }
        [TileMetadata(47)] public ushort X47Tile { get => Data.GetUInt16(xAddress[47]); set => Data.SetUInt16(xAddress[47], value); }
        [TileMetadata(48)] public ushort X48Tile { get => Data.GetUInt16(xAddress[48]); set => Data.SetUInt16(xAddress[48], value); }
        [TileMetadata(49)] public ushort X49Tile { get => Data.GetUInt16(xAddress[49]); set => Data.SetUInt16(xAddress[49], value); }
        [TileMetadata(50)] public ushort X50Tile { get => Data.GetUInt16(xAddress[50]); set => Data.SetUInt16(xAddress[50], value); }
        [TileMetadata(51)] public ushort X51Tile { get => Data.GetUInt16(xAddress[51]); set => Data.SetUInt16(xAddress[51], value); }
        [TileMetadata(52)] public ushort X52Tile { get => Data.GetUInt16(xAddress[52]); set => Data.SetUInt16(xAddress[52], value); }
        [TileMetadata(53)] public ushort X53Tile { get => Data.GetUInt16(xAddress[53]); set => Data.SetUInt16(xAddress[53], value); }
        [TileMetadata(54)] public ushort X54Tile { get => Data.GetUInt16(xAddress[54]); set => Data.SetUInt16(xAddress[54], value); }
        [TileMetadata(55)] public ushort X55Tile { get => Data.GetUInt16(xAddress[55]); set => Data.SetUInt16(xAddress[55], value); }
        [TileMetadata(56)] public ushort X56Tile { get => Data.GetUInt16(xAddress[56]); set => Data.SetUInt16(xAddress[56], value); }
        [TileMetadata(57)] public ushort X57Tile { get => Data.GetUInt16(xAddress[57]); set => Data.SetUInt16(xAddress[57], value); }
        [TileMetadata(58)] public ushort X58Tile { get => Data.GetUInt16(xAddress[58]); set => Data.SetUInt16(xAddress[58], value); }
        [TileMetadata(59)] public ushort X59Tile { get => Data.GetUInt16(xAddress[59]); set => Data.SetUInt16(xAddress[59], value); }
        [TileMetadata(60)] public ushort X60Tile { get => Data.GetUInt16(xAddress[60]); set => Data.SetUInt16(xAddress[60], value); }
        [TileMetadata(61)] public ushort X61Tile { get => Data.GetUInt16(xAddress[61]); set => Data.SetUInt16(xAddress[61], value); }
        [TileMetadata(62)] public ushort X62Tile { get => Data.GetUInt16(xAddress[62]); set => Data.SetUInt16(xAddress[62], value); }
        [TileMetadata(63)] public ushort X63Tile { get => Data.GetUInt16(xAddress[63]); set => Data.SetUInt16(xAddress[63], value); }
    }
}
