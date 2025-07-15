using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables.CHR;
using SF3.Sprites;
using SF3.Types;
using SF3.Utils;

namespace SF3.Models.Structs.CHR {
    public class Animation : Struct {
        public Animation(IByteData data, int id, string name, int address, int animationIndex, AnimationFrameTable animationFrames, FrameTable frameTable) : base(data, id, name, address, 0 /* abstract */) {
            AnimationIndex = animationIndex;
            AnimationFrames = animationFrames;
            FrameTable = frameTable;

            if (animationIndex != id)
                ;

            _firstFrameWithTexture = animationFrames?.FirstOrDefault(x => x.HasTexture);

            _framesWithTextures = AnimationFrames
                .Where(x => x.HasTexture)
                .SelectMany(x => {
                    var frames = new List<int>();
                    var frameID = x.FrameID;
                    for (int i = 0; i < CHR_Utils.DirectionsToFrameCount(x.Directions); i++)
                        if (frameID + i < FrameTable.Length)
                            frames.Add(frameID + i);
                    return frames;
                })
                .Select(x => FrameTable[x])
                .ToArray();

            AnimationInfo = CHR_Utils.GetUniqueAnimationInfoByHash(Hash);

            AnimationInfo.SpriteName = SpriteName;
            AnimationInfo.Width      = _framesWithTextures.Length > 0 ? _framesWithTextures[0].Width  : 0;
            AnimationInfo.Height     = _framesWithTextures.Length > 0 ? _framesWithTextures[0].Height : 0;
            AnimationInfo.Directions = (_firstFrameWithTexture == null) ? 1 : _firstFrameWithTexture.Directions;
            AnimationInfo.FrameCommandCount = FrameCommandCount;
            AnimationInfo.Duration   = Duration;
            AnimationInfo.FrameTexturesMissing = FrameTexturesMissing;

            int texCount = 0;
            var aniNameLower = AnimationName.ToLower();
            var frameIdsModified = new HashSet<int>();

            string[] GetAnimationFrameHashes(AnimationFrame aniFrame) {
                if (!aniFrame.HasTexture)
                    return null;

                var frameID = aniFrame.FrameID;
                var dirs = CHR_Utils.DirectionsToFrameCount(aniFrame.Directions);
                var hashes = new string[dirs];
                var maxFrames = FrameTable.Length;

                var shouldBeDirs = dirs;
                if (aniNameLower == "should be 4 directions")
                    shouldBeDirs = 4;
                else if (aniNameLower == "should be 1 direction")
                    shouldBeDirs = 1;

                for (int i = 0; i < dirs; i++) {
                    var num = aniFrame.FrameID + i;
                    if (num < maxFrames)
                        hashes[i] = FrameTable[num].TextureHash;
                }
                return hashes;
            }

            AnimationInfo.AnimationFrames = AnimationFrames
                .Select(x => {
                    if (x.HasTexture)
                        texCount++;
                    return (x.FrameID < 0xF1 || x.FrameID == 0xFC)
                        ? new AnimationFrameDef(GetAnimationFrameHashes(x), x.Duration)
                        : new AnimationFrameDef((SpriteAnimationFrameCommandType) x.FrameID, x.Duration);
                }).ToArray();

            var uniqueFramesWithTextures = _framesWithTextures.Distinct().ToArray();
            TotalCompressedFramesSize = (uint) uniqueFramesWithTextures.Sum(x => x.TextureCompressedSize);
        }

        public AnimationFrameTable AnimationFrames { get; }
        public FrameTable FrameTable { get; }

        private readonly AnimationFrame _firstFrameWithTexture;
        private readonly Frame[] _framesWithTextures;

        [TableViewModelColumn(displayOrder: 0, displayName: "Index")]
        public int AnimationIndex { get; }

        [TableViewModelColumn(displayOrder: 0.1f, displayName: "Type", minWidth: 100)]
        public AnimationType AnimationType => (AnimationType) AnimationIndex;

