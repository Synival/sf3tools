using SF3.Types;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Loads models from its respective XML file(s).
        /// </summary>
        /// <returns>Return 'true' on success, or 'false' if the .XML file(s) do not exist or are in use.</returns>
        public abstract bool Load();

        /// <summary>
        /// The Scenario the contained models belong to.
        /// </summary>
        public ScenarioType Scenario => _fileEditor.Scenario;

        private ISF3FileEditor _fileEditor;

    }

    public abstract class ModelArray<T> : ModelArray, IModelArray<T> where T : class
    {
        protected ModelArray(ISF3FileEditor fileEditor) : base(fileEditor)
        {
        }

/*
        /// <summary>
        /// The XML file to load for this resource.
        /// </summary>
        public abstract string ResourceFile { get; }
*/

        /// <summary>
        /// A mutable array of models of type T.
        /// </summary>
        public T[] Models => _models;

        protected T[] _models = null;
    }
}
