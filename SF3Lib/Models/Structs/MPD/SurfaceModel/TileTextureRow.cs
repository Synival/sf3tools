using System;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.SurfaceModel {
    public class TileTextureRow : Struct {
        private readonly int[] xAddress = new int[64];

        public TileTextureRow(IByteData data, int id, string name, int address, bool hasRotation)
        : base(data, id, name, address, 128) {
            HasRotation = hasRotation;

            for (var i = 0; i < xAddress.Length; i++) {
                var block = i / 4;
                var x = i % 4;
                xAddress[i] = Address + (block * 16 + x) * 2;
            }
        }

        public ushort[] GetRowCopy() {
            // TODO: somehow cache this?
            var tiles = new ushort[64];
            for (var i = 0; i < tiles.Length; i++)
                tiles[i] = (ushort) Data.GetWord(xAddress[i]);
            return tiles;
        }

        public byte GetTextureFlags(int x)
            => (byte) (this[x] >> 8 & 0xFF);
        public void SetTextureFlags(int x, byte value)
            => this[x] = (ushort) ((this[x] & 0xFF) + (value << 8));

        public byte GetTextureID(int x)
            => (byte) (this[x] & 0xFF);
        public void SetTextureID(int x, byte value)
            => this[x] = (ushort) ((this[x] & 0xFF00) + value);

        public TextureRotateType GetRotate(int x)
            => HasRotation ? (TextureRotateType) (GetTextureFlags(x) & 0x03) : 0x00;
        public void SetRotate(int x, TextureRotateType value) {
            if (HasRotation)
                SetTextureFlags(x, (byte) (GetTextureFlags(x) & ~0x03 | (byte) value));
        }

        public TextureFlipType GetFlip(int x)
            => (TextureFlipType) (GetTextureFlags(x) & 0x30);
        public void SetFlip(int x, TextureFlipType value)
            => SetTextureFlags(x, (byte) (GetTextureFlags(x) & ~0x30 | (byte) value));

        public bool GetIsFlatFlag(int x)
            => (GetTextureFlags(x) & 0x80) == 0x80;
        public void SetIsFlatFlag(int x, bool value)
            => SetTextureFlags(x, (byte) (GetTextureFlags(x) & ~0x80 | (value ? 0x80 : 0x00)));

        public ushort this[int index] {
            get => (ushort) Data.GetWord(xAddress[index]);
            set => Data.SetWord(xAddress[index], value);
        }

        public bool HasRotation { get; }

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            // TODO: null
            public TileMetadataAttribute(int x) : base(addressField: null, displayName: "X" + x.ToString("D2"), displayOrder: x, displayFormat: "X4", minWidth: 50) { }
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 64 individual properties.
        [TileMetadata( 0)] public ushort X00Tile { get => this[ 0]; set => this[ 0] = value; }
        [TileMetadata( 1)] public ushort X01Tile { get => this[ 1]; set => this[ 1] = value; }
        [TileMetadata( 2)] public ushort X02Tile { get => this[ 2]; set => this[ 2] = value; }
        [TileMetadata( 3)] public ushort X03Tile { get => this[ 3]; set => this[ 3] = value; }
        [TileMetadata( 4)] public ushort X04Tile { get => this[ 4]; set => this[ 4] = value; }
        [TileMetadata( 5)] public ushort X05Tile { get => this[ 5]; set => this[ 5] = value; }
        [TileMetadata( 6)] public ushort X06Tile { get => this[ 6]; set => this[ 6] = value; }
        [TileMetadata( 7)] public ushort X07Tile { get => this[ 7]; set => this[ 7] = value; }
        [TileMetadata( 8)] public ushort X08Tile { get => this[ 8]; set => this[ 8] = value; }
        [TileMetadata( 9)] public ushort X09Tile { get => this[ 9]; set => this[ 9] = value; }
        [TileMetadata(10)] public ushort X10Tile { get => this[10]; set => this[10] = value; }
        [TileMetadata(11)] public ushort X11Tile { get => this[11]; set => this[11] = value; }
        [TileMetadata(12)] public ushort X12Tile { get => this[12]; set => this[12] = value; }
        [TileMetadata(13)] public ushort X13Tile { get => this[13]; set => this[13] = value; }
        [TileMetadata(14)] public ushort X14Tile { get => this[14]; set => this[14] = value; }
        [TileMetadata(15)] public ushort X15Tile { get => this[15]; set => this[15] = value; }
        [TileMetadata(16)] public ushort X16Tile { get => this[16]; set => this[16] = value; }
        [TileMetadata(17)] public ushort X17Tile { get => this[17]; set => this[17] = value; }
        [TileMetadata(18)] public ushort X18Tile { get => this[18]; set => this[18] = value; }
        [TileMetadata(19)] public ushort X19Tile { get => this[19]; set => this[19] = value; }
        [TileMetadata(20)] public ushort X20Tile { get => this[20]; set => this[20] = value; }
        [TileMetadata(21)] public ushort X21Tile { get => this[21]; set => this[21] = value; }
        [TileMetadata(22)] public ushort X22Tile { get => this[22]; set => this[22] = value; }
        [TileMetadata(23)] public ushort X23Tile { get => this[23]; set => this[23] = value; }
        [TileMetadata(24)] public ushort X24Tile { get => this[24]; set => this[24] = value; }
        [TileMetadata(25)] public ushort X25Tile { get => this[25]; set => this[25] = value; }
        [TileMetadata(26)] public ushort X26Tile { get => this[26]; set => this[26] = value; }
        [TileMetadata(27)] public ushort X27Tile { get => this[27]; set => this[27] = value; }
        [TileMetadata(28)] public ushort X28Tile { get => this[28]; set => this[28] = value; }
        [TileMetadata(29)] public ushort X29Tile { get => this[29]; set => this[29] = value; }
        [TileMetadata(30)] public ushort X30Tile { get => this[30]; set => this[30] = value; }
        [TileMetadata(31)] public ushort X31Tile { get => this[31]; set => this[31] = value; }
        [TileMetadata(32)] public ushort X32Tile { get => this[32]; set => this[32] = value; }
        [TileMetadata(33)] public ushort X33Tile { get => this[33]; set => this[33] = value; }
        [TileMetadata(34)] public ushort X34Tile { get => this[34]; set => this[34] = value; }
        [TileMetadata(35)] public ushort X35Tile { get => this[35]; set => this[35] = value; }
        [TileMetadata(36)] public ushort X36Tile { get => this[36]; set => this[36] = value; }
        [TileMetadata(37)] public ushort X37Tile { get => this[37]; set => this[37] = value; }
        [TileMetadata(38)] public ushort X38Tile { get => this[38]; set => this[38] = value; }
        [TileMetadata(39)] public ushort X39Tile { get => this[39]; set => this[39] = value; }
        [TileMetadata(40)] public ushort X40Tile { get => this[40]; set => this[40] = value; }
        [TileMetadata(41)] public ushort X41Tile { get => this[41]; set => this[41] = value; }
        [TileMetadata(42)] public ushort X42Tile { get => this[42]; set => this[42] = value; }
        [TileMetadata(43)] public ushort X43Tile { get => this[43]; set => this[43] = value; }
        [TileMetadata(44)] public ushort X44Tile { get => this[44]; set => this[44] = value; }
        [TileMetadata(45)] public ushort X45Tile { get => this[45]; set => this[45] = value; }
        [TileMetadata(46)] public ushort X46Tile { get => this[46]; set => this[46] = value; }
        [TileMetadata(47)] public ushort X47Tile { get => this[47]; set => this[47] = value; }
        [TileMetadata(48)] public ushort X48Tile { get => this[48]; set => this[48] = value; }
        [TileMetadata(49)] public ushort X49Tile { get => this[49]; set => this[49] = value; }
        [TileMetadata(50)] public ushort X50Tile { get => this[50]; set => this[50] = value; }
        [TileMetadata(51)] public ushort X51Tile { get => this[51]; set => this[51] = value; }
        [TileMetadata(52)] public ushort X52Tile { get => this[52]; set => this[52] = value; }
        [TileMetadata(53)] public ushort X53Tile { get => this[53]; set => this[53] = value; }
        [TileMetadata(54)] public ushort X54Tile { get => this[54]; set => this[54] = value; }
        [TileMetadata(55)] public ushort X55Tile { get => this[55]; set => this[55] = value; }
        [TileMetadata(56)] public ushort X56Tile { get => this[56]; set => this[56] = value; }
        [TileMetadata(57)] public ushort X57Tile { get => this[57]; set => this[57] = value; }
        [TileMetadata(58)] public ushort X58Tile { get => this[58]; set => this[58] = value; }
        [TileMetadata(59)] public ushort X59Tile { get => this[59]; set => this[59] = value; }
        [TileMetadata(60)] public ushort X60Tile { get => this[60]; set => this[60] = value; }
        [TileMetadata(61)] public ushort X61Tile { get => this[61]; set => this[61] = value; }
        [TileMetadata(62)] public ushort X62Tile { get => this[62]; set => this[62] = value; }
        [TileMetadata(63)] public ushort X63Tile { get => this[63]; set => this[63] = value; }
    }
}
