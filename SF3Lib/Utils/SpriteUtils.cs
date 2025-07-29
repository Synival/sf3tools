using System.Collections.Generic;
using System.IO;
using SF3.Sprites;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Utils {
    public static class SpriteUtils {
        private static Dictionary<string, SpriteDef> s_spriteDefs = null;

        /// <summary>
        /// Loads all .SF3Sprite files in the "Sprites" directory.
        /// </summary>
        /// <returns>The number of new SpriteDef's successfully loaded.</returns>
        public static int LoadAllSpriteDefs() {
            if (s_spriteDefs == null)
                s_spriteDefs = new Dictionary<string, SpriteDef>();

            var spriteDefFiles = Directory.GetFiles(ResourceFile("Sprites"), "*.SF3Sprite");
            int loadedCount = 0;

            foreach (var file in spriteDefFiles) {
                try {
                    var spriteDef = SpriteDef.FromJSON(File.ReadAllText(file));
                    if (!s_spriteDefs.ContainsKey(spriteDef.Name)) {
                        s_spriteDefs.Add(spriteDef.Name, spriteDef);
                        loadedCount++;
                    }
                }
                catch {
                    // TODO: how to log this error?
                }
            }

            return loadedCount;
        }

        /// <summary>
        /// Retrieves a SpriteDef from the filesystem or cache.
        /// SpriteDef's are stored in the 'Resources/Sprites' folder.
        /// </summary>
        /// <param name="name">Name (not filename) of the SpriteDef to retrieve.</param>
        /// <returns>A deserialized SpriteDef if the file was available and valid. Otherwise, 'null'.</returns>
        public static SpriteDef GetSpriteDef(string name) {
            if (name == null)
                return null;

            if (s_spriteDefs == null)
                LoadAllSpriteDefs();

            return s_spriteDefs.TryGetValue(name, out var spriteDef) ? spriteDef : null;
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
