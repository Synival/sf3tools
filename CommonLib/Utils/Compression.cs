using System;
using System.IO;
using System.Linq;

namespace CommonLib.Utils {
    /// <summary>
    /// Utilities for compression and decompression. Used for MPD chunks, for example.
    /// </summary>
    public static class Compression {
        /// <summary>
        /// Takes a compressed byte array and returns its decompressed data.
        /// All credit to AggroCrag for the original decompression code!
        /// </summary>
        /// <param name="data">The compressed data to decompress.</param>
        /// <param name="logFile">Optional log output file. Can be 'null' to write no log.</param>
        /// <returns>A decompressed set of bytes.</returns>
        public static byte[] Decompress(byte[] data, string logFile = null) {
            using (var logWriter = (logFile == null) ? null : new StreamWriter(logFile)) {
                byte[] outputArray = new byte[0x20000];
                int outputPosition = 0;
                bool prevRaw = false;
                int bufferLoc = 0;

                int pos = 0;
                while (pos < data.Length) {
                    byte ctrl1 = data[pos++];
                    byte ctrl2 = data[pos++];

                    int control = ctrl1 << 8 | ctrl2;
                    for (int i = 0; i < 16; i++) {
                        //1 == control
                        if ((control & (1 << (15 - i))) != 0) {
                            if (prevRaw) {
                                logWriter?.WriteLine();
                            }
                            int currentLoc = pos;

                            byte val1 = data[pos++];
                            byte val2 = data[pos++];

                            if (val1 == 0 && val2 == 0) {
                                logWriter?.WriteLine(bufferLoc.ToString("X2") + ": Ending due to 0000 control value at " + currentLoc.ToString("X2"));
                                break;
                            }
                            else {
                                int count = (val2 & 0x1F) + 2;
                                int offset = (val1 << 3) | ((val2 & 0xE0) >> 5);
                                logWriter?.WriteLine(bufferLoc.ToString("X2") + ": Copy " + (count * 2).ToString("X2") + " from offset " + (offset * 2).ToString("X2") + " at " + currentLoc.ToString("X2"));
                                bufferLoc += count * 2;
                                int windowPos = outputPosition - offset * 2;
                                for (int j = 0; j < count; j++) {
                                    outputArray[outputPosition++] = outputArray[windowPos++];
                                    outputArray[outputPosition++] = outputArray[windowPos++];
                                }
                            }
                            prevRaw = false;
                        }
                        else //2 == data
                        {
                            if (!prevRaw)
                                logWriter?.Write(bufferLoc.ToString("X2") + ": Raw at " + pos.ToString("X2") + ": ");

                            byte val1 = data[pos++];
                            byte val2 = data[pos++];

                            outputArray[outputPosition++] = val1;
                            outputArray[outputPosition++] = val2;

                            logWriter?.Write(val1.ToString("X2"));
                            logWriter?.Write(val2.ToString("X2"));
                            bufferLoc += 2;
                            prevRaw = true;
                        }
                    }
                }

                return outputArray.Take(outputPosition).ToArray();
            }
        }

        /// <summary>
        /// Takes an uncompressed byte array and returns its compressed data.
        /// All credit to AggroCrag for the origianl compression code!
        /// </summary>
        /// <param name="data">The uncompressed data to compress.</param>
        /// <param name="logFile">Optional log output file. Can be 'null' to write no log.</param>
        /// <returns>A compressed set of bytes.</returns>
        public static byte[] Compress(byte[] data, string logFile = null) {
            return CompressBytes(data);
        }

        public static byte[] CompressBytes(byte[] buffer) {
            return new CompressionEngine(buffer).Compress();
        }

        private class CompressionEngine {
            private const int MAXIMUM_COPY_LENGTH = 66; //...why?
            private const int MINIMUM_COPY_LENGTH = 3;
            private byte[] _decompressedBytes;
            private byte[] _compressedBytes;
            private int _currentControlLocation;
            private ushort _currentControl;
            private int _currentLocation;
            private int _currentOutputLocation;
            private int _controlCounter = 0;

