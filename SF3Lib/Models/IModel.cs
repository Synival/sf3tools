using SF3.FileEditors;

namespace SF3.Models {
    /// <summary>
    /// Abstraction of any model stored in SF3 data tables.
    /// </summary>
    public interface IModel {
        IFileEditor FileEditor { get; }
        string Name { get; }
        int ID { get; }
        int Address { get; }
        int Size { get; }
    }
}
