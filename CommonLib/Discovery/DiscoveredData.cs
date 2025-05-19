using CommonLib.Types;

namespace CommonLib.Discovery {
    public class DiscoveredData {
        public DiscoveredData(uint address, int? size, DiscoveredDataType type, string typeName, string name) {
            Address  = address;
            Size     = size;
            Type     = type;
            TypeName = typeName ?? "";
            Name     = name ?? "";
        }

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

        private string GetDisplayName() {
            return $"{TypeName ?? ""} {Name ?? ""}";
        }

        public string _displayName = null;
        public string DisplayName => _displayName;

        public bool IsUnidentifiedPointer => Type == DiscoveredDataType.Pointer && Name == "";
    }
}
