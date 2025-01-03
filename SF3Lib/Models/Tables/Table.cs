using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Structs;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Tables {
    /// <summary>
    /// Base implementation for any table of SF3 data that can be modified.
    /// </summary>
    public abstract class Table : ITable {
        protected Table(IByteData data, string resourceFile, int address) {
            Data = data;
            ResourceFile = resourceFile;
            Address = address;
        }

        /// <summary>
        /// Loads models from the byte data provided provided in the constructor.
        /// </summary>
        /// <returns>'true' if models were loaded successfully, otherwise 'false'.</returns>
        public abstract bool Load();

        /// <summary>
        /// Resets all models loaded from Load().
        /// </summary>
        /// <returns>'true' if successful (or no models are loaded), 'false' on failure.</returns>
        public abstract bool Reset();

        public IByteData Data { get; }
        public string ResourceFile { get; }
        public int Address { get; }

        public abstract bool IsLoaded { get; }
        public abstract IStruct[] RowObjs { get; }
        public virtual int? MaxSize => null;
    }

    /// <summary>
    /// Base implementation for a specific table of SF3 data that can be modified.
    /// </summary>
    public abstract class Table<T> : Table, ITable<T> where T : class, IStruct {
        protected Table(IByteData data, int address) : base(data, null, address) {
        }

        protected Table(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
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
        /// <param name="newModel">The last model read from the table.</param>
        /// <returns>'true' if reading should continue, 'false' if reading should not continue.</returns>
        public delegate bool ContinueReadingPredicate(Dictionary<int, T> currentRows, T newModel);

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
        /// <param name="addEndModel">If 'ture', any new model loaded will still be added if 'pred' returns 'false'.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool LoadFromResourceFile(Func<int, string, int, T> makeTFunc, ContinueReadingPredicate pred, bool addEndModel = true) {
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
                            var predResult = pred != null ? pred(rows, newModel) : true;
                            if (!predResult && !addEndModel)
                                break;

                            if (id < 0 || MaxSize != null && id >= MaxSize)
                                throw new IndexOutOfRangeException();

                            rows.Add(id, newModel);
                            if (!predResult && addEndModel)
                                break;

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
        /// <param name="addEndModel">If 'ture', any new model loaded will still be added if 'pred' returns 'false'.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool LoadUntilMax(Func<int, int, T> makeTFunc, ContinueReadingPredicate pred, bool addEndModel = true) {
            var rowDict = new Dictionary<int, T>();
            var rows = new List<T>();

            try {
                T prevModel = null;
                var address = Address;
                var max = MaxSize ?? 65536;
                for (var id = 0; id < max; ++id) {
                    var newModel = makeTFunc(id, address);
                    var predResult = pred != null ? pred(rowDict, newModel) : true;
                    if (!predResult && !addEndModel)
                        break;

                    rowDict[id] = newModel;
                    rows.Add(newModel);
                    address += newModel.Size;

                    if (!predResult && addEndModel)
                        break;

                    prevModel = newModel;
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

        public override IStruct[] RowObjs => _rows;

        [BulkCopyRecurse]
        public T[] Rows => _rows;

        public override bool IsLoaded => _rows != null;

        protected T[] _rows = null;
    }
}
