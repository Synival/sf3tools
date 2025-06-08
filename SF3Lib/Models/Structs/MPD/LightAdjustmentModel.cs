using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class LightAdjustmentModel : Struct {
        private readonly int _rAdjustAddr;
        private readonly int _gAdjustAddr;
        private readonly int _bAdjustAddr;
        private readonly int _groundRAdjustAddr;
        private readonly int _groundGAdjustAddr;
        private readonly int _groundBAdjustAddr;
        private readonly int _palette3TransparencyAddr;

        public LightAdjustmentModel(IByteData data, int id, string name, int address, ScenarioType scenario)
        : base(data, id, name, address, scenario >= ScenarioType.Scenario3 ? 0x0E : 0x06) {
            Scenario = scenario;

            _rAdjustAddr               = Address + 0x00; // 2 bytes
            _gAdjustAddr               = Address + 0x02; // 2 bytes
            _bAdjustAddr               = Address + 0x04; // 2 bytes
            _groundRAdjustAddr         = (scenario >= ScenarioType.Scenario3) ? Address + 0x06 : -1; // 2 bytes
            _groundGAdjustAddr         = (scenario >= ScenarioType.Scenario3) ? Address + 0x08 : -1; // 2 bytes
            _groundBAdjustAddr         = (scenario >= ScenarioType.Scenario3) ? Address + 0x0A : -1; // 2 bytes
            _palette3TransparencyAddr  = (scenario >= ScenarioType.Scenario3) ? Address + 0x0C : -1; // 2 bytes
        }

        public ScenarioType Scenario { get; }
        public bool HasGroundAdjust => Scenario >= ScenarioType.Scenario3;
        public bool HasPalette3Transparency => Scenario >= ScenarioType.Scenario3;

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_rAdjustAddr), displayOrder: 0, displayName: "R +/-")]
        public short RAdjustment {
            get => (short) Data.GetWord(_rAdjustAddr);
            set => Data.SetWord(_rAdjustAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_gAdjustAddr), displayOrder: 1, displayName: "G +/-")]
        public short GAdjustment {
            get => (short) Data.GetWord(_gAdjustAddr);
            set => Data.SetWord(_gAdjustAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_bAdjustAddr), displayOrder: 2, displayName: "B +/-")]
        public short BAdjustment {
            get => (short) Data.GetWord(_bAdjustAddr);
            set => Data.SetWord(_bAdjustAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundRAdjustAddr), displayOrder: 3, displayName: "Ground R +/- (Scn3+)", visibilityProperty: nameof(HasGroundAdjust))]
        public short GroundRAdjustment {
            get => HasGroundAdjust ? (short) Data.GetWord(_groundRAdjustAddr) : (short) 0;
            set {
                if (HasGroundAdjust)
                    Data.SetWord(_groundRAdjustAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundGAdjustAddr), displayOrder: 4, displayName: "Ground G +/- (Scn3+)", visibilityProperty: nameof(HasGroundAdjust))]
        public short GroundGAdjustment {
            get => HasGroundAdjust ? (short) Data.GetWord(_groundGAdjustAddr) : (short) 0;
            set {
                if (HasGroundAdjust)
                    Data.SetWord(_groundGAdjustAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundBAdjustAddr), displayOrder: 5, displayName: "Ground B +/- (Scn3+)", visibilityProperty: nameof(HasGroundAdjust))]
        public short GroundBAdjustment {
            get => HasGroundAdjust ? (short) Data.GetWord(_groundBAdjustAddr) : (short) 0;
            set {
                if (HasGroundAdjust)
                    Data.SetWord(_groundBAdjustAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_palette3TransparencyAddr), displayOrder: 6, displayFormat: "X2", displayName: "Palette3 Transparency (Scn3+)", visibilityProperty: nameof(HasPalette3Transparency))]
        public ushort Palette3Transparency {
            get => HasPalette3Transparency ? (ushort) Data.GetWord(_palette3TransparencyAddr) : (ushort) 0;
            set {
                if (HasPalette3Transparency)
                    Data.SetWord(_palette3TransparencyAddr, value);
            }
        }
    }
}
