using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.NamedValues;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Files {
    /// <summary>
    /// File for any kind of file that has tables.
    /// </summary>
    public abstract class TableFile : ITableFile {
        protected TableFile(IRawData data, INameGetterContext nameContext) {
            Data = data;
            NameGetterContext = nameContext;

            // TODO: remove this when we Dispose() ourselves!!
            Data.IsModifiedChanged += (s, e) => IsModifiedChanged?.Invoke(this, EventArgs.Empty);
        }

        private int _isModifiedGuard = 0;

        public virtual bool IsModified {
            get => Data.IsModified;
            set => Data.IsModified = value;
        }

        public ScopeGuard IsModifiedChangeBlocker()
            => new ScopeGuard(() => _isModifiedGuard++, () => _isModifiedGuard--);

        public event EventHandler IsModifiedChanged;

        /// <summary>
        /// Initializes all tables by running MakeTables(). Should be called IMMEDIATELY after creation,
        /// with a factory pattern.
        /// </summary>
        /// <returns>'true' if all tables are loaded. Otherwise 'false'.</returns>
        protected bool Init() {
            Tables = MakeTables();
            foreach (var ma in Tables.Where(x => x != null && !x.IsLoaded)) {
                if (!ma.Load())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a collection of empty ITable's to be populated on LoadFile().
        /// </summary>
        /// <returns>A collection of unloaded ITable's.</returns>
        public abstract IEnumerable<ITable> MakeTables();

        /// <summary>
        /// Performs finishing tasks, aborting finishing if 'false' is returned.
        /// </summary>
        /// <returns>'true' if successful, 'false' if not.</returns>
        public virtual bool OnFinish() => Data.Finish();

        public bool Finish() {
            if (!OnFinish())
                return false;
            Finished?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public virtual void Dispose() => Data.Dispose();

        /// <summary>
        /// The underlying data for this table file. Don't modify this directly!!
        /// </summary>
        public IRawData Data { get; }

        public INameGetterContext NameGetterContext { get; }

        public IEnumerable<ITable> Tables { get; private set; }

        public virtual string Title => "";

        public event EventHandler Finished;
    }
}
