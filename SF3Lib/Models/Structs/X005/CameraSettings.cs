using System;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X005 {
    public class CameraSettings : Struct {
        private readonly int _setting1DistAddr;
        private readonly int _setting2DistAddr;
        private readonly int _setting3DistAddr;
        private readonly int _baseAngleAddr;
        private readonly int _setting2AngleAdjustAddr;
        private readonly int _setting3AngleAdjustAddr;

        public CameraSettings(IByteData data, int id, string name, ScenarioType scenario)
        : base(data, id, name, 0x00 /* address not applicable here */, 0x00 /* size not applicable here */) {
            switch (scenario) {
                case ScenarioType.Scenario1:
                    _setting1DistAddr = 0x42A0;
                    _setting2DistAddr = 0x42A8;
                    _setting3DistAddr = 0x434C; // This doesn't seem right... AC?
                    _baseAngleAddr    = 0x453A;
                    // What is 0x4274?
                    _setting2AngleAdjustAddr = 0x427C;
                    _setting3AngleAdjustAddr = 0x4340;
                    break;

                case ScenarioType.Scenario2:
                    _setting1DistAddr = 0x4300;
                    _setting2DistAddr = 0x4308;
                    _setting3DistAddr = 0x430C;
                    _baseAngleAddr    = 0x465A;
                    _setting2AngleAdjustAddr = 0x42F4;
                    _setting3AngleAdjustAddr = 0x42F6;
                    break;

                case ScenarioType.Scenario3:
                    _setting1DistAddr = 0x42EC;
                    _setting2DistAddr = 0x42F4;
                    _setting3DistAddr = 0x42F8;
                    _baseAngleAddr    = 0x4646;
                    _setting2AngleAdjustAddr = 0x42E0;
                    _setting3AngleAdjustAddr = 0x42E2;
                    break;

                case ScenarioType.PremiumDisk:
                    _setting1DistAddr = 0x42FC;
                    _setting2DistAddr = 0x4304;
                    _setting3DistAddr = 0x4308;
                    _baseAngleAddr    = 0x4656;
                    _setting2AngleAdjustAddr = 0x42F0;
                    _setting3AngleAdjustAddr = 0x42F2;
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
        public ushort BaseAngle {
            get => (ushort) Data.GetWord(_baseAngleAddr);
            set => Data.SetWord(_baseAngleAddr, value);
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
    }
}
