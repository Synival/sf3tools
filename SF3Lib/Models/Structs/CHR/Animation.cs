using System.IO;
using System.Linq;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables.CHR;
using SF3.Types;
using SF3.Utils;

namespace SF3.Models.Structs.CHR {
    public class Animation : Struct {
        public Animation(IByteData data, int id, string name, int address, AnimationFrameTable animationFrames) : base(data, id, name, address, 0 /* abstract */) {
            AnimationFrames = animationFrames;

            _firstAnimationFrame = animationFrames?.FirstOrDefault(x => x.FrameID < 0xF0);
            _frameTable = _firstAnimationFrame.FrameTable;
            _frames = AnimationFrames
                .Where(x => x.FrameID < 0xF0 && _frameTable.Length > x.FrameID)
                .Select(x => _frameTable[x.FrameID])
                .ToArray();
        }

        public AnimationFrameTable AnimationFrames { get; }
        private readonly AnimationFrame _firstAnimationFrame;
        private readonly FrameTable _frameTable;
        private readonly Frame[] _frames;

        [TableViewModelColumn(displayOrder: 0)]
        public AnimationType AnimationType => AnimationFrames.AnimationType;

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
    }
}
