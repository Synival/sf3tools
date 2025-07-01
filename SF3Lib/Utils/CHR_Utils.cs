using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SF3.Models.Structs.CHR;

namespace SF3.Utils {
    public static class CHR_Utils {
        private static Dictionary<string, UniqueFrameInfo> s_uniqueFramesByHash = null;
        private static Dictionary<string, UniqueAnimationInfo> s_uniqueAnimationsByHash = null;

        public static UniqueFrameInfo GetUniqueFrameInfoByHash(string hash, int width, int height) {
            LoadUniqueFramesByHashTable();
            if (!s_uniqueFramesByHash.ContainsKey(hash.ToLower()))
                s_uniqueFramesByHash[hash] = new UniqueFrameInfo(hash, "Unknown", width, height, "Unknown");
            return s_uniqueFramesByHash[hash];
        }

        public static UniqueAnimationInfo AddUniqueAnimationInfo(string hash, string spriteName, string animationName, int width, int height, int directions, int frames, int duration, int missingFrames) {
            LoadUniqueAnimationsByHashTable();
            if (s_uniqueAnimationsByHash.TryGetValue(hash.ToLower(), out var val))
                return val;
            return s_uniqueAnimationsByHash[hash] = new UniqueAnimationInfo(hash, spriteName, animationName, width, height, directions, frames, duration, missingFrames);
        }

        public static UniqueAnimationInfo GetUniqueAnimationInfoByHash(string hash, int width, int height, int directions) {
            LoadUniqueAnimationsByHashTable();
            if (!s_uniqueAnimationsByHash.ContainsKey(hash.ToLower()))
                s_uniqueAnimationsByHash[hash] = new UniqueAnimationInfo(hash, "Unknown", "Unknown", width, height, directions, 0, 0, 0);
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
                        var widthAttr  = xml.GetAttribute("width");
                        var heightAttr = xml.GetAttribute("height");

                        if (hash == null || sprite == null || widthAttr == null || heightAttr == null)
                            continue;

                        int width, height;
                        if (!int.TryParse(widthAttr, out width) || !int.TryParse(heightAttr, out height))
                            continue;

                        var animation = xml.GetAttribute("animation") ?? "";
                        s_uniqueFramesByHash.Add(hash.ToLower(), new UniqueFrameInfo(hash, sprite, width, height, animation));
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
                        s_uniqueAnimationsByHash.Add(hash.ToLower(), new UniqueAnimationInfo(hash, sprite, animation, width, height, directions, frames, duration, missingFrames));
                    }
                }
            }
        }

        public static void WriteUniqueFramesByHashXML(StreamWriter stream) {
            var frameInfos = s_uniqueFramesByHash.Values
                .OrderBy(x => x.SpriteName)
                .ThenBy(x => x.Width)
                .ThenBy(x => x.Height)
                .ThenBy(x => x.AnimationName)
                .ThenBy(x => x.TextureHash)
                .ToArray();

            stream.NewLine = "\n";
            stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            stream.WriteLine("<items>");
            foreach (var fi in frameInfos)
                stream.WriteLine($"    <item hash=\"{fi.TextureHash}\" sprite=\"{fi.SpriteName}\" width=\"{fi.Width}\" height=\"{fi.Height}\" animation=\"{fi.AnimationName}\" />");
            stream.WriteLine("</items>");
        }

        public static void WriteUniqueAnimationsByHashXML(StreamWriter stream) {
            var animationInfos = s_uniqueAnimationsByHash.Values
                .OrderBy(x => x.SpriteName)
                .ThenBy(x => x.Width)
                .ThenBy(x => x.Height)
                .ThenBy(x => x.AnimationName)
                .ThenBy(x => x.Directions)
                .ThenBy(x => x.Frames)
                .ThenBy(x => x.Duration)
                .ThenBy(x => x.MissingFrames)
                .ThenBy(x => x.AnimationHash)
                .ToArray();

            stream.NewLine = "\n";
            stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            stream.WriteLine("<items>");
            foreach (var ai in animationInfos) {
                var spriteName = (ai.SpriteName == "") ? "None" : ai.SpriteName;

                var missingFramesStr = (ai.MissingFrames == 0) ? "" : $" missingFrames=\"{ai.MissingFrames}\"";
                stream.WriteLine($"    <item hash=\"{ai.AnimationHash}\" sprite=\"{spriteName}\" animation=\"{ai.AnimationName}\" width=\"{ai.Width}\" height=\"{ai.Height}\" directions=\"{ai.Directions}\" frames=\"{ai.Frames}\" duration=\"{ai.Duration}\"{missingFramesStr} />");
            }
            stream.WriteLine("</items>");
        }
    }
}
