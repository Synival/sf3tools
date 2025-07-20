namespace SF3.Utils {
    public static class SpriteUtils {
        /// <summary>
        /// Returns the name of a sprite as it would be stored in the filesystem, with invalid characters replaced.
        /// </summary>
        /// <param name="name">The name of the sprite.</param>
        /// <returns>A filesystem-friendly name of the sprite.</returns>
        public static string FilesystemName(string name)
            => CHR_Utils.FilesystemName(name);
    }
}
