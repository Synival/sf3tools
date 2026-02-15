using System.Collections.Generic;
using CommonLib;

namespace SF3.Actors {
    /// <summary>
    /// Abstract representation of any collection of actors.
    /// </summary>
    public interface IActorCollection : IIndexedEnumerableWithLength<IActor> {
        bool IsBattle { get; }
    }
}
