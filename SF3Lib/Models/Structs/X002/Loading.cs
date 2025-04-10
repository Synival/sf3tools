using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X002 {
    public class Loading : Struct {
        private readonly int _sceneIDAddr;
        private readonly int _x1Addr;
        private readonly int _chpAddr;
        private readonly int _x5Addr;
        private readonly int _musicAddr;
        private readonly int _mpdAddr;
        private readonly int _unknown0x0CAddr;
        private readonly int _chrAddr;

        public Loading(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            _sceneIDAddr     = Address + 0x00; // 2 bytes
            _x1Addr          = Address + 0x02; // 2 bytes
            _chpAddr         = Address + 0x04; // 2 bytes
            _x5Addr          = Address + 0x06; // 2 bytes
            _musicAddr       = Address + 0x08; // 2 bytes
            _mpdAddr         = Address + 0x0a; // 2 bytes
            _unknown0x0CAddr = Address + 0x0c; // 2 bytes
            _chrAddr         = Address + 0x0e; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public int SceneID {
            get => Data.GetWord(_sceneIDAddr);
            set => Data.SetWord(_sceneIDAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, minWidth: 140, displayFormat: "X3")]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int X1 {
            get => Data.GetWord(_x1Addr);
            set => Data.SetWord(_x1Addr, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayName: "CHP? IsBattle?")]
        [BulkCopy]
        public int CHP {
            get => Data.GetWord(_chpAddr);
            set => Data.SetWord(_chpAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3, minWidth: 140, displayFormat: "X3")]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int X5 {
            get => Data.GetWord(_x5Addr);
            set => Data.SetWord(_x5Addr, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X2", minWidth: 210)]
        [BulkCopy]
        [NameGetter(NamedValueType.Music)]
        public int Music {
            get => Data.GetWord(_musicAddr);
            set => Data.SetWord(_musicAddr, value);
        }

        [TableViewModelColumn(displayOrder: 5, minWidth: 140, displayFormat: "X3")]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MPD {
            get => Data.GetWord(_mpdAddr);
            set => Data.SetWord(_mpdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayName: "# of Maps?")]
        [BulkCopy]
        public int Unknown0x0C {
            get => Data.GetWord(_unknown0x0CAddr);
            set => Data.SetWord(_unknown0x0CAddr, value);
        }

        [TableViewModelColumn(displayOrder: 7, minWidth: 140, displayFormat: "X3")]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int CHR {
            get => Data.GetWord(_chrAddr);
            set => Data.SetWord(_chrAddr, value);
        }
    }
}
