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
            _frames = AnimationFrames
                .Where(x => x.FrameID < 0xF0)
                .SelectMany(x => {
                    var frames = new List<int>();
                    var frameID = x.FrameID;
                    for (int i = 0; i < directions; i++)
                        if (frameID + i < _frameTable.Length)
                            frames.Add(frameID + i);
                    return frames;
                })
                .Select(x => _frameTable[x])
                .ToArray();
        }

        public AnimationFrameTable AnimationFrames { get; }
        private readonly AnimationFrame _firstAnimationFrame;
        private readonly FrameTable _frameTable;
        private readonly Frame[] _frames;

        [TableViewModelColumn(displayOrder: 0)]
        public AnimationType AnimationType => AnimationFrames.AnimationType;

        [TableViewModelColumn(displayOrder: 0.5f, minWidth: 200)]
        public string SpriteName {
            get {
                return string.Join(", ", _frames.Select(x => $"{x.FrameInfo.SpriteName}").Distinct().OrderBy(x => x));
            }
            set {
                foreach (var frame in _frames)
                    frame.FrameInfo.SpriteName = value;

                var resourcePath = Path.Combine("..", "..", "..", "..", "SF3Lib", CommonLib.Utils.ResourceUtils.ResourceFile("SpriteFramesByHash.xml"));
                using (var file = File.OpenWrite(resourcePath))
                    using (var writer = new StreamWriter(file))
                        CHRUtils.WriteSpriteFramesByHashXML(writer);
            }
        }

/*
        [TableViewModelColumn(displayOrder: 1, minWidth: 300)]
        public string AnimationName {
            get {
                return string.Join(", ", _frames.Select(x => $"[{x.FrameInfo.AnimationName}]").Distinct().OrderBy(x => x));
            }
            set {
                foreach (var frame in _frames) {
                    if (frame.FrameInfo.AnimationName == "")
                        frame.FrameInfo.AnimationName = value;
                    else {
                        var tokens = frame.FrameInfo.AnimationName.Split('|').ToList();
                        if (!tokens.Contains(value)) {
                            tokens.Add(value);
                            frame.FrameInfo.AnimationName = string.Join("|", tokens.OrderBy(x => x));
                        }
                    }
                }

                var resourcePath = Path.Combine("..", "..", "..", "..", "SF3Lib", CommonLib.Utils.ResourceUtils.ResourceFile("SpriteFramesByHash.xml"));
                using (var file = File.OpenWrite(resourcePath))
                    using (var writer = new StreamWriter(file))
                        SpriteFrameTextueUtils.WriteSpriteFramesByHashXML(writer);
            }
        }
*/

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
    }
}
