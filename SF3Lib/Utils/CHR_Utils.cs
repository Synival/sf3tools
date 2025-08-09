using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using SF3.Sprites;
using SF3.Types;

namespace SF3.Utils {
    public static class CHR_Utils {
        private static Dictionary<string, UniqueFrameDef> s_uniqueFramesByHash = null;
        private static Dictionary<string, UniqueAnimationDef> s_uniqueAnimationsByHash = null;
        private static Dictionary<string, UniqueSpriteInfoDef> s_uniqueSpriteInfosByName = new Dictionary<string, UniqueSpriteInfoDef>();

        public static UniqueAnimationDef GetUniqueAnimationInfoByHash(string hash) {
            LoadUniqueAnimationsByHashTable();
            if (!s_uniqueAnimationsByHash.ContainsKey(hash.ToLower())) {
                s_uniqueAnimationsByHash[hash] = new UniqueAnimationDef(hash, "Unknown", "Unknown", 0, 0, 0, 0, 0, 0,
                    new AnimationCommand[0]);
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

                        int width, height, frames, directionsInt;
                        if (!int.TryParse(widthAttr, out width) || !int.TryParse(heightAttr, out height) || !int.TryParse(directionsAttr, out directionsInt) || !int.TryParse(framesAttr, out frames))
                            continue;
                        var directions = (SpriteDirectionCountType) directionsInt;

                        if (sprite == "")
                            sprite = "None";

                        int missingFrames = int.TryParse(missingAttr, out var missingFramesOut) ? missingFramesOut : 0;
                        int duration = int.TryParse(durationAttr, out var durationOut) ? durationOut : 0;
                        s_uniqueAnimationsByHash.Add(hash.ToLower(), new UniqueAnimationDef(hash, sprite, animation, width, height, directions, frames, duration, missingFrames,
                            new AnimationCommand[0]));
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
                stream.WriteLine($"    <item hash=\"{ai.AnimationHash}\" sprite=\"{spriteName}\" animation=\"{ai.AnimationName}\" width=\"{ai.Width}\" height=\"{ai.Height}\" directions=\"{(int) ai.Directions}\" frames=\"{ai.FrameCommandCount}\" duration=\"{ai.Duration}\"{missingFramesStr} />");
            }
            stream.WriteLine("</items>");
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

        public static int DirectionsToFrameCount(SpriteDirectionCountType directions) {
            switch (directions) {
                case SpriteDirectionCountType.TwoNoFlip: return 2;
                case SpriteDirectionCountType.Four:      return 4;
                case SpriteDirectionCountType.Five:      return 5;
                case SpriteDirectionCountType.Six:       return 6;
                case SpriteDirectionCountType.Eight:     return 8;
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

        private class AnimationHashCommand {
            public SpriteAnimationCommandType Command;
            public int Parameter;
            public int FrameID;
            public SpriteDirectionCountType Directions;
            public ITexture Image;
            public int FramesMissing;
        }

        public static string CreateAnimationHash(SpriteDirectionCountType directions, Animation animation, Dictionary<string, FrameGroup> frameGroups, Dictionary<string, ITexture> texturesByHash) {
            SpriteDirectionCountType currentDirections = directions;

            var hashInfos = animation.AnimationCommands
                .Select(x => {
                    var frameCount = DirectionsToFrameCount(currentDirections);
                    var frames = (!x.HasFrame)
                        ? null
                        : (x.FrameGroup != null)
                        ? Enumerable.Range(0, frameCount)
                            .Select(y => FrameNumberToSpriteDir(frameCount, y))
                            .Select(y => frameGroups[x.FrameGroup].Frames.TryGetValue(y, out var frame) ? frame : null)
                            .ToArray()
                        : (x.FramesByDirection != null)
                        ? Enumerable.Range(0, frameCount)
                            .Select(y => FrameNumberToSpriteDir(frameCount, y))
                            .Select(y => x.FramesByDirection.TryGetValue(y, out var frame) ? frameGroups[frame.FrameGroup].Frames[frame.Direction] : null)
                            .ToArray()
                        : null;

                    if (x.Command == SpriteAnimationCommandType.SetDirectionCount)
                        currentDirections = (SpriteDirectionCountType) x.Parameter;

                    return new AnimationHashCommand() {
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

        private static ITexture StackedImageFromFrames(Frame[] frames, Dictionary<string, ITexture> texturesByHash) {
            if (frames == null)
                return null;

            var textures = frames
                .Where(x => x != null && texturesByHash.ContainsKey(x.Hash))
                .Select(x => texturesByHash[x.Hash])
                .ToArray();

            return TextureUtils.StackTextures(0, 0, 0, textures);
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

        public static SpriteFrameDirection[] GetCHR_FrameGroupDirections(SpriteDirectionCountType directions)
            => GetCHR_FrameGroupDirections(DirectionsToFrameCount(directions));

        public static SpriteFrameDirection[] GetCHR_FrameGroupDirections(int directions) {
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
    }
}
