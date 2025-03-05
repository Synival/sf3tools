using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class CollisionBlockRow : Struct {
        public CollisionBlockRow(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x40) {
        }

        public int this[int x] {
            get => Data.GetDouble(Address + x * 0x04);
            set => Data.SetDouble(Address + x * 0x04, value);
        }

        public int Length => 16;

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            public TileMetadataAttribute(int x) : base(displayName: "X" + x.ToString("D2") + "Offset", displayOrder: x, isPointer: true) { }
        }

        [TileMetadata(0)]  public int X0Offset  { get => Data.GetDouble(Address + 0x00); set => Data.SetDouble(Address + 0x00, value); }
        [TileMetadata(1)]  public int X1Offset  { get => Data.GetDouble(Address + 0x04); set => Data.SetDouble(Address + 0x04, value); }
        [TileMetadata(2)]  public int X2Offset  { get => Data.GetDouble(Address + 0x08); set => Data.SetDouble(Address + 0x08, value); }
        [TileMetadata(3)]  public int X3Offset  { get => Data.GetDouble(Address + 0x0C); set => Data.SetDouble(Address + 0x0C, value); }
        [TileMetadata(4)]  public int X4Offset  { get => Data.GetDouble(Address + 0x10); set => Data.SetDouble(Address + 0x10, value); }
        [TileMetadata(5)]  public int X5Offset  { get => Data.GetDouble(Address + 0x14); set => Data.SetDouble(Address + 0x14, value); }
        [TileMetadata(6)]  public int X6Offset  { get => Data.GetDouble(Address + 0x18); set => Data.SetDouble(Address + 0x18, value); }
        [TileMetadata(7)]  public int X7Offset  { get => Data.GetDouble(Address + 0x1C); set => Data.SetDouble(Address + 0x1C, value); }
        [TileMetadata(8)]  public int X8Offset  { get => Data.GetDouble(Address + 0x20); set => Data.SetDouble(Address + 0x20, value); }
        [TileMetadata(9)]  public int X9Offset  { get => Data.GetDouble(Address + 0x24); set => Data.SetDouble(Address + 0x24, value); }
        [TileMetadata(10)] public int X10Offset { get => Data.GetDouble(Address + 0x28); set => Data.SetDouble(Address + 0x28, value); }
        [TileMetadata(11)] public int X11Offset { get => Data.GetDouble(Address + 0x2C); set => Data.SetDouble(Address + 0x2C, value); }
        [TileMetadata(12)] public int X12Offset { get => Data.GetDouble(Address + 0x30); set => Data.SetDouble(Address + 0x30, value); }
        [TileMetadata(13)] public int X13Offset { get => Data.GetDouble(Address + 0x34); set => Data.SetDouble(Address + 0x34, value); }
        [TileMetadata(14)] public int X14Offset { get => Data.GetDouble(Address + 0x38); set => Data.SetDouble(Address + 0x38, value); }
        [TileMetadata(15)] public int X15Offset { get => Data.GetDouble(Address + 0x3C); set => Data.SetDouble(Address + 0x3C, value); }
    }
}
