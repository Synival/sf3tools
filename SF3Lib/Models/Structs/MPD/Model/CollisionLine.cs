using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Tables.MPD.Model;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Model {
    public class CollisionLine : Struct, IMPD_CollisionLine {
        private readonly int _point1Addr;
        private readonly int _point2Addr;
        private readonly int _angleAddr;
        private readonly int _tagAddr;
        private readonly int _flag2XXToDisableAddr;

        public CollisionLine(IByteData data, int id, string name, int address, CollisionPointTable pointTable, IEnumerable<CollisionLineIndexTable> blockLines)
        : base(data, id, name, address, 0x08) {
            PointTable = pointTable;
            BlockLines = blockLines;

            _point1Addr   = Address + 0x00; // 2 bytes
            _point2Addr   = Address + 0x02; // 2 bytes
            _angleAddr    = Address + 0x04; // 2 bytes
            _tagAddr      = Address + 0x06; // 1 byte
            _flag2XXToDisableAddr = Address + 0x07; // 1 byte
        }

        public override string ToString()
            => $"({X1,4:X}, {Y1,4:X}), ({X2,4:X}, {Y2,4:X}) (Angle={Angle,7:0.00}) (Unknown={Tag,2})" + (FlagToDisable.HasValue ? $" (Flag={FlagToDisable.Value:X2})" : "");

        public CollisionPointTable PointTable { get; }
        public IEnumerable<CollisionLineIndexTable> BlockLines { get; }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_point1Addr), displayOrder: 0, displayFormat: "X2")]
        public ushort Point1Index {
            get => Data.GetUInt16(_point1Addr);
            set => Data.SetUInt16(_point1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_point2Addr), displayOrder: 1, displayFormat: "X2")]
        public ushort Point2Index {
            get => Data.GetUInt16(_point2Addr);
            set => Data.SetUInt16(_point2Addr, value);
        }

        public IMPD_CollisionPoint Point1 {
            get {
                var index = Point1Index;
                return (index >= 0 && index < PointTable.Count) ? PointTable[index] : null;
            }
            set => Point1Index = (ushort) ((value as CollisionPoint)?.ID ?? 0);
        }

        public IMPD_CollisionPoint Point2 {
            get {
                var index = Point2Index;
                return (index >= 0 && index < PointTable.Count) ? PointTable[index] : null;
            }
            set => Point2Index = (ushort) ((value as CollisionPoint)?.ID ?? 0);
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
            get => Data.GetUInt8(_tagAddr);
            set => Data.SetUInt8(_tagAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_flag2XXToDisableAddr), displayOrder: 4, displayFormat: "X2")]
        public byte Flag2XXToDisable {
            get => Data.GetUInt8(_flag2XXToDisableAddr);
            set => Data.SetUInt8(_flag2XXToDisableAddr, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 4.1f, displayFormat: "X3", minWidth: 200)]
        [NameGetter(NamedValueType.GameFlag)]
        public int? FlagToDisable {
            get {
                var flag200 = Flag2XXToDisable;
                return (flag200 == 0) ? (int?) null : flag200 + 0x200;
            }
            set => Flag2XXToDisable = (value >= 0x200 && value <= 0x2FF) ? (byte) (value - 0x200) : (byte) 0;
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

        public IEnumerable<(int X, int Y)> GetReferencingBlocks() {
            return BlockLines
                .Where(x => x.Rows.Any(y => y.LineIndex == ID))
                .Select(x => (X: x.BlockX, Y: x.BlockY))
                .ToArray();
        }

        [TableViewModelColumn(addressField: null, displayName: "Blocks", displayOrder: 9f, minWidth: 300)]
        public string BlocksStr {
            get {
                return string.Join(" ", GetReferencingBlocks()
                    .OrderBy(x => x.Y)
                    .ThenBy(x => x.X)
                    .Select(x => $"({x.X}, {x.Y})")
                );
            }
        }
    }
}
