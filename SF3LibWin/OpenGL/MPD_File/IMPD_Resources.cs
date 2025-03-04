using System;
using SF3.Models.Files.MPD;

namespace SF3.Win.OpenGL.MPD_File {
    public interface IMPD_Resources : IResources {
        /// <summary>
        /// Updates resources based on the MPD file given.
        /// </summary>
        /// <param name="mpdFile">The MPD file from which to build new resources.</param>
        void Update(IMPD_File mpdFile);
    }
}
