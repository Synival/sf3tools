using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    /// <summary>
    /// Base implementation for a specific table of SF3 data that can be modified.
    /// </summary>
    public abstract class FixedSizeTable<T> : Table<T>, IFixedSizeTable<T> where T : class, IStruct {
        protected FixedSizeTable(IByteData data, string name, int address, int size) : base(data, name, address) {
            Size = size;
        }

        /// <summary>
        /// Loads all rows until Size is reached.
        /// </summary>
        /// <param name="makeTFunc">Factory function to make the model.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool Load(Func<int, int, T> makeTFunc) {
            _rows = new T[Size];

            try {
                var address = Address;
                for (var id = 0; id < Size; ++id) {
                    _rows[id] = makeTFunc(id, address);
                    address += _rows[id].Size;
                }
            }
            catch {
                return false;
            }
            return true;
        }

        public int Size { get; }
    }
}
