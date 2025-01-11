using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    /// <summary>
    /// NOTE: This might contain more than camera settings, but that's all we know about at the moment.
    /// </summary>
    public class CameraSettings : Struct {
        private readonly int _unknownAddr;

        private readonly int _unknownBoxX1Addr;
        private readonly int _unknownBoxY1Addr;
        private readonly int _unknownBoxX2Addr;
        private readonly int _unknownBoxY2Addr;

        private readonly int _cameraBoundaryX1Addr;
        private readonly int _cameraBoundaryY1Addr;
        private readonly int _cameraBoundaryX2Addr;
        private readonly int _cameraBoundaryY2Addr;

        public CameraSettings(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x12) {
            _unknownAddr          = address + 0x00; // 2 bytes

            _unknownBoxX1Addr     = address + 0x02; // 2 bytes
            _unknownBoxY1Addr     = address + 0x04; // 2 bytes
            _unknownBoxX2Addr     = address + 0x06; // 2 bytes
            _unknownBoxY2Addr     = address + 0x08; // 2 bytes

            _cameraBoundaryX1Addr = address + 0x0A; // 2 bytes
            _cameraBoundaryY1Addr = address + 0x0C; // 2 bytes
            _cameraBoundaryX2Addr = address + 0x0E; // 2 bytes
            _cameraBoundaryY2Addr = address + 0x10; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Unknown", displayOrder: 0, displayFormat: "X4")]
        public ushort Unknown {
            get => (ushort) Data.GetWord(_unknownAddr);
            set => Data.SetWord(_unknownAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Unknown Box X1", displayOrder: 1)]
        public short UnknownBoxX1 {
            get => (short) Data.GetWord(_unknownBoxX1Addr);
            set => Data.SetWord(_unknownBoxX1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Unknown Box Y1", displayOrder: 2)]
        public short UnknownBoxY1 {
            get => (short) Data.GetWord(_unknownBoxY1Addr);
            set => Data.SetWord(_unknownBoxY1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Unknown Box X2", displayOrder: 3)]
        public short UnknownBoxX2 {
            get => (short) Data.GetWord(_unknownBoxX2Addr);
            set => Data.SetWord(_unknownBoxX2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Unknown Box Y2", displayOrder: 4)]
        public short UnknownBoxY2 {
            get => (short) Data.GetWord(_unknownBoxY2Addr);
            set => Data.SetWord(_unknownBoxY2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Camera Boundary X1", displayOrder: 5)]
        public short CameraBoundaryX1 {
            get => (short) Data.GetWord(_cameraBoundaryX1Addr);
            set => Data.SetWord(_cameraBoundaryX1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Camera Boundary Y1", displayOrder: 6)]
        public short CameraBoundaryY1 {
            get => (short) Data.GetWord(_cameraBoundaryY1Addr);
            set => Data.SetWord(_cameraBoundaryY1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Camera Boundary X2", displayOrder: 7)]
        public short CameraBoundaryX2 {
            get => (short) Data.GetWord(_cameraBoundaryX2Addr);
            set => Data.SetWord(_cameraBoundaryX2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Camera Boundary Y2", displayOrder: 8)]
        public short CameraBoundaryY2 {
            get => (short) Data.GetWord(_cameraBoundaryY2Addr);
            set => Data.SetWord(_cameraBoundaryY2Addr, value);
        }
    }
}
