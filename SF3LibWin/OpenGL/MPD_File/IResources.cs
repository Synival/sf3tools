using System;

namespace SF3.Win.OpenGL.MPD_File {
    public interface IResources : IDisposable {
        /// <summary>
        /// Performs any one-time initialization necessary after instantiation.
        /// </summary>
        void Init();

        /// <summary>
        /// Disposes of any resources loaded in.
        /// </summary>
        void Reset();
    }
}
