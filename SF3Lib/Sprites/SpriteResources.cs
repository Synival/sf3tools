using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using SF3.Extensions;
using SF3.Types;
using SF3.Utils;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Sprites {
    public static class SpriteResources {
        private static Dictionary<string, SpriteDef> s_spriteDefs = new Dictionary<string, SpriteDef>();
        private static HashSet<string> s_spriteDefFilesLoaded = new HashSet<string>();
        private static Dictionary<string, FrameRefSet> s_frameRefsByHash = new Dictionary<string, FrameRefSet>();
        private static Dictionary<string, AnimationRef> s_animationRefsByHash = new Dictionary<string, AnimationRef>();
        private static HashSet<string> s_animationRefsLoaded = new HashSet<string>();

        private static bool s_frameRefsLoaded = false;

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
        public static void LoadFrameRefs() {
            var filename = FrameHashLookupsFile ?? ResourceFile("FrameHashLookups.json");
            var jsonText = File.ReadAllText(filename);
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, FrameRef[]>>(jsonText);

            foreach (var lookupKv in jsonObj) {
                foreach (var frameRef in lookupKv.Value)
                    frameRef.ImageHash = lookupKv.Key;

                if (!s_frameRefsByHash.ContainsKey(lookupKv.Key))
                    s_frameRefsByHash.Add(lookupKv.Key, new FrameRefSet(lookupKv.Value));
                else {
                    foreach (var frame in lookupKv.Value)
                        s_frameRefsByHash[lookupKv.Key].Add(frame);
                }
            }

            s_frameRefsLoaded = true;
        }

        /// <summary>
        /// Rewrites the list of sprite frames that can be looked up by hash.
        /// This will rewrite 'Resources/FrameHashLookups.json' by default, or the file set by SetFrameHashLookupsFile().
        /// </summary>
        public static void WriteFrameRefsJSON() {
            var filename = FrameHashLookupsFile ?? ResourceFile("FrameHashLookups.json");

            // We could just use SerializeObject(), but then the file would either be one giant blob, or a
            // 100,000 line long mess, so let's do some custom writing.
            using (var file = File.Open(FrameHashLookupsFile, FileMode.Create)) {
                using (var stream = new StreamWriter(file)) {
                    stream.NewLine = "\n";
                    stream.WriteLine("{");

                    var lookupKvArray = s_frameRefsByHash
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
        public static int AddFrameRefs(SpriteDef spriteDef) {
            int framesAdded = 0;
            foreach (var spritesheetKv in spriteDef.Spritesheets) {
                var frameSize = Spritesheet.KeyToDimensions(spritesheetKv.Key);
                var spritesheet = spritesheetKv.Value;

                var bitmapFilename = SpritesheetImageFile(spriteDef.Name, frameSize.Width, frameSize.Height);
                using (var bitmap = new Bitmap(new MemoryStream(File.ReadAllBytes(bitmapFilename)))) {
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
                                if (AddFrameRef(hash, spriteDef.Name, frameSize.Width, frameSize.Height, frameGroupName, frameDir))
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
        public static bool AddFrameRef(string hash, string spriteName, int frameWidth, int frameHeight, string frameGroupName, SpriteFrameDirection frameDir) {
            var frameHashLookup = new FrameRef() {
                SpriteName     = spriteName,
                FrameWidth     = frameWidth,
                FrameHeight    = frameHeight,
                FrameGroupName = frameGroupName,
                FrameDirection = frameDir,
                ImageHash      = hash,
            };

            if (!s_frameRefsByHash.ContainsKey(hash)) {
                s_frameRefsByHash.Add(hash, new FrameRefSet(new FrameRef[] { frameHashLookup }));
                return true;
            }

            var hashSet = s_frameRefsByHash[hash];
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
        public static FrameRefSet GetFrameRefsByImageHash(string imageHash) {
            if (!s_frameRefsLoaded)
                LoadFrameRefs();
            return s_frameRefsByHash.TryGetValue(imageHash, out var frames) ? frames : new FrameRefSet(imageHash);
        }

        /// <summary>
        /// Adds animation hash lookups for all frames in a sprite.
        /// </summary>
        /// <param name="spriteName">Name of the sprite which contains all the animations to be added.</param>
        /// <returns>The number of new animations hash lookups added.</returns>
        public static int AddAnimationRefs(string spriteName) {
            if (s_animationRefsLoaded.Contains(spriteName))
                return 0;
            s_animationRefsLoaded.Add(spriteName);
            var spriteDef = GetSpriteDef(spriteName);

            int animationsAdded = 0;
            foreach (var spritesheetKv in spriteDef.Spritesheets) {
                var frameSize = Spritesheet.KeyToDimensions(spritesheetKv.Key);
                var spritesheet = spritesheetKv.Value;

                foreach (var animationSetKv in spritesheet.AnimationSetsByDirections) {
                    var directions = animationSetKv.Key;
                    foreach (var animationKv in animationSetKv.Value.AnimationsByName) {
                        var animationName = animationKv.Key;
                        var animation = animationKv.Value;
                        var animationHash = CreateAnimationHash(spriteDef, frameSize.Width, frameSize.Height, directions, animation.AnimationCommands);

                        // TODO: Hash collisions and duplication animations can indeed happen. We should handle that!
                        if (s_animationRefsByHash.ContainsKey(animationHash))
                            continue;

                        var uniqueAnim = new AnimationRef() {
                            AnimationHash = animationHash,
                            SpriteName    = spriteDef.Name,
                            FrameWidth    = frameSize.Width,
                            FrameHeight   = frameSize.Height,
                            Directions    = directions,
                            AnimationName = animationName,
                        };
                        s_animationRefsByHash.Add(animationHash, uniqueAnim);
                        animationsAdded++;
                    }
                }
            }
            return animationsAdded;
        }

        /// <summary>
        /// Returns an animation reference baesd on an animation hash. Can be used to fetch an animation name from a CHR file.
        /// </summary>
        /// <param name="spriteName">The name of the sprite to which this animation belongs.</param>
        /// <param name="animationHash">The hash generated for the animation.</param>
        /// <returns></returns>
        public static AnimationRef GetAnimationRefByImageHash(string spriteName, string animationHash) {
            // Load the sprite def to get the animation hashes.
            if (!s_animationRefsLoaded.Contains(spriteName))
                AddAnimationRefs(spriteName);

            //LoadUniqueAnimationsByHashTable();
            if (!s_animationRefsByHash.ContainsKey(animationHash.ToLower())) {
                s_animationRefsByHash[animationHash] = new AnimationRef() {
                    AnimationHash = animationHash,
                    SpriteName = "(Unknown)",
                    FrameWidth = 0,
                    FrameHeight = 0,
                    Directions = 0,
                    AnimationName = "(Unknown)"
                };
            }

            var animation = s_animationRefsByHash[animationHash];
            return animation;
        }

        /// <summary>
        /// Generates a unique animation hash based on animation commands found in a CHR file.
        /// </summary>
        /// <param name="animationCommands">Animation commands from an animation in a CHR file.</param>
        /// <returns>A unique hash corresponding to the animation.</returns>
        public static string CreateAnimationHash(Models.Structs.CHR.AnimationCommand[] animationCommands) {
            var hashInfos = animationCommands
                .Select(x => new AnimationHashCommand() {
                    Command    = x.CommandType,
                    Parameter  = x.Parameter,
                    FrameID    = x.IsFrameCommand ? x.Command : -1,
                    Directions = x.Directions,
                    ImageHash  = (x.IsFrameCommand) ? x.GetTexture(x.Directions)?.Hash : null,
                    FramesMissing = x.FramesMissing
                })
                .ToArray();

            return CreateAnimationHash(hashInfos);
        }

        /// <summary>
        /// Generates a unique animation hash based on animation commands found in a SpriteDef.
        /// </summary>
        /// <param name="spriteDef">The SpriteDef which contains the animation.</param>
        /// <param name="frameWidth">The width of the frames in the animation.</param>
        /// <param name="frameHeight">The height of the frames in the animation.</param>
        /// <param name="directions">The initial number of directions in this animation.</param>
        /// <param name="animationCommands">The commands in this animation.</param>
        /// <returns>A unique hash corresponding to the animation.</returns>
        public static string CreateAnimationHash(SpriteDef spriteDef, int frameWidth, int frameHeight, SpriteDirectionCountType directions, AnimationCommand[] animationCommands) {
            var spritesheetKey = Spritesheet.DimensionsToKey(frameWidth, frameHeight);
            if (spriteDef == null || spriteDef.Spritesheets == null || !spriteDef.Spritesheets.ContainsKey(spritesheetKey))
                return null;

            // We'll need to get images from the spritesheet. Check if it exists.
            var spritesheetImageFile = SpritesheetImageFile(spriteDef.Name, frameWidth, frameHeight);
            Bitmap spritesheetBitmap = null;
            if (File.Exists(spritesheetImageFile)) {
                try {
                    spritesheetBitmap = new Bitmap(new MemoryStream(File.ReadAllBytes(spritesheetImageFile)));
                }
                catch {
                    // TODO: log error?
                }
            }

            AnimationHashCommand[] hashInfos;
            using (spritesheetBitmap) {
                var spritesheet = spriteDef.Spritesheets[spritesheetKey];
                var animationFrameCount = directions.GetAnimationFrameCount();
                hashInfos = animationCommands
                    .Select(x => {
                        // Account for changing frame counts.
                        if (x.Command == SpriteAnimationCommandType.SetDirectionCount)
                            animationFrameCount = ((SpriteDirectionCountType) x.Parameter).GetAnimationFrameCount();

                        // Non-frame commands are straight-forward for hash generation.
                        if (x.Command != SpriteAnimationCommandType.Frame) {
                            return new AnimationHashCommand() {
                                Command    = x.Command,
                                Parameter  = x.Parameter,
                                FrameID    = -1,
                                Directions = directions,
                                ImageHash  = null,
                                FramesMissing = 0
                            };
                        }

                        // For frames, we'll need to get the actual image. Start by getting the frame references.
                        FrameRef[] frameRefs = null;
                        var frameGroupDirections = CHR_Utils.GetFrameGroupDirections(animationFrameCount);
                        if (x.FrameGroup != null) {
                            frameRefs = frameGroupDirections
                                .Select(y => new FrameRef() {
                                    SpriteName = spriteDef.Name,
                                    FrameWidth = frameWidth,
                                    FrameHeight = frameHeight,
                                    FrameDirection = y,
                                    FrameGroupName = x.FrameGroup
                                })
                                .ToArray();
                        }
                        else {
                            frameRefs = frameGroupDirections
                                .Select(y => {
                                    if (!x.FramesByDirection.ContainsKey(y))
                                        return null;

                                    var frame = x.FramesByDirection[y];
                                    return new FrameRef() {
                                        SpriteName = spriteDef.Name,
                                        FrameWidth = frameWidth,
                                        FrameHeight = frameHeight,
                                        FrameDirection = frame.Direction,
                                        FrameGroupName = frame.FrameGroup
                                    };
                                })
                                .ToArray();
                        }

                        // Get the image on the spritesheet.
                        var textures = new List<ITexture>();
                        if (spritesheetBitmap != null) {
                            foreach (var frameRef in frameRefs.Where(y => y != null)) {
                                if (spritesheet.FrameGroupsByName.TryGetValue(frameRef.FrameGroupName, out var frameGroup)) {
                                    if (frameGroup.Frames.TryGetValue(frameRef.FrameDirection, out var frame)) {
                                        var frameImageData = spritesheetBitmap.GetDataAt(frame.SpritesheetX, frame.SpritesheetY, frameWidth, frameHeight);
                                        if (frameImageData != null)
                                            textures.Add(new TextureABGR1555(0, 0, 0, frameImageData));
                                    }
                                }
                            }
                        }
                        var stackedTexture = TextureUtils.StackTextures(0, 0, 0, textures.ToArray());

                        return new AnimationHashCommand() {
                            Command    = x.Command,
                            Parameter  = x.Parameter,
                            FrameID    = -1,
                            Directions = directions,
                            ImageHash  = stackedTexture?.Hash,
                            FramesMissing = animationFrameCount - textures.Count
                        };
                    })
                    .ToArray();
            }

            return CreateAnimationHash(hashInfos);
        }

        private class AnimationHashCommand {
            public SpriteAnimationCommandType Command;
            public int Parameter;
            public int FrameID;
            public SpriteDirectionCountType Directions;
            public string ImageHash;
            public int FramesMissing;
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
                    hashStr += (aniCommand.ImageHash != null) ? $"{aniCommand.ImageHash}_{aniCommand.Parameter:x2}" : $"{aniCommand.FrameID:x2},{aniCommand.Parameter:x2}";
                    if (aniCommand.FramesMissing > 0)
                        hashStr += $"_M{aniCommand.FramesMissing})";
                }
            }

            using (var md5 = MD5.Create())
                return BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(hashStr))).Replace("-", "").ToLower();
        }
    }
}
