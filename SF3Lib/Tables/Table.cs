using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tables {
    /// <summary>
    /// Base implementation for any table of SF3 data that can be modified.
    /// </summary>
    public abstract class Table : ITable {
        protected Table(ISF3FileEditor fileEditor) {
            _fileEditor = fileEditor;
        }

        public abstract bool Load();
        public abstract bool Reset();

        public ScenarioType Scenario => _fileEditor.Scenario;

        public abstract string ResourceFile { get; }
        public abstract bool IsLoaded { get; }
        public abstract object[] RowObjs { get; }

        private readonly ISF3FileEditor _fileEditor;

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

        public override object[] RowObjs => _rows;

        [BulkCopyRecurse]
        public T[] Rows => _rows;

        public override bool IsLoaded => _rows != null;

        protected T[] _rows = null;
    }
}
