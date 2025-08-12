using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using SF3.Extensions;
using SF3.Types;
using SF3.Utils;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Sprites {
    public static class SpriteResources {
        private static Dictionary<string, SpriteDef> s_spriteDefs = new Dictionary<string, SpriteDef>();
        private static HashSet<string> s_spriteDefFilesLoaded = new HashSet<string>();
        private static Dictionary<string, FrameHashLookupSet> s_frameHashLookups = new Dictionary<string, FrameHashLookupSet>();
        private static Dictionary<string, UniqueAnimationDef> s_uniqueAnimationsByHash = null;

        private static bool s_frameHashLookupsLoaded = false;

        /// <summary>
        /// The path where .SF3Sprite files can be found. When 'null', the path 'Resources/Sprites' will be used.
        /// </summary>
        public static string SpritePath { get; set; } = null;

        /// <summary>
        /// The path where .png files for spritesheets can be found. When 'null', the path 'Resources/Spritesheets' will be used.
        /// </summary>
        public static string SpritesheetPath { get; set; } = null;

        /// <summary>
        /// Full path to the file which contains sprite frame lookups by hash. When 'null', the path 'Resources/FrameHashLookups.json' will be used.
        /// </summary>
        public static string FrameHashLookupsFile { get; set; } = null;

        /// <summary>
        /// Returns an individual spritesheet's key used for filenames or storage.
        /// </summary>
        /// <param name="spriteName">The name of the spite.</param>
        /// <param name="frameWidth">The width of each frame in the spritesheet.</param>
        /// <param name="frameHeight">The height of each frame in the spritesheet.</param>
        /// <returns>A string with a unique key for this spritesheet.</returns>
        public static string SpritesheetImageKey(string spriteName, int frameWidth, int frameHeight)
            => $"{spriteName} ({frameWidth}x{frameHeight})";

        /// <summary>
        /// Returns the full path of a spritesheet image. Root path is 'SpritesheetPath' if set, or 'Resources/Spritesheets' if not.
        /// </summary>
        /// <param name="spritesheetImageKey">The unique key for the spritesheet, returned by SpritesheetImageKey().</param>
        /// <returns>A string with the full path (relative if 'SpritesheetPath' is unset) of the spritesheet.</returns>
        public static string SpritesheetImageFile(string spritesheetImageKey)
            => Path.Combine(SpritesheetPath ?? ResourceFile("Spritesheets"), $"{FilesystemName(spritesheetImageKey)}.png");

        /// <summary>
        /// Returns the full path of a spritesheet image. Root path is 'SpritesheetPath' if set, or 'Resources/Spritesheets' if not.
        /// </summary>
        /// <param name="spriteName">The name of the spite.</param>
        /// <param name="frameWidth">The width of each frame in the spritesheet.</param>
        /// <param name="frameHeight">The height of each frame in the spritesheet.</param>
        /// <returns>A string with the full path (relative if 'SpritesheetPath' is unset) of the spritesheet.</returns>
        public static string SpritesheetImageFile(string spriteName, int frameWidth, int frameHeight)
            => SpritesheetImageFile(SpritesheetImageKey(spriteName, frameWidth, frameHeight));

        /// <summary>
        /// Returns the name of a sprite as it would be stored in the filesystem, with invalid characters replaced.
        /// </summary>
        /// <param name="name">The name of the sprite.</param>
        /// <returns>A filesystem-friendly name of the sprite.</returns>
        public static string FilesystemName(string name)
            => ResourceUtils.FilesystemName(name);

        /// <summary>
        /// Loads all .SF3Sprite files in the "Sprites" directory.
        /// </summary>
        /// <returns>The number of new SpriteDef's successfully loaded.</returns>
        public static int LoadAllSpriteDefs() {
            var spriteDefFiles = Directory.GetFiles(SpritePath ?? ResourceFile("Sprites"), "*.SF3Sprite");

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
                SpriteDef spriteDef;
                if (file == "(Unknown).SF3Sprite")
                    spriteDef = null;
                else
                    spriteDef = SpriteDef.FromJSON(File.ReadAllText(file));

                s_spriteDefFilesLoaded.Add(fileWithoutExt);
                if (s_spriteDefs.ContainsKey(spriteDef.Name))
                    return null;

                s_spriteDefs.Add(spriteDef.Name, spriteDef);
                return spriteDef;
            }
            catch {
                // TODO: how to log this error?
                s_spriteDefFilesLoaded.Add(fileWithoutExt);
                return null;
            }
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

            var filename = Path.Combine(SpritePath ?? ResourceFile("Sprites"), FilesystemName(name) + ".SF3Sprite");
            return LoadSpriteDef(filename);
        }

        /// <summary>
        /// Loads the list of sprite frames that can be looked up by hash.
        /// This will look in 'Resources/FrameHashLookups.json' by default, or the file set by SetFrameHashLookupsFile().
        /// If the file has already been loaded, any new frames present are loaded.
        /// </summary>
        public static void LoadFrameHashLookups() {
            var filename = FrameHashLookupsFile ?? ResourceFile("FrameHashLookups.json");
            var jsonText = File.ReadAllText(filename);
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, FrameHashLookup[]>>(jsonText);

            foreach (var lookupKv in jsonObj) {
                foreach (var frameRef in lookupKv.Value)
                    frameRef.ImageHash = lookupKv.Key;

                if (!s_frameHashLookups.ContainsKey(lookupKv.Key))
                    s_frameHashLookups.Add(lookupKv.Key, new FrameHashLookupSet(lookupKv.Value));
                else {
                    foreach (var frame in lookupKv.Value)
                        s_frameHashLookups[lookupKv.Key].Add(frame);
                }
            }

            s_frameHashLookupsLoaded = true;
        }

        /// <summary>
        /// Rewrites the list of sprite frames that can be looked up by hash.
        /// This will rewrite 'Resources/FrameHashLookups.json' by default, or the file set by SetFrameHashLookupsFile().
        /// </summary>
        public static void WriteFrameHashLookupsJSON() {
            var filename = FrameHashLookupsFile ?? ResourceFile("FrameHashLookups.json");

            // We could just use SerializeObject(), but then the file would either be one giant blob, or a
            // 100,000 line long mess, so let's do some custom writing.
            using (var file = File.Open(FrameHashLookupsFile, FileMode.Create)) {
                using (var stream = new StreamWriter(file)) {
                    stream.NewLine = "\n";
                    stream.WriteLine("{");

                    var lookupKvArray = s_frameHashLookups
                        .OrderBy(x => string.Join("|", x.Value.Select(y => y.SpriteName)))
                        .ThenBy(x => string.Join("|", x.Value.Select(y => y.FrameGroupName)))
                        .ThenBy(x => x.Value.Min(y => y.FrameWidth))
                        .ThenBy(x => x.Value.Min(y => y.FrameHeight))
                        .ToArray();

                    for (int i = 0; i < lookupKvArray.Length; i++) {
                        var lookupKv = lookupKvArray[i];
                        var hash = lookupKv.Key;
                        var frames = lookupKv.Value;
                        stream.Write($"\"{hash}\": ");
                        stream.Write(JsonConvert.SerializeObject(frames.ToArray()));
                        stream.WriteLine((i < lookupKvArray.Length - 1) ? "," : "");
                    }
                    stream.WriteLine("}");
                }
            }
        }

        /// <summary>
        /// Adds frame hash lookups for all frames in a sprite.
        /// </summary>
        /// <param name="spriteDef">The sprite which contains all the frames to be added.</param>
        /// <returns>The number of new frame hash lookups added.</returns>
        public static int AddFrameHashLookups(SpriteDef spriteDef) {
            int framesAdded = 0;
            foreach (var spritesheetKv in spriteDef.Spritesheets) {
                var frameSize = Spritesheet.KeyToDimensions(spritesheetKv.Key);
                var spritesheet = spritesheetKv.Value;

                var bitmapFilename = SpritesheetImageFile(spriteDef.Name, frameSize.Width, frameSize.Height);
                using (var bitmap = new Bitmap(bitmapFilename)) {
                    foreach (var frameGroupKv in spritesheet.FrameGroupsByName) {
                        var frameGroupName = frameGroupKv.Key;
                        var frameGroup = frameGroupKv.Value;
                        var frames = frameGroup.Frames;

                        foreach (var frameKv in frames) {
                            var frameDir = frameKv.Key;
                            var frame = frameKv.Value;

                            var x1 = frame.SpritesheetX;
                            var y1 = frame.SpritesheetY;
                            var x2 = x1 + frameSize.Width;
                            var y2 = y1 + frameSize.Height;

                            if (x1 >= 0 && y1 >= 0 && x2 <= bitmap.Width && y2 <= bitmap.Height) {
                                var bitmapData = bitmap.GetDataAt(x1, y1, frameSize.Width, frameSize.Height);
                                var texture = new TextureABGR1555(0, 0, 0, bitmapData);
                                var hash = texture.Hash;
                                if (AddFrameHashLookup(hash, spriteDef.Name, frameSize.Width, frameSize.Height, frameGroupName, frameDir))
                                    framesAdded++;
                            }
                        }
                    }
                }
            }
            return framesAdded;
        }

        /// <summary>
        /// Adds a sprite frame hash lookup, if it doesn't exist already.
        /// </summary>
        /// <param name="hash">The hash of the frame image.</param>
        /// <param name="spriteName">The name of the sprite to which this frame belongs.</param>
        /// <param name="frameWidth">The width of the frame.</param>
        /// <param name="frameHeight">The height of the frame.</param>
        /// <param name="frameGroupName">The frame group name of the frame (e.g, 'Idle 1').</param>
        /// <param name="frameDir">The direction of this frame.</param>
        /// <returns></returns>
        public static bool AddFrameHashLookup(string hash, string spriteName, int frameWidth, int frameHeight, string frameGroupName, SpriteFrameDirection frameDir) {
            var frameHashLookup = new FrameHashLookup() {
                SpriteName     = spriteName,
                FrameWidth     = frameWidth,
                FrameHeight    = frameHeight,
                FrameGroupName = frameGroupName,
                FrameDirection = frameDir,
                ImageHash      = hash,
            };

            if (!s_frameHashLookups.ContainsKey(hash)) {
                s_frameHashLookups.Add(hash, new FrameHashLookupSet(new FrameHashLookup[] { frameHashLookup }));
                return true;
            }

            var hashSet = s_frameHashLookups[hash];
            if (hashSet.Contains(frameHashLookup))
                return false;

            hashSet.Add(frameHashLookup);
            return true;
        }

        /// <summary>
        /// Returns an array of frame references based on an image hash.
        /// </summary>
        /// <param name="imageHash">An MD5 hash generated from an TextureABGR1555 of a frame image.</param>
        /// <returns>An array of FrameHashLookup's identifying where this frame image is used. If this frame image is unknown, an empty array is returned.</returns>
        public static FrameHashLookupSet GetFrameRefsForImageHash(string imageHash) {
            if (!s_frameHashLookupsLoaded)
                LoadFrameHashLookups();
            return s_frameHashLookups.TryGetValue(imageHash, out var frames) ? frames : new FrameHashLookupSet(imageHash);
        }

        public static UniqueAnimationDef GetUniqueAnimationInfoByHash(string hash) {
            LoadUniqueAnimationsByHashTable();
            if (!s_uniqueAnimationsByHash.ContainsKey(hash.ToLower()))
                s_uniqueAnimationsByHash[hash] = new UniqueAnimationDef(hash, "(Unknown)", 0, 0, 0, "(Unknown)");

            var animation = s_uniqueAnimationsByHash[hash];
            return animation;
        }

        private static void LoadUniqueAnimationsByHashTable() {
            if (s_uniqueAnimationsByHash != null)
                return;
            s_uniqueAnimationsByHash = new Dictionary<string, UniqueAnimationDef>();

            try {
                using (var stream = new FileStream(ResourceFile("SpriteAnimationsByHash.xml"), FileMode.Open, FileAccess.Read)) {
                    var settings = new XmlReaderSettings {
                        IgnoreComments = true,
                        IgnoreWhitespace = true
                    };

                    var xml = XmlReader.Create(stream, settings);
                    _ = xml.Read();

                    var nameDict = new Dictionary<int, string>();
                    while (!xml.EOF) {
                        _ = xml.Read();
                        if (xml.HasAttributes) {
                            var hash           = xml.GetAttribute("hash");
                            var sprite         = xml.GetAttribute("sprite");
                            var widthAttr      = xml.GetAttribute("width");
                            var heightAttr     = xml.GetAttribute("height");
                            var directionsAttr = xml.GetAttribute("directions");
                            var animation      = xml.GetAttribute("animation");

                            if (hash == null || sprite == null || animation == null || widthAttr == null || heightAttr == null || directionsAttr == null)
                                continue;

                            int width, height, directionsInt;
                            if (!int.TryParse(widthAttr, out width) || !int.TryParse(heightAttr, out height) || !int.TryParse(directionsAttr, out directionsInt))
                                continue;
                            var directions = (SpriteDirectionCountType) directionsInt;

                            if (sprite == "")
                                sprite = "None";

                            s_uniqueAnimationsByHash.Add(hash.ToLower(), new UniqueAnimationDef(hash, sprite, width, height, directions, animation));
                        }
                    }
                }
            }
            catch {
                // TDOO what to do here??
            }
        }

        private class AnimationHashCommand {
            public SpriteAnimationCommandType Command;
            public int Parameter;
            public int FrameID;
            public SpriteDirectionCountType Directions;
            public ITexture Image;
            public int FramesMissing;
        }

        public static string CreateAnimationHash(Models.Structs.CHR.AnimationCommand[] animationCommands) {
            var hashInfos = animationCommands
                .Select(x => new AnimationHashCommand() {
                    Command    = x.CommandType,
                    Parameter  = x.Parameter,
                    FrameID    = x.IsFrameCommand ? x.Command : -1,
                    Directions = x.Directions,
                    Image      = (x.IsFrameCommand) ? x.GetTexture(x.Directions) : null,
                    FramesMissing = x.FramesMissing
                })
                .ToArray();

            return CreateAnimationHash(hashInfos);
        }

        private static string CreateAnimationHash(AnimationHashCommand[] animationHashCommands) {
            // Build a unique hash string for this animation.
            var hashStr = "";
            foreach (var aniCommand in animationHashCommands) {
                if (hashStr != "")
                    hashStr += "_";

                if (aniCommand.Command != SpriteAnimationCommandType.Frame) {
                    var cmd   = aniCommand.Command;
                    var param = aniCommand.Parameter;

                    // Don't bother appending stops.
                    if (cmd == SpriteAnimationCommandType.Stop)
                        hashStr += "f2";
                    else
                        hashStr += $"{((int) cmd):x2},{aniCommand.Parameter:x2}";
                }
                else {
                    var tex = aniCommand.Image;
                    hashStr += (tex != null) ? $"{tex.Hash}_{aniCommand.Parameter:x2}" : $"{aniCommand.FrameID:x2},{aniCommand.Parameter:x2}";
                    if (aniCommand.FramesMissing > 0)
                        hashStr += $"_M{aniCommand.FramesMissing})";
                }
            }

            using (var md5 = MD5.Create())
                return BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(hashStr))).Replace("-", "").ToLower();
        }
    }
}
