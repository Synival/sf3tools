using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using SF3.Models.Structs.CHR;
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

            var frame = s_uniqueFramesByHash[hash];
            frame.RefCount++;
            return frame;
        }

        public static UniqueAnimationDef GetUniqueAnimationInfoByHash(string hash) {
            LoadUniqueAnimationsByHashTable();
            if (!s_uniqueAnimationsByHash.ContainsKey(hash.ToLower())) {
                s_uniqueAnimationsByHash[hash] = new UniqueAnimationDef(hash, "Unknown", "Unknown", 0, 0, 0, 0, 0, 0,
                    new AnimationFrameDef[0]);
            }

            var animation = s_uniqueAnimationsByHash[hash];
            animation.RefCount++;
            return animation;
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

        public static void WriteUniqueFramesByHashXML(StreamWriter stream, bool onlyReferenced) {
            var frameInfos = s_uniqueFramesByHash.Values
                .Where(x => !onlyReferenced || x.RefCount > 0)
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

        public static void WriteUniqueAnimationsByHashXML(StreamWriter stream, bool onlyReferenced) {
            var animationInfos = s_uniqueAnimationsByHash.Values
                .Where(x => !onlyReferenced || x.RefCount > 0)
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
                if (spriteName == "Explosion") {
                    return new string[] {
                        "d33e04f92840fa8d80d642441797bafe",
                        "b1e27aa018409de6bfd73f8afb883a65",
                    };
                }
                // Murasame (P1) has some head nodding/shaking animations that mistakenly use Mursame (P1) (Weaponless) idle frames.
                // (He doesn't have a nodding/shaking animation with his weapon)
                else if (spriteName == "Murasame (P1)") {
                    return new string[] {
                        "6435253706983a8ba65853305367d6a5",
                        "c8f2ea332f813be6d36667520efab236",
                        "da43f81c86b23710d965d38646220bcb",
                        "83f8871fbf7eff0fdf90592f288ff2dd",
                        "1775c3e62b666260aa56c8b7dbc28657",
                        "1a699b3d2010a2431bda1da6702f0a2a",
                        "ef24efab5459d4e6f28c74e9c3d572f9",
                        "9f5d571cf3863086fc0764b0a2a50a9c",
                        "3d607c240092ee65fa5477d374aa81fe",
                        "b23aa2dd6ce9034733e0191253f6420a",
                        "e884430806eb8f0674ac914c5c1a869d",
                        "ea899f09df61d9d5740eec177e2e006c",
                        "6738c467a0197cf04f04d8c2269d4a71",
                    };
                }
                else
                    return new string[0];
            }

            string[] ExtraAnimations(string spriteName) {
                // Explosions need some StillFrame animations from Transparency.
                if (spriteName == "Explosion") {
                    return new string[] {
                        "23708b71160689a09af4ca935a2ea04c",
                        "08f1faa76b0f430a925ed3c83c646dd5",
                    };
                }
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
                        .Concat(ExtraAnimations(x.Key).Select(y => s_uniqueAnimationsByHash[y]))
                        .ToArray());
                })
                .Where(x => x.Spritesheets.Count > 0)
                .ToArray();
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

        public static void AddSpriteHeaderInfo(string spriteName, int spriteId, byte verticalOffset, byte unknown0x08, byte collisionSize, float scale) {
            if (!s_uniqueSpriteInfosByName.ContainsKey(spriteName))
                s_uniqueSpriteInfosByName.Add(spriteName, new UniqueSpriteInfoDef());

            var isCharacterSprite = spriteId < 60 && (spriteName.Contains("(U)") || spriteName.Contains("(P1)") || spriteName.Contains("(P2)"));
            s_uniqueSpriteInfosByName[spriteName].AddInfo(spriteId, verticalOffset, unknown0x08, collisionSize, scale, isCharacterSprite);
        }

        public static Dictionary<string, UniqueSpriteInfoDef> GetUniqueSpriteInfos() => s_uniqueSpriteInfosByName;

        private class AnimationHashFrame {
            public SpriteAnimationFrameCommandType Command;
            public int Parameter;
            public int FrameID;
            public int Directions;
            public ITexture Image;
            public int FramesMissing;
        }

        public static string CreateAnimationHash(int directions, AnimationDef animation, Dictionary<string, FrameGroupDef> frameGroups, Dictionary<string, ITexture> texturesByHash) {
            int currentDirections = directions;

            var hashInfos = animation.AnimationFrames
                .Select(x => {
                    var frameCount = DirectionsToFrameCount(currentDirections);
                    var frames = (!x.HasFrame)
                        ? null
                        : (x.FrameGroup != null)
                        ? Enumerable.Range(0, frameCount)
                            .Select(y => FrameNumberToSpriteDir(frameCount, y))
                            .Select(y => frameGroups[x.FrameGroup].Frames.TryGetValue(y, out var frame) ? frame : null)
                            .ToArray()
                        : (x.Frames != null)
                        ? Enumerable.Range(0, frameCount)
                            .Select(y => FrameNumberToSpriteDir(frameCount, y).ToString())
                            .Select(y => x.Frames.TryGetValue(y, out var frame) ? frameGroups[frame.Frame].Frames[frame.Direction] : null)
                            .ToArray()
                        : null;

                    if (x.Command == SpriteAnimationFrameCommandType.SetDirectionCount)
                        currentDirections = x.Parameter;

                    return new AnimationHashFrame() {
                        Command    = x.Command,
                        Parameter  = x.Parameter,
                        FrameID    = -1,
                        Directions = currentDirections,
                        Image      = StackedImageFromFrames(frames, texturesByHash),
                        FramesMissing = frames?.Count(y => y == null) ?? 0
                    };
                })
                .ToArray();

            return CreateAnimationHash(hashInfos);
        }

        private static ITexture StackedImageFromFrames(FrameDef[] frames, Dictionary<string, ITexture> texturesByHash) {
            if (frames == null)
                return null;

            var textures = frames
                .Where(x => x != null)
                .Select(x => texturesByHash[x.Hash])
                .ToArray();

            return TextureUtils.StackTextures(0, 0, 0, textures);
        }

        public static string CreateAnimationHash(AnimationFrame[] animationFrames) {
            var hashInfos = animationFrames
                .Select(x => new AnimationHashFrame() {
                    Command    = x.HasTexture ? SpriteAnimationFrameCommandType.Frame : (SpriteAnimationFrameCommandType) x.FrameID,
                    Parameter  = x.Duration,
                    FrameID    = x.HasTexture ? x.FrameID : -1,
                    Directions = x.Directions,
                    Image      = (x.HasTexture) ? x.GetTexture(x.Directions) : null,
                    FramesMissing = x.FramesMissing
                })
                .ToArray();

            return CreateAnimationHash(hashInfos);
        }

        private static string CreateAnimationHash(AnimationHashFrame[] animationHashFrames) {
            // Build a unique hash string for this animation.
            var hashStr = "";
            foreach (var aniFrame in animationHashFrames) {
                if (hashStr != "")
                    hashStr += "_";

                if (aniFrame.Command != SpriteAnimationFrameCommandType.Frame) {
                    var cmd   = aniFrame.Command;
                    var param = aniFrame.Parameter;

                    // Don't bother appending stops.
                    if (cmd == SpriteAnimationFrameCommandType.Stop)
                        hashStr += "f2";
                    else
                        hashStr += $"{((int) cmd):x2},{aniFrame.Parameter:x2}";
                }
                else {
                    var tex = aniFrame.Image;
                    hashStr += (tex != null) ? $"{tex.Hash}_{aniFrame.Parameter:x2}" : $"{aniFrame.FrameID:x2},{aniFrame.Parameter:x2}";
                    if (aniFrame.FramesMissing > 0)
                        hashStr += $"_M{aniFrame.FramesMissing})";
                }
            }

            using (var md5 = MD5.Create())
                return BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(hashStr))).Replace("-", "").ToLower();
        }

        public static SpriteFrameDirection[] GetCHR_FrameGroupDirections(int directions) {
            switch (directions) {
                case 1:
                case 0x11:
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
                        SpriteFrameDirection.SSE,
                        SpriteFrameDirection.ESE,
                        SpriteFrameDirection.ENE,
                        SpriteFrameDirection.NNE,
                        SpriteFrameDirection.S,
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

        public static string FilesystemName(string name) {
            return name
                .Replace(" | ", ", ")
                .Replace("|", ",")
                .Replace("?", "X")
                .Replace("-", "_")
                .Replace(":", "_")
                .Replace("/", "_");
        }
    }
}
