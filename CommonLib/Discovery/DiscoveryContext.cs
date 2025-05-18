using System;
using System.Collections.Generic;
using System.Linq;
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
                    AddUnidentifiedPointer(addr);
                    count++;
                }
            }

            return count;
        }

        public void AddUnidentifiedPointer(uint addr) {
            if (addr % 4 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 4");

            // TODO: what if a pointer is already there?
            DiscoveredDataByAddress[addr] = new DiscoveredData(addr, 4, Types.DiscoveredDataType.Pointer, "*");
        }

        public Dictionary<uint, DiscoveredData[]> GetUnidentifiedPointersByValue() {
            return DiscoveredDataByAddress
                .Where(x => x.Value.IsUnidentifiedPointer)
                .Select(x => x.Value)
                .GroupBy(x => Data.GetUInt((int) (x.Address - Address)))
                .ToDictionary(x => x.Key, x => x.ToArray());
        }

        public byte[] Data { get; }
        public uint Address { get; }
        public Dictionary<uint, DiscoveredData> DiscoveredDataByAddress = new Dictionary<uint, DiscoveredData>();
    }
}
