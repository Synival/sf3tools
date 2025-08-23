using System;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    /// <summary>
    /// Base implementation for a specific table of SF3 data that can be modified.
    /// </summary>
    public abstract class AddressedTable<T> : Table<T>, IAddressedTable<T> where T : class, IStruct {
        protected AddressedTable(IByteData data, string name, int[] addresses) : base(data, name, 0 /* TODO: would be nice if we didn't even have this */) {
            Addresses = addresses;
        }

        /// <summary>
        /// Loads all rows until Size is reached.
        /// </summary>
        /// <param name="makeTFunc">Factory function to make the model.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool Load(Func<int, int, T> makeTFunc) {
            _rows = new T[Addresses.Length];
            try {
                for (var id = 0; id < Addresses.Length; ++id)
                    _rows[id] = makeTFunc(id, Addresses[id]);
            }
            catch {
                return false;
            }
            return true;
        }

        public int[] Addresses { get; }
        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;
    }
}
