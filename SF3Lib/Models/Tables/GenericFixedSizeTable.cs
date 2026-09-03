using System;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    public class GenericFixedSizeTable<T> : FixedSizeTable<GenericDataStruct<T>> {
        protected GenericFixedSizeTable(
            IByteData data, string name, string typeName, int address, int count, int typeSize,
            Func<IByteData, int /*addr*/, T> getter, Action<IByteData, int /*addr*/, T> setter
        ) : base(data, name, address, count) {
            TypeName = typeName;
            _typeSize = typeSize;
            _getter = getter;
            _setter = setter;
        }

        public static GenericFixedSizeTable<T> Create(
            IByteData data, string name, string typeName, int address, int count, int typeSize,
            Func<IByteData, int /*addr*/, T> getter, Action<IByteData, int /*addr*/, T> setter
        ) {
            return Create(() => new GenericFixedSizeTable<T>(data, name, typeName, address, count, typeSize, getter, setter));
        }

        public override bool Load()
            => Load((id, address) => new GenericDataStruct<T>(Data, id, $"{TypeName}_{id:D3}", address, _typeSize, _getter, _setter));

        public string TypeName { get; }

        private readonly int _typeSize;
        private readonly Func<IByteData, int /*addr*/, T> _getter;
        private readonly Action<IByteData, int /*addr*/, T> _setter;
    }
}
