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
        private static Dictionary<string, UniqueFrameDef> s_uniqueFramesByHash = null;
        private static Dictionary<string, UniqueAnimationDef> s_uniqueAnimationsByHash = null;
        private static Dictionary<string, UniqueSpriteInfoDef> s_uniqueSpriteInfosByName = new Dictionary<string, UniqueSpriteInfoDef>();

        public static UniqueFrameDef GetUniqueFrameInfoByHash(string hash) {
            LoadUniqueFramesByHashTable();
            if (!s_uniqueFramesByHash.ContainsKey(hash.ToLower()))
                s_uniqueFramesByHash[hash] = new UniqueFrameDef(hash, "Unknown", 0, 0, "Unknown", SpriteFrameDirection.Unset);
            return s_uniqueFramesByHash[hash];
        }

        public static UniqueAnimationDef GetUniqueAnimationInfoByHash(string hash) {
            LoadUniqueAnimationsByHashTable();
            if (!s_uniqueAnimationsByHash.ContainsKey(hash.ToLower())) {
                s_uniqueAnimationsByHash[hash] = new UniqueAnimationDef(hash, "Unknown", "Unknown", 0, 0, 0, 0, 0, 0,
                    new AnimationFrameDef[0]);
            }
            return s_uniqueAnimationsByHash[hash];
        }

        private static void LoadUniqueFramesByHashTable() {
            if (s_uniqueFramesByHash != null)
                return;
            s_uniqueFramesByHash = new Dictionary<string, UniqueFrameDef>();

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
                        s_uniqueFramesByHash.Add(hash.ToLower(), new UniqueFrameDef(hash, sprite, width, height, frame, direction));
                    }
                }
            }
        }

        private static void LoadUniqueAnimationsByHashTable() {
            if (s_uniqueAnimationsByHash != null)
                return;
            s_uniqueAnimationsByHash = new Dictionary<string, UniqueAnimationDef>();

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
                        s_uniqueAnimationsByHash.Add(hash.ToLower(), new UniqueAnimationDef(hash, sprite, animation, width, height, directions, frames, duration, missingFrames,
                            new AnimationFrameDef[0]));
                    }
                }
            }
        }

        public static void WriteUniqueFramesByHashXML(StreamWriter stream) {
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
            foreach (var fi in frameInfos)
                stream.WriteLine($"    <item hash=\"{fi.TextureHash}\" sprite=\"{fi.SpriteName}\" width=\"{fi.Width}\" height=\"{fi.Height}\" frame=\"{fi.FrameName}\" direction=\"{fi.Direction}\" />");

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

        public static SpriteDef[] CreateAllSpriteDefs() {
            string[] ApplicableSpriteNames(string spriteName) {
                // Edmund's P1 sprites are special because some frames are shared with and without a weapon.
                // (His cape is so big, the rendered frames are the same when his back is turned)
                // Make some very specific corrections to include the duplicated frames in both sprite
                // definitions.
                if (spriteName == "Edmund (P1)")
                    return new string[] { "Edmund (P1)", "Edmund (P1) (Sword/Weaponless)" };
                else if (spriteName == "Edmund (P1) (Weaponless)")
                    return new string[] { "Edmund (P1) (Weaponless)", "Edmund (P1) (Sword/Weaponless)" };
                else
                    return spriteName.Split('|').Select(x => x.Trim()).ToArray();
            }

            string[] ExtraFrameHashes(string spriteName) {
                // Explosions need to steal some transparency frames.
                if (spriteName == "Explosion")
                    return new string[] { "d33e04f92840fa8d80d642441797bafe" };
                else
                    return new string[0];
            }

            return s_uniqueAnimationsByHash.Values
                .GroupBy(x => x.SpriteName)
                .Select(x => {
                    var spriteNames = ApplicableSpriteNames(x.Key);
                    var extraFrameHashes = ExtraFrameHashes(x.Key);
                    var frames = s_uniqueFramesByHash
                        .Where(y => y.Value.TextureHash != null && (spriteNames.Contains(y.Value.SpriteName) || extraFrameHashes.Contains(y.Value.TextureHash)))
                        .GroupBy(y => y.Value.TextureHash)
                        .Select(y => y.First().Value)
                        .OrderBy(y => y.Width)
                        .OrderBy(y => y.Height)
                        .OrderBy(y => y.FrameName)
                        .OrderBy(y => y.Direction)
                        .OrderBy(y => y.TextureHash)
                        .ToArray();
                    return new SpriteDef(x.Key, frames, x
                        .GroupBy(y => y.AnimationHash)
                        .Select(y => y.First())
                        .ToArray());
                })
                .Where(x => x.Spritesheets.Count > 0)
                .ToArray();
        }

        public static void WriteUniqueAnimationsByHashJSON(StreamWriter stream) {
            stream.NewLine = "\n";
            var animationInfos = CreateAllSpriteDefs();
            stream.Write(JsonConvert.SerializeObject(animationInfos, Newtonsoft.Json.Formatting.Indented));
        }

        public static SpriteFrameDirection FrameNumberToSpriteDir(int dirs, int num) {
            switch (dirs) {
                case 4:
                case 6:
                    switch (num) {
                        case 0:  return SpriteFrameDirection.SSE;
                        case 1:  return SpriteFrameDirection.ESE;
                        case 2:  return SpriteFrameDirection.ENE;
                        case 3:  return SpriteFrameDirection.NNE;
                        case 4:  return SpriteFrameDirection.S;
                        default: return SpriteFrameDirection.N;
                    }

                case 8:
                    switch (num) {
                        case 0:  return SpriteFrameDirection.SSE;
                        case 1:  return SpriteFrameDirection.ESE;
                        case 2:  return SpriteFrameDirection.ENE;
                        case 3:  return SpriteFrameDirection.NNE;
                        case 4:  return SpriteFrameDirection.NNW;
                        case 5:  return SpriteFrameDirection.WNW;
                        case 6:  return SpriteFrameDirection.WSW;
                        default: return SpriteFrameDirection.SSW;
                    }

                case 5:
                    switch (num) {
                        case 0:  return SpriteFrameDirection.S;
                        case 1:  return SpriteFrameDirection.SE;
                        case 2:  return SpriteFrameDirection.E;
                        case 3:  return SpriteFrameDirection.NE;
                        default: return SpriteFrameDirection.N;
                    }

                default:
                    return SpriteFrameDirection.First + num;
            }
        }

        public static int DirectionsToFrameCount(int directions) {
            switch (directions) {
                case 2:  return 2;
                case 4:  return 4;
                case 5:  return 5;
                case 6:  return 6;
                case 8:  return 8;
                default: return 1;
            }
        }

        public static void AddSpriteHeaderInfo(string spriteName, byte verticalOffset, byte unknown0x08, byte collisionSize, float scale) {
            if (!s_uniqueSpriteInfosByName.ContainsKey(spriteName))
                s_uniqueSpriteInfosByName.Add(spriteName, new UniqueSpriteInfoDef());
            s_uniqueSpriteInfosByName[spriteName].AddInfo(verticalOffset, unknown0x08, collisionSize, scale);
        }

        public static Dictionary<string, UniqueSpriteInfoDef> GetUniqueSpriteInfos() => s_uniqueSpriteInfosByName;
    }
}
