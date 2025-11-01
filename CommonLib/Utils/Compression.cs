using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;

namespace CommonLib.Utils {
    /// <summary>
    /// Utilities for compression and decompression. Used for MPD chunks, for example.
    /// </summary>
    public static class Compression {
        /// <summary>
        /// Perform the LZSS decompression algorithm used in Shining Force III.
        /// (The algorithm is word-based; this is a convenience overlord to work with bytes.)
        /// All credit to Agrathejagged for the original decompression code/algorithm: https://github.com/Agrathejagged
        /// </summary>
        /// <param name="data">LZSS-compressed data to decompress.</param>
        /// <returns>The decompressed array of data in bytes.</returns>
        public static byte[] DecompressLZSS(byte[] data)
            => DecompressLZSS(data, null, out _, out _);

        /// <summary>
        /// Perform the LZSS decompression algorithm used in Shining Force III.
        /// All credit to Agrathejagged for the original decompression code/algorithm: https://github.com/Agrathejagged
        /// </summary>
        /// <param name="data">LZSS-compressed data to decompress.</param>
        /// <returns>The decompressed array of data in words.</returns>
        public static ushort[] DecompressLZSS(ushort[] data)
            => DecompressLZSS(data, null, out _, out _);

        /// <summary>
        /// Perform the LZSS decompression algorithm used in Shining Force III.
        /// (The algorithm is word-based; this is a convenience overlord to work with bytes.)
        /// All credit to Agrathejagged for the original decompression code/algorithm: https://github.com/Agrathejagged
        /// </summary>
        /// <param name="data">LZSS-compressed data to decompress.</param>
        /// <param name="maxOutput">Maximum amount of data to write. Useful if the input is unknown and could overflow.</param>
        /// <param name="bytesRead">Output parameter: the number of bytes read from 'data'.</param>
        /// <param name="endDataFound">Output parameter: set to 'true' if and ending word of 0x0000 was found in a set contol bit. Otherwise set to false.</param>
        /// <returns>The decompressed array of data in bytes.</returns>
        public static byte[] DecompressLZSS(byte[] data, int? maxOutput, out int bytesRead, out bool endDataFound) {
            if (data.Length % 2 == 1)
                throw new ArgumentException(nameof(data) + ": must be an even number of bytes");
            var output = DecompressLZSS(data.ToUShorts(), maxOutput * 2, out var wordsRead, out endDataFound).ToByteArray();
            bytesRead = wordsRead * 2;
            return output;
        }

        /// <summary>
        /// Perform the LZSS decompression algorithm used in Shining Force III.
        /// All credit to Agrathejagged for the original decompression code/algorithm: https://github.com/Agrathejagged
        /// </summary>
        /// <param name="data">LZSS-compressed data to decompress.</param>
        /// <param name="maxOutput">Maximum amount of data to write. Useful if the input is unknown and could overflow.</param>
        /// <param name="wordsRead">Output parameter: the number of words read from 'data'.</param>
        /// <param name="endDataFound">Output parameter: set to 'true' if and ending word of 0x0000 was found in a set contol bit. Otherwise set to false.</param>
        /// <returns>The decompressed array of data in words.</returns>
        public static ushort[] DecompressLZSS(ushort[] data, int? maxOutput, out int wordsRead, out bool endDataFound) {
            endDataFound = false;
            wordsRead = 0;

            var outputArray = new ushort[maxOutput ?? 0x10000];
            int outPos = 0;
            maxOutput = maxOutput ?? outputArray.Length;

            // Decompress until we've run out of data or we've hit 'maxOutput'.
            int pos = 0;
            while (pos < data.Length && !endDataFound) {
                // Fetch a 16-bit 'control' value.
                ushort control = data[pos++];

                // Start reading 16-bit data -- 1 word for each bit in the 'control' value (so 16 words).
                for (int i = 0; i < 16 && pos < data.Length; i++) {
                    ushort value = data[pos++];

                    // (15, 14, 13, ... 0)
                    var bit = (1 << (15 - i));

                    // Control bit set = data is a lookup:
                    // - First 11 bits: Offset (in # of words) of data to copy. Applied negatively to 'outPos'.
                    // - Last 5 bits: Length of data to copy.
                    if ((control & bit) != 0) {
                        var currentLoc = pos;
                        if (value == 0) {
                            endDataFound = true;
                            goto breakEntireLoop;
                        }

                        if (outPos >= maxOutput)
                            goto breakEntireLoop;

                        byte copyLen = (byte) ((value & 0x1F) + 2);
                        ushort copyOffset = (ushort) ((value & 0xFFE0) >> 5);

                        // Copy from the window. If the position to copy within the window is bogus, just use '0' for the value.
                        // TODO: Maybe output more error details in this case?
                        var windowPos = outPos - copyOffset;
                        for (int j = 0; j < copyLen && outPos < maxOutput; j++) {
                            outputArray[outPos++] = (windowPos >= 0 && windowPos < maxOutput) ? outputArray[windowPos] : (ushort) 0;
                            windowPos++;
                        }
                    }
                    // Control bit unset = data is literal, inserted once
                    else {
                        if (outPos >= maxOutput)
                            goto breakEntireLoop;
                        outputArray[outPos++] = value;
                    }
                }
            }
breakEntireLoop:

            wordsRead = pos;
            return outputArray.Take(outPos).ToArray();
        }

