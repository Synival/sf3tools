using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables.CHR;
using SF3.Sprites;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class Animation : Struct {
        public Animation(IByteData data, int id, string name, int address, int animationIndex, int spriteDirections, AnimationCommandTable animationCommands, FrameTable frameTable)
        : base(data, id, name, address, 0 /* abstract */) {
            AnimationIndex        = animationIndex;
            SpriteDirections      = spriteDirections;
            AnimationCommandTable = animationCommands;
            FrameTable            = frameTable;

            _framesWithTextures = AnimationCommandTable
                .Where(x => x.IsFrameCommand)
                .SelectMany(x => {
                    var frames = new List<int>();
                    var frameID = x.Command;
                    for (int i = 0; i < x.Directions.GetAnimationFrameCount(); i++)
                        if (frameID + i < FrameTable.Length)
                            frames.Add(frameID + i);
                    return frames;
                })
                .Select(x => FrameTable[x])
                .ToArray();

            AnimationRef = SpriteResources.GetUniqueAnimationInfo(SpriteName, Hash);

            var uniqueFramesWithTextures = _framesWithTextures.Distinct().ToArray();
            TotalCompressedFramesSize = (uint) uniqueFramesWithTextures.Sum(x => x.TextureCompressedSize);
        }

        public int SpriteDirections { get; }
        public AnimationCommandTable AnimationCommandTable { get; }
        public FrameTable FrameTable { get; }

        private readonly Frame[] _framesWithTextures;

        [TableViewModelColumn(displayOrder: 0)]
        public SpriteDirectionCountType Directions => AnimationRef.Directions;

        [TableViewModelColumn(displayOrder: 0.05f, displayName: "Index")]
        public int AnimationIndex { get; }

        [TableViewModelColumn(displayOrder: 0.1f, displayName: "Type", minWidth: 100)]
        public AnimationType AnimationType => (AnimationType) AnimationIndex;

        [TableViewModelColumn(displayOrder: 0.5f, minWidth: 200)]
        public string SpriteName {
            get {
                if (_framesWithTextures.Length == 0)
                    return "None";

                // Return the most frequent frame name to resolve animations with frames that appear in multiple spritesheets.
                // This isn't a guarantee that it will work, but solves all vanilla game cases and is very unlikely to ever choose the wrong sprite.
                return _framesWithTextures
                    .SelectMany(x => x.SpriteName.Split('|').Select(y => y.Trim()))
                    .GroupBy(x => x)
                    .OrderByDescending(x => x.Count())
                    .First().Key;
            }
        }

        [TableViewModelColumn(displayOrder: 1, minWidth: 300)]
        public string AnimationName => AnimationRef.AnimationName;

        [TableViewModelColumn(displayOrder: 2.25f)]
        public int CommandCount => AnimationCommandTable.Count();

        [TableViewModelColumn(displayOrder: 2.3f)]
        public int FrameCommandCount => AnimationCommandTable.Count(x => x.IsFrameCommand);

        [TableViewModelColumn(displayOrder: 2.4f)]
        public string FinalCommand {
            get {
                var lastCommand = AnimationCommandTable.LastOrDefault();
                if (lastCommand == null)
                    return null;
                return $"{lastCommand.Command:X2},{lastCommand.Parameter:X2}";
            }
        }

        [TableViewModelColumn(displayOrder: 2.5f)]
        public int Duration => AnimationCommandTable.Sum(x => x.IsFrameCommand ? x.Parameter : 0);

        [TableViewModelColumn(displayOrder: 2)]
        public int FrameTexturesMissing => AnimationCommandTable.Sum(x => x.FramesMissing);

        private string _hash = null;
        [TableViewModelColumn(displayOrder: 3, minWidth: 300)]
        public string Hash {
            get {
                if (_hash == null)
                    _hash = SpriteResources.CreateAnimationHash(AnimationCommandTable.ToArray());
                return _hash;
            }
        }

        public AnimationRef AnimationRef { get; }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X4")]
        public uint TotalCompressedFramesSize { get; }
    }
}
