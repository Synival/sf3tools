using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Tables {
    /// <summary>
    /// Base implementation for a specific table of SF3 data that can be modified.
    /// </summary>
    public abstract class ResourceTable<T> : Table<T>, IResourceTable<T> where T : class, IStruct {
        protected ResourceTable(IByteData data, string name, string resourceFile, int address, int? maxSize = null) : base(data, name, address) {
            ResourceFile = resourceFile;
            MaxSize = maxSize;
        }

        /// <summary>
        /// Loads all rows from the resource file, sorted by value.
        /// </summary>
        /// <param name="makeTFunc">Factory function to make the model.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool Load(Func<int, string, int, T> makeTFunc)
            => Load(makeTFunc, null);

        /// <summary>
        /// Loads all rows from the resource file, sorted by value.
        /// </summary>
        /// <param name="makeTFunc">Factory function to make the model.</param>
        /// <param name="pred">Optional predicate function to check whether or not the reader should continue.</param>
        /// <param name="addEndModel">If 'ture', any new model loaded will still be added if 'pred' returns 'false'.</param>
        /// <returns>'true' on success, 'false' if any or exception occurred during reading.</returns>
        public bool Load(Func<int, string, int, T> makeTFunc, ContinueReadingPredicate pred, bool addEndModel = true) {
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
                        if (MaxSize.HasValue && rows.Count >= MaxSize.Value)
                            break;

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

        public string ResourceFile { get; }
        public int? MaxSize { get; }
        public override int TerminatorSize => 0;
    }
}