        /// <summary>
        /// Returns data compressed using LZSS encoding.
        /// (The algorithm is word-based; this is a convenience overlord to work with bytes.)
        /// All credit to Agrathejagged for the original compression code: https://github.com/Agrathejagged
        /// </summary>
        /// <param name="data">The uncompressed data to compress.</param>
        /// <param name="optimize">When true, the maximum allowed window size (0x0800) is used instead of what SF3 appears to use (0x07E0).
        /// This is slightly more optimized for larger files, but produces many more differences and may not be compatible.</param>
        /// <returns>An array of bytes compressed with LZSS encoding.</returns>
        public static byte[] CompressLZSS(byte[] data, bool optimize = false) {
            if (data.Length % 2 == 1)
                throw new ArgumentException(nameof(data) + ": must be an even number of bytes");
            var compressedData = CompressLZSS(data.ToUShorts());
            return compressedData.ToByteArray();
        }

        /// <summary>
        /// Returns data compressed using LZSS encoding.
        /// All credit to Agrathejagged for the original compression code: https://github.com/Agrathejagged
        /// </summary>
        /// <param name="data">The uncompressed data to compress.</param>
        /// <param name="optimize">When true, the maximum allowed window size (0x0800) is used instead of what SF3 appears to use (0x07E0).
        /// This is slightly more optimized for larger files, but produces many more differences and may not be compatible.</param>
        /// <returns>An array of words compressed with LZSS encoding.</returns>
        public static ushort[] CompressLZSS(ushort[] data, bool optimize = false) {
            // The "copy length" segment of the data is 5-bits (max value 0x1F). The number of words to copy is:
            //     copyLength + 2
            // ...the max value of which is 0x21 (33).
            const int MAX_COPY_LENGTH = 0x21;

            // Copy values must be at least 2 words.
            const int MIN_COPY_LENGTH = 2;

            // Maximum possible value for the copy offset is 0x0800 (11 bits, so 0x01 << 11).
            // However, it looks like the SF3 compression chose 0x07E0 for its max value, for some reason.
            // TODO: Does the game do something special of values >= 0x07E0 are found?
            int windowSize = optimize ? 0x0800 : 0x07E0;

            // gamble: will we ever end up with that catastrophic state where we compress the file bigger than it originally was?
            // (gamble lost -- for low sizes like 4 bytes, the compressed version is actually larger, by double.
            //  let's add at least 8 bytes.)
            ushort[] outputArray = new ushort[(int) (data.Length * 1.25) + 8];

            int pos = 0;
            ushort currentControl = 0;
            int controlPos = 0;
            int outPos = 1;
            int controlCounter = 0;

            int GetMatchLen(int currentPos, int searchPos) {
                int currentPosSub = currentPos;
                int searchPosSub = searchPos;
                int matchLen = 0;

                while (matchLen < MAX_COPY_LENGTH && currentPosSub < data.Length && searchPosSub < data.Length && data[currentPosSub] == data[searchPosSub]) {
                    currentPosSub++;
                    searchPosSub++;
                    matchLen++;
                }

                return matchLen;
            }

            while (pos < data.Length) {
                // Initialize "best match" values that indicate "no match found".
                int bestMatchLen = MIN_COPY_LENGTH - 1;
                int bestMatchPos = -1;

                // Look for the largest dictionary match that's occurred so far in the data.
                // Allow reading ahead into the future if a match was found -- the decompressor will "copy itself".
                var searchLimit = pos - windowSize;
                for (int searchPos = pos - 1; searchPos >= 0 && searchPos > searchLimit; searchPos--) {
                    // Get the length of matching data for data at this position.
                    var matchLen = GetMatchLen(pos, searchPos);

                    // If this is the new best match, take note of the offset and number of matches.
                    if (matchLen > bestMatchLen) {
                        bestMatchPos = searchPos;
                        bestMatchLen = matchLen;
                    }

                    // If this is the max size for a match, stop searching -- it's impossible to get any better than this.
                    if (matchLen == MAX_COPY_LENGTH)
                        break;
                }

                // If a match was found, append a "copy" value.
                if (bestMatchPos != -1) {
                    var matchOffset = pos - bestMatchPos;
                    outputArray[outPos++] = (ushort) ((matchOffset << 5) | ((bestMatchLen - 2) & 0x1F));
                    currentControl |= (ushort) (1 << (15 - controlCounter));
                    pos += bestMatchLen;
                }
                // Otherwise, append a "literal" value.
                else
                    outputArray[outPos++] = data[pos++];

                // Move to the next "control" bit.
                controlCounter++;

                // If all "control" bits have been filled, commit the "control" value and move to the next one.
                if (controlCounter == 16) {
                    outputArray[controlPos] = currentControl;
                    controlPos = outPos++;

                    currentControl = 0;
                    controlCounter = 0;
                }
            }

            // Write 0x0000 with a set control bit to indicate end-of-data.
            outputArray[outPos++] = 0;

            currentControl |= (ushort) (1 << (15 - controlCounter));
            outputArray[controlPos] = currentControl;

            return outputArray.Take(outPos).ToArray();
        }

