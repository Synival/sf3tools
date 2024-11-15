using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.NamedValues;
using SF3.RawEditors;
using SF3.Tables;

namespace SF3.Editors {
    /// <summary>
    /// Editor for any kind of file that has tables.
    /// </summary>
    public abstract class TableEditor : ITableEditor {
        protected TableEditor(IRawEditor editor, INameGetterContext nameContext) {
            Editor = editor;
            NameGetterContext = nameContext;

            // TODO: remove this when we Dispose() ourselves!!
            Editor.IsModifiedChanged += (s, e) => this.IsModifiedChanged?.Invoke(this, EventArgs.Empty);
        }

        private int _isModifiedGuard = 0;

        public virtual bool IsModified {
            get => Editor.IsModified;
            set => Editor.IsModified = value;
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
        /// Performs finalization tasks, aborting finalization if 'false' is returned.
        /// </summary>
        /// <returns>'true' if successful, 'false' if not.</returns>
        public virtual bool OnFinalize() => Editor.Finalize();

        public bool Finalize() {
            if (!OnFinalize())
                return false;
            Finalized?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public virtual void Dispose() => Editor.Dispose();

        /// <summary>
        /// The underlying data editor for this table editor. Don't modify this directly!!
        /// </summary>
        public IRawEditor Editor { get; }

        public INameGetterContext NameGetterContext { get; }

        public IEnumerable<ITable> Tables { get; private set; }

        public virtual string Title => "";

        public event EventHandler Finalized;
    }
}
