using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static SF3.Utils.Resources;

namespace SF3.Tables {
    /// <summary>
    /// Base implementation for any table of SF3 data that can be modified.
    /// </summary>
    public abstract class Table : ITable {
        protected Table(ISF3FileEditor fileEditor) {
            FileEditor = fileEditor;
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

        public ISF3FileEditor FileEditor { get; }
        public ScenarioType Scenario => FileEditor.Scenario;
        public abstract int Address { get; }

        public abstract string ResourceFile { get; }
        public abstract bool IsLoaded { get; }
        public abstract object[] RowObjs { get; }
        public virtual int? MaxSize => null;
    }

    /// <summary>
    /// Base implementation for a specific table of SF3 data that can be modified.
    /// </summary>
    public abstract class Table<T> : Table, ITable<T> where T : class {
        protected Table(ISF3FileEditor fileEditor) : base(fileEditor) {
        }

        public override bool Reset() {
            _rows = null;
            return true;
        }

        /// <summary>
        /// Loads all rows from the resource file, sorted by value.
        /// TODO: make 'T' always an IModel!
        /// </summary>
        public bool LoadFromResourceFile(Func<int, string, int, T> makeTFunc) {
            var rows = new Dictionary<int, T>();
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                var address = Address;
                while (!xml.EOF) {
                    _ = xml.Read();
                    if (xml.HasAttributes) {
                        var id = Convert.ToInt32(xml.GetAttribute(0), 16);
                        var name = xml.GetAttribute(1);
                        var newModel = makeTFunc(id, name, address);
                        rows.Add(id, newModel);
                        address += ((IModel) newModel).Size;

                        if (id < 0 || (MaxSize != null && id >= MaxSize))
                            throw new IndexOutOfRangeException();
                    }
                }
            }
            catch {
                return false;
            }
            finally {
                _rows = rows.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
                stream?.Close();
            }
            return true;
        }

        public override object[] RowObjs => _rows;

        [BulkCopyRecurse]
        public T[] Rows => _rows;

        public override bool IsLoaded => _rows != null;

        protected T[] _rows = null;
    }
}
