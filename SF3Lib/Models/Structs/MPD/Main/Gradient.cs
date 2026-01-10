using System;
using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.MPD;

namespace SF3.Models.Structs.MPD.Main {
    public class Gradient : Struct, IMPD_Gradient {
        private readonly int _topPositionAddr;
        private readonly int _bottomPositionAddr;
        private readonly int _topRAddr;
        private readonly int _topGAddr;
        private readonly int _topBAddr;
        private readonly int _bottomRAddr;
        private readonly int _bottomGAddr;
        private readonly int _bottomBAddr;
        private readonly int _partsAffectedBitsAddr;
        private readonly int _groundIntensityAddr;
        private readonly int _skyIntensityAddr;
        private readonly int _modelsAndSurfaceIntensityAddr;

        public Gradient(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x18) {
            _topPositionAddr           = Address + 0x00; // 2 bytes
            _bottomPositionAddr        = Address + 0x02; // 2 bytes
            _topRAddr                  = Address + 0x04; // 2 bytes
            _topGAddr                  = Address + 0x06; // 2 bytes
            _topBAddr                  = Address + 0x08; // 2 bytes
            _bottomRAddr               = Address + 0x0A; // 2 bytes
            _bottomGAddr               = Address + 0x0C; // 2 bytes
            _bottomBAddr               = Address + 0x0E; // 2 bytes
            _partsAffectedBitsAddr     = Address + 0x10; // 2 bytes
            _groundIntensityAddr       = Address + 0x12; // 2 bytes
            _skyIntensityAddr          = Address + 0x14; // 2 bytes
            _modelsAndSurfaceIntensityAddr = Address + 0x16; // 2 bytes

            _gradientTopColor        = new GradientTopColorClass(this);
            _gradientBottomColor     = new GradientBottomColorClass(this);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_topPositionAddr), displayName: "TopPosition", displayOrder: 0, displayFormat: "X2")]
        public ushort TopPositionRaw {
            get => (ushort) Data.GetWord(_topPositionAddr);
            set => Data.SetWord(_topPositionAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_bottomPositionAddr), displayName: "BottomPosition", displayOrder: 1, displayFormat: "X2")]
        public ushort BottomPositionRaw {
            get => (ushort) Data.GetWord(_bottomPositionAddr);
            set => Data.SetWord(_bottomPositionAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_topRAddr), displayOrder: 2, displayFormat: "X2")]
        public ushort TopR {
            get => (ushort) Data.GetWord(_topRAddr);
            set => Data.SetWord(_topRAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_topGAddr), displayOrder: 3, displayFormat: "X2")]
        public ushort TopG {
            get => (ushort) Data.GetWord(_topGAddr);
            set => Data.SetWord(_topGAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_topBAddr), displayOrder: 4, displayFormat: "X2")]
        public ushort TopB {
            get => (ushort) Data.GetWord(_topBAddr);
            set => Data.SetWord(_topBAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_bottomRAddr), displayOrder: 5, displayFormat: "X2")]
        public ushort BottomR {
            get => (ushort) Data.GetWord(_bottomRAddr);
            set => Data.SetWord(_bottomRAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_bottomGAddr), displayOrder: 6, displayFormat: "X2")]
        public ushort BottomG {
            get => (ushort) Data.GetWord(_bottomGAddr);
            set => Data.SetWord(_bottomGAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_bottomBAddr), displayOrder: 7, displayFormat: "X2")]
        public ushort BottomB {
            get => (ushort) Data.GetWord(_bottomBAddr);
            set => Data.SetWord(_bottomBAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_partsAffectedBitsAddr), displayOrder: 8, displayFormat: "X1")]
        public ushort PartsAffectedBits {
            get => (ushort) Data.GetWord(_partsAffectedBitsAddr);
            set => Data.SetWord(_partsAffectedBitsAddr, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 8.1f)]
        public bool AffectsGround {
            get => (PartsAffectedBits & 0x01) == 0x01;
            set => PartsAffectedBits = (ushort) (PartsAffectedBits & ~0x01 | (value ? 0x01 : 0x00));
        }

        [TableViewModelColumn(addressField: null, displayOrder: 8.2f)]
        public bool AffectsSky {
            get => (PartsAffectedBits & 0x02) == 0x02;
            set => PartsAffectedBits = (ushort) (PartsAffectedBits & ~0x02 | (value ? 0x02 : 0x00));
        }

        [TableViewModelColumn(addressField: null, displayOrder: 8.3f)]
        public bool AffectsModelsAndSurface {
            get => (PartsAffectedBits & 0x04) == 0x04;
            set => PartsAffectedBits = (ushort) (PartsAffectedBits & ~0x04 | (value ? 0x04 : 0x00));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundIntensityAddr), displayName: "GroundItensity", displayOrder: 9, displayFormat: "X2")]
        public ushort GroundIntensityRaw {
            get => (ushort) Data.GetWord(_groundIntensityAddr);
            set => Data.SetWord(_groundIntensityAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_skyIntensityAddr), displayName: "SkyIntensity", displayOrder: 10, displayFormat: "X2")]
        public ushort SkyIntensityRaw {
            get => (ushort) Data.GetWord(_skyIntensityAddr);
            set => Data.SetWord(_skyIntensityAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_modelsAndSurfaceIntensityAddr), displayName: "ModelsAndSurfaceIntensity", displayOrder: 11, displayFormat: "X2")]
        public ushort ModelsAndSurfaceIntensityRaw {
            get => (ushort) Data.GetWord(_modelsAndSurfaceIntensityAddr);
            set => Data.SetWord(_modelsAndSurfaceIntensityAddr, value);
        }

        public float TopPosition {
            get => TopPositionRaw / 255.0f;
            set => TopPositionRaw = (byte) Math.Round(value * 0xFF);
        }

        public float BottomPosition {
            get => BottomPositionRaw / 255.0f;
            set => BottomPositionRaw = (byte) Math.Round(value * 0xFF);
        }

        private class GradientTopColorClass : IColorRGB555 {
            public GradientTopColorClass(Gradient gradient) {
                Gradient = gradient;
            }

            public readonly Gradient Gradient;

            public byte R { get => (byte) Gradient.TopR; set => Gradient.TopR = value; }
            public byte G { get => (byte) Gradient.TopG; set => Gradient.TopG = value; }
            public byte B { get => (byte) Gradient.TopB; set => Gradient.TopB = value; }
        }

        private GradientTopColorClass _gradientTopColor;
        public IColorRGB555 TopColor {
            get => _gradientTopColor;
            set {
                if (value != null) {
                    _gradientTopColor.R = value.R;
                    _gradientTopColor.G = value.G;
                    _gradientTopColor.B = value.B;
                }
            }
        }

        private class GradientBottomColorClass : IColorRGB555 {
            public GradientBottomColorClass(Gradient gradient) {
                Gradient = gradient;
            }

            public readonly Gradient Gradient;

            public byte R { get => (byte) Gradient.BottomR; set => Gradient.BottomR = value; }
            public byte G { get => (byte) Gradient.BottomG; set => Gradient.BottomG = value; }
            public byte B { get => (byte) Gradient.BottomB; set => Gradient.BottomB = value; }
        }

        private GradientBottomColorClass _gradientBottomColor;
        public IColorRGB555 BottomColor {
            get => _gradientBottomColor;
            set {
                if (value != null) {
                    _gradientBottomColor.R = value.R;
                    _gradientBottomColor.G = value.G;
                    _gradientBottomColor.B = value.B;
                }
            }
        }

        public float GroundIntensity {
            get => GroundIntensityRaw / (float) 0x1F;
            set => GroundIntensityRaw = (byte) Math.Round(value / 0x1F * 255);
        }

        public float SkyIntensity {
            get => SkyIntensityRaw / (float) 0x1F;
            set => SkyIntensityRaw = (byte) Math.Round(value * 0x1F);
        }

        public float ModelsAndSurfaceIntensity {
            get => ModelsAndSurfaceIntensityRaw / (float) 0x1F;
            set => ModelsAndSurfaceIntensityRaw = (byte) Math.Round(value * 0x1F);
        }
    }
}
