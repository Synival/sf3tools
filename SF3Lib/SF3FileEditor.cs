using SF3.Models;
using SF3.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3
{
    /// <summary>
    /// IFileEditor specifically for files in Shining Force 3
    /// </summary>
    public abstract class SF3FileEditor : FileEditor, ISF3FileEditor
    {
        public SF3FileEditor(ScenarioType scenario)
        {
            Scenario = scenario;
            ModelArrays = MakeModelArrays();
        }

        /// <summary>
        /// Creates a collection of empty IModelArray's to be populated on LoadFile().
        /// </summary>
        /// <returns>A collection of unloaded IModelArray's.</returns>
        public abstract IEnumerable<IModelArray> MakeModelArrays();

        public override bool LoadFile(string filename)
        {
            if (!base.LoadFile(filename))
            {
                return false;
            }

            foreach (var ma in ModelArrays)
            {
                if (!ma.Load())
                {
                    return false;
                }
            }

            return true;
        }

        public ScenarioType Scenario { get; }

        public IEnumerable<IModelArray> ModelArrays { get; }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + " (" + Scenario.ToString() + ")"
            : base.BaseTitle;
    }
}
