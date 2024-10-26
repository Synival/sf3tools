using System.Collections.Generic;
using System.IO;
using SF3.Tables;
using SF3.Types;

namespace SF3.FileEditors {
    /// <summary>
    /// IFileEditor specifically for files in Shining Force 3
    /// </summary>
    public abstract class SF3FileEditor : FileEditor, ISF3FileEditor {
        public SF3FileEditor(ScenarioType scenario) {
            Scenario = scenario;
        }

        /// <summary>
        /// Creates a collection of empty ITable's to be populated on LoadFile().
        /// </summary>
        /// <returns>A collection of unloaded ITable's.</returns>
        public abstract IEnumerable<ITable> MakeTables();

        /// <summary>
        /// Unsets or deinitialize any model ITable's populated in MakeTables().
        /// </summary>
        public abstract void DestroyTables();

        /// <summary>
        /// Occurs when data is loaded but before the Table's are created.
        /// This is a good place to check file data to determine how to create the models.
        /// If 'false' is returned, loading is aborted.
        /// </summary>
        /// <returns>'true' on success, otherwise 'false'.</returns>
        public virtual bool OnLoadBeforeMakeTables()
            => true;

        public override bool LoadFile(string filename, Stream stream) {
            if (!base.LoadFile(filename, stream))
                return false;
            if (!OnLoadBeforeMakeTables())
                return false;

            Tables = MakeTables();
            foreach (var ma in Tables) {
                if (!ma.Load())
                    return false;
            }

            return true;
        }

        public override bool CloseFile() {
            if (!IsLoaded)
                return true;
            if (!base.CloseFile())
                return false;

            DestroyTables();
            Tables = null;
            return true;
        }

        public ScenarioType Scenario { get; }

        public IEnumerable<ITable> Tables { get; private set; }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + " (" + Scenario.ToString() + ")"
            : base.BaseTitle;
    }
}
