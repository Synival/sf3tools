using System.Collections.Generic;
using System.IO;
using SF3.Models;
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
        /// Creates a collection of empty IModelArray's to be populated on LoadFile().
        /// </summary>
        /// <returns>A collection of unloaded IModelArray's.</returns>
        public abstract IEnumerable<IModelArray> MakeModelArrays();

        /// <summary>
        /// Unsets or deinitialize any model IModelArray's populated in MakeModelArrays().
        /// </summary>
        public abstract void DestroyModelArrays();

        /// <summary>
        /// Occurs when data is loaded but before the ModelArray's are created.
        /// This is a good place to check file data to determine how to create the models.
        /// If 'false' is returned, loading is aborted.
        /// </summary>
        /// <returns>'true' on success, otherwise 'false'.</returns>
        public virtual bool OnLoadBeforeMakeModelArrays()
            => true;

        public override bool LoadFile(string filename, Stream stream) {
            if (!base.LoadFile(filename, stream))
                return false;
            if (!OnLoadBeforeMakeModelArrays())
                return false;

            ModelArrays = MakeModelArrays();
            foreach (var ma in ModelArrays) {
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

            DestroyModelArrays();
            ModelArrays = null;
            return true;
        }

        public ScenarioType Scenario { get; }

        public IEnumerable<IModelArray> ModelArrays { get; private set; }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + " (" + Scenario.ToString() + ")"
            : base.BaseTitle;
    }
}
