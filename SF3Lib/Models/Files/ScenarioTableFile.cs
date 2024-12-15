using CommonLib.NamedValues;
using SF3.RawData;
using SF3.Types;

namespace SF3.Models.Files {
    /// <summary>
    /// Table data that also has a Scenario associated with it. Seems like overkill, but this is so frequent,
    /// we might as well have it to avoid lots of code duplication.
    /// </summary>
    public abstract class ScenarioTableFile : TableFile, IScenarioFile {
        protected ScenarioTableFile(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext) {
            Scenario = scenario;
        }

        public ScenarioType Scenario { get; }

        public override string Title => base.Title + Scenario.ToString();
    }
}
