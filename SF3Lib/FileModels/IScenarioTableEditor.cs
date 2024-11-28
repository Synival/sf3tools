namespace SF3.FileModels {
    /// <summary>
    /// Table editor that also has a Scenario associated with it. Seems like overkill, but this is so frequent,
    /// we might as well have it to avoid lots of code duplication.
    /// </summary>
    public interface IScenarioTableEditor : IScenarioEditor, ITableEditor {
    }
}
