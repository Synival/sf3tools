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

        public DiscoveredData AddUnidentifiedPointer(uint addr) {
            if (addr % 4 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 4");

            // TODO: what if a something is already there?
            var newData = DiscoveredPointersByAddress[addr] = new DiscoveredData(addr, 4, DiscoveredDataType.Pointer, "void*", "");
            UpdatePointersToDiscoveredData(DiscoveredPointersByAddress[addr]);
            return newData;
        }

        public DiscoveredData AddPointer(uint addr, string typeName, string name) {
            if (addr % 4 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 4");

            // TODO: what if a something is already there?
            var newData = DiscoveredPointersByAddress[addr] = new DiscoveredData(addr, 4, DiscoveredDataType.Pointer, typeName, name);
            UpdatePointersToDiscoveredData(DiscoveredPointersByAddress[addr]);
            return newData;
        }

        public DiscoveredData AddFunction(uint addr, string typeName, string name, int? size) {
            if (addr % 2 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 2");

            // TODO: what if a something is already there?
            // TODO: parameters?
            var newData = DiscoveredDataByAddress[addr] = new DiscoveredData(addr, size, DiscoveredDataType.Function, typeName, name);
            UpdatePointersToDiscoveredData(DiscoveredDataByAddress[addr]);
            return newData;
        }

        public DiscoveredData AddArray(uint addr, string typeName, string name, int? size) {
            if (addr % 4 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 4");

            // TODO: what if a something is already there?
            var newData = DiscoveredArraysByAddress[addr] = new DiscoveredData(addr, size, DiscoveredDataType.Array, typeName, name);
            UpdatePointersToDiscoveredData(DiscoveredArraysByAddress[addr]);
            return newData;
        }

        public DiscoveredData[] GetAll() {
            var discoveredList = new List<DiscoveredData>();
            discoveredList.AddRange(DiscoveredDataByAddress.Values);
            discoveredList.AddRange(DiscoveredArraysByAddress.Values);
            discoveredList.AddRange(DiscoveredStructsByAddress.Values);
            discoveredList.AddRange(DiscoveredPointersByAddress.Values);
            return discoveredList.OrderBy(x => x.Address).ThenBy(x => x.Type).ToArray();
        }

        public Dictionary<uint, DiscoveredData[]> GetUnidentifiedPointersByValue() {
            return DiscoveredPointersByAddress.Values
                .Where(x => x.IsUnidentifiedPointer)
                .GroupBy(x => Data.GetUInt((int) (x.Address - Address)))
                .ToDictionary(x => x.Key, x => x.ToArray());
        }

        public Dictionary<uint, DiscoveredData[]> GetPointersByValue() {
            return DiscoveredPointersByAddress.Values
                .Where(x => x.Type == DiscoveredDataType.Pointer)
                .GroupBy(x => Data.GetUInt((int) (x.Address - Address)))
                .ToDictionary(x => x.Key, x => x.ToArray());
        }

        public DiscoveredData[] GetUnidentifiedPointersByValue(uint ptrValue) {
            return DiscoveredPointersByAddress.Values
                .Where(x => x.IsUnidentifiedPointer && Data.GetUInt((int) (x.Address - Address)) == ptrValue)
                .ToArray();
        }

        public DiscoveredData[] GetPointers()
            => DiscoveredPointersByAddress.Values.ToArray();

        public DiscoveredData[] GetPointersByValue(uint ptrValue) {
            return DiscoveredPointersByAddress.Values
                .Where(x => Data.GetUInt((int) (x.Address - Address)) == ptrValue)
                .ToArray();
        }

        public DiscoveredData[] GetFunctions() {
            return DiscoveredDataByAddress.Values
                .Where(x => x.Type == DiscoveredDataType.Function)
                .ToArray();
        }

        public DiscoveredData[] GetArrays()
            => DiscoveredArraysByAddress.Values.ToArray();

        public DiscoveredData[] GetStructs()
            => DiscoveredStructsByAddress.Values.ToArray();

        public int UpdatePointersToDiscoveredData(DiscoveredData data) {
            var pointers = GetUnidentifiedPointersByValue(data.Address);
            var newType = data.TypeName + "*";
            var newName = data.Name;
            var updates = pointers.Length;
            foreach (var pointer in pointers) {
                pointer.TypeName = newType;
                pointer.Name = newName;
                updates += UpdatePointersToDiscoveredData(pointer);
            }
            return updates;
        }

        public byte[] Data { get; }
        public uint Address { get; }

        private Dictionary<uint, DiscoveredData> DiscoveredArraysByAddress   = new Dictionary<uint, DiscoveredData>();
        private Dictionary<uint, DiscoveredData> DiscoveredStructsByAddress  = new Dictionary<uint, DiscoveredData>();
        private Dictionary<uint, DiscoveredData> DiscoveredPointersByAddress = new Dictionary<uint, DiscoveredData>();
        private Dictionary<uint, DiscoveredData> DiscoveredDataByAddress     = new Dictionary<uint, DiscoveredData>();
    }
}
