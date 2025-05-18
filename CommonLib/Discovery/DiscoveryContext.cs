using System;
using System.Collections.Generic;
using CommonLib.Utils;

namespace CommonLib.Discovery {
    public class DiscoveryContext {
        public DiscoveryContext(byte[] dataCopy, uint address) {
            Data = dataCopy;
            Address = address;
        }

        public int DiscoverUnknownPointersWithinValueRange(uint min, uint max) {
            int count = 0;

            // Look for anything that potentially could be a script (filter out obvious negatives)
            var dataAddrMax = Data.Length - 3;
            for (uint dataAddr = 0; dataAddr < dataAddrMax; dataAddr += 4) {
                var addr = dataAddr + Address;
                var value = Data.GetUInt((int) dataAddr);
                if (value >= min && value < max) {
                    AddUnknownPointer(addr);
                    count++;
                }
            }

            return count;
        }

        public void AddUnknownPointer(uint addr) {
            if (addr % 4 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 4");

            // TODO: what if a pointer is already there?
            DiscoveredDataByAddress[addr] = new DiscoveredData(addr, 4, Types.DiscoveredDataType.Pointer, "void*");
        }

        public byte[] Data { get; }
        public uint Address { get; }
        public Dictionary<uint, DiscoveredData> DiscoveredDataByAddress = new Dictionary<uint, DiscoveredData>();
    }
}
