using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables.CHR;
using SF3.Types;
using SF3.Utils;

namespace SF3.Models.Structs.CHR {
    public class Animation : Struct {
        public Animation(IByteData data, int id, string name, int address, AnimationFrameTable animationFrames) : base(data, id, name, address, 0 /* abstract */) {
            AnimationFrames = animationFrames;
            var directions = animationFrames.SpriteDirections;

            _firstAnimationFrame = animationFrames?.FirstOrDefault(x => x.FrameID < 0xF0);
            _frameTable = _firstAnimationFrame?.FrameTable;
            _framesWithTextures = AnimationFrames
                .Where(x => x.HasTexture)
                .SelectMany(x => {
                    var frames = new List<int>();
                    var frameID = x.FrameID;
                    for (int i = 0; i < x.Directions; i++)
                        if (frameID + i < _frameTable.Length)
                            frames.Add(frameID + i);
                    return frames;
                })
                .Select(x => _frameTable[x])
                .ToArray();

            var width  = _framesWithTextures.Length > 0 ? _framesWithTextures[0].Width  : 0;
            var height = _framesWithTextures.Length > 0 ? _framesWithTextures[0].Height : 0;

            AnimationInfo = CHRUtils.GetUniqueAnimationInfoByHash(Hash, width, height, _firstAnimationFrame?.Directions ?? 1);
            AnimationInfo.SpriteName = SpriteName;

            var uniqueFramesWithTextures = _framesWithTextures.Distinct().ToArray();
            TotalCompressedFramesSize = (uint) uniqueFramesWithTextures.Sum(x => x.TextureCompressedSize);
        }

        public AnimationFrameTable AnimationFrames { get; }
        private readonly AnimationFrame _firstAnimationFrame;
        private readonly FrameTable _frameTable;
        private readonly Frame[] _framesWithTextures;

        [TableViewModelColumn(displayOrder: 0)]
        public AnimationType AnimationType => AnimationFrames.AnimationType;

        [TableViewModelColumn(displayOrder: 0.5f, minWidth: 200)]
        public string SpriteName {
            get {
                if (_framesWithTextures.Length == 0)
                    return "None";
                else
                    return string.Join(", ", _framesWithTextures.Select(x => $"{x.FrameInfo.SpriteName}").Distinct().OrderBy(x => x));
            }
            set {
                var lastSpriteName = SpriteName;
                foreach (var frame in _framesWithTextures)
                    frame.FrameInfo.SpriteName = value;

                var resourcePath = Path.Combine("..", "..", "..", "..", "SF3Lib", CommonLib.Utils.ResourceUtils.ResourceFile("SpriteFramesByHash.xml"));
                using (var file = File.OpenWrite(resourcePath))
                    using (var writer = new StreamWriter(file))
                        CHRUtils.WriteUniqueFramesByHashXML(writer);

                var newSpriteName = SpriteName;
                if (lastSpriteName != newSpriteName) {
                    AnimationInfo.SpriteName = SpriteName;
                    var animPath = Path.Combine("..", "..", "..", "..", "SF3Lib", CommonLib.Utils.ResourceUtils.ResourceFile("SpriteAnimationsByHash.xml"));
                    using (var file = File.OpenWrite(animPath))
                        using (var writer = new StreamWriter(file))
                            CHRUtils.WriteUniqueAnimationsByHashXML(writer);
                }
            }
        }

        [TableViewModelColumn(displayOrder: 1, minWidth: 300)]
        public string AnimationName {
            get => AnimationInfo.AnimationName;
            set {
                AnimationInfo.AnimationName = value;
                var resourcePath = Path.Combine("..", "..", "..", "..", "SF3Lib", CommonLib.Utils.ResourceUtils.ResourceFile("SpriteAnimationsByHash.xml"));
                using (var file = File.OpenWrite(resourcePath))
                    using (var writer = new StreamWriter(file))
                        CHRUtils.WriteUniqueAnimationsByHashXML(writer);
            }
        }

        [TableViewModelColumn(displayOrder: 2)]
        public int TotalFramesMissing => AnimationFrames.Sum(x => x.FramesMissing);

        private string _hash = null;
        [TableViewModelColumn(displayOrder: 3, minWidth: 300)]
        public string Hash {
            get {
                if (_hash == null) {
                    // Build a unique hash string for this animation.
                    var hashStr = "";
                    foreach (var aniFrame in AnimationFrames) {
                        // If this is the last frame (a command), filter out some commands that could prevent detection of uniqueness.
                        if (aniFrame.IsFinalFrame) {
                            var cmd   = aniFrame.FrameID;
                            var param = aniFrame.Duration;

                            // Don't bother appending stops.
                            if (cmd == 0xF2)
                                break;
                            // If jumping back to the first frame is the same as stopping, don't add this either.
                            else if (cmd == 0xFE && param == 0 && AnimationFrames.Length <= 2)
                                break;
                            // If jumping to another animation, we don't care which one -- just add FF and be done.
                            else if (cmd == 0xFF) {
                                hashStr += "_ff";
                                break;
                            }
                            // Add the frame as normal.
                        }

                        if (hashStr != "")
                            hashStr += "_";

                        var tex = aniFrame.HasTexture ? aniFrame.GetTexture(aniFrame.Directions) : null;
                        hashStr += (tex != null) ? $"{tex.Hash}_{aniFrame.Duration:x2}" : $"{aniFrame.FrameID:x2}{aniFrame.Duration:x2}";
                    }

                    using (var md5 = MD5.Create())
                        _hash = BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(hashStr))).Replace("-", "").ToLower();
                }
                return _hash;
            }
        }

        public UniqueAnimationInfo AnimationInfo { get; }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X4")]
        public uint TotalCompressedFramesSize { get; }
    }
}
