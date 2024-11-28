using SF3.RawData;

namespace SF3.Models.Structs {
    /// <summary>
    /// Abstraction of any model stored in SF3 data tables.
    /// </summary>
    public interface IStruct {
        IRawData Editor { get; }
        string Name { get; }
        int ID { get; }
        int Address { get; }
        int Size { get; }
    }
}
