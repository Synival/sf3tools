using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Utils;
using SF3.Types;

namespace SF3.CHR {
    public class CHR_Writer {
        public CHR_Writer(Stream stream) {
            StreamStartPosition = stream.Position;
            Stream = stream;
        }

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
            ushort spriteId, ushort width, ushort height, byte directions,
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
            _frameTablePointers.Add(currentPos + 0x10);
            _animationTablePointers.Add(currentPos + 0x14);

            // Write the entry to the stream.
            Write(outputData.Data.GetDataCopyOrReference());
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
        /// <param name="animationIndex">Index of the current sprite's animation to which this frame belongs.</param>
        public void StartAnimationForCurrentSprite(int animationIndex) {
            _animationCommandTablePointers[animationIndex] = Stream.Position;
        }

        /// <summary>
        /// Writes a single frame for an animation of a sprite.
        /// </summary>
        /// <param name="spriteIndex">Index of the sprite to which this table belongs.</param>
        /// <param name="command">Command for this frame.</param>
        /// <param name="parameter">Parameter for this command.</param>
        /// <param name="frameKeys">List of keys for each frame. Used for assigning the FrameID when frames are written.</param>
        public void WriteAnimationCommand(int spriteIndex, SpriteAnimationCommandType command, int parameter, string[] frameKeys) {
            // If applicable, track the set of frames expected for this animation frame and its offset.
            // The FrameID will be updated later, when the FrameTable is built.
            if (frameKeys != null) {
                if (!_animationFrameRefsBySpriteIndex.ContainsKey(spriteIndex))
                    _animationFrameRefsBySpriteIndex.Add(spriteIndex, new List<AnimationFrameRef>());

                _animationFrameRefsBySpriteIndex[spriteIndex].Add(new AnimationFrameRef() {
                    Offset = Stream.Position,
                    FrameKeys = frameKeys
                });
            }

            Write(((ushort) command).ToByteArray());
            Write(((ushort) parameter).ToByteArray());
        }

        /// <summary>
        /// Writes the animation table for the last sprite written. Table size and values are determined by the
        /// frame tables written earlier.
        /// </summary>
        /// <param name="spriteIndex">Index of the sprite to which this table belongs.</param>
        public void WriteAnimationTable(int spriteIndex) {
            AtPointer(_animationTablePointers[spriteIndex], (offset) => {
                Stream.Write(offset.ToByteArray(), 0, 4);
                _animationTablePointers[spriteIndex] = 0;
            });

            // Determine the size of the table based on the number of animations.
            // (It's always 16 except for XOP101.CHR, which has more entries for Masqurin (U).)
            // Account for an additional terminating 0 at the end.
            var highestAnimationIndex = (_animationCommandTablePointers.Count > 0) ? _animationCommandTablePointers.Max(x => x.Key) : 0;
            var tableSize = Math.Max(16, highestAnimationIndex + 2);

            for (int i = 0; i < tableSize; i++) {
                var offset = (int) (_animationCommandTablePointers.ContainsKey(i) ? (_animationCommandTablePointers[i] - StreamStartPosition) : 0);
                Write(offset.ToByteArray());
            }

            _animationCommandTablePointers.Clear();
        }

        /// <summary>
        /// Writes a single frame to the frame table. The offset for the image specified by
        /// 'frameIdentifier' will be assigned when the image itself is written.
        /// </summary>
        /// <param name="imageId">A unique keywod for the image this represents. This can be a hash or,
        /// <param name="aniFrameKey">An identifier for matching up animation frames, in format "FrameGroup (Dir)"</param>
        /// if the image is intentionally duplicated, any identifying string unique to this CHR.</param>
        public void WriteFrameTableFrame(int spriteIndex, string imageId, string aniFrameKey) {
            InitFrameTableOffset(spriteIndex);

            if (!_frameImagePointers.ContainsKey(imageId))
                _frameImagePointers.Add(imageId, new List<long>());
            _frameImagePointers[imageId].Add(Stream.Position);
            _currentSpriteFrameKeys.Add(aniFrameKey);

            // Get the image offset, if it's already been written.
            var imageOffset = _frameImageOffsets.TryGetValue(imageId, out var offsetOut) ? offsetOut : 0;

            // Placeholder for image offset
            Write(imageOffset.ToByteArray());
        }

        /// <summary>
        /// Writes the final, terminating uint(0) of a FrameTable.
        /// </summary>
        /// <param name="spriteIndex">Index of the sprite to which this frame table belongs.</param>
        public void WriteFrameTableTerminator(int spriteIndex) {
            InitFrameTableOffset(spriteIndex);

            // Now that the table is done, we can assign FrameIDs to animation frames.
            AssignAnimationCommandFrameIDs(spriteIndex);
            _currentSpriteFrameKeys.Clear();

            // Two blank uints; one for a terminator, another for padding.
            Write(new byte[8]);
        }

