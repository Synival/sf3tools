using System;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X005 {
    public class CameraSettings : Struct {
        private readonly int _setting1DistAddr;
        private readonly int _setting2DistAddr;
        private readonly int _setting3DistAddr;
        private readonly int _setting1AngleAddr;
        private readonly int _setting2AngleAdjustAddr;
        private readonly int _setting3AngleAdjustAddr;
        private readonly int _verticalOffsetAddr;

        public CameraSettings(IByteData data, int id, string name, ScenarioType scenario)
        : base(data, id, name, 0x00 /* address not applicable here */, 0x00 /* size not applicable here */) {
            switch (scenario) {
                case ScenarioType.Scenario1:
                    _setting1DistAddr        = 0x42A0;
                    _setting2DistAddr        = 0x42A8;
                    _setting3DistAddr        = 0x434C;
                    _setting1AngleAddr       = 0x453A;
                    _setting2AngleAdjustAddr = 0x427C;
                    _setting3AngleAdjustAddr = 0x4340;
                    _verticalOffsetAddr      = 0x4580;
                    break;

                case ScenarioType.Scenario2:
                    _setting1DistAddr        = 0x4300;
                    _setting2DistAddr        = 0x4308;
                    _setting3DistAddr        = 0x430C;
                    _setting1AngleAddr       = 0x465A;
                    _setting2AngleAdjustAddr = 0x42F4;
                    _setting3AngleAdjustAddr = 0x42F6;
                    _verticalOffsetAddr      = 0x46A0;
                    break;

                case ScenarioType.Scenario3:
                    _setting1DistAddr        = 0x42EC;
                    _setting2DistAddr        = 0x42F4;
                    _setting3DistAddr        = 0x42F8;
                    _setting1AngleAddr       = 0x4646;
                    _setting2AngleAdjustAddr = 0x42E0;
                    _setting3AngleAdjustAddr = 0x42E2;
                    _verticalOffsetAddr      = 0x468C;
                    break;

                case ScenarioType.PremiumDisk:
                    _setting1DistAddr        = 0x42FC;
                    _setting2DistAddr        = 0x4304;
                    _setting3DistAddr        = 0x4308;
                    _setting1AngleAddr       = 0x4656;
                    _setting2AngleAdjustAddr = 0x42F0;
                    _setting3AngleAdjustAddr = 0x42F2;
                    _verticalOffsetAddr      = 0x469C;
                    break;

                default:
                    throw new ArgumentException("Scenario not supported: " + scenario.ToString());
            }
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X4")]
        [BulkCopy]
        public ushort Setting1Dist {
            get => (ushort) Data.GetWord(_setting1DistAddr);
            set => Data.SetWord(_setting1DistAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X4")]
        [BulkCopy]
        public ushort Setting2Dist {
            get => (ushort) Data.GetWord(_setting2DistAddr);
            set => Data.SetWord(_setting2DistAddr, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X4")]
        [BulkCopy]
        public ushort Setting3Dist {
            get => (ushort) Data.GetWord(_setting3DistAddr);
            set => Data.SetWord(_setting3DistAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X4")]
        [BulkCopy]
        public ushort Setting1Angle {
            get => (ushort) Data.GetWord(_setting1AngleAddr);
            set => Data.SetWord(_setting1AngleAddr, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X4")]
        [BulkCopy]
        public ushort Setting2AngleAdjust {
            get => (ushort) Data.GetWord(_setting2AngleAdjustAddr);
            set => Data.SetWord(_setting2AngleAdjustAddr, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayFormat: "X4")]
        [BulkCopy]
        public ushort Setting3AngleAdjust {
            get => (ushort) Data.GetWord(_setting3AngleAdjustAddr);
            set => Data.SetWord(_setting3AngleAdjustAddr, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayFormat: "X4")]
        [BulkCopy]
        public ushort VerticalOffset {
            get => (ushort) Data.GetWord(_verticalOffsetAddr);
            set => Data.SetWord(_verticalOffsetAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3.1f, displayName: nameof(Setting2Angle) + " (calc'd)", displayFormat: "X4")]
        public ushort Setting2Angle {
            get => (ushort) ((short) Setting1Angle + (short) Setting2AngleAdjust);
            set => Setting2AngleAdjust = (ushort) ((short) value - (short) Setting1Angle);
        }

        [TableViewModelColumn(displayOrder: 3.2f, displayName: nameof(Setting3Angle) + " (calc'd)", displayFormat: "X4")]
        public ushort Setting3Angle {
            get => (ushort) ((short) Setting1Angle + (short) Setting3AngleAdjust);
            set => Setting3AngleAdjust = (ushort) ((short) value - (short) Setting1Angle);
        }
    }
}
