using SF3.MPD.Interfaces;

namespace SF3.Win.OpenGL.GLResources.MPD {
    public interface IMPD_Resources : IResources {
        /// <summary>
        /// Updates resources based on the MPD file given.
        /// </summary>
        /// <param name="mpdFile">The MPD file from which to build new resources.</param>
        void Update(IMPD mpdFile);
    }
}
