using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class CollisionBlockRow : Struct {
        public CollisionBlockRow(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x40) {
        }

        public uint this[int x] {
            get => (uint) Data.GetDouble(Address + x * 0x04);
            set => Data.SetDouble(Address + x * 0x04, (int) value);
        }

        public int Length => 16;

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            public TileMetadataAttribute(int x) : base(displayName: "X" + x.ToString("D2") + "Offset", displayOrder: x, isPointer: true) { }
        }

        [TileMetadata(0)]  public uint X0Offset  { get => (uint) Data.GetDouble(Address + 0x00); set => Data.SetDouble(Address + 0x00, (int) value); }
        [TileMetadata(1)]  public uint X1Offset  { get => (uint) Data.GetDouble(Address + 0x04); set => Data.SetDouble(Address + 0x04, (int) value); }
        [TileMetadata(2)]  public uint X2Offset  { get => (uint) Data.GetDouble(Address + 0x08); set => Data.SetDouble(Address + 0x08, (int) value); }
        [TileMetadata(3)]  public uint X3Offset  { get => (uint) Data.GetDouble(Address + 0x0C); set => Data.SetDouble(Address + 0x0C, (int) value); }
        [TileMetadata(4)]  public uint X4Offset  { get => (uint) Data.GetDouble(Address + 0x10); set => Data.SetDouble(Address + 0x10, (int) value); }
        [TileMetadata(5)]  public uint X5Offset  { get => (uint) Data.GetDouble(Address + 0x14); set => Data.SetDouble(Address + 0x14, (int) value); }
        [TileMetadata(6)]  public uint X6Offset  { get => (uint) Data.GetDouble(Address + 0x18); set => Data.SetDouble(Address + 0x18, (int) value); }
        [TileMetadata(7)]  public uint X7Offset  { get => (uint) Data.GetDouble(Address + 0x1C); set => Data.SetDouble(Address + 0x1C, (int) value); }
        [TileMetadata(8)]  public uint X8Offset  { get => (uint) Data.GetDouble(Address + 0x20); set => Data.SetDouble(Address + 0x20, (int) value); }
        [TileMetadata(9)]  public uint X9Offset  { get => (uint) Data.GetDouble(Address + 0x24); set => Data.SetDouble(Address + 0x24, (int) value); }
        [TileMetadata(10)] public uint X10Offset { get => (uint) Data.GetDouble(Address + 0x28); set => Data.SetDouble(Address + 0x28, (int) value); }
        [TileMetadata(11)] public uint X11Offset { get => (uint) Data.GetDouble(Address + 0x2C); set => Data.SetDouble(Address + 0x2C, (int) value); }
        [TileMetadata(12)] public uint X12Offset { get => (uint) Data.GetDouble(Address + 0x30); set => Data.SetDouble(Address + 0x30, (int) value); }
        [TileMetadata(13)] public uint X13Offset { get => (uint) Data.GetDouble(Address + 0x34); set => Data.SetDouble(Address + 0x34, (int) value); }
        [TileMetadata(14)] public uint X14Offset { get => (uint) Data.GetDouble(Address + 0x38); set => Data.SetDouble(Address + 0x38, (int) value); }
        [TileMetadata(15)] public uint X15Offset { get => (uint) Data.GetDouble(Address + 0x3C); set => Data.SetDouble(Address + 0x3C, (int) value); }
    }
}
