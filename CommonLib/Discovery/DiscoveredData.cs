using CommonLib.Types;

namespace CommonLib.Discovery {
    public class DiscoveredData {
        public DiscoveredData(int address, int? size, DiscoveredDataType type, string name) {
            Address = address;
            Size    = size;
            Type    = type;
            Name    = name;
        }

        public int Address { get; set; }
        public int? Size { get; set; }
        public DiscoveredDataType Type { get; set; }
        public string Name { get; set; }
    }
}
