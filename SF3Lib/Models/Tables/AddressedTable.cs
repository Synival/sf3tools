using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    /// <summary>
    /// Base implementation for a specific table of SF3 data that can be modified.
    /// </summary>
    public abstract class AddressedTable<T> : BaseTable<T>, IAddressedTable<T> where T : class, IStruct {
        protected AddressedTable(IByteData data, int[] addresses) : base(data, 0 /* TODO: would be nice if we didn't even have this */) {
            Addresses = addresses;
        }

        /// <summary>
        /// Loads all rows until Size is reached.
        /// </summary>
        /// <param name="makeTFunc">Factory function to make the model.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool Load(Func<int, int, T> makeTFunc) {
            var rowDict = new Dictionary<int, T>();
            var rows = new List<T>();

            try {
                for (var id = 0; id < Addresses.Length; ++id) {
                    var newModel = makeTFunc(id, Addresses[id]);
                    rowDict[id] = newModel;
                    rows.Add(newModel);
                }
            }
            catch {
                return false;
            }
            finally {
                _rows = rows.ToArray();
            }
            return true;
        }

        public int[] Addresses { get; }
    }
}
