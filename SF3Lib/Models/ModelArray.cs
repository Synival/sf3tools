using SF3.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.Models
{
    public abstract class ModelArray : IModelArray
    {
        protected ModelArray(ISF3FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        public abstract bool Load();
        public abstract bool Reset();

        public ScenarioType Scenario => _fileEditor.Scenario;

        public abstract string ResourceFile { get; }
        public abstract bool IsLoaded { get; }


        private ISF3FileEditor _fileEditor;

    }

    public abstract class ModelArray<T> : ModelArray, IModelArray<T> where T : class
    {
        protected ModelArray(ISF3FileEditor fileEditor) : base(fileEditor)
        {
        }

        public override bool Reset()
        {
            _models = null;
            return true;
        }

        public T[] Models => _models;

        public override string ResourceFile { get; }
        public override bool IsLoaded => _models != null;

        protected T[] _models = null;
    }
}