        /// <summary>
        /// Takes an array of bytes compressed with an algorithm specific to ABGR1555 sprites and returns the recompressed color data.
        /// Algorithm is based on decompiled 'getDecompressedSpriteData()' by Ryudo:
        /// https://github.com/RyudoSynbios/game-tools-collection/blob/82d807ded40d5d4320f1f1b3e90970c18fc2db0f/src/lib/templates/shining-force-3-saturn/romEditor/utils/image.ts
        /// </summary>
        /// <param name="data">Data which contains the compressed data (e.g, a CHR/CHP file).</param>
        /// <param name="offset">Offset to the chunk of data pointed to by an offset in the FrameTable of a Sprite.</param>
        /// <param name="sizeOut">The amount of bytes read.</param>
        /// <returns></returns>
        public static ushort[] DecompressSpriteData(byte[] data, uint offset, out uint sizeOut) {
            var decompressedData = new List<ushort>();
            var dataPos = offset + 0x04u;
            var nextFeedPos = data.GetUInt32((int) offset);
            var dataEnd = offset + nextFeedPos;

            // The 'feed' is the last set of bytes read in the upper byte, with a single bit following it, like a marker for the end.
            // Every time a bit is read from the feed, it's left-shifted to the right.
            // When the value is exactly 0x8000, that means the end of the feed has been reached, and it must be replenished.
            // Use this value to start, so the first read will replenish the feed.
            ushort feed = 0x8000;
            var feedPos = 0u;

            // Functions for the feed.
            void ReplenishFeed() {
                feedPos = nextFeedPos;
                nextFeedPos++;
                feed = (ushort) ((data[offset + feedPos] << 8) | 0x0080);
            }

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

            sizeOut = nextFeedPos + (nextFeedPos % 2);
            return decompressedData.ToArray();
        }