        /// <summary>
        /// Writes a compressed frame image. Any pointers in the frame table that share the
        /// the same 'imageId' are updated to point to the new image.
        /// </summary>
        /// <param name="imageId">A unique keywod for the image this represents. This can be a hash or,
        /// if the image is intentionally duplicated, any identifying string unique to this CHR.</param>
        /// <param name="compressedImage">Byte representation of the image, already compressed.</param>
        public void WriteFrameImage(string imageId, byte[] compressedImage) {
            // Update existing pointers to this image.
            if (_frameImagePointers.TryGetValue(imageId, out var offsets)) {
                AtPointers(offsets.ToArray(), (offset) => {
                    Stream.Write(offset.ToByteArray(), 0, 4);
                });
                _frameImagePointers.Remove(imageId);
            }

            // Remember the address of this image.
            _frameImageOffsets.Add(imageId, (int) (Stream.Position - StreamStartPosition));

            Write(compressedImage);
        }

        /// <summary>
        /// Writes enough bytes so the position is divisible by 'alignment'.
        /// </summary>
        /// <param name="alignment">The number of bytes the stream's length should be visible by.</param>
        public void WriteToAlignTo(int alignment) {
            var pos = Stream.Position;
            if (pos % alignment != 0)
                Write(new byte[alignment - (pos % alignment)]);
        }

        /// <summary>
        /// Ends the CHR file by writing some necessary padding.
        /// </summary>
        public void Finish() {
            // Write an additional 4 bytes at the end.
            Write(new byte[4]);
        }

        private void InitFrameTableOffset(int spriteIndex) {
            AtPointer(_frameTablePointers[spriteIndex], (offset) => {
                Stream.Write(offset.ToByteArray(), 0, 4);
                _frameTablePointers[spriteIndex] = 0;
            });
        }

        private void AssignAnimationCommandFrameIDs(int spriteIndex) {
            // Don't do anything if this sprite doesn't have any frames.
            if (!_animationFrameRefsBySpriteIndex.ContainsKey(spriteIndex))
                return;

            // Returns 'true' if the sequence of 'keys' is found at 'index'.
            bool KeysAreAtIndex(int index, string[] keys) {
                return
                    (keys.Length == 0) ||
                    (keys[0] == (index < _currentSpriteFrameKeys.Count ? _currentSpriteFrameKeys[index] : null)
                        && KeysAreAtIndex(index + 1, keys.Skip(1).ToArray()));
            }

            // Gets the first FrameID where 'keys' is found in sequence.
            ushort? GetFrameIDForKeys(string[] keys) {
                return Enumerable
                    .Range(0, _currentSpriteFrameKeys.Count)
                    .Select(x => (ushort?) x)
                    .FirstOrDefault(x => KeysAreAtIndex(x.Value, keys));
            }

            // Assign FrameIDs to all animation frames.
            foreach (var frameRef in _animationFrameRefsBySpriteIndex[spriteIndex]) {
                var frameId = GetFrameIDForKeys(frameRef.FrameKeys);
                if (frameId.HasValue)
                    AtPointer(frameRef.Offset, _ => Stream.Write(frameId.Value.ToByteArray(), 0, 2));
                else
                    ; // TODO: what to do if not found?
            }
        }

        /// <summary>
        /// Writes arbitrary data to the stream. Should only be used when the stream position is at the end.
        /// </summary>
        /// <param name="bytes">Bytes to write to the stream.</param>
        public void Write(byte[] bytes) {
            Stream.Write(bytes, 0, bytes.Length);
            BytesWritten += bytes.Length;
        }

        private void AtPointer(long ptr, Action<int> action) {
            if (ptr == 0)
                return;

            var oldPosition = Stream.Position;
            Stream.Position = ptr;
            action((int) (oldPosition - StreamStartPosition));
            Stream.Position = oldPosition;
        }

        private void AtPointers(long[] ptrs, Action<int> action) {
            var oldPosition = Stream.Position;
            var currentPosition = oldPosition;
            foreach (var ptr in ptrs) {
                if (ptr != 0) {
                    Stream.Position = currentPosition = ptr;
                    action((int) (oldPosition - StreamStartPosition));
                }
            }
            if (currentPosition != oldPosition)
                Stream.Position = oldPosition;
        }

        public long StreamStartPosition { get; }
        public Stream Stream { get; }
        public int BytesWritten { get; private set; } = 0;

        private class AnimationFrameRef {
            public override string ToString() => $"0x{Offset:X4} = {string.Join(", ", FrameKeys)}";

            public long Offset;
            public string[] FrameKeys;
        }

        private List<long> _frameTablePointers = new List<long>();
        private List<long> _animationTablePointers = new List<long>();
        private Dictionary<int, long> _animationCommandTablePointers = new Dictionary<int, long>();
        private Dictionary<int, List<AnimationFrameRef>> _animationFrameRefsBySpriteIndex = new Dictionary<int, List<AnimationFrameRef>>();
        private Dictionary<string, List<long>> _frameImagePointers = new Dictionary<string, List<long>>();
        private Dictionary<string, int> _frameImageOffsets = new Dictionary<string, int>();
        private List<string> _currentSpriteFrameKeys = new List<string>();
    }
}
