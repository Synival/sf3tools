using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables.CHR;
using SF3.Sprites;
using SF3.Types;
using SF3.Utils;

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
                    for (int i = 0; i < CHR_Utils.DirectionsToFrameCount(x.Directions); i++)
                        if (frameID + i < FrameTable.Length)
                            frames.Add(frameID + i);
                    return frames;
                })
                .Select(x => FrameTable[x])
                .ToArray();

            AnimationInfo = CHR_Utils.GetUniqueAnimationInfoByHash(Hash);

            // Update animation info, in case it's out of date.
            var firstCommandWithTexture = animationCommands?.FirstOrDefault(x => x.IsFrameCommand);
            var firstFrameWithTexture = _framesWithTextures.FirstOrDefault();
            AnimationInfo.SpriteName = SpriteName;
            AnimationInfo.Width  = firstFrameWithTexture?.Width ?? 0;
            AnimationInfo.Height = firstFrameWithTexture?.Height ?? 0;
            AnimationInfo.Directions = firstCommandWithTexture?.Directions ?? 0;
            AnimationInfo.FrameCommandCount = AnimationCommandTable.Length;
            AnimationInfo.Duration = Duration;
            AnimationInfo.FrameTexturesMissing = FrameTexturesMissing;

            int texCount = 0;
            var aniNameLower = AnimationName.ToLower();
            var frameIdsModified = new HashSet<int>();

            string[] GetCommandFrameHashes(AnimationCommand aniCommand) {
                if (!aniCommand.IsFrameCommand)
                    return null;

                var frameID = aniCommand.Command;
                var dirs = CHR_Utils.DirectionsToFrameCount(aniCommand.Directions);
                var hashes = new string[dirs];
                var maxFrames = FrameTable.Length;

                for (int i = 0; i < dirs; i++) {
                    var num = aniCommand.Command + i;
                    if (num < maxFrames)
                        hashes[i] = FrameTable[num].TextureHash;
                }
                return hashes;
            }

            AnimationInfo.AnimationCommands = AnimationCommandTable
                .Select(x => {
                    var isFrameCommand = x.IsFrameCommand;
                    if (isFrameCommand)
                        texCount++;
                    return (isFrameCommand)
                        ? new Sprites.AnimationCommand(GetCommandFrameHashes(x), x.Parameter)
                        : new Sprites.AnimationCommand((SpriteAnimationCommandType) x.Command, x.Parameter);
                }).ToArray();

            var uniqueFramesWithTextures = _framesWithTextures.Distinct().ToArray();
            TotalCompressedFramesSize = (uint) uniqueFramesWithTextures.Sum(x => x.TextureCompressedSize);
        }

        public int SpriteDirections { get; }
        public AnimationCommandTable AnimationCommandTable { get; }
        public FrameTable FrameTable { get; }

        private readonly Frame[] _framesWithTextures;

        [TableViewModelColumn(displayOrder: 0)]
        public int Directions => AnimationInfo.Directions;

        [TableViewModelColumn(displayOrder: 0.05f, displayName: "Index")]
        public int AnimationIndex { get; }

        [TableViewModelColumn(displayOrder: 0.1f, displayName: "Type", minWidth: 100)]
        public AnimationType AnimationType => (AnimationType) AnimationIndex;

        [TableViewModelColumn(displayOrder: 0.5f, minWidth: 200)]
        public string SpriteName {
            get {
                if (_framesWithTextures.Length == 0)
                    return "None";

                var distinctNames = _framesWithTextures.Select(x => $"{x.FrameInfo.SpriteName}").Distinct().OrderBy(x => x).ToArray();
                if (distinctNames.Length == 1)
                    return distinctNames[0];

                // If the frames in this animation are shared by more than one named sprite, give them a proper name for known cases.

                // Murasame has some animations where his weapon is there, then suddenly not there.
                // They have to go somewhere, so place them in Murasame (P1).
                if (distinctNames.Length == 2 && distinctNames[0] == "Murasame (P1)" && distinctNames[1] == "Murasame (P1) (Weaponless)")
                    return "Murasame (P1)";

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
                    if (frame.FrameInfo != null && frame.FrameInfo.SpriteName != value)
                        frame.FrameInfo.SpriteName = value;

                var newSpriteName = SpriteName;
                if (AnimationInfo != null && lastSpriteName != newSpriteName)
                    AnimationInfo.SpriteName = SpriteName;

                // TODO: update appropriate SpriteDefs:
                // 1) old sprite def (if it exists)
                // 2) new sprite def (create if doesn't exist?)
            }
        }

        [TableViewModelColumn(displayOrder: 1, minWidth: 300)]
        public string AnimationName {
            get => AnimationInfo.AnimationName;
            set {
                if (AnimationInfo != null && AnimationInfo.AnimationName != value) {
                    AnimationInfo.AnimationName = value;

                    // TODO: update appropriate SpriteDefs:
                    // 1) old sprite def (if it exists)
                    // 2) new sprite def (create if doesn't exist?)
                }
            }
        }

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
                    _hash = CHR_Utils.CreateAnimationHash(AnimationCommandTable.ToArray());
                return _hash;
            }
        }

        public UniqueAnimationDef AnimationInfo { get; }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X4")]
        public uint TotalCompressedFramesSize { get; }
    }
}
