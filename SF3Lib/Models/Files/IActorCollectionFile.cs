using System.Collections.Generic;
using SF3.Actors;

namespace SF3.Models.Files {
    public interface IActorCollectionFile {
        IEnumerable<IActorCollection> ActorCollections { get; }
    }
}