            public CompressionEngine(byte[] buffer) {
                _decompressedBytes = buffer;
                // gamble: will we ever end up with that catastrophic state where we compress the file bigger than it originally was?
                // (gamble lost -- for low sizes like 4 bytes, the compressed version is actually larger, by double.
                //  let's add at least 8 bytes.)
                _compressedBytes = new byte[(int) (buffer.Length * 1.25) + 8];
            }

            public byte[] Compress() {
                _currentLocation = 0;
                _currentControl = 0;
                _currentControlLocation = 0;
                _currentOutputLocation = 2;

                // first value is guaranteed to be appended raw
                AppendRawValue();

                while (_currentLocation < _decompressedBytes.Length) {
                    int seekLocation = _currentLocation - 2;

                    // match must be higher than 4--impossible to represent anything smaller.
                    int largestMatch = MINIMUM_COPY_LENGTH;
                    int bestOffset = -1;
                    while (seekLocation >= 0 && ((_currentLocation - seekLocation) < 0x1000)) {
                        int currentMatch = 0;
                        while (currentMatch < MAXIMUM_COPY_LENGTH && (_currentLocation + currentMatch < _decompressedBytes.Length) && MatchOffsets(seekLocation + currentMatch, _currentLocation + currentMatch)) {
                            currentMatch += 2;
                        }
                        if (currentMatch > largestMatch) {
                            bestOffset = seekLocation;
                            largestMatch = currentMatch;
                        }
                        if (currentMatch == MAXIMUM_COPY_LENGTH) {
                            break;
                        }
                        seekLocation -= 2;
                    }
                    if (bestOffset != -1) {
                        AppendRemoteValue(largestMatch / 2, (_currentLocation - bestOffset) / 2);
                        _currentLocation += largestMatch;
                    }
                    else {
                        AppendRawValue();
                    }
                }
                AppendClose();
                return _compressedBytes.Take(_currentOutputLocation).ToArray();
            }

            private bool MatchOffsets(int location1, int location2) {
                return _decompressedBytes[location1] == _decompressedBytes[location2] && _decompressedBytes[location1 + 1] == _decompressedBytes[location2 + 1];
            }

            private void AppendRawValue() {
                _compressedBytes[_currentOutputLocation++] = _decompressedBytes[_currentLocation++];
                _compressedBytes[_currentOutputLocation++] = _decompressedBytes[_currentLocation++];
                _controlCounter++;
                if (_controlCounter == 16) {
                    CommitControlValue();
                }
            }

            private void AppendRemoteValue(int count, int offset) {
                ushort combinedValue = (ushort)((offset << 5) | (count - 2));
                byte[] bytes = BitConverter.GetBytes(combinedValue);
                _compressedBytes[_currentOutputLocation++] = bytes[1];
                _compressedBytes[_currentOutputLocation++] = bytes[0];
                _currentControl |= (ushort) (1 << (15 - _controlCounter));
                _controlCounter++;
                if (_controlCounter == 16) {
                    CommitControlValue();
                }
            }

            private void AppendClose() {
                //control value set to 00
                _compressedBytes[_currentOutputLocation++] = 0;
                _compressedBytes[_currentOutputLocation++] = 0;
                _currentControl |= (ushort) (1 << (15 - _controlCounter));
                CommitControlValue();
                _currentOutputLocation -= 2; //cheap hack--we don't need the NEXT control value
            }

            private void CommitControlValue() {
                _compressedBytes[_currentControlLocation] = (byte) ((_currentControl >> 8) & 0xFF);
                _compressedBytes[_currentControlLocation + 1] = (byte) ((_currentControl >> 0) & 0xFF);
                _currentControlLocation += 0x22;
                _currentOutputLocation += 2;
                _currentControl = 0;
                _controlCounter = 0;
            }
        }

    }
}
