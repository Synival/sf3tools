using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using SF3.Sprites;
using SF3.Types;

namespace SF3.Utils {
    public static class CHR_Utils {
        private static Dictionary<string, UniqueFrameInfo> s_uniqueFramesByHash = null;
        private static Dictionary<string, UniqueAnimationInfo> s_uniqueAnimationsByHash = null;

        public static UniqueFrameInfo GetUniqueFrameInfoByHash(string hash) {
            LoadUniqueFramesByHashTable();
            if (!s_uniqueFramesByHash.ContainsKey(hash.ToLower()))
                s_uniqueFramesByHash[hash] = new UniqueFrameInfo(hash, "Unknown", 0, 0, "Unknown", SpriteFrameDirection.Unset);
            return s_uniqueFramesByHash[hash];
        }

        public static UniqueAnimationInfo GetUniqueAnimationInfoByHash(string hash) {
            LoadUniqueAnimationsByHashTable();
            if (!s_uniqueAnimationsByHash.ContainsKey(hash.ToLower())) {
                s_uniqueAnimationsByHash[hash] = new UniqueAnimationInfo(hash, "Unknown", "Unknown", 0, 0, 0, 0, 0, 0,
                    new UniqueSpriteAnimationCollectionDef.Variant.Animation.Frame[0]);
            }
            return s_uniqueAnimationsByHash[hash];
        }

