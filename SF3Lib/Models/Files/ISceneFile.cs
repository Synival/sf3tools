using System.Collections.Generic;
using SF3.Scenes;

namespace SF3.Models.Files {
    public interface ISceneFile {
        IEnumerable<IScene> Scenes { get; }
    }
}
