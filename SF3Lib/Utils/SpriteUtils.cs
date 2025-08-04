using System.Collections.Generic;
using System.IO;
using System.Linq;
using SF3.Sprites;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Utils {
    public static class SpriteUtils {
        private static Dictionary<string, SpriteDef> s_spriteDefs = new Dictionary<string, SpriteDef>();
        private static HashSet<string> s_spriteDefFilesLoaded = new HashSet<string>();
        private static string s_spritePath = null;
        private static string s_spritesheetPath = null;

        /// <summary>
        /// Loads all .SF3Sprite files in the "Sprites" directory.
        /// </summary>
        /// <returns>The number of new SpriteDef's successfully loaded.</returns>
        public static int LoadAllSpriteDefs() {
            var spriteDefFiles = Directory.GetFiles(s_spritePath ?? ResourceFile("Sprites"), "*.SF3Sprite");

            int loadedCount = 0;
            foreach (var file in spriteDefFiles)
                loadedCount += (LoadSpriteDef(file) != null) ? 1 : 0;

            return loadedCount;
        }

        /// <summary>
        /// Attempts to load a SpriteDef from a file. Returns the new SpriteDef or 'null' on failure.
        /// of loaded SpriteDef's.
        /// </summary>
        /// <param name="file">Filename of the SpriteDef to load.</param>
        /// <returns>Returns the SpriteDef loaded if the file could be read successfully and a *new* SpriteDef was loaded. Otherwise returns 'null'.</returns>
        public static SpriteDef LoadSpriteDef(string file) {
            var fileWithoutExt = Path.GetFileNameWithoutExtension(file);
            if (s_spriteDefFilesLoaded.Contains(fileWithoutExt))
                return null;

            try {
                var spriteDef = SpriteDef.FromJSON(File.ReadAllText(file));
                s_spriteDefFilesLoaded.Add(fileWithoutExt);

                if (!s_spriteDefs.ContainsKey(spriteDef.Name)) {
                    s_spriteDefs.Add(spriteDef.Name, spriteDef);
                    return spriteDef;
                }
            }
            catch {
                // TODO: how to log this error?
            }

            return null;
        }

        /// <summary>
        /// Returns an array of all loaded sprites in alphabetical order by name.
        /// </summary>
        /// <returns>A SpriteDef[] with every loaded sprite, in alphabetical order by name.</returns>
        public static SpriteDef[] GetAllLoadedSpriteDefs()
            => s_spriteDefs.Select(x => x.Value).OrderBy(x => x.Name).ToArray();

        /// <summary>
        /// Retrieves a SpriteDef from the filesystem or cache.
        /// SpriteDef's are stored in the 'Resources/Sprites' folder.
        /// </summary>
        /// <param name="name">Name (not filename) of the SpriteDef to retrieve.</param>
        /// <returns>A deserialized SpriteDef if the file was available and valid. Otherwise, 'null'.</returns>
        public static SpriteDef GetSpriteDef(string name) {
            if (name == null)
                return null;

            if (s_spriteDefs.TryGetValue(name, out var spriteDef))
                return spriteDef;

            var filename = Path.Combine(s_spritePath ?? ResourceFile("Sprites"), FilesystemName(name) + ".SF3Sprite");
            return LoadSpriteDef(filename);
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
            => Path.Combine(s_spritesheetPath ?? ResourceFile("Spritesheets"), filename);

        /// <summary>
        /// Sets the path to use when retrieving sprites.
        /// </summary>
        /// <param name="path">Path that contains spritesheets.</param>
        public static void SetSpritePath(string path)
            => s_spritePath = path;

        /// <summary>
        /// Sets the path to use when retrieving spritesheets.
        /// </summary>
        /// <param name="path">Path that contains spritesheets.</param>
        public static void SetSpritesheetPath(string path)
            => s_spritesheetPath = path;

        /// <summary>
        /// Converts a number of directions to the order expected in a spritesheet.
        /// </summary>
        /// <param name="directions">The number of directions for the frame group.</param>
        /// <returns>A set of SpriteFrameDirection's in order as they would appear vertically in a spritesheet.</returns>
        public static SpriteFrameDirection[] SpritesheetFrameGroupDirections(int directions) {
            switch (directions) {
                case 1:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.First,
                    };

                case 2:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.First,
                        SpriteFrameDirection.Second,
                    };

                case 4:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.SSE,
                        SpriteFrameDirection.ESE,
                        SpriteFrameDirection.ENE,
                        SpriteFrameDirection.NNE,
                    };

                case 5:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.S,
                        SpriteFrameDirection.SE,
                        SpriteFrameDirection.E,
                        SpriteFrameDirection.NE,
                        SpriteFrameDirection.N,
                    };

                case 6:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.S,
                        SpriteFrameDirection.SSE,
                        SpriteFrameDirection.ESE,
                        SpriteFrameDirection.ENE,
                        SpriteFrameDirection.NNE,
                        SpriteFrameDirection.N,
                    };

                case 8:
                    return new SpriteFrameDirection[] {
                        SpriteFrameDirection.SSE,
                        SpriteFrameDirection.ESE,
                        SpriteFrameDirection.ENE,
                        SpriteFrameDirection.NNE,
                        SpriteFrameDirection.NNW,
                        SpriteFrameDirection.WNW,
                        SpriteFrameDirection.WSW,
                        SpriteFrameDirection.SSW,
                    };

                default:
                    return null;
            }
        }
    }
}
