namespace SF3.Models.Files {
    /// <summary>
    /// Table file that also has a Scenario associated with it. Seems like overkill, but this is so frequent,
    /// we might as well have it to avoid lots of code duplication.
    /// </summary>
    public interface IScenarioTableFile : IScenarioFile, ITableFile {
    }
}
