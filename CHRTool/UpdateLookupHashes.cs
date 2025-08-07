using System;
using System.Drawing;
using System.IO;
using SF3.Sprites;
using SF3.Utils;
using SF3.Extensions;
using SF3;
using SF3.Types;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CHRTool {
    public static class UpdateLookupHashes {
        // TODO: move this into SF3Lib
        private class FrameHashLookup {
            public string SpriteName;
            public int FrameWidth;
            public int FrameHeight;
            public string FrameGroupName;

            [JsonConverter(typeof(StringEnumConverter))]
            public SpriteFrameDirection FrameDirection;

            public override bool Equals(object obj) {
                return obj is FrameHashLookup lookup
                    && SpriteName     == lookup.SpriteName
                    && FrameWidth     == lookup.FrameWidth
                    && FrameHeight    == lookup.FrameHeight
                    && FrameGroupName == lookup.FrameGroupName
                    && FrameDirection == lookup.FrameDirection;
            }

            public override int GetHashCode() => HashCode.Combine(SpriteName, FrameWidth, FrameHeight, FrameGroupName, FrameDirection);
        }

        public static int Run(string[] args, string spriteDir, string spritesheetDir, string hashLookupDir) {
            // (any extra options would go here.)

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Console.Error.WriteLine("Unrecognized arguments in 'update-hashes' command:");
                Console.Error.Write($"    {string.Join(" ", args)}");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            Console.WriteLine("Updating lookup hashes...");
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"Sprite directory:      {spriteDir}");
            Console.WriteLine($"Spritesheet directory: {spritesheetDir}");
            Console.WriteLine($"Hash lookup directory: {hashLookupDir}");
            Console.WriteLine("------------------------------------------------------------------------------");

            // Fetch all .SF3Sprite files
            string[] files;
            try {
                Console.WriteLine("Getting list of SF3Sprites...");
                files = Directory.GetFiles(spriteDir, "*.SF3Sprite");
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"  Couldn't get SF3Sprite files from path '{spriteDir}':");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // Get the curent list of frame hash lookups.
            var frameHashLookupFilename = Path.Combine(hashLookupDir, "FrameHashLookups.json");
            var frameHashLookupSet = new Dictionary<string, HashSet<FrameHashLookup>>();
            try {
                if (!File.Exists(frameHashLookupFilename))
                    Console.WriteLine($"Couldn't find '{frameHashLookupFilename}'. Creating file from scratch.");
                else {
                    Console.WriteLine($"Loading '{frameHashLookupFilename}'...");
                    var jsonText = File.ReadAllText(frameHashLookupFilename);
                    var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, FrameHashLookup[]>>(jsonText);
                    frameHashLookupSet = jsonObj
                        .ToDictionary(x => x.Key, x => new HashSet<FrameHashLookup>(x.Value));
                }
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"  Couldn't fetch current frame hash lookups:");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                Console.Error.WriteLine($"  The file has likely been corrupted. Consider deleting it and trying again");
                return 1;
            }

            // Open SF3Sprite files and their accompanying spritesheets.
            int totalFramesAdded = 0;
            Console.WriteLine("Checking sprites for new frame hashes:");
            foreach (var file in files) {
                Console.WriteLine($"  {Path.GetFileName(file)}");
                try {
                    var jsonText = File.ReadAllText(file);
                    var spriteDef = SpriteDef.FromJSON(jsonText);
                    var framesAdded = AddFrameHashLookups(frameHashLookupSet, spriteDef, spritesheetDir);
                    totalFramesAdded += framesAdded;

                    if (framesAdded > 0)
                        Console.WriteLine($"    {framesAdded} frame(s) added");
                }
                catch (Exception e) {
                    Console.WriteLine();
                    Console.Error.WriteLine($"  Couldn't update hashes sheets for '{file}':");
                    Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                }
            }

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"Writing '{frameHashLookupFilename}'...");
            try {
                // We could just use SerializeObject(), but then the file would either be one giant blob, or a
                // 100,000 line long mess, so let's do some custom writing.
                using (var file = File.Open(frameHashLookupFilename, FileMode.Create)) {
                    using (var stream = new StreamWriter(file)) {
                        stream.WriteLine("{");

                        var lookupKvArray = frameHashLookupSet
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
            catch (Exception e) {
                Console.WriteLine();
                Console.Error.WriteLine($"  Couldn't write '{frameHashLookupFilename}':");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
            }

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"Done. {totalFramesAdded} frame(s) added.");
            return 0;
        }

        private static int AddFrameHashLookups(Dictionary<string, HashSet<FrameHashLookup>> set, SpriteDef spriteDef, string spritesheetDir) {
            int framesAdded = 0;
            foreach (var spritesheetKv in spriteDef.Spritesheets) {
                var frameSize = Spritesheet.KeyToDimensions(spritesheetKv.Key);
                var spritesheet = spritesheetKv.Value;

                var bitmapFilename = Path.Combine(spritesheetDir, $"{SpriteUtils.FilesystemName(spriteDef.Name)} ({spritesheetKv.Key}).png");
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
                                if (AddFrameHashLookup(set, hash, spriteDef.Name, frameSize.Width, frameSize.Height, frameGroupName, frameDir))
                                    framesAdded++;
                            }
                        }
                    }
                }
            }
            return framesAdded;
        }

        private static bool AddFrameHashLookup(Dictionary<string, HashSet<FrameHashLookup>> set, string hash, string spriteName, int frameWidth, int frameHeight, string frameGroupName, SpriteFrameDirection frameDir) {
            var frameHashLookup = new FrameHashLookup() {
                SpriteName     = spriteName,
                FrameWidth     = frameWidth,
                FrameHeight    = frameHeight,
                FrameGroupName = frameGroupName,
                FrameDirection = frameDir,
            };

            if (!set.ContainsKey(hash)) {
                set.Add(hash, new HashSet<FrameHashLookup>() { frameHashLookup });
                return true;
            }

            var hashSet = set[hash];
            if (hashSet.Contains(frameHashLookup))
                return false;

            hashSet.Add(frameHashLookup);
            return true;
        }
    }
}
