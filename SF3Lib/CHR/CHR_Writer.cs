using System;
using System.Collections.Generic;
using System.IO;
using CommonLib.Arrays;
using CommonLib.Utils;
using SF3.Files;

namespace SF3.CHR {
    /// <summary>
    /// Performs the writing of binary data to a CHR file.
    /// </summary>
    public class CHR_Writer : BinaryFileWriter {
        public CHR_Writer(Stream stream) : base(stream) {}

        /// <summary>
        /// Writes a single 0x18 byte-long entry for the sprite table.
        /// </summary>
        /// <param name="spriteId">ID of the sprite.</param>
        /// <param name="width">Width of the sprite.</param>
        /// <param name="height">Height of the sprite.</param>
        /// <param name="directions">Number of directions this sprite faces.</param>
        /// <param name="verticalOffset">Vertical offset for rendering.</param>
        /// <param name="unknown0x08">(Unknown value)</param>
        /// <param name="collisionSize">Size for collision.</param>
        /// <param name="promotionLevel">Promotion level this sprite is for, when applicable. If not, set to 0.</param>
        /// <param name="scale">Scale fo the this sprite, with 0x10000 as the baseline (1.0).</param>
        public void WriteHeaderEntry(
            int spriteIndex, ushort spriteId, ushort width, ushort height, byte directions,
            byte verticalOffset, byte unknown0x08, byte collisionSize, byte promotionLevel,
            int scale
        ) {
            // Build the 0x18 bytes for the header entry.
            var outputData = new ByteData.ByteData(new ByteArray(0x18));
            outputData.SetWord(0x00, spriteId);
            outputData.SetWord(0x02, width);
            outputData.SetWord(0x04, height);
            outputData.SetByte(0x06, directions);
            outputData.SetByte(0x07, verticalOffset);
            outputData.SetByte(0x08, unknown0x08);
            outputData.SetByte(0x09, collisionSize);
            outputData.SetByte(0x0A, promotionLevel);
            // (1 byte of padding here)
            outputData.SetDouble(0x0C, scale);
            outputData.SetDouble(0x10, 0x00000000); // Frame table offset
            outputData.SetDouble(0x14, 0x00000000); // Animation table offset

            // Track the position of the frame table and animation table offsets in the output stream.
            // We'll set them later when we actually write those tables.
            var currentPos = Stream.Position;

            if (!_spriteInfoBySpriteIndex.ContainsKey(spriteIndex))
                _spriteInfoBySpriteIndex.Add(spriteIndex, new SpriteInfo());

            var spriteInfo = GetSpriteInfo(spriteIndex);
            spriteInfo.UnassignedFrameTablePointerOffset     = currentPos + 0x10;
            spriteInfo.UnassignedAnimationTablePointerOffset = currentPos + 0x14;

            // Write the entry to the stream.
            Write(outputData.GetDataCopyOrReference());
        }

        /// <summary>
        /// Writes the final 0x18 byte-long indicating the end of the header.
        /// </summary>
        public void WriteHeaderTerminator() {
            Write(new byte[] {
                0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            });
        }

        /// <summary>
        /// Informs the CHR_Writer that an animation frame table is beginning. Must be done before writing frames.
        /// </summary>
        /// <param name="spriteIndex">Index of the sprite to which this animation belongs.</param>
        /// <param name="animationIndex">Index of the sprite's animation to which this frame belongs.</param>
        public void StartAnimationCommandTable(int spriteIndex, int animationIndex) {
            var spriteInfo = GetSpriteInfo(spriteIndex);
            spriteInfo.AnimationOffsetTableOffsets.Add(animationIndex, Stream.Position);
            spriteInfo.AnimationOffsetTableSize = Math.Max(spriteInfo.AnimationOffsetTableSize, animationIndex + 1);
        }

        /// <summary>
        /// Writes a single frame for an animation of a sprite.
        /// </summary>
        /// <param name="command">Command or FrameID for this frame.</param>
        /// <param name="parameter">Parameter for this command.</param>
        public void WriteAnimationCommand(int command, int parameter) {
            Write(((ushort) command).ToByteArray());
            Write(((ushort) parameter).ToByteArray());
        }

        /// <summary>
        /// Writes the animation table for the last sprite written. Table size and values are determined by the
        /// frame tables written earlier.
        /// </summary>
        /// <param name="spriteIndex">Index of the sprite to which this table belongs.</param>
        public void WriteAnimationTable(int spriteIndex) {
            var spriteInfo = GetSpriteInfo(spriteIndex);
            if (spriteInfo.UnassignedAnimationTablePointerOffset.HasValue) {
                AtOffset(spriteInfo.UnassignedAnimationTablePointerOffset.Value, (currentPos) => Stream.Write(currentPos.ToByteArray(), 0, 4));
                spriteInfo.UnassignedAnimationTablePointerOffset = null;
            }

            // Determine the size of the table based on the number of animations.
            // (It's always 16 except for XOP101.CHR, which has more entries for Masqurin (U).)
            // Account for an additional terminating 0 at the end.
            var tableSize = Math.Max(16, spriteInfo.AnimationOffsetTableSize + 1);

            var animationOffsetTable = spriteInfo.AnimationOffsetTableOffsets;
            for (int i = 0; i < tableSize; i++) {
                var offset = (int) (animationOffsetTable.ContainsKey(i) ? (animationOffsetTable[i] - StreamStartPosition) : 0);
                Write(offset.ToByteArray());
            }
        }

