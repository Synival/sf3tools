using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    /// <summary>
    /// Base implementation for a specific table of SF3 data that can be modified.
    /// </summary>
    public abstract class FixedSizeTable<T> : BaseTable<T>, IFixedSizeTable<T> where T : class, IStruct {
        protected FixedSizeTable(IByteData data, int address, int size) : base(data, address) {
            Size = size;
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
                var address = Address;
                for (var id = 0; id < Size; ++id) {
                    var newModel = makeTFunc(id, address);

                    rowDict[id] = newModel;
                    rows.Add(newModel);
                    address += newModel.Size;
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

        public int Size { get; }
    }
}
