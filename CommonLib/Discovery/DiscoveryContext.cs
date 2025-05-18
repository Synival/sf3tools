using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;
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

            // TODO: what if a something is already there?
            DiscoveredDataByAddress[addr] = new DiscoveredData(addr, 4, DiscoveredDataType.Pointer, "*");
        }

        public DiscoveredData AddFunction(uint addr, string name, int? size) {
            if (addr % 2 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 2");

            // TODO: what if a something is already there?
            // TODO: parameters?
            var newData = DiscoveredDataByAddress[addr] = new DiscoveredData(addr, size, DiscoveredDataType.Function, $"{name}(...)");
            UpdatePointersToDiscoveredData(DiscoveredDataByAddress[addr]);
            return newData;
        }

        public DiscoveredData AddArray(uint addr, string name, int? size) {
            if (addr % 4 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 4");

            // TODO: what if a something is already there?
            var newData = DiscoveredDataByAddress[addr] = new DiscoveredData(addr, size, DiscoveredDataType.Array, $"{name}[]");
            UpdatePointersToDiscoveredData(DiscoveredDataByAddress[addr]);
            return newData;
        }

        public DiscoveredData[] GetAll()
            => DiscoveredDataByAddress.Values.ToArray();

        public Dictionary<uint, DiscoveredData[]> GetUnidentifiedPointersByValue() {
            return DiscoveredDataByAddress.Values
                .Where(x => x.IsUnidentifiedPointer)
                .GroupBy(x => Data.GetUInt((int) (x.Address - Address)))
                .ToDictionary(x => x.Key, x => x.ToArray());
        }

        public Dictionary<uint, DiscoveredData[]> GetPointersByValue() {
            return DiscoveredDataByAddress.Values
                .Where(x => x.Type == DiscoveredDataType.Pointer)
                .GroupBy(x => Data.GetUInt((int) (x.Address - Address)))
                .ToDictionary(x => x.Key, x => x.ToArray());
        }

        public DiscoveredData[] GetUnidentifiedPointersByValue(uint ptrValue) {
            return DiscoveredDataByAddress.Values
                .Where(x => x.IsUnidentifiedPointer && Data.GetUInt((int) (x.Address - Address)) == ptrValue)
                .ToArray();
        }

        public DiscoveredData[] GetPointers() {
            return DiscoveredDataByAddress.Values
                .Where(x => x.Type == DiscoveredDataType.Pointer)
                .ToArray();
        }

        public DiscoveredData[] GetPointersByValue(uint ptrValue) {
            return DiscoveredDataByAddress.Values
                .Where(x => x.Type == DiscoveredDataType.Pointer && Data.GetUInt((int) (x.Address - Address)) == ptrValue)
                .ToArray();
        }

        public DiscoveredData[] GetFunctions() {
            return DiscoveredDataByAddress.Values
                .Where(x => x.Type == DiscoveredDataType.Function)
                .ToArray();
        }

        public DiscoveredData[] GetArrays() {
            return DiscoveredDataByAddress.Values
                .Where(x => x.Type == DiscoveredDataType.Array)
                .ToArray();
        }

        public int UpdatePointersToDiscoveredData(DiscoveredData data) {
            var pointers = GetUnidentifiedPointersByValue(data.Address);
            var newName = data.Name + "*";
            foreach (var pointer in pointers)
                pointer.Name = newName;
            return pointers.Length;
        }

        public byte[] Data { get; }
        public uint Address { get; }

        private Dictionary<uint, DiscoveredData> DiscoveredDataByAddress = new Dictionary<uint, DiscoveredData>();
    }
}
