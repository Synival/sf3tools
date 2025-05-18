using System.Collections.Generic;

namespace CommonLib.Discovery {
    public class DiscoveryContext {
        public DiscoveryContext(byte[] dataCopy) {
            Data = dataCopy;
        }

        public byte[] Data { get; }
        public Dictionary<uint, DiscoveredData> DiscoveredDataByAddress = new Dictionary<uint, DiscoveredData>();
    }
}