        private static void LoadUniqueFramesByHashTable() {
            if (s_uniqueFramesByHash != null)
                return;
            s_uniqueFramesByHash = new Dictionary<string, UniqueFrameInfo>();

            using (var stream = new FileStream(CommonLib.Utils.ResourceUtils.ResourceFile("SpriteFramesByHash.xml"), FileMode.Open, FileAccess.Read)) {
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
                        var hash       = xml.GetAttribute("hash");
                        var sprite     = xml.GetAttribute("sprite");
                        var frameAttr  = xml.GetAttribute("frame");
                        var widthAttr  = xml.GetAttribute("width");
                        var heightAttr = xml.GetAttribute("height");
                        var directionAttr = xml.GetAttribute("direction");

                        if (hash == null || sprite == null || widthAttr == null || heightAttr == null)
                            continue;

                        int width, height;
                        if (!int.TryParse(widthAttr, out width) || !int.TryParse(heightAttr, out height))
                            continue;

                        var frame = frameAttr ?? "";
                        var direction = Enum.TryParse<SpriteFrameDirection>(directionAttr, out var directionOut) ? directionOut : SpriteFrameDirection.Unset;
                        s_uniqueFramesByHash.Add(hash.ToLower(), new UniqueFrameInfo(hash, sprite, width, height, frame, direction));
                    }
                }
            }
        }

        private static void LoadUniqueAnimationsByHashTable() {
            if (s_uniqueAnimationsByHash != null)
                return;
            s_uniqueAnimationsByHash = new Dictionary<string, UniqueAnimationInfo>();

            using (var stream = new FileStream(CommonLib.Utils.ResourceUtils.ResourceFile("SpriteAnimationsByHash.xml"), FileMode.Open, FileAccess.Read)) {
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
                        var animation      = xml.GetAttribute("animation");
                        var widthAttr      = xml.GetAttribute("width");
                        var heightAttr     = xml.GetAttribute("height");
                        var directionsAttr = xml.GetAttribute("directions");
                        var framesAttr     = xml.GetAttribute("frames");
                        var durationAttr   = xml.GetAttribute("duration");
                        var missingAttr    = xml.GetAttribute("missingFrames");

                        if (hash == null || sprite == null || animation == null || widthAttr == null || heightAttr == null || directionsAttr == null)
                            continue;

                        int width, height, directions, frames;
                        if (!int.TryParse(widthAttr, out width) || !int.TryParse(heightAttr, out height) || !int.TryParse(directionsAttr, out directions) || !int.TryParse(framesAttr, out frames))
                            continue;

                        if (sprite == "")
                            sprite = "None";

                        int missingFrames = int.TryParse(missingAttr, out var missingFramesOut) ? missingFramesOut : 0;
                        int duration = int.TryParse(durationAttr, out var durationOut) ? durationOut : 0;
                        s_uniqueAnimationsByHash.Add(hash.ToLower(), new UniqueAnimationInfo(hash, sprite, animation, width, height, directions, frames, duration, missingFrames,
                            new UniqueSpriteAnimationCollectionDef.Variant.Animation.Frame[0]));
                    }
                }
            }
        }

        public static void WriteUniqueFramesByHashXML(StreamWriter stream) {
            foreach (var fi in s_uniqueFramesByHash.Values) {
                var dirs = new HashSet<SpriteFrameDirection>(fi.Directions.Where(x => x != SpriteFrameDirection.Unset));
                var standardDirs = dirs.Where(x =>
                    x == SpriteFrameDirection.SSE ||
                    x == SpriteFrameDirection.ESE ||
                    x == SpriteFrameDirection.ENE ||
                    x == SpriteFrameDirection.NNE
                ).ToArray();
                if (standardDirs.Length == 1 && dirs.Count != 1)
                    dirs = new HashSet<SpriteFrameDirection>() { standardDirs[0] };

                fi.Direction = dirs.Count == 1 ? dirs.First() : SpriteFrameDirection.Unset;
                fi.FrameName = string.Join(", ", fi.AnimationNames.OrderBy(x => x));
            }

            var frameInfos = s_uniqueFramesByHash.Values
                .OrderBy(x => x.SpriteName)
                .ThenBy(x => x.Width)
                .ThenBy(x => x.Height)
                .ThenBy(x => x.FrameName)
                .ThenBy(x => x.Direction)
                .ThenBy(x => x.TextureHash)
                .ToArray();

            stream.NewLine = "\n";
            stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            stream.WriteLine("<items>");
            foreach (var fi in frameInfos) {
                if (fi.AnimationNames.Count == 0) {
                    fi.Direction = SpriteFrameDirection.Unset;
                    fi.FrameName = "Unused";
                }

                stream.WriteLine($"    <item hash=\"{fi.TextureHash}\" sprite=\"{fi.SpriteName}\" width=\"{fi.Width}\" height=\"{fi.Height}\" frame=\"{fi.FrameName}\" direction=\"{fi.Direction}\" />");
            }

            stream.WriteLine("</items>");
        }

        public static void WriteUniqueAnimationsByHashXML(StreamWriter stream) {
            var animationInfos = s_uniqueAnimationsByHash.Values
                .OrderBy(x => x.SpriteName)
                .ThenBy(x => x.Width)
                .ThenBy(x => x.Height)
                .ThenBy(x => x.AnimationName)
                .ThenBy(x => x.Directions)
                .ThenBy(x => x.FrameCommandCount)
                .ThenBy(x => x.Duration)
                .ThenBy(x => x.FrameTexturesMissing)
                .ThenBy(x => x.AnimationHash)
                .ToArray();

            stream.NewLine = "\n";
            stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            stream.WriteLine("<items>");
            foreach (var ai in animationInfos) {
                var spriteName = (ai.SpriteName == "") ? "None" : ai.SpriteName;

                var missingFramesStr = (ai.FrameTexturesMissing == 0) ? "" : $" missingFrames=\"{ai.FrameTexturesMissing}\"";
                stream.WriteLine($"    <item hash=\"{ai.AnimationHash}\" sprite=\"{spriteName}\" animation=\"{ai.AnimationName}\" width=\"{ai.Width}\" height=\"{ai.Height}\" directions=\"{ai.Directions}\" frames=\"{ai.FrameCommandCount}\" duration=\"{ai.Duration}\"{missingFramesStr} />");
            }
            stream.WriteLine("</items>");
        }

        public static UniqueSpriteAnimationCollectionDef[] GetAllUniqueSpriteAnimationDefs() {
            return s_uniqueAnimationsByHash.Values
                .GroupBy(x => x.SpriteName)
                .Select(x => new UniqueSpriteAnimationCollectionDef(x.Key, x.ToArray()))
                .ToArray();
        }

        public static void WriteUniqueAnimationsByHashJSON(StreamWriter stream) {
            stream.NewLine = "\n";
            var animationInfos = GetAllUniqueSpriteAnimationDefs();
            stream.Write(JsonConvert.SerializeObject(animationInfos, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
