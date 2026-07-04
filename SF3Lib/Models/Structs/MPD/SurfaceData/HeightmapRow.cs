using System;
using CommonLib.Attributes;
using CommonLib.Types;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Surface {
    public class HeightmapRow : Struct {
        private readonly int[] xAddress = new int[64];

        public HeightmapRow(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 256) {
            for (var i = 0; i < xAddress.Length; i++)
                xAddress[i] = Address + i * 4;
        }

        public uint this[int index] {
            get => Data.GetUInt32(xAddress[index]);
            set => Data.SetUInt32(xAddress[index], value);
        }

        public byte GetHeight(int x, CornerType corner) {
            // Heights are clockwise from bottom-right.
            switch (corner) {
                case CornerType.BottomRight:
                    return Data.GetUInt8(xAddress[x] + 0);
                case CornerType.BottomLeft:
                    return Data.GetUInt8(xAddress[x] + 1);
                case CornerType.TopLeft:
                    return Data.GetUInt8(xAddress[x] + 2);
                case CornerType.TopRight:
                    return Data.GetUInt8(xAddress[x] + 3);
                default:
                    throw new ArgumentException(nameof(corner));
            }
        }

        public void SetHeight(int x, CornerType corner, byte value) {
            // Heights are clockwise from bottom-right.
            switch (corner) {
                case CornerType.BottomRight:
                    Data.SetUInt8(xAddress[x] + 0, value);
                    break;
                case CornerType.BottomLeft:
                    Data.SetUInt8(xAddress[x] + 1, value);
                    break;
                case CornerType.TopLeft:
                    Data.SetUInt8(xAddress[x] + 2, value);
                    break;
                case CornerType.TopRight:
                    Data.SetUInt8(xAddress[x] + 3, value);
                    break;
                default:
                    throw new ArgumentException(nameof(corner));
            }
        }

        public byte[] GetHeights(int x)
            => ConvertHeightsToByteArray(this[x]);

        public void SetHeights(int x, byte[] heights) {
            if (heights.Length != 4)
                throw new ArgumentException(nameof(heights));
            this[x] = ConvertByteArrayHeights(heights);
        }

        public static byte[] ConvertHeightsToByteArray(uint heights) {
            // Heights are clockwise from bottom-right.
            var byteHeights = new byte[4];
            byteHeights[(int) CornerType.BottomRight] = (byte) ((heights >> 24) & 0xFF);
            byteHeights[(int) CornerType.BottomLeft]  = (byte) ((heights >> 16) & 0xFF);
            byteHeights[(int) CornerType.TopLeft]     = (byte) ((heights >>  8) & 0xFF);
            byteHeights[(int) CornerType.TopRight]    = (byte) ((heights >>  0) & 0xFF);
            return byteHeights;
        }

        public static uint ConvertByteArrayHeights(byte[] heights) {
            if (heights.Length != 4)
                throw new ArgumentException(nameof(heights));

            // Heights are clockwise from bottom-right.
            return
                (((uint) heights[(int) CornerType.BottomRight]) << 24) +
                (((uint) heights[(int) CornerType.BottomLeft])  << 16) +
                (((uint) heights[(int) CornerType.TopLeft])     <<  8) +
                (((uint) heights[(int) CornerType.TopRight])    <<  0);
        }

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            // TODO: address!
            public TileMetadataAttribute(int x) : base(addressField: null, displayName: "X" + x.ToString("D2"), displayOrder: x, displayFormat: "X8", minWidth: 75) { }
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 64 individual properties.
        [TileMetadata(0)] public uint X0Tile { get => Data.GetUInt32(xAddress[0]); set => Data.SetUInt32(xAddress[0], value); }
        [TileMetadata(1)] public uint X1Tile { get => Data.GetUInt32(xAddress[1]); set => Data.SetUInt32(xAddress[1], value); }
        [TileMetadata(2)] public uint X2Tile { get => Data.GetUInt32(xAddress[2]); set => Data.SetUInt32(xAddress[2], value); }
        [TileMetadata(3)] public uint X3Tile { get => Data.GetUInt32(xAddress[3]); set => Data.SetUInt32(xAddress[3], value); }
        [TileMetadata(4)] public uint X4Tile { get => Data.GetUInt32(xAddress[4]); set => Data.SetUInt32(xAddress[4], value); }
        [TileMetadata(5)] public uint X5Tile { get => Data.GetUInt32(xAddress[5]); set => Data.SetUInt32(xAddress[5], value); }
        [TileMetadata(6)] public uint X6Tile { get => Data.GetUInt32(xAddress[6]); set => Data.SetUInt32(xAddress[6], value); }
        [TileMetadata(7)] public uint X7Tile { get => Data.GetUInt32(xAddress[7]); set => Data.SetUInt32(xAddress[7], value); }
        [TileMetadata(8)] public uint X8Tile { get => Data.GetUInt32(xAddress[8]); set => Data.SetUInt32(xAddress[8], value); }
        [TileMetadata(9)] public uint X9Tile { get => Data.GetUInt32(xAddress[9]); set => Data.SetUInt32(xAddress[9], value); }
        [TileMetadata(10)] public uint X10Tile { get => Data.GetUInt32(xAddress[10]); set => Data.SetUInt32(xAddress[10], value); }
        [TileMetadata(11)] public uint X11Tile { get => Data.GetUInt32(xAddress[11]); set => Data.SetUInt32(xAddress[11], value); }
        [TileMetadata(12)] public uint X12Tile { get => Data.GetUInt32(xAddress[12]); set => Data.SetUInt32(xAddress[12], value); }
        [TileMetadata(13)] public uint X13Tile { get => Data.GetUInt32(xAddress[13]); set => Data.SetUInt32(xAddress[13], value); }
        [TileMetadata(14)] public uint X14Tile { get => Data.GetUInt32(xAddress[14]); set => Data.SetUInt32(xAddress[14], value); }
        [TileMetadata(15)] public uint X15Tile { get => Data.GetUInt32(xAddress[15]); set => Data.SetUInt32(xAddress[15], value); }
        [TileMetadata(16)] public uint X16Tile { get => Data.GetUInt32(xAddress[16]); set => Data.SetUInt32(xAddress[16], value); }
        [TileMetadata(17)] public uint X17Tile { get => Data.GetUInt32(xAddress[17]); set => Data.SetUInt32(xAddress[17], value); }
        [TileMetadata(18)] public uint X18Tile { get => Data.GetUInt32(xAddress[18]); set => Data.SetUInt32(xAddress[18], value); }
        [TileMetadata(19)] public uint X19Tile { get => Data.GetUInt32(xAddress[19]); set => Data.SetUInt32(xAddress[19], value); }
        [TileMetadata(20)] public uint X20Tile { get => Data.GetUInt32(xAddress[20]); set => Data.SetUInt32(xAddress[20], value); }
        [TileMetadata(21)] public uint X21Tile { get => Data.GetUInt32(xAddress[21]); set => Data.SetUInt32(xAddress[21], value); }
        [TileMetadata(22)] public uint X22Tile { get => Data.GetUInt32(xAddress[22]); set => Data.SetUInt32(xAddress[22], value); }
        [TileMetadata(23)] public uint X23Tile { get => Data.GetUInt32(xAddress[23]); set => Data.SetUInt32(xAddress[23], value); }
        [TileMetadata(24)] public uint X24Tile { get => Data.GetUInt32(xAddress[24]); set => Data.SetUInt32(xAddress[24], value); }
        [TileMetadata(25)] public uint X25Tile { get => Data.GetUInt32(xAddress[25]); set => Data.SetUInt32(xAddress[25], value); }
        [TileMetadata(26)] public uint X26Tile { get => Data.GetUInt32(xAddress[26]); set => Data.SetUInt32(xAddress[26], value); }
        [TileMetadata(27)] public uint X27Tile { get => Data.GetUInt32(xAddress[27]); set => Data.SetUInt32(xAddress[27], value); }
        [TileMetadata(28)] public uint X28Tile { get => Data.GetUInt32(xAddress[28]); set => Data.SetUInt32(xAddress[28], value); }
        [TileMetadata(29)] public uint X29Tile { get => Data.GetUInt32(xAddress[29]); set => Data.SetUInt32(xAddress[29], value); }
        [TileMetadata(30)] public uint X30Tile { get => Data.GetUInt32(xAddress[30]); set => Data.SetUInt32(xAddress[30], value); }
        [TileMetadata(31)] public uint X31Tile { get => Data.GetUInt32(xAddress[31]); set => Data.SetUInt32(xAddress[31], value); }
        [TileMetadata(32)] public uint X32Tile { get => Data.GetUInt32(xAddress[32]); set => Data.SetUInt32(xAddress[32], value); }
        [TileMetadata(33)] public uint X33Tile { get => Data.GetUInt32(xAddress[33]); set => Data.SetUInt32(xAddress[33], value); }
        [TileMetadata(34)] public uint X34Tile { get => Data.GetUInt32(xAddress[34]); set => Data.SetUInt32(xAddress[34], value); }
        [TileMetadata(35)] public uint X35Tile { get => Data.GetUInt32(xAddress[35]); set => Data.SetUInt32(xAddress[35], value); }
        [TileMetadata(36)] public uint X36Tile { get => Data.GetUInt32(xAddress[36]); set => Data.SetUInt32(xAddress[36], value); }
        [TileMetadata(37)] public uint X37Tile { get => Data.GetUInt32(xAddress[37]); set => Data.SetUInt32(xAddress[37], value); }
        [TileMetadata(38)] public uint X38Tile { get => Data.GetUInt32(xAddress[38]); set => Data.SetUInt32(xAddress[38], value); }
        [TileMetadata(39)] public uint X39Tile { get => Data.GetUInt32(xAddress[39]); set => Data.SetUInt32(xAddress[39], value); }
        [TileMetadata(40)] public uint X40Tile { get => Data.GetUInt32(xAddress[40]); set => Data.SetUInt32(xAddress[40], value); }
        [TileMetadata(41)] public uint X41Tile { get => Data.GetUInt32(xAddress[41]); set => Data.SetUInt32(xAddress[41], value); }
        [TileMetadata(42)] public uint X42Tile { get => Data.GetUInt32(xAddress[42]); set => Data.SetUInt32(xAddress[42], value); }
        [TileMetadata(43)] public uint X43Tile { get => Data.GetUInt32(xAddress[43]); set => Data.SetUInt32(xAddress[43], value); }
        [TileMetadata(44)] public uint X44Tile { get => Data.GetUInt32(xAddress[44]); set => Data.SetUInt32(xAddress[44], value); }
        [TileMetadata(45)] public uint X45Tile { get => Data.GetUInt32(xAddress[45]); set => Data.SetUInt32(xAddress[45], value); }
        [TileMetadata(46)] public uint X46Tile { get => Data.GetUInt32(xAddress[46]); set => Data.SetUInt32(xAddress[46], value); }
        [TileMetadata(47)] public uint X47Tile { get => Data.GetUInt32(xAddress[47]); set => Data.SetUInt32(xAddress[47], value); }
        [TileMetadata(48)] public uint X48Tile { get => Data.GetUInt32(xAddress[48]); set => Data.SetUInt32(xAddress[48], value); }
        [TileMetadata(49)] public uint X49Tile { get => Data.GetUInt32(xAddress[49]); set => Data.SetUInt32(xAddress[49], value); }
        [TileMetadata(50)] public uint X50Tile { get => Data.GetUInt32(xAddress[50]); set => Data.SetUInt32(xAddress[50], value); }
        [TileMetadata(51)] public uint X51Tile { get => Data.GetUInt32(xAddress[51]); set => Data.SetUInt32(xAddress[51], value); }
        [TileMetadata(52)] public uint X52Tile { get => Data.GetUInt32(xAddress[52]); set => Data.SetUInt32(xAddress[52], value); }
        [TileMetadata(53)] public uint X53Tile { get => Data.GetUInt32(xAddress[53]); set => Data.SetUInt32(xAddress[53], value); }
        [TileMetadata(54)] public uint X54Tile { get => Data.GetUInt32(xAddress[54]); set => Data.SetUInt32(xAddress[54], value); }
        [TileMetadata(55)] public uint X55Tile { get => Data.GetUInt32(xAddress[55]); set => Data.SetUInt32(xAddress[55], value); }
        [TileMetadata(56)] public uint X56Tile { get => Data.GetUInt32(xAddress[56]); set => Data.SetUInt32(xAddress[56], value); }
        [TileMetadata(57)] public uint X57Tile { get => Data.GetUInt32(xAddress[57]); set => Data.SetUInt32(xAddress[57], value); }
        [TileMetadata(58)] public uint X58Tile { get => Data.GetUInt32(xAddress[58]); set => Data.SetUInt32(xAddress[58], value); }
        [TileMetadata(59)] public uint X59Tile { get => Data.GetUInt32(xAddress[59]); set => Data.SetUInt32(xAddress[59], value); }
        [TileMetadata(60)] public uint X60Tile { get => Data.GetUInt32(xAddress[60]); set => Data.SetUInt32(xAddress[60], value); }
        [TileMetadata(61)] public uint X61Tile { get => Data.GetUInt32(xAddress[61]); set => Data.SetUInt32(xAddress[61], value); }
        [TileMetadata(62)] public uint X62Tile { get => Data.GetUInt32(xAddress[62]); set => Data.SetUInt32(xAddress[62], value); }
        [TileMetadata(63)] public uint X63Tile { get => Data.GetUInt32(xAddress[63]); set => Data.SetUInt32(xAddress[63], value); }
    }
}
