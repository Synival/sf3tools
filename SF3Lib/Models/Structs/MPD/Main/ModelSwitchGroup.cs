using CommonLib;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Main {
    public class ModelSwitchGroup : Struct, IMPD_ModelSwitchGroup {
        private readonly int _flagAddr;
        private readonly int _visibleModelsWhenFlagOffOffsetAddr;
        private readonly int _visibleModelsWhenFlagOnOffsetAddr;
        private readonly int _stateAddr;

        public ModelSwitchGroup(IByteData data, int id, string name, int address, INameGetterContext nameGetterContext)
        : base(data, id, name, address, 0x10) {
            NameGetterContext = nameGetterContext;

            _flagAddr                           = Address + 0x00; // 4 bytes
            _visibleModelsWhenFlagOffOffsetAddr = Address + 0x04; // 4 bytes
            _visibleModelsWhenFlagOnOffsetAddr  = Address + 0x08; // 4 bytes
            _stateAddr                          = Address + 0x0C; // 4 bytes

            if (Flag != -1) {
                var offOffset = VisibleModelsWhenFlagOffOffset;
                if (offOffset >= 0x00290000u)
                     ModelInstancesVisibleWhenOffTable = ModelIDTable.Create(data, nameof(ModelInstancesVisibleWhenOffTable), (int) (offOffset - 0x00290000u));

                var onOffset = VisibleModelsWhenFlagOnOffset;
                if (onOffset >= 0x00290000u)
                     ModelInstancesVisibleWhenOnTable = ModelIDTable.Create(data, nameof(ModelInstancesVisibleWhenOnTable), (int) (onOffset - 0x00290000u));
            }
        }

        public INameGetterContext NameGetterContext { get; }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_flagAddr), displayOrder: 0, displayFormat: "X3", minWidth: 200)]
        [NameGetter(NamedValueType.GameFlag)]
        public int Flag {
            get => Data.GetDouble(_flagAddr);
            set => Data.SetDouble(_flagAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_visibleModelsWhenFlagOffOffsetAddr), displayOrder: 1, displayName: "Visible Models (Flag Off)", isPointer: true)]
        public uint VisibleModelsWhenFlagOffOffset {
            get => (uint) Data.GetDouble(_visibleModelsWhenFlagOffOffsetAddr);
            set => Data.SetDouble(_visibleModelsWhenFlagOffOffsetAddr, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_visibleModelsWhenFlagOnOffsetAddr), displayOrder: 2, displayName: "Visible Models (Flag On)", isPointer: true)]
        public uint VisibleModelsWhenFlagOnOffset {
            get => (uint) Data.GetDouble(_visibleModelsWhenFlagOnOffsetAddr);
            set => Data.SetDouble(_visibleModelsWhenFlagOnOffsetAddr, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_stateAddr), displayOrder: 3, displayName: "State (set in-game)", displayFormat: "X2")]
        public uint State {
            get => (uint) Data.GetDouble(_stateAddr);
            set => Data.SetDouble(_stateAddr, (int) value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 4, displayName: "State (in editor)")]
        public bool StateInEditor { get; set; }

        public ModelIDTable ModelInstancesVisibleWhenOffTable { get; }
        public ModelIDTable ModelInstancesVisibleWhenOnTable { get; }

        public string DropdownName {
            get {
                var flagName = NameGetterContext.GetName(this, null, Flag, new object[] { NamedValueType.GameFlag });
                return $"{Name}: Flag {Flag:X3} ({flagName})";
            }
        }

        public IEnumerableWithLength<int> ModelInstancesVisibleWhenOff => ModelInstancesVisibleWhenOffTable;
        public IEnumerableWithLength<int> ModelInstancesVisibleWhenOn => ModelInstancesVisibleWhenOnTable;
    }
}
