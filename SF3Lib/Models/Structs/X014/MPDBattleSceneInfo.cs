using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class MPDBattleSceneInfo : Struct {
        private readonly int _mpdFileIdAddr;
        private readonly int _battleSceneFileIdAddr;
        private readonly int _skyIdAddr;
        private readonly int _lightingStyleAddr;
        private readonly int _fogStyleAddr;
        private readonly int _ffffAddr;

        public MPDBattleSceneInfo(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x10) {
            _mpdFileIdAddr         = Address + 0x00;
            _battleSceneFileIdAddr = Address + 0x04;
            _skyIdAddr             = Address + 0x08;
            _lightingStyleAddr     = Address + 0x0A;
            _fogStyleAddr          = Address + 0x0C;
            _ffffAddr              = Address + 0x0E;
        }

        [TableViewModelColumn(addressField: nameof(_mpdFileIdAddr), displayOrder: 0, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int MPDFileID {
            get => Data.GetInt32(_mpdFileIdAddr);
            set => Data.SetInt32(_mpdFileIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_battleSceneFileIdAddr), displayOrder: 1, displayFormat: "X3", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int BattleSceneFileID {
            get => Data.GetInt32(_battleSceneFileIdAddr);
            set => Data.SetInt32(_battleSceneFileIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_skyIdAddr), displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public ushort SkyID {
            get => Data.GetUInt16(_skyIdAddr);
            set => Data.SetUInt16(_skyIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lightingStyleAddr), displayOrder: 3, minWidth: 150)]
        [BulkCopy]
        public LightingStyleType LightingStyle {
            get => (LightingStyleType) Data.GetUInt16(_lightingStyleAddr);
            set => Data.SetUInt16(_lightingStyleAddr, (ushort) value);
        }

        [TableViewModelColumn(addressField: nameof(_fogStyleAddr), displayOrder: 4, minWidth: 100)]
        [BulkCopy]
        public FogStyleType FogStyle {
            get => (FogStyleType) Data.GetUInt16(_fogStyleAddr);
            set => Data.SetUInt16(_fogStyleAddr, (ushort) value);
        }

        [TableViewModelColumn(addressField: nameof(_ffffAddr), displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public ushort FFFF {
            get => Data.GetUInt16(_ffffAddr);
            set => Data.SetUInt16(_ffffAddr, value);
        }
    }
}
