using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables.CHR;
using SF3.Types;
using SF3.Utils;

namespace SF3.Models.Structs.CHR {
    public class AnimationFrame : Struct {
        private readonly int _frameIdAddr;
        private readonly int _durationAddr;

        public AnimationFrame(IByteData data, int id, string name, int address, int spriteIndex, int spriteId, int directions, int animationIndex, FrameTable frameTable)
        : base(data, id, name, address, 0x4) {
            SpriteIndex    = spriteIndex;
            SpriteID       = spriteId;
            AnimationIndex = animationIndex;
            FrameTable     = frameTable;

            _frameIdAddr  = Address + 0x00; // 2 bytes
            _durationAddr = Address + 0x02; // 2 bytes

            // Number of directions changes with the 0xF1 command.
            Directions = FrameID == 0xF1 ? Duration : directions;
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
        public int FrameID {
            get => Data.GetWord(_frameIdAddr);
            set => Data.SetWord(_frameIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_durationAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int Duration {
            get => Data.GetWord(_durationAddr);
            set => Data.SetWord(_durationAddr, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 2, displayFormat: "X2")]
        public int Directions { get; }

        [TableViewModelColumn(addressField: null, displayOrder: 2.5f)]
        public int FramesMissing => HasTexture ? (CHR_Utils.DirectionsToFrameCount(Directions) - GetFrameCount(Directions)) : 0;

        [TableViewModelColumn(displayOrder: 3)]
        public bool IsFinalFrame {
            get {
                var cmd = FrameID;
                return (cmd == 0xF2 || (cmd == 0xFE && Duration < (ID * 2 + 2)) || cmd == 0xFF);
            }
        }

        [TableViewModelColumn(displayOrder: 4)]
        public bool HasTexture {
            get {
                var cmd = FrameID;
                // (NOTE: Command 0xFC is a special command, but it's broken and sets the frame to 0xFC with a duration. Stupid, huh?)
                return cmd < 0xF1 || cmd == 0xF4 || cmd == 0xF5 || (cmd >= 0xF7 && cmd <= 0xFC);
            }
        }

        public int GetFrameCount(int directions) {
            if (FrameTable == null || !HasTexture)
                return 0;

            int expectedFrameCount = CHR_Utils.DirectionsToFrameCount(directions);
            return Math.Max(0, Math.Min(FrameTable.Length - FrameID, expectedFrameCount));
        }

        private readonly Dictionary<int, ITexture> _texturesByFrameCount = new Dictionary<int, ITexture>();
        public ITexture GetTexture(int directions) {
            if (FrameTable == null)
                return null;

            int frameCount = CHR_Utils.DirectionsToFrameCount(directions);
            if (_texturesByFrameCount.TryGetValue(frameCount, out var tex))
                return tex;

            var frameMin = FrameID;
            var frameMax = frameMin + frameCount;
            var frames = FrameTable
                .Where(x => x.ID >= frameMin && x.ID < frameMax)
                .Select(x => x.Texture)
                .ToArray();

            tex = TextureUtils.StackTextures(0, 0, 0, frames);
            _texturesByFrameCount[frameCount] = tex;
            return tex;
        }

        [TableViewModelColumn(displayOrder: 5, minWidth: 200)]
        public string TextureHash => GetTexture(Directions)?.Hash;
    }
}
