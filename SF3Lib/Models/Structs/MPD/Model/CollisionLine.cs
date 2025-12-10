using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Tables.MPD.Model;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Model {
    public class CollisionLine : Struct {
        private readonly int _point1Addr;
        private readonly int _point2Addr;
        private readonly int _angleAddr;
        private readonly int _tagAddr;
        private readonly int _ifFlag2XXOffAddr;

        public CollisionLine(IByteData data, int id, string name, int address, CollisionPointTable pointTable) : base(data, id, name, address, 0x08) {
            PointTable = pointTable;

            _point1Addr   = Address + 0x00; // 2 bytes
            _point2Addr   = Address + 0x02; // 2 bytes
            _angleAddr    = Address + 0x04; // 2 bytes
            _tagAddr      = Address + 0x06; // 1 byte
            _ifFlag2XXOffAddr = Address + 0x07; // 1 byte
        }

        public override string ToString()
            => $"({X1,4}, {Y1,4}), ({X2,4}, {Y2,4}) (Angle={Angle,7:0.00}) (Unknown={Tag,2})" + (IfFlagOff.HasValue ? $" (Flag={IfFlagOff.Value:X2})" : "");

        public CollisionPointTable PointTable { get; }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_point1Addr), displayOrder: 0, displayFormat: "X2")]
        public ushort Point1Index {
            get => (ushort) Data.GetWord(_point1Addr);
            set => Data.SetWord(_point1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_point2Addr), displayOrder: 1, displayFormat: "X2")]
        public ushort Point2Index {
            get => (ushort) Data.GetWord(_point2Addr);
            set => Data.SetWord(_point2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_angleAddr), displayOrder: 2, minWidth: 100)]
        public float Angle {
            get => Data.GetCompressedFIXED(_angleAddr).Float * 180.0f;
            set => Data.SetCompressedFIXED(_angleAddr, new CompressedFIXED(value / 180.0f, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_tagAddr), displayOrder: 3, displayFormat: "X2")]
        public byte Tag {
            get => (byte) Data.GetByte(_tagAddr);
            set => Data.SetByte(_tagAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_ifFlag2XXOffAddr), displayOrder: 4, displayFormat: "X2")]
        public byte IfFlagIn2XXOff {
            get => (byte) Data.GetByte(_ifFlag2XXOffAddr);
            set => Data.SetByte(_ifFlag2XXOffAddr, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 4.1f, displayFormat: "X3", minWidth: 200)]
        [NameGetter(NamedValueType.GameFlag)]
        public int? IfFlagOff {
            get {
                var flag200 = IfFlagIn2XXOff;
                return (flag200 == 0) ? (int?) null : flag200 + 0x200;
            }
        }

        public CollisionPoint Point1 {
            get {
                var index = Point1Index;
                return (index >= 0 && index < PointTable.Length) ? PointTable[index] : null;
            }
            set => Point1Index = (ushort) (value?.ID ?? 0);
        }

        public CollisionPoint Point2 {
            get {
                var index = Point2Index;
                return (index >= 0 && index < PointTable.Length) ? PointTable[index] : null;
            }
            set => Point2Index = (ushort) (value?.ID ?? 0);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 5f, minWidth: 40)]
        public short X1 {
            get => Point1?.X ?? 0;
            set {
                var point = Point1;
                if (point != null)
                    point.X = value;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 6f, minWidth: 40)]
        public short Y1 {
            get => Point1?.Y ?? 0;
            set {
                var point = Point1;
                if (point != null)
                    point.Y = value;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 7f, minWidth: 40)]
        public short X2 {
            get => Point2?.X ?? 0;
            set {
                var point = Point2;
                if (point != null)
                    point.X = value;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 8f, minWidth: 40)]
        public short Y2 {
            get => Point2?.Y ?? 0;
            set {
                var point = Point2;
                if (point != null)
                    point.Y = value;
            }
        }
    }
}
