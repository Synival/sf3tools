using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    /// <summary>
    /// Base implementation for a specific table of SF3 data that can be modified.
    /// </summary>
    public abstract class TerminatedTable<T> : Table<T>, ITerminatedTable<T> where T : class, IStruct {
        protected TerminatedTable(IByteData data, int address, int? maxSize = null) : base(data, address) {
            MaxSize = maxSize;
        }

        /// <summary>
        /// Loads all rows until MaxSize is reached.
        /// </summary>
        /// <param name="makeTFunc">Factory function to make the model.</param>
        /// <param name="pred">Optional predicate function to check whether or not resource adding should continue.</param>
        /// <param name="addEndModel">If 'ture', any new model loaded will still be added if 'pred' returns 'false'.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool Load(Func<int, int, T> makeTFunc, ContinueReadingPredicate pred, bool addEndModel = true) {
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

        public int? MaxSize { get; }
    }
}
