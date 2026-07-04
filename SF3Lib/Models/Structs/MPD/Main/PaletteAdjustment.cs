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

        public PaletteAdjustment(IByteData data, int id, string name, int address, bool hasGroundAndShadow, bool isTruncated)
        : base(data, id, name, address, hasGroundAndShadow ? 0x0E : 0x06) {
            HasGroundAdjustment   = hasGroundAndShadow;
            HasShadowTransparency = hasGroundAndShadow; 
            IsTruncated           = isTruncated;

            _lightRAdjustAddr       = Address + 0x00; // 2 bytes
            _lightGAdjustAddr       = Address + 0x02; // 2 bytes
            _lightBAdjustAddr       = Address + 0x04; // 2 bytes
            _groundRAdjustAddr      = hasGroundAndShadow ? Address + 0x06 : -1; // 2 bytes
            _groundGAdjustAddr      = hasGroundAndShadow ? Address + 0x08 : -1; // 2 bytes
            _groundBAdjustAddr      = hasGroundAndShadow ? Address + 0x0A : -1; // 2 bytes
            _shadowTransparencyAddr = hasGroundAndShadow ? Address + 0x0C : -1; // 2 bytes
        }

        public bool HasGroundAdjustment { get; }
        public bool HasShadowTransparency { get; }

        [TableViewModelColumn(displayOrder: 0, displayName: "Is Truncated", visibilityProperty: nameof(HasGroundAdjustment))]
        public bool IsTruncated { get; }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_lightRAdjustAddr), displayOrder: 0.1f, displayName: "LightR +/-", displayFormat: "-X2")]
        public short LightRAdjustment {
            get => Data.GetInt16(_lightRAdjustAddr);
            set => Data.SetInt16(_lightRAdjustAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_lightGAdjustAddr), displayOrder: 1, displayName: "LightG +/-", displayFormat: "-X2")]
        public short LightGAdjustment {
            get => Data.GetInt16(_lightGAdjustAddr);
            set => Data.SetInt16(_lightGAdjustAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_lightBAdjustAddr), displayOrder: 2, displayName: "LightB +/-", displayFormat: "-X2")]
        public short LightBAdjustment {
            get => Data.GetInt16(_lightBAdjustAddr);
            set => Data.SetInt16(_lightBAdjustAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundRAdjustAddr), displayOrder: 3, displayName: "Ground R +/- (Scn3+)", displayFormat: "-X2", visibilityProperty: nameof(HasGroundAdjustment))]
        public short GroundRAdjustment {
            get => HasGroundAdjustment ? Data.GetInt16(_groundRAdjustAddr) : (short) 0;
            set {
                if (HasGroundAdjustment)
                    Data.SetInt16(_groundRAdjustAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundGAdjustAddr), displayOrder: 4, displayName: "Ground G +/- (Scn3+)", displayFormat: "-X2", visibilityProperty: nameof(HasGroundAdjustment))]
        public short GroundGAdjustment {
            get => HasGroundAdjustment ? Data.GetInt16(_groundGAdjustAddr) : (short) 0;
            set {
                if (HasGroundAdjustment)
                    Data.SetInt16(_groundGAdjustAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundBAdjustAddr), displayOrder: 5, displayName: "Ground B +/- (Scn3+)", displayFormat: "-X2", visibilityProperty: nameof(HasGroundAdjustment))]
        public short GroundBAdjustment {
            get => HasGroundAdjustment ? Data.GetInt16(_groundBAdjustAddr) : (short) 0;
            set {
                if (HasGroundAdjustment)
                    Data.SetInt16(_groundBAdjustAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_shadowTransparencyAddr), displayOrder: 6, displayName: "Shadow Transparency (Scn3+)", displayFormat: "X2", visibilityProperty: nameof(HasShadowTransparency))]
        public ushort ShadowTransparency {
            get => HasShadowTransparency ? (ushort) Data.GetWord(_shadowTransparencyAddr) : (ushort) 0x0F;
            set {
                if (HasShadowTransparency)
                    Data.SetWord(_shadowTransparencyAddr, value);
            }
        }
    }
}
