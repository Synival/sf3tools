using System;
using System.Collections.Generic;

namespace CommonLib.Discovery {
    public class DiscoveryContext {
        public DiscoveryContext(byte[] dataCopy) {
            Data = dataCopy;
        }

        public void AddUnknownPointer(uint addr) {
            if (addr % 4 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 4");

            // TODO: what if a pointer is already there?
            DiscoveredDataByAddress[addr] = new DiscoveredData(addr, 4, Types.DiscoveredDataType.Pointer, "void*");
        }

        public byte[] Data { get; }
        public Dictionary<uint, DiscoveredData> DiscoveredDataByAddress = new Dictionary<uint, DiscoveredData>();
    }
}
