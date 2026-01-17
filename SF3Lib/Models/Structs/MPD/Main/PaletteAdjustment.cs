using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Main {
    public class PaletteAdjustment : Struct {
        private readonly int _lightRAdjustAddr;
        private readonly int _lightGAdjustAddr;
        private readonly int _lightBAdjustAddr;
        private readonly int _groundRAdjustAddr;
        private readonly int _groundGAdjustAddr;
        private readonly int _groundBAdjustAddr;
        private readonly int _shadowTransparencyAddr;

        public PaletteAdjustment(IByteData data, int id, string name, int address, ScenarioType scenario)
        : base(data, id, name, address, scenario >= ScenarioType.Scenario3 ? 0x0E : 0x06) {
            Scenario = scenario;

            _lightRAdjustAddr       = Address + 0x00; // 2 bytes
            _lightGAdjustAddr       = Address + 0x02; // 2 bytes
            _lightBAdjustAddr       = Address + 0x04; // 2 bytes
            _groundRAdjustAddr      = scenario >= ScenarioType.Scenario3 ? Address + 0x06 : -1; // 2 bytes
            _groundGAdjustAddr      = scenario >= ScenarioType.Scenario3 ? Address + 0x08 : -1; // 2 bytes
            _groundBAdjustAddr      = scenario >= ScenarioType.Scenario3 ? Address + 0x0A : -1; // 2 bytes
            _shadowTransparencyAddr = scenario >= ScenarioType.Scenario3 ? Address + 0x0C : -1; // 2 bytes
        }

        public ScenarioType Scenario { get; }
        public bool HasGroundAdjustment => Scenario >= ScenarioType.Scenario3;
        public bool HasShadowTransparency => Scenario >= ScenarioType.Scenario3;

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_lightRAdjustAddr), displayOrder: 0, displayName: "LightR +/-", displayFormat: "-X2")]
        public short LightRAdjustment {
            get => (short) Data.GetWord(_lightRAdjustAddr);
            set => Data.SetWord(_lightRAdjustAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_lightGAdjustAddr), displayOrder: 1, displayName: "LightG +/-", displayFormat: "-X2")]
        public short LightGAdjustment {
            get => (short) Data.GetWord(_lightGAdjustAddr);
            set => Data.SetWord(_lightGAdjustAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_lightBAdjustAddr), displayOrder: 2, displayName: "LightB +/-", displayFormat: "-X2")]
        public short LightBAdjustment {
            get => (short) Data.GetWord(_lightBAdjustAddr);
            set => Data.SetWord(_lightBAdjustAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundRAdjustAddr), displayOrder: 3, displayName: "Ground R +/- (Scn3+)", displayFormat: "-X2", visibilityProperty: nameof(HasGroundAdjustment))]
        public short GroundRAdjustment {
            get => HasGroundAdjustment ? (short) Data.GetWord(_groundRAdjustAddr) : (short) 0;
            set {
                if (HasGroundAdjustment)
                    Data.SetWord(_groundRAdjustAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundGAdjustAddr), displayOrder: 4, displayName: "Ground G +/- (Scn3+)", displayFormat: "-X2", visibilityProperty: nameof(HasGroundAdjustment))]
        public short GroundGAdjustment {
            get => HasGroundAdjustment ? (short) Data.GetWord(_groundGAdjustAddr) : (short) 0;
            set {
                if (HasGroundAdjustment)
                    Data.SetWord(_groundGAdjustAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundBAdjustAddr), displayOrder: 5, displayName: "Ground B +/- (Scn3+)", displayFormat: "-X2", visibilityProperty: nameof(HasGroundAdjustment))]
        public short GroundBAdjustment {
            get => HasGroundAdjustment ? (short) Data.GetWord(_groundBAdjustAddr) : (short) 0;
            set {
                if (HasGroundAdjustment)
                    Data.SetWord(_groundBAdjustAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_shadowTransparencyAddr), displayOrder: 6, displayName: "Shadow Transparency (Scn3+)", displayFormat: "X2", visibilityProperty: nameof(HasShadowTransparency))]
        public ushort ShadowTransparency {
            get => HasShadowTransparency ? (ushort) Data.GetWord(_shadowTransparencyAddr) : (ushort) 0;
            set {
                if (HasShadowTransparency)
                    Data.SetWord(_shadowTransparencyAddr, value);
            }
        }
    }
}
