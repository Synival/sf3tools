using System.Collections.Generic;
using System.IO;
using SF3.Sprites;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Utils {
    public static class SpriteUtils {
        private static Dictionary<string, SpriteDef> s_spriteDefs = new Dictionary<string, SpriteDef>();

        /// <summary>
        /// Retrieves a SpriteDef from the filesystem or cache.
        /// SpriteDef's are stored in the 'Resources/Sprites' folder.
        /// </summary>
        /// <param name="name">Name (not filename) of the SpriteDef to retrieve.</param>
        /// <returns>A deserialized SpriteDef if the file was available and valid. Otherwise, 'null'.</returns>
        public static SpriteDef GetSpriteDef(string name) {
            if (name == null)
                return null;

            // Cache loaded SpriteDef's.
            if (s_spriteDefs.ContainsKey(name))
                return s_spriteDefs[name];

            // Attempt to load a SpriteDef in the 'Resources/Sprites' folder.
            SpriteDef spriteDef = null;
            try {
                var spriteDefPath = ResourceFile(Path.Combine("Sprites", $"{FilesystemName(name)}.SF3Sprite"));
                spriteDef = SpriteDef.FromJSON(File.ReadAllText(spriteDefPath));
            }
            catch {
                return null;
            }
            if (spriteDef == null)
                return null;

            // Only cached if opening and deserialization was successful.
            s_spriteDefs[name] = spriteDef;
            return spriteDef;
        }

        /// <summary>
        /// Returns the name of a sprite as it would be stored in the filesystem, with invalid characters replaced.
        /// </summary>
        /// <param name="name">The name of the sprite.</param>
        /// <returns>A filesystem-friendly name of the sprite.</returns>
        public static string FilesystemName(string name)
            => CHR_Utils.FilesystemName(name);

        /// <summary>
        /// Returns the full path of a spritesheet image.
        /// </summary>
        /// <param name="filename">The filename of the spritesheet, without the path.</param>
        /// <returns>A string with the relative path of the spritesheet.</returns>
        public static string SpritesheetImagePath(string filename)
            => ResourceFile(Path.Combine("Spritesheets", filename));
    }
}
