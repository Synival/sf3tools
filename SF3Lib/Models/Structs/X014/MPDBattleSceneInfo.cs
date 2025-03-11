using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X014 {
    public class MPDBattleSceneInfo : Struct {
        private readonly int _mpdFileIdAddr;
        private readonly int _battleSceneFileIdAddr;
        private readonly int _skyBoxIdAddr;
        private readonly int _lightingStyleAddr;
        private readonly int _fogStyleAddr;
        private readonly int _ffffAddr;

        public MPDBattleSceneInfo(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x10) {
            _mpdFileIdAddr         = Address + 0x00;
            _battleSceneFileIdAddr = Address + 0x04;
            _skyBoxIdAddr          = Address + 0x08;
            _lightingStyleAddr     = Address + 0x0A;
            _fogStyleAddr          = Address + 0x0C;
            _ffffAddr              = Address + 0x0E;
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X4", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int MPDFileID {
            get => Data.GetDouble(_mpdFileIdAddr);
            set => Data.SetDouble(_mpdFileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X4", minWidth: 120)]
        [NameGetter(NamedValueType.FileIndex)]
        [BulkCopy]
        public int BattleSceneFileID {
            get => Data.GetDouble(_battleSceneFileIdAddr);
            set => Data.SetDouble(_battleSceneFileIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public ushort SkyBoxID {
            get => (ushort) Data.GetWord(_skyBoxIdAddr);
            set => Data.SetWord(_skyBoxIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3, minWidth: 150)]
        [BulkCopy]
        public LightingStyleType LightingStyle {
            get => (LightingStyleType) Data.GetWord(_lightingStyleAddr);
            set => Data.SetWord(_lightingStyleAddr, (ushort) value);
        }

        [TableViewModelColumn(displayOrder: 4, minWidth: 100)]
        [BulkCopy]
        public FogStyleType FogStyle {
            get => (FogStyleType) Data.GetWord(_fogStyleAddr);
            set => Data.SetWord(_fogStyleAddr, (ushort) value);
        }

        [TableViewModelColumn(displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public ushort FFFF {
            get => (ushort) Data.GetWord(_ffffAddr);
            set => Data.SetWord(_ffffAddr, value);
        }
    }
}
