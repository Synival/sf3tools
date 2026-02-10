namespace SF3.Models.Files {
    /// <summary>
    /// Table file that is also a file loaded in-game. Seems like overkill, but this is so frequent,
    /// we might as well have it to avoid lots of code duplication.
    /// </summary>
    public interface IGameTableFile : IGameFile, ITableFile {
    }
}