        /// <summary>
        /// Writes a single frame to the frame table. The offset for the image specified by
        /// 'frameIdentifier' will be assigned when the image itself is written.
        /// </summary>
        /// <param name="spriteIndex">Index of the sprite to which this frame table belongs.</param>
        /// <param name="frameKey">A unique keyword for the image this represents. This can be a hash or,
        /// if the image is intentionally duplicated, any identifying string unique to this CHR.</param>
        public void WriteFrameTableFrame(int spriteIndex, string frameKey) {
            AssignUnassignedFrameTablePointerToCurrentPosition(spriteIndex);

            var frameImageInfo = GetFrameImageInfo(frameKey);
            frameImageInfo.UnassignedPointerOffsets.Add(Stream.Position);

            // Get the frame image offset if it's already been written. Otherwise, write '0' as a placeholder.
            var frameImageOffset = frameImageInfo.Offset ?? 0;
            Write(((uint) frameImageOffset).ToByteArray());
        }

        /// <summary>
        /// Writes the final, terminating uint(0) of a FrameTable.
        /// </summary>
        /// <param name="spriteIndex">Index of the sprite to which this frame table belongs.</param>
        public void WriteFrameTableTerminator(int spriteIndex) {
            // In case we're finishing a frame table with no frames, ensure that the sprite's frame table is pointing here to the terminator.
            AssignUnassignedFrameTablePointerToCurrentPosition(spriteIndex);

            // Two blank uints; one for a terminator, another for padding.
            Write(new byte[8]);
        }

        /// <summary>
        /// Writes a compressed frame image. Any pointers in the frame table that share the
        /// the same 'frameKey' are updated to point to the new image.
        /// </summary>
        /// <param name="frameKey">A unique keyword for the image this represents. This can be a hash or,
        /// if the image is intentionally duplicated, any identifying string unique to this CHR.</param>
        /// <param name="compressedImage">Byte representation of the image, already compressed.</param>
        public void WriteFrameImage(string frameKey, byte[] compressedImage) {
            // Update existing pointers to this image.
            var frameImageInfo = GetFrameImageInfo(frameKey);
            if (frameImageInfo.UnassignedPointerOffsets.Count > 0) {
                AtOffsets(frameImageInfo.UnassignedPointerOffsets.ToArray(), (offset) => Stream.Write(offset.ToByteArray(), 0, 4));
                frameImageInfo.UnassignedPointerOffsets.Clear();
            }

            // Remember the offset of this image.
            frameImageInfo.Offset = Stream.Position - StreamStartPosition;

            Write(compressedImage);
        }

        /// <summary>
        /// Ends the CHR file by writing some necessary padding.
        /// </summary>
        public override void Finish() {
            // Write an additional 4 bytes at the end.
            Write(new byte[4]);
        }

        private void AssignUnassignedFrameTablePointerToCurrentPosition(int spriteIndex) {
            var spriteInfo = GetSpriteInfo(spriteIndex);
            if (spriteInfo.UnassignedFrameTablePointerOffset.HasValue) {
                AtOffset(spriteInfo.UnassignedFrameTablePointerOffset.Value, (curPosition) => Stream.Write(curPosition.ToByteArray(), 0, 4));
                spriteInfo.UnassignedFrameTablePointerOffset = null;
            }
        }

        private SpriteInfo GetSpriteInfo(int spriteIndex) {
            if (_spriteInfoBySpriteIndex.TryGetValue(spriteIndex, out var info))
                return info;
            return _spriteInfoBySpriteIndex[spriteIndex] = new SpriteInfo();
        }

        private FrameImageInfo GetFrameImageInfo(string frameKey) {
            if (_frameImageInfoByFrameKey.TryGetValue(frameKey, out var info))
                return info;
            return _frameImageInfoByFrameKey[frameKey] = new FrameImageInfo();
        }

        private class SpriteInfo {
            public long? UnassignedFrameTablePointerOffset;
            public long? UnassignedAnimationTablePointerOffset;
            public Dictionary<int, long> AnimationOffsetTableOffsets = new Dictionary<int, long>();

            public int AnimationOffsetTableSize = 0;
        }

        private Dictionary<int, SpriteInfo> _spriteInfoBySpriteIndex = new Dictionary<int, SpriteInfo>();

        private class FrameImageInfo {
            public long? Offset;
            public List<long> UnassignedPointerOffsets = new List<long>();
        }

        private Dictionary<string, FrameImageInfo> _frameImageInfoByFrameKey = new Dictionary<string, FrameImageInfo>();
    }
}
