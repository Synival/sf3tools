using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Tables.CHR;
using SF3.Types;
using SF3.Utils;

namespace SF3.Models.Structs.CHR {
    public class AnimationCommand : Struct {
        private readonly int _frameIdAddr;
        private readonly int _durationAddr;

        public AnimationCommand(IByteData data, int id, string name, int address, int spriteIndex, int spriteId, SpriteDirectionCountType directions, int animationIndex, FrameTable frameTable)
        : base(data, id, name, address, 0x4) {
            SpriteIndex    = spriteIndex;
            SpriteID       = spriteId;
            AnimationIndex = animationIndex;
            FrameTable     = frameTable;

            _frameIdAddr  = Address + 0x00; // 2 bytes
            _durationAddr = Address + 0x02; // 2 bytes

            // Number of directions changes with the 0xF1 command.
            Directions = (CommandType == SpriteAnimationCommandType.SetDirectionCount) ? (SpriteDirectionCountType) Parameter : directions;
        }

        [TableViewModelColumn(addressField: null, displayOrder: -0.4f, displayFormat: "X2")]
        public int SpriteIndex { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.3f, displayFormat: "X2", minWidth: 200)]
        [NameGetter(NamedValueType.Sprite)]
        public int SpriteID { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.2f, displayName: "Index")]
        public int AnimationIndex { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.1f, displayName: "Type", minWidth: 100)]
        public AnimationType AnimationType => (AnimationType) AnimationIndex;

        public FrameTable FrameTable { get; }

        [TableViewModelColumn(addressField: nameof(_frameIdAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public ushort Command {
            get => Data.GetUInt16(_frameIdAddr);
            set => Data.SetUInt16(_frameIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 0.5f, minWidth: 150)]
        public SpriteAnimationCommandType CommandType
            => IsFrameCommand ? SpriteAnimationCommandType.Frame : (SpriteAnimationCommandType) Command;

        [TableViewModelColumn(addressField: nameof(_durationAddr), displayOrder: 1, displayFormat: "X2", displayName: "Parameter/Duration")]
        [BulkCopy]
        public ushort Parameter {
            get => Data.GetUInt16(_durationAddr);
            set => Data.SetUInt16(_durationAddr, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 2)]
        public SpriteDirectionCountType Directions { get; }

        [TableViewModelColumn(addressField: null, displayOrder: 2.5f)]
        public int FramesMissing => IsFrameCommand ? (Directions.GetAnimationFrameCount() - GetFrameCount(Directions)) : 0;

        [TableViewModelColumn(displayOrder: 3)]
        public bool IsEndingCommand {
            get {
                var cmdType = CommandType;
                return
                    (cmdType == SpriteAnimationCommandType.Stop) ||
                    (cmdType == SpriteAnimationCommandType.GotoCommandOffset && Parameter < (ID * 2 + 2)) ||
                    (cmdType == SpriteAnimationCommandType.GotoAnimation);
            }
        }

        [TableViewModelColumn(displayOrder: 4)]
        public bool IsFrameCommand {
            get {
                var cmd = Command;
                // (NOTE: Command 0xFC is a special command, but it's broken and sets the frame to 0xFC with a duration. Stupid, huh?)
                return cmd < 0xF1 || cmd == 0xFC;
            }
        }

        public int GetFrameCount(SpriteDirectionCountType directions) {
            if (FrameTable == null || !IsFrameCommand)
                return 0;

            int expectedFrameCount = directions.GetAnimationFrameCount();
            return Math.Max(0, Math.Min(FrameTable.Count - Command, expectedFrameCount));
        }

        private readonly Dictionary<int, string> _textureHashByFrameCount = new Dictionary<int, string>();
        private readonly Dictionary<int, ITextureData> _texturesByFrameCount = new Dictionary<int, ITextureData>();

        public ITextureData GetTexture(SpriteDirectionCountType directions) {
            if (FrameTable == null || !IsFrameCommand)
                return null;

            int frameCount = directions.GetAnimationFrameCount();
            if (_texturesByFrameCount.TryGetValue(frameCount, out var tex))
                return tex;

            var frameMin = Command;
            var frameMax = frameMin + frameCount;

            var frames = FrameTable
                .Where(x => x.ID >= frameMin && x.ID < frameMax)
                .Select(x => x.Texture)
                .ToArray();
            tex = TextureUtils.StackTextures(frames, ImageDataCanSet.Never);
            _texturesByFrameCount[frameCount] = tex;

            var frameTableCount = FrameTable.Count;
            var frameHash = Enumerable.Range(frameMin, frameCount)
                .Select(x => (x < frameTableCount) ? FrameTable[x] : null)
                .Select(x => (x?.Texture != null) ? $"({x.Texture.Hash})" : "()")
                .Aggregate((a, b) => a + b);

            _textureHashByFrameCount.Add(frameCount, Encoding.ASCII.GetBytes(frameHash).CreateTextureHash());

            return tex;
        }

        public string GetTextureHash(SpriteDirectionCountType directions) {
            // Has the side-effect of assigning _textureHashByFrameCount.
            _ = GetTexture(directions);
            return IsFrameCommand ? _textureHashByFrameCount[directions.GetAnimationFrameCount()] : null;
        }

        [TableViewModelColumn(displayOrder: 5, minWidth: 200)]
        public string TextureHash => GetTextureHash(Directions);
    }
}
