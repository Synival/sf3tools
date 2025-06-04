using System;
using System.Linq;
using CommonLib.Types;

namespace CommonLib.Discovery {
    public class DiscoveredData {
        public DiscoveredData(DiscoveryContext context, uint address, int? size, DiscoveredDataType type, string typeName, string name, uint? value) {
            Context  = context;
            Address  = address;
            Size     = size;
            Type     = type;
            TypeName = typeName ?? "";
            Name     = name ?? "";
            Value    = value;
        }

        public DiscoveryContext Context { get; }
        public uint Address { get; set; }
        public int? Size { get; set; }

        private DiscoveredDataType _type = (DiscoveredDataType) (-1);
        public DiscoveredDataType Type {
            get => _type;
            set {
                if (_type != value) {
                    _type = value;
                    _displayName = GetDisplayName();
                }
            }
        }

        private string _typeName = null;
        public string TypeName {
            get => _typeName;
            set {
                if (_typeName != value) {
                    _typeName = value;
                    _displayName = GetDisplayName();
                }
            }
        }

        private string _name = null;
        public string Name {
            get => _name;
            set {
                if (_name != value) {
                    _name = value;
                    _displayName = GetDisplayName();
                }
            }
        }

        private uint? _value = null;
        public uint? Value {
            get => _value;
            set {
                if (_value != value) {
                    _value = value;
                    _displayName = GetDisplayName();
                }
            }
        }

        private string GetDisplayName() {
            var strings = new string[4];
            int i = 0;

            if (TypeName?.Length > 0)
                strings[i++] = TypeName;
            if (Name?.Length > 0)
                strings[i++] = Name;
            if (Size.HasValue && Value.HasValue) {
                strings[i++] = "(0x" + Value.Value.ToString("X" + Math.Min(8, Math.Max(2, Size.Value * 2)));

                bool addedValue = false;
                if (Type == DiscoveredDataType.Pointer) {
                    var d = Context.GetDiscoveryAt(Value.Value);
                    if (d != null) {
                        strings[i++] = $"-> {d.DisplayName})";
                        addedValue = true;
                    }
                }
                if (!addedValue)
                    strings[i - 1] += ")";
            }

            return string.Join(" ", strings.Take(i));
        }

        public string _displayName = null;
        public string DisplayName => _displayName;

        public bool IsUnidentifiedPointer => Type == DiscoveredDataType.Pointer && Name == "";
        public bool HasNestedData => Size.HasValue && (Type == DiscoveredDataType.Function || Type == DiscoveredDataType.Array || Type == DiscoveredDataType.Struct);
    }
}
