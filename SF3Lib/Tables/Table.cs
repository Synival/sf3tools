using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Attributes;
using SF3.RawEditors;
using SF3.Models;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    /// <summary>
    /// Base implementation for any table of SF3 data that can be modified.
    /// </summary>
    public abstract class Table : ITable {
        protected Table(IRawEditor fileEditor, string resourceFile, int address) {
            FileEditor = fileEditor;
            ResourceFile = resourceFile;
            Address = address;
        }

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public abstract bool Load();

        /// <summary>
        /// Resets all data loaded from Load().
        /// </summary>
        /// <returns>'true' if successful (or no data is loaded), 'false' on failure.</returns>
        public abstract bool Reset();

        public IRawEditor FileEditor { get; }
        public string ResourceFile { get; }
        public int Address { get; }

        public abstract bool IsLoaded { get; }
        public abstract IModel[] RowObjs { get; }
        public virtual int? MaxSize => null;
    }

    /// <summary>
    /// Base implementation for a specific table of SF3 data that can be modified.
    /// </summary>
    public abstract class Table<T> : Table, ITable<T> where T : class, IModel {
        protected Table(IRawEditor fileEditor, int address) : base(fileEditor, null, address) {
        }

        protected Table(IRawEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Reset() {
            _rows = null;
            return true;
        }

        /// <summary>
        /// Function to determine whether or not reading from a table should continue.
        /// If 'false' is returned, the 'currentModel' will not be committed.
        /// </summary>
        /// <param name="currentRows">The dictionary of rows by ID read thus far.</param>
        /// <param name="prevModel">The last model read and committed to the table.</param>
        /// <param name="currentModel">The model just read that is about to be committed to the table unless 'false' is returned.</param>
        /// <returns>'true' if reading should continue, 'false' if reading should not continue.</returns>
        public delegate bool ContinueReadingPredicate(Dictionary<int, T> currentRows, T prevModel, T currentModel);

        /// <summary>
        /// Loads all rows from the resource file, sorted by value.
        /// </summary>
        /// <param name="makeTFunc">Factory function to make the model.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool LoadFromResourceFile(Func<int, string, int, T> makeTFunc)
            => LoadFromResourceFile(makeTFunc, null);

        /// <summary>
        /// Loads all rows from the resource file, sorted by value.
        /// </summary>
        /// <param name="makeTFunc">Factory function to make the model.</param>
        /// <param name="pred">Optional predicate function to check whether or not the reader should continue.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool LoadFromResourceFile(Func<int, string, int, T> makeTFunc, ContinueReadingPredicate pred) {
            var rows = new Dictionary<int, T>();
            try {
                // Get the size of our rows so we can determine the address of elements.
                var size = makeTFunc(0, "", Address).Size;

                // Read all elements.
                using (var stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read)) {
                    var xml = MakeXmlReader(stream);
                    _ = xml.Read();

                    T prevModel = null;
                    while (!xml.EOF) {
                        _ = xml.Read();
                        if (xml.HasAttributes) {
                            var id = Convert.ToInt32(xml.GetAttribute(0), 16);
                            var name = xml.GetAttribute(1);
                            var newModel = makeTFunc(id, name, Address + id * size);
                            if (pred != null && !pred(rows, prevModel, newModel))
                                break;

                            rows.Add(id, newModel);

                            if (id < 0 || (MaxSize != null && id >= MaxSize))
                                throw new IndexOutOfRangeException();
                            prevModel = newModel;
                        }
                    }
                }
            }
            catch {
                return false;
            }
            finally {
                _rows = rows.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
            }
            return true;
        }

        /// <summary>
        /// Loads all rows until MaxSize is reached.
        /// </summary>
        /// <param name="makeTFunc">Factory function to make the model.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool LoadUntilMax(Func<int, int, T> makeTFunc)
            => LoadUntilMax(makeTFunc, null);

        /// <summary>
        /// Loads all rows until MaxSize is reached.
        /// </summary>
        /// <param name="makeTFunc">Factory function to make the model.</param>
        /// <param name="pred">Optional predicate function to check whether or not resource adding should continue.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool LoadUntilMax(Func<int, int, T> makeTFunc, ContinueReadingPredicate pred) {
            var rowDict = new Dictionary<int, T>();
            var rows = new T[(int) MaxSize];

            try {
                T prevModel = null;
                var address = Address;
                for (var id = 0; id < rows.Length; ++id) {
                    var newModel = makeTFunc(id, address);
                    if (pred != null && !pred(rowDict, prevModel, newModel))
                        break;

                    rowDict[id] = newModel;
                    rows[id] = newModel;
                    address += newModel.Size;

                    prevModel = newModel;
                }
            }
            catch {
                return false;
            }
            finally {
                _rows = rows;
            }
            return true;
        }

        public override IModel[] RowObjs => _rows;

        [BulkCopyRecurse]
        public T[] Rows => _rows;

        public override bool IsLoaded => _rows != null;

        protected T[] _rows = null;
    }
}
