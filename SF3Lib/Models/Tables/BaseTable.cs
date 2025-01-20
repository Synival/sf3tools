using System.Collections;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    /// <summary>
    /// Base implementation for a specific table of SF3 data that can be modified.
    /// </summary>
    public abstract class BaseTable<T> : IBaseTable, IBaseTable<T> where T : class, IStruct {
        /// <summary>
        /// Function to determine whether or not reading from a table should continue.
        /// If 'false' is returned, the 'currentModel' will not be committed.
        /// </summary>
        /// <param name="currentRows">The dictionary of rows by ID read thus far.</param>
        /// <param name="newModel">The last model read from the table.</param>
        /// <returns>'true' if reading should continue, 'false' if reading should not continue.</returns>
        public delegate bool ContinueReadingPredicate(Dictionary<int, T> currentRows, T newModel);

        protected BaseTable(IByteData data, int address) {
            Data = data;
            Address = address;
        }

        public abstract bool Load();

        public virtual bool Unload() {
            _rows = null;
            return true;
        }

        public IByteData Data { get; }
        public int Address { get; }
        public bool IsLoaded => _rows != null;
        public IStruct[] RowObjs => _rows;
        public int Length => RowObjs.Length;

        [BulkCopyRecurse]
        public T[] Rows => _rows;

        public T this[int index] => _rows[index];

        protected T[] _rows = null;

        IEnumerator IEnumerable.GetEnumerator() => RowObjs.GetEnumerator();
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>) _rows).GetEnumerator();
    }
}
