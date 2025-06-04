using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib.Types;
using CommonLib.Utils;

namespace CommonLib.Discovery {
    public class DiscoveryContext {
        public DiscoveryContext(byte[] dataCopy, uint address) {
            Data = dataCopy;
            Address = address;
        }

        public int DiscoverUnknownPointersToValueRange(uint min, uint max) {
            int count = 0;

            // Look for anything that potentially could be a script (filter out obvious negatives)
            var dataAddrMax = Data.Length - 3;
            for (uint dataAddr = 0; dataAddr < dataAddrMax; dataAddr += 4) {
                var addr = dataAddr + Address;
                var value = Data.GetUInt((int) dataAddr);
                if (value >= min && value < max) {
                    if (value >= dataAddrMax + Address)
                        ;
                    AddUnidentifiedPointer(addr);
                    count++;
                }
            }

            return count;
        }

        public DiscoveredData AddUnidentifiedPointer(uint addr) {
            if (addr % 4 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 4");

            // Don't re-discover existing pointers.
            if (DiscoveredPointersByAddress.TryGetValue(addr, out var oldData))
                return oldData;

            AddUnknownAtPointerValue(addr);
            RemoveUnknownsAt(addr);
            var newData = DiscoveredPointersByAddress[addr] = new DiscoveredData(this, addr, 4, DiscoveredDataType.Pointer, "void*", "", Data.GetUInt((int) (addr - Address)));
            UpdatePointersToDiscoveredData(DiscoveredPointersByAddress[addr]);
            return newData;
        }

        public DiscoveredData AddPointer(uint addr, string typeName, string name) {
            if (addr % 4 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 4");

            AddUnknownAtPointerValue(addr);
            RemoveUnknownsAt(addr);
            // TODO: what if a something is already there?
            var newData = DiscoveredPointersByAddress[addr] = new DiscoveredData(this, addr, 4, DiscoveredDataType.Pointer, typeName, name, Data.GetUInt((int) (addr - Address)));
            UpdatePointersToDiscoveredData(DiscoveredPointersByAddress[addr]);
            return newData;
        }

        private void AddUnknownAtPointerValue(uint addr) {
            if (addr > Address && addr < Address + Data.Length - 3) {
                var value = Data.GetUInt((int) (addr - Address));
                if (!HasDiscoveryAt(value))
                    // TODO: track what's pointing to it?
                    DiscoveredUnknownsByAddress[value] = new DiscoveredData(this, value, null, DiscoveredDataType.Unknown, "Unknown", "unknown", null);
            }
        }

        public DiscoveredData AddFunction(uint addr, string typeName, string name, int? size) {
            if (addr % 2 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 2");

            // TODO: what if a something is already there?
            // TODO: parameters?
            RemoveUnknownsAt(addr);
            var newData = DiscoveredFunctionsByAddress[addr] = new DiscoveredData(this, addr, size, DiscoveredDataType.Function, typeName, name, null);
            UpdatePointersToDiscoveredData(DiscoveredFunctionsByAddress[addr]);
            return newData;
        }

        public DiscoveredData AddArray(uint addr, string typeName, string name, int? size) {
            if (addr % 2 != 0)
                throw new ArgumentException(nameof(addr) + " must have an alignment of 2");

            // TODO: what if a something is already there?
            var newData = DiscoveredArraysByAddress[addr] = new DiscoveredData(this, addr, size, DiscoveredDataType.Array, typeName, name, null);
            RemoveUnknownsAt(addr);
            UpdatePointersToDiscoveredData(DiscoveredArraysByAddress[addr]);
            return newData;
        }

        public DiscoveredData[] GetAllOrdered() {
            var discoveredList = new List<DiscoveredData>();
            discoveredList.AddRange(DiscoveredFunctionsByAddress.Values);
            discoveredList.AddRange(DiscoveredArraysByAddress.Values);
            discoveredList.AddRange(DiscoveredStructsByAddress.Values);
            discoveredList.AddRange(DiscoveredPointersByAddress.Values);
            discoveredList.AddRange(DiscoveredUnknownsByAddress.Values);
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

        public DiscoveredData[] GetFunctions()
            => DiscoveredFunctionsByAddress.Values.ToArray();

        public DiscoveredData[] GetArrays()
            => DiscoveredArraysByAddress.Values.ToArray();

        public DiscoveredData[] GetStructs()
            => DiscoveredStructsByAddress.Values.ToArray();

        public DiscoveredData[] GetUnknowns()
            => DiscoveredUnknownsByAddress.Values.ToArray();

        public int UpdatePointersToDiscoveredData(DiscoveredData data) {
            if (data.Address < Address || data.Address >= Address + Data.Length)
                DiscoverUnknownPointersToValueRange(data.Address, data.Address + 4);

            var pointers = GetUnidentifiedPointersByValue(data.Address);
            var newType = data.TypeName + "*";
            var newName = $"ptr_{(data.Name == "" ? null : data.Name) ?? "unnamed"}";
            var updates = pointers.Length;
            foreach (var pointer in pointers) {
                RemoveUnknownsAt(pointer.Address);
                pointer.TypeName = newType;
                pointer.Name = newName;
                updates += UpdatePointersToDiscoveredData(pointer);
            }
            return updates;
        }

        public void RemoveUnknownsAt(uint addr) {
            if (DiscoveredUnknownsByAddress.ContainsKey(addr))
                DiscoveredUnknownsByAddress.Remove(addr);
        }

        private class CreateReportRow {
            public int Indentation;
            public DiscoveredData Data;
        }

        public string CreateReport()
            => CreateReport(GetAllOrdered(), true);

        public string CreateReport(DiscoveredData[] dataSet, bool withType) {
            var discoveries = dataSet
                .OrderBy(x => x.Address)
                .ThenBy(x => x.Type)
                .Select(x => new CreateReportRow { Indentation = 0, Data = x })
                .ToArray();

            for (int i = 0; i < discoveries.Length; i++) {
                var d = discoveries[i];
                var data = d.Data;
                if (data.HasNestedData) {
                    var endAddr = data.Address + data.Size.Value;
                    for (int j = i + 1; j < discoveries.Length; j++) {
                        var d2 = discoveries[j];
                        if (d2.Data.Address < endAddr)
                            d2.Indentation++;
                        else
                            break;
                    }
                }
            }

            var sb = new StringBuilder();

            for (int i = 0; i < discoveries.Length; i++) {
                var d = discoveries[i];
                var data = d.Data;
                var hasNestedData = data.HasNestedData;
                var nextD = (i + 1 < discoveries.Length) ? discoveries[i + 1] : null;
                var nextIndentation = nextD?.Indentation ?? 0;
                var autoCloseBrace = hasNestedData && nextIndentation <= d.Indentation;
                var targetIndentation = (hasNestedData && !autoCloseBrace) ? d.Indentation + 1 : d.Indentation;

                var addrWithSign = ValueUtils.SignedHexStr(data.Address - Address, "X4");
                var typeStr = withType ? $" | {data.Type,-8}" : "";
                var addr =
                    ((data.Address < Address || data.Address >= Address + Data.Length) ? "!" : " ")
                    + $"0x{data.Address:X8} /{addrWithSign,8}{typeStr}";
                var code = new string(' ', d.Indentation * 3) + data.DisplayName + (hasNestedData ? (autoCloseBrace ? " {}" : " {") : "");

                sb.AppendLine($"{addr} | {code}");
                while (targetIndentation > nextIndentation)
                    sb.AppendLine($"                      |          | {new string(' ', --targetIndentation * 3)}}}");
            }

            return sb.ToString();
        }

        public DiscoveredData GetDiscoveryAt(uint addr) {
            DiscoveredData d;
            if (DiscoveredFunctionsByAddress.TryGetValue(addr, out d))
                return d;
            if (DiscoveredArraysByAddress.TryGetValue(addr, out d))
                return d;
            if (DiscoveredStructsByAddress.TryGetValue(addr, out d))
                return d;
            if (DiscoveredPointersByAddress.TryGetValue(addr, out d))
                return d;
            if (DiscoveredUnknownsByAddress.TryGetValue(addr, out d))
                return d;
            return null;
        }

        public bool HasDiscoveryAt(uint addr) {
            return
                DiscoveredFunctionsByAddress.ContainsKey(addr) ||
                DiscoveredArraysByAddress.ContainsKey(addr)    ||
                DiscoveredStructsByAddress.ContainsKey(addr)   ||
                DiscoveredPointersByAddress.ContainsKey(addr)  ||
                DiscoveredUnknownsByAddress.ContainsKey(addr);
        }

        public byte[] Data { get; }
        public uint Address { get; }
        public bool HasDiscoveries => DiscoveredFunctionsByAddress.Count > 0 || DiscoveredArraysByAddress.Count > 0 || DiscoveredStructsByAddress.Count > 0 || DiscoveredPointersByAddress.Count > 0;

        private Dictionary<uint, DiscoveredData> DiscoveredFunctionsByAddress = new Dictionary<uint, DiscoveredData>();
        private Dictionary<uint, DiscoveredData> DiscoveredArraysByAddress    = new Dictionary<uint, DiscoveredData>();
        private Dictionary<uint, DiscoveredData> DiscoveredStructsByAddress   = new Dictionary<uint, DiscoveredData>();
        private Dictionary<uint, DiscoveredData> DiscoveredPointersByAddress  = new Dictionary<uint, DiscoveredData>();
        private Dictionary<uint, DiscoveredData> DiscoveredUnknownsByAddress  = new Dictionary<uint, DiscoveredData>();
    }
}
