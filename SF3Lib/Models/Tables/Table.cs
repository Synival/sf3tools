using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    /// <summary>
    /// Base implementation for a specific table of SF3 data that can be modified.
    /// </summary>
    public abstract class Table<T> : ITable, ITable<T> where T : class, IStruct {
        /// <summary>
        /// Function to determine whether or not reading from a table should continue.
        /// If 'false' is returned, the 'currentModel' will not be committed.
        /// </summary>
        /// <param name="currentRows">The dictionary of rows by ID read thus far.</param>
        /// <param name="newModel">The last model read from the table.</param>
        /// <returns>'true' if reading should continue, 'false' if reading should not continue.</returns>
        public delegate bool ContinueReadingPredicate(Dictionary<int, T> currentRows, T newModel);

        protected Table(IByteData data, string name, int address) {
            Data = data;
            Name = name;
            Address = address;
        }

        public abstract bool Load();

        public virtual bool Unload() {
            _rows = null;
            return true;
        }

        public IByteData Data { get; }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }
        public bool IsLoaded => _rows != null;
        public IStruct[] RowObjs => _rows;
        public int Length => RowObjs.Length;
        public int SizeInBytes => IsLoaded ? _rows.Sum(x => x.Size) : 0;
        public abstract int TerminatorSize { get; }

        [BulkCopyRecurse]
        public T[] Rows => _rows;

        public T this[int index] => _rows[index];

        protected T[] _rows = null;

        IEnumerator IEnumerable.GetEnumerator() => RowObjs.GetEnumerator();
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>) _rows).GetEnumerator();
    }
}
