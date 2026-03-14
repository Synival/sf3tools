using CommonLib;
using SF3.Actors;

namespace SF3.Scenes {
    /// <summary>
    /// Abstract representation of a scene with actors, scripts, battle info, etc.
    /// </summary>
    public interface IScene : IIndexedEnumerableWithLength<IActor> {
        /// <summary>
        /// When true, this scene is in the context of a battle.
        /// </summary>
        bool IsBattle { get; }

        /// <summary>
        /// The name of the scene used for display.
        /// </summary>
        string SceneName { get; }
    }
}