        /// <summary>
        /// Compresses a stream of ABGR1555 colors to a compression format used in Shining Force 3's CHR format.
        /// </summary>
        /// <param name="dataIn">Data input as a series of 16-bit color values.</param>
        /// <param name="offset">Position of the data.</param>
        /// <param name="sizeInShorts">Number of bytes to read from the data.</param>
        /// <returns>An array of bytes in CHR frame compressed format.</returns>
        public static byte[] CompressSpriteData(ushort[] dataIn, uint offset, int sizeInShorts) {
            // Create buffers that shouldn't be larger than the incoming data.
            var dataOut = new byte[sizeInShorts * 2 + 4];
            var bitstreamOut = new byte[sizeInShorts + 0x10];

            // As colors are read, new colors are stored in a lookup table of size 0x80.
            // The last three elements are always assigned to colors 0x0000, 0x8000, and 0x7FFF.
            var colorsStored = new ushort[0x80];
            colorsStored[0x7D] = 0x0000;
            colorsStored[0x7E] = 0x8000;
            colorsStored[0x7F] = 0x7FFF;

            // Maintain a dictionary of lookup indices for all colors used.
            var colorLookupRef = new Dictionary<ushort, byte>();
            colorLookupRef[0x0000] = 0x7D;
            colorLookupRef[0x8000] = 0x7E;
            colorLookupRef[0x7FFF] = 0x7F;

            byte colorStoragePos = 0;
            uint posDataOut = 4;
            uint posBitstreamOut = 0;
            byte posBitstreamBit = 0;

            ushort currentColor = 0;
            int currentColorCount = 0;

            void ReadColor(ushort color) {
                // Transparent colors other than 0x0000 and 0x7FFF cannot be compressed. Replace them with 0x0000.
                if (color > 0x0000 && color < 0x7FFF)
                    color = 0x0000;

                if (currentColorCount == 0)
                    currentColor = color;
                else if (color != currentColor) {
                    WriteColor();
                    currentColor = color;
                }
                currentColorCount++;
            }

            void WriteColor() {
                if (currentColorCount == 0)
                    return;

                // If the color to write is already in the stored value table, just write the index byte.
                if (colorLookupRef.ContainsKey(currentColor))
                    dataOut[posDataOut++] = colorLookupRef[currentColor];
                // Otherwise, we write the entire color (16 bits) and store the color.
                else {
                    // Write the color.
                    dataOut[posDataOut++] = (byte) ((currentColor & 0xFF00) >> 8);
                    dataOut[posDataOut++] = (byte)  (currentColor & 0x00FF);

                    // If the desired storage position of this color already has a value (which is 0x0000 by default
                    // and can never have that value naturally), remove the old color from the lookup dictionary.
                    if (colorsStored[colorStoragePos] != 0x0000)
                        colorLookupRef.Remove(colorsStored[colorStoragePos]);

                    // Store the color.
                    colorLookupRef[currentColor] = colorStoragePos;
                    colorsStored[colorStoragePos] = currentColor;

                    // Increment the storage position, and loop around before the permanent colors at 0x7D, 0x7E, and 0x7F.
                    colorStoragePos = (byte) ((colorStoragePos + 1) % 0x7D);
                }

                // Before we write to the bitstream, we need to know how many bits are required to store 'currentColorCount'.
                var countBits = 0;
                var countTmp = currentColorCount;
                while (countTmp > 0) {
                    countBits++;
                    countTmp >>= 1;
                }

                // For each bit required to store the size, write a 1. Then write a 0.
                // Starts at 1 because the top-most bit is assumed to exist, therefore not written.
                for (int i = 1; i < countBits; i++)
                    WriteBit(true);
                WriteBit(false);

                // Write 'currentColorCount' in highest-first bit order, skipping the highest bit because it is always 1.
                for (int i = 1; i < countBits; i++)
                    WriteBit((currentColorCount & (1 << (countBits - i - 1))) != 0);

                // Reset the 'current' values to indicate that there are no colors to write.
                currentColor = 0;
                currentColorCount = 0;
            }

            void WriteBit(bool bitOn) {
                // Append a single bit to the bitstream.
                if (bitOn)
                    bitstreamOut[posBitstreamOut] |= (byte) (1 << (7 - posBitstreamBit));
                posBitstreamBit++;
                if (posBitstreamBit == 8) {
                    posBitstreamBit = 0;
                    posBitstreamOut++;
                }
            }

            // Read all the data, writing along the way when the color changes.
            for (var posIn = 0; posIn < dataIn.Length; posIn++)
                ReadColor(dataIn[posIn + offset]);

            // Write the final color.
            WriteColor();

            // If any bits were written to the current position, move it forward so it now marks the end properly.
            if (posBitstreamBit > 0)
                posBitstreamOut++;

            // Must write an even number of bytes.
            if ((posDataOut + posBitstreamOut) % 2 == 1)
                bitstreamOut[posBitstreamOut++] = 0xFF;

            // The first 4 bytes are the position of the bitstream.
            dataOut[0] = (byte) ((posDataOut & 0xFF000000) >> 24);
            dataOut[1] = (byte) ((posDataOut & 0x00FF0000) >> 16);
            dataOut[2] = (byte) ((posDataOut & 0x0000FF00) >>  8);
            dataOut[3] = (byte)  (posDataOut & 0x000000FF);

            // Returned 'dataOut' and 'bitstreamOut' combined.
            return dataOut.Take((int) posDataOut).Concat(bitstreamOut.Take((int) posBitstreamOut)).ToArray();
        }
    }
}
