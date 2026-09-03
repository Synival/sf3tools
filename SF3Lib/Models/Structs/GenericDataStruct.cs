using System;
using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs {
    public class GenericDataStruct<T> : Struct {
        public GenericDataStruct(
            IByteData data, int id, string name, int address, int size, Func<IByteData,
            int /*addr*/, T> getter, Action<IByteData, int /*addr*/, T> setter
        ) : base(data, id, name, address, size) {
            _valueAddr = Address + 0x00; // n bytes
            _getter = getter;
            _setter = setter;
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_valueAddr), minWidth: 120)]
        public T Value {
            get => _getter(Data, _valueAddr);
            set => _setter(Data, _valueAddr, value);
        }

        private readonly int _valueAddr;
        private readonly Func<IByteData, int /*addr*/, T> _getter;
        private readonly Action<IByteData, int /*addr*/, T> _setter;
    }
}
