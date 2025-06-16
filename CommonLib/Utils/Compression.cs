using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonLib.Utils {
    /// <summary>
    /// Utilities for compression and decompression. Used for MPD chunks, for example.
    /// </summary>
    public static class Compression {
        public static byte[] Decompress(byte[] data)
            => Decompress(data, null, out _);

        /// <summary>
        /// Takes a compressed byte array and returns its decompressed data.
        /// All credit to Agrathejagged for the decompression code: https://github.com/Agrathejagged
        /// </summary>
        /// <param name="data">The compressed data to decompress.</param>
        /// <returns>A decompressed set of bytes.</returns>
        public static byte[] Decompress(byte[] data, int? maxOutput, out int bytesRead) {
            if (data.Length % 2 == 1)
                throw new ArgumentException(nameof(data) + ": must be an even number of bytes");

            byte[] outputArray = new byte[maxOutput ?? 0x20000];
            int outputPosition = 0;
            int bufferLoc = 0;

            int pos = 0;
            while (pos < data.Length && (!maxOutput.HasValue || outputPosition < maxOutput.Value)) {
                byte ctrl1 = data[pos++];
                byte ctrl2 = data[pos++];

                int control = ctrl1 << 8 | ctrl2;
                for (int i = 0; i < 16; i++) {
                    //1 == control
                    if ((control & (1 << (15 - i))) != 0) {
                        int currentLoc = pos;

                        byte val1 = data[pos++];
                        byte val2 = data[pos++];

                        if (val1 == 0 && val2 == 0)
                            break;

                        int count = (val2 & 0x1F) + 2;
                        int offset = (val1 << 3) | ((val2 & 0xE0) >> 5);
                        bufferLoc += count * 2;
                        int windowPos = outputPosition - offset * 2;
                        for (int j = 0; j < count; j++) {
                            outputArray[outputPosition++] = outputArray[windowPos++];
                            outputArray[outputPosition++] = outputArray[windowPos++];
                        }
                    }
                    // 2 == data
                    else {
                        byte val1 = data[pos++];
                        byte val2 = data[pos++];

                        outputArray[outputPosition++] = val1;
                        outputArray[outputPosition++] = val2;

                        bufferLoc += 2;
                    }
                }
            }

            bytesRead = pos;
            return outputArray.Take(outputPosition).ToArray();
        }

        /// <summary>
        /// Takes an uncompressed byte array and returns its compressed data.
        /// All credit to Agrathejagged for the compression code: https://github.com/Agrathejagged
        /// </summary>
        /// <param name="data">The uncompressed data to compress.</param>
        /// <returns>A compressed set of bytes.</returns>
        public static byte[] Compress(byte[] data) {
            if (data.Length % 2 == 1)
                throw new ArgumentException(nameof(data) + ": must be an even number of bytes");

            const int MAXIMUM_COPY_LENGTH = 66; //...why?
            const int MINIMUM_COPY_LENGTH = 3;

            // gamble: will we ever end up with that catastrophic state where we compress the file bigger than it originally was?
            // (gamble lost -- for low sizes like 4 bytes, the compressed version is actually larger, by double.
            //  let's add at least 8 bytes.)
            byte[] compressedBytes = new byte[(int) (data.Length * 1.25) + 8];

            int currentLocation = 0;
            ushort currentControl = 0;
            int currentControlLocation = 0;
            int currentOutputLocation = 2;
            int controlCounter = 0;

            bool MatchOffsets(int location1, int location2)
                => data[location1] == data[location2] && data[location1 + 1] == data[location2 + 1];

            void CommitControlValue() {
                compressedBytes[currentControlLocation] = (byte) ((currentControl >> 8) & 0xFF);
                compressedBytes[currentControlLocation + 1] = (byte) ((currentControl >> 0) & 0xFF);
                currentControlLocation += 0x22;
                currentOutputLocation += 2;
                currentControl = 0;
                controlCounter = 0;
            }

            void AppendRawValue() {
                compressedBytes[currentOutputLocation++] = data[currentLocation++];
                compressedBytes[currentOutputLocation++] = data[currentLocation++];
                controlCounter++;
                if (controlCounter == 16)
                    CommitControlValue();
            }

            void AppendRemoteValue(int count, int offset) {
                ushort combinedValue = (ushort)((offset << 5) | (count - 2));
                byte[] bytes = BitConverter.GetBytes(combinedValue);
                compressedBytes[currentOutputLocation++] = bytes[1];
                compressedBytes[currentOutputLocation++] = bytes[0];
                currentControl |= (ushort) (1 << (15 - controlCounter));
                controlCounter++;
                if (controlCounter == 16)
                    CommitControlValue();
            }

            void AppendClose() {
                // control value set to 00
                compressedBytes[currentOutputLocation++] = 0;
                compressedBytes[currentOutputLocation++] = 0;
                currentControl |= (ushort) (1 << (15 - controlCounter));
                CommitControlValue();
                currentOutputLocation -= 2; // cheap hack -- we don't need the NEXT control value
            }

            while (currentLocation < data.Length) {
                int seekLocation = currentLocation - 2;

                // match must be higher than 4 -- impossible to represent anything smaller.
                int largestMatch = MINIMUM_COPY_LENGTH;
                int bestOffset = -1;
                while (seekLocation >= 0 && ((currentLocation - seekLocation) < 0x1000)) {
                    int currentMatch = 0;
                    while (currentMatch < MAXIMUM_COPY_LENGTH && (currentLocation + currentMatch < data.Length) &&
                           MatchOffsets(seekLocation + currentMatch, currentLocation + currentMatch)) {
                        currentMatch += 2;
                    }
                    if (currentMatch > largestMatch) {
                        bestOffset = seekLocation;
                        largestMatch = currentMatch;
                    }
                    if (currentMatch == MAXIMUM_COPY_LENGTH)
                        break;

                    seekLocation -= 2;
                }
                if (bestOffset != -1) {
                    AppendRemoteValue(largestMatch / 2, (currentLocation - bestOffset) / 2);
                    currentLocation += largestMatch;
                }
                else
                    AppendRawValue();
            }

            AppendClose();
            return compressedBytes.Take(currentOutputLocation).ToArray();
        }

        /// <summary>
        /// Takes an array of bytes compressed with an algorithm specific to ABGR1555 sprites and returns the recompressed color data.
        /// Algorithm is based on decompiled 'getDecompressedSpriteData()' by Ryudo:
        /// https://github.com/RyudoSynbios/game-tools-collection/blob/82d807ded40d5d4320f1f1b3e90970c18fc2db0f/src/lib/templates/shining-force-3-saturn/romEditor/utils/image.ts
        /// </summary>
        /// <param name="data">Data which contains the compressed data (e.g, a CHR/CHP file).</param>
        /// <param name="offset">Offset to the chunk of data pointed to by an offset in the FrameTable of a Sprite.</param>
        /// <returns></returns>
        public static ushort[] DecompressedSpriteData(byte[] data, uint offset) {
            var decompressedData = new List<ushort>();
            var dataPos = offset + 0x04u;
            var dataEnd = offset + data.GetUInt32((int) offset);

            // The 'feed' is the last set of bytes read in the upper byte, with a single bit following it, like a marker for the end.
            // Every time a bit is read from the feed, it's left-shifted to the right.
            // When the value is exactly 0x8000, that means the end of the feed has been reached, and it must be replenished.
            // Use this value to start, so the first read will replenish the feed.
            ushort feed = 0x8000;
            var feedPos = dataEnd;

            // Functions for the feed.
            void ReplenishFeed()
                => feed = (ushort) ((data[feedPos++] << 8) | 0x0080);

            bool PopTopBitInFeed() {
                var bit = (feed & 0x8000) != 0;
                feed <<= 1;
                return bit;
            }

            bool GetNextBitInFeed() {
                if (feed == 0x8000)
                    ReplenishFeed();
                return PopTopBitInFeed();
            }

            // This is a buffer used for previously-seen values that can be added again. It effectively cuts the number of bytes per
            // commonly-used color in half. It has some special values at the end that are never replaced.
            var buffer = new ushort[0x80];
            var bufferPos = 0;
            buffer[0x7D] = 0x0000;
            buffer[0x7E] = 0x8000;
            buffer[0x7F] = 0x7fff;

            // Read data until we've reached the end.
            while (dataPos < dataEnd) {
                var bufferLookupOrUpperByte = data[dataPos++];
                ushort value;

                // This algorithm is specifically catered towards ABGR1555. The only colors read and added to the
                // buffer have their alpha bit on (0x8000). If the alpha bit is *not* on, this value is a lookup
                // of a value read previously from the buffer. (Values 7D, 7E, and 7F are special reserved values)
                if ((bufferLookupOrUpperByte & 0x80) == 0)
                    value = buffer[bufferLookupOrUpperByte];
                // Otherwise, it's a new value. Add it to the buffer, minding not to erase values at 7D, 7E, and 7F.
                else {
                    value = (ushort) ((bufferLookupOrUpperByte << 0x08) | data[dataPos++]);
                    buffer[bufferPos++] = value;
                    bufferPos %= 0x7D;
                }

                // Determine how many times to add this value.
                // First, read the number of bits used for the 'count' value.
                // (The highest bit is always '1', so this bit does not need to be stored.)
                var bitsForCount = 1;
                while (GetNextBitInFeed())
                    bitsForCount++;

                // Read a binary number from the bit feed, left-shifting for each bit.
                // (Start with '1' for the automatically-provided highest bit)
                var count = 1;
                while (bitsForCount > 1) {
                    count = (count << 1) | (GetNextBitInFeed() ? 1 : 0);
                    bitsForCount--;
                }

                // We have the value, and the count -- add it.
                for (int i = 0; i < count; i++)
                    decompressedData.Add(value);
            }

            return decompressedData.ToArray();
        }
    }
}