        [TableViewModelColumn(displayOrder: 0.5f, minWidth: 200)]
        public string SpriteName {
            get {
                if (_framesWithTextures.Length == 0)
                    return "None";

                var distinctNames = _framesWithTextures. Select(x => $"{x.FrameInfo.SpriteName}").Distinct().OrderBy(x => x).ToArray();
                if (distinctNames.Length == 1)
                    return distinctNames[0];

                // Murasame has some animations where his weapon is there, then suddenly not there.
                // They're bugs, so place them in a special "(Weapon Disappears)" sprite category.
                if (distinctNames.Length == 2 && distinctNames[0] == "Murasame (P1)" && distinctNames[1] == "Murasame (P1) (Weaponless)")
                    return "Murasame (P1) (Weapon Disappears)";

                // Edmund's P1 sprites are special because some frames are shared with and without a weapon.
                // (His cape is so big, the rendered frames are the same when his back is turned)
                // Make some very specific corrections to include the duplicated frames in both sprite
                // definitions.
                if (distinctNames.Length == 2 && distinctNames[0] == "Edmund (P1)" && distinctNames[1] == "Edmund (P1) (Sword/Weaponless)")
                    return "Edmund (P1)";
                if (distinctNames.Length == 2 && distinctNames[0] == "Edmund (P1) (Sword/Weaponless)" && distinctNames[1] == "Edmund (P1) (Weaponless)")
                    return "Edmund (P1) (Weaponless)";

                // Explosions have transparent frames sometimes.
                if (distinctNames.Length == 2 && distinctNames[0] == "Explosion" && distinctNames[1] == "Transparency")
                    return "Explosion";

                return string.Join(" | ", distinctNames);
            }
            set {
                var lastSpriteName = SpriteName;
                foreach (var frame in _framesWithTextures)
                    frame.FrameInfo.SpriteName = value;

                var resourcePath = Path.Combine("..", "..", "..", "..", "SF3Lib", CommonLib.Utils.ResourceUtils.ResourceFile("SpriteFramesByHash.xml"));
                using (var file = File.Open(resourcePath, FileMode.Create))
                    using (var writer = new StreamWriter(file))
                        CHR_Utils.WriteUniqueFramesByHashXML(writer);

                var newSpriteName = SpriteName;
                if (lastSpriteName != newSpriteName) {
                    AnimationInfo.SpriteName = SpriteName;
                    var animPath = Path.Combine("..", "..", "..", "..", "SF3Lib", CommonLib.Utils.ResourceUtils.ResourceFile("SpriteAnimationsByHash.xml"));
                    using (var file = File.Open(animPath, FileMode.Create))
                        using (var writer = new StreamWriter(file))
                            CHR_Utils.WriteUniqueAnimationsByHashXML(writer);
                }
            }
        }

        [TableViewModelColumn(displayOrder: 1, minWidth: 300)]
        public string AnimationName {
            get => AnimationInfo.AnimationName;
            set {
                AnimationInfo.AnimationName = value;
                var resourcePath = Path.Combine("..", "..", "..", "..", "SF3Lib", CommonLib.Utils.ResourceUtils.ResourceFile("SpriteAnimationsByHash.xml"));
                using (var file = File.Open(resourcePath, FileMode.Create))
                    using (var writer = new StreamWriter(file))
                        CHR_Utils.WriteUniqueAnimationsByHashXML(writer);
            }
        }

        [TableViewModelColumn(displayOrder: 2.25f, displayName: "Frames+Commands")]
        public int FrameCommandCount => AnimationFrames.Count();

        [TableViewModelColumn(displayOrder: 2.3f, displayName: "Image Frames")]
        public int ImageFrameCount => AnimationFrames.Count(x => x.HasTexture);

        [TableViewModelColumn(displayOrder: 2.4f)]
        public string FinalFrame {
            get {
                var lastFrame = AnimationFrames.LastOrDefault();
                if (lastFrame == null)
                    return null;
                return $"{lastFrame.FrameID:X2},{lastFrame.Duration:X2}";
            }
        }

        [TableViewModelColumn(displayOrder: 2.5f)]
        public int Duration => AnimationFrames.Sum(x => x.HasTexture ? x.Duration : 0);

        [TableViewModelColumn(displayOrder: 2)]
        public int FrameTexturesMissing => AnimationFrames.Sum(x => x.FramesMissing);

        private string _hash = null;
        [TableViewModelColumn(displayOrder: 3, minWidth: 300)]
        public string Hash {
            get {
                if (_hash == null) {
                    // Build a unique hash string for this animation.
                    var hashStr = "";
                    foreach (var aniFrame in AnimationFrames) {
                        if (hashStr != "")
                            hashStr += "_";

                        if (!aniFrame.HasTexture) {
                            var cmd   = aniFrame.FrameID;
                            var param = aniFrame.Duration;

                            // Don't bother appending stops.
                            if (cmd == 0xF2)
                                hashStr += "f2";
                            else
                                hashStr += $"{cmd:x2},{aniFrame.Duration:x2}";
                        }
                        else {
                            var tex = aniFrame.HasTexture ? aniFrame.GetTexture(aniFrame.Directions) : null;
                            hashStr += (tex != null) ? $"{tex.Hash}_{aniFrame.Duration:x2}" : $"{aniFrame.FrameID:x2},{aniFrame.Duration:x2}";
                        }
                    }

                    using (var md5 = MD5.Create())
                        _hash = BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(hashStr))).Replace("-", "").ToLower();
                }
                return _hash;
            }
        }

        public UniqueAnimationDef AnimationInfo { get; }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X4")]
        public uint TotalCompressedFramesSize { get; }
    }
}
