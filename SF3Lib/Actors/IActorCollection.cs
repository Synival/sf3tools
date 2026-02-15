using CommonLib;

namespace SF3.Actors {
    /// <summary>
    /// Abstract representation of any collection of actors.
    /// </summary>
    public interface IActorCollection : IIndexedEnumerableWithLength<IActor> {
        /// <summary>
        /// When true, the actors in this collection come from a battle.
        /// </summary>
        bool IsBattle { get; }

        /// <summary>
        /// The name of the actor collection used for display.
        /// </summary>
        string ActorCollectionName { get; }
    }
}
