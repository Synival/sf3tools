using CommonLib.NamedValues;
using SF3.RawEditors;
using SF3.Types;

namespace SF3.Models.Files {
    /// <summary>
    /// Table editor that also has a Scenario associated with it. Seems like overkill, but this is so frequent,
    /// we might as well have it to avoid lots of code duplication.
    /// </summary>
    public abstract class ScenarioTableFile : TableFile, IScenarioFile {
        protected ScenarioTableFile(IRawEditor editor, INameGetterContext nameContext, ScenarioType scenario) : base(editor, nameContext) {
            Scenario = scenario;
        }

        public ScenarioType Scenario { get; }

        public override string Title => base.Title + Scenario.ToString();
    }
}
