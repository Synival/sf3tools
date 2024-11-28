using SF3.RawEditors;

namespace SF3.Structs {
    /// <summary>
    /// Abstraction of any model stored in SF3 data tables.
    /// </summary>
    public interface IStruct {
        IRawEditor Editor { get; }
        string Name { get; }
        int ID { get; }
        int Address { get; }
        int Size { get; }
    }
}
