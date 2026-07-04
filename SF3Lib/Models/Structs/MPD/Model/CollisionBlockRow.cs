using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class CollisionBlockRow : Struct {
        public CollisionBlockRow(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x40) {
        }

        public uint this[int x] {
            get => Data.GetUInt32(Address + x * 0x04);
            set => Data.SetUInt32(Address + x * 0x04, value);
        }

        public int Length => 16;

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            // TODO: address!
            public TileMetadataAttribute(int x) : base(addressField: null, displayName: "X" + x.ToString("D2") + "Offset", displayOrder: x, isPointer: true) { }
        }

        [TileMetadata(0)]  public uint X0Offset  { get => Data.GetUInt32(Address + 0x00); set => Data.SetUInt32(Address + 0x00, value); }
        [TileMetadata(1)]  public uint X1Offset  { get => Data.GetUInt32(Address + 0x04); set => Data.SetUInt32(Address + 0x04, value); }
        [TileMetadata(2)]  public uint X2Offset  { get => Data.GetUInt32(Address + 0x08); set => Data.SetUInt32(Address + 0x08, value); }
        [TileMetadata(3)]  public uint X3Offset  { get => Data.GetUInt32(Address + 0x0C); set => Data.SetUInt32(Address + 0x0C, value); }
        [TileMetadata(4)]  public uint X4Offset  { get => Data.GetUInt32(Address + 0x10); set => Data.SetUInt32(Address + 0x10, value); }
        [TileMetadata(5)]  public uint X5Offset  { get => Data.GetUInt32(Address + 0x14); set => Data.SetUInt32(Address + 0x14, value); }
        [TileMetadata(6)]  public uint X6Offset  { get => Data.GetUInt32(Address + 0x18); set => Data.SetUInt32(Address + 0x18, value); }
        [TileMetadata(7)]  public uint X7Offset  { get => Data.GetUInt32(Address + 0x1C); set => Data.SetUInt32(Address + 0x1C, value); }
        [TileMetadata(8)]  public uint X8Offset  { get => Data.GetUInt32(Address + 0x20); set => Data.SetUInt32(Address + 0x20, value); }
        [TileMetadata(9)]  public uint X9Offset  { get => Data.GetUInt32(Address + 0x24); set => Data.SetUInt32(Address + 0x24, value); }
        [TileMetadata(10)] public uint X10Offset { get => Data.GetUInt32(Address + 0x28); set => Data.SetUInt32(Address + 0x28, value); }
        [TileMetadata(11)] public uint X11Offset { get => Data.GetUInt32(Address + 0x2C); set => Data.SetUInt32(Address + 0x2C, value); }
        [TileMetadata(12)] public uint X12Offset { get => Data.GetUInt32(Address + 0x30); set => Data.SetUInt32(Address + 0x30, value); }
        [TileMetadata(13)] public uint X13Offset { get => Data.GetUInt32(Address + 0x34); set => Data.SetUInt32(Address + 0x34, value); }
        [TileMetadata(14)] public uint X14Offset { get => Data.GetUInt32(Address + 0x38); set => Data.SetUInt32(Address + 0x38, value); }
        [TileMetadata(15)] public uint X15Offset { get => Data.GetUInt32(Address + 0x3C); set => Data.SetUInt32(Address + 0x3C, value); }
    }
}
