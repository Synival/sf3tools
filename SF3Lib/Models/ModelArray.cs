using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models {
    /// <summary>
    /// Base implementation for any collection of SF3 models that can be modified.
    /// </summary>
    public abstract class ModelArray : IModelArray {
        protected ModelArray(ISF3FileEditor fileEditor) {
            _fileEditor = fileEditor;
        }

        public abstract bool Load();
        public abstract bool Reset();

        public ScenarioType Scenario => _fileEditor.Scenario;

        public abstract string ResourceFile { get; }
        public abstract bool IsLoaded { get; }
        public abstract object[] ModelObjs { get; }

        private ISF3FileEditor _fileEditor;

    }

    /// <summary>
    /// Base implementation for a specific collection of SF3 models that can be modified.
    /// </summary>
    public abstract class ModelArray<T> : ModelArray, IModelArray<T> where T : class {
        protected ModelArray(ISF3FileEditor fileEditor) : base(fileEditor) {
        }

        public override bool Reset() {
            _models = null;
            return true;
        }

        public override object[] ModelObjs => _models;

        [BulkCopyRecurse]
        public T[] Models => _models;

        public override bool IsLoaded => _models != null;

        protected T[] _models = null;
    }
}
