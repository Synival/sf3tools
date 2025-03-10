using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class MPDBattleSceneInfo : Struct {
        private readonly int _mpdFileIdAddr;
        private readonly int _battleSceneFileIdAddr;
        private readonly int _unknown1Addr;
        private readonly int _unknown2Addr;
        private readonly int _hasFogAddr;
        private readonly int _unknown3Addr;

        public MPDBattleSceneInfo(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x10) {
            _mpdFileIdAddr         = Address + 0x00;
            _battleSceneFileIdAddr = Address + 0x04;
            _unknown1Addr          = Address + 0x08;
            _unknown2Addr          = Address + 0x0A;
            _hasFogAddr            = Address + 0x0C;
            _unknown3Addr          = Address + 0x0E;
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X4", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public uint MPDFileID {
            get => (uint) Data.GetDouble(_mpdFileIdAddr);
            set => Data.SetDouble(_mpdFileIdAddr, (int) value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X4", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public uint BattleSceneFileID {
            get => (uint) Data.GetDouble(_battleSceneFileIdAddr);
            set => Data.SetDouble(_battleSceneFileIdAddr, (int) value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public ushort Unknown1 {
            get => (ushort) Data.GetWord(_unknown1Addr);
            set => Data.SetWord(_unknown1Addr, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public ushort Unknown2 {
            get => (ushort) Data.GetWord(_unknown2Addr);
            set => Data.SetWord(_unknown2Addr, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public ushort HasFog {
            get => (ushort) Data.GetWord(_hasFogAddr);
            set => Data.SetWord(_hasFogAddr, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public ushort Unknown3 {
            get => (ushort) Data.GetWord(_unknown3Addr);
            set => Data.SetWord(_unknown3Addr, value);
        }
    }
}
