using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Town {
    public class Enter : Struct {
        private readonly int _sceneNumberAddr; // 2 bytes
        private readonly int _ifFlagOnAddr;    // 2 bytes
        private readonly int _xPosAddr;        // 2 bytes
        private readonly int _unknown0x06Addr; // 2 bytes
        private readonly int _zPosAddr;        // 2 bytes
        private readonly int _directionAddr;   // 2 bytes
        private readonly int _cameraAddr;      // 2 bytes
        private readonly int _unknown0x0EAddr; // 2 bytes

        public Enter(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            _sceneNumberAddr = Address;        // 2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            _ifFlagOnAddr    = Address + 0x02; // 2 bytes
            _xPosAddr        = Address + 0x04; // 2 bytes
            _unknown0x06Addr = Address + 0x06; // 2 bytes
            _zPosAddr        = Address + 0x08; // 2 bytes
            _directionAddr   = Address + 0x0a; // 2 bytes
            _cameraAddr      = Address + 0x0c; // 2 bytes
            _unknown0x0EAddr = Address + 0x0e; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public int SceneID {
            get => Data.GetWord(_sceneNumberAddr);
            set => Data.SetWord(_sceneNumberAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X3")]
        [BulkCopy]
        [NameGetter(NamedValueType.GameFlag)]
        public int IfFlagOn {
            get => Data.GetWord(_ifFlagOnAddr);
            set => Data.SetWord(_ifFlagOnAddr, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayName: "xPos")]
        [BulkCopy]
        public int XPos {
            get => Data.GetWord(_xPosAddr);
            set => Data.SetWord(_xPosAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "+0x06", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x06 {
            get => Data.GetWord(_unknown0x06Addr);
            set => Data.SetWord(_unknown0x06Addr, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "zPos")]
        [BulkCopy]
        public int ZPos {
            get => Data.GetWord(_zPosAddr);
            set => Data.SetWord(_zPosAddr, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "Direction", displayFormat: "X4")]
        [BulkCopy]
        public int Direction {
            get => Data.GetWord(_directionAddr);
            set => Data.SetWord(_directionAddr, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayName: "Camera", displayFormat: "X4")]
        [BulkCopy]
        public int Camera {
            get => Data.GetWord(_cameraAddr);
            set => Data.SetWord(_cameraAddr, value);
        }

        [TableViewModelColumn(displayOrder: 7, displayName: "+0x0E", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x0E {
            get => Data.GetWord(_unknown0x0EAddr);
            set => Data.SetWord(_unknown0x0EAddr, value);
        }
    }
}
