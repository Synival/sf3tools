using System.IO;
using System.Linq;

namespace CommonLib.Utils {
    /// <summary>
    /// Utilities for compression and decompression. Used for MPD chunks, for example.
    /// </summary>
    public static class Compression {
        /// <summary>
        /// Takes incoming byte stream and returns its decompressed data.
        /// </summary>
        /// <param name="data">The data to decompress.</param>
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

                            if (pos >= data.Length)
                                goto unexpectedEOF;
                            byte val1 = data[pos++];

                            if (pos >= data.Length)
                                goto unexpectedEOF;
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
                                    if (outputPosition >= outputArray.Length)
                                        goto unexpectedEOF;
                                    outputArray[outputPosition++] = outputArray[windowPos++];

                                    if (outputPosition >= outputArray.Length)
                                        goto unexpectedEOF;
                                    outputArray[outputPosition++] = outputArray[windowPos++];
                                }
                            }
                            prevRaw = false;
                        }
                        else //2 == data
                        {
                            if (!prevRaw)
                                logWriter?.Write(bufferLoc.ToString("X2") + ": Raw at " + pos.ToString("X2") + ": ");

                            if (pos >= data.Length)
                                goto unexpectedEOF;
                            byte val1 = data[pos++];

                            if (pos >= data.Length)
                                goto unexpectedEOF;
                            byte val2 = data[pos++];

                            if (outputPosition >= outputArray.Length)
                                goto unexpectedBufferTooSmall;
                            outputArray[outputPosition++] = val1;

                            if (outputPosition >= outputArray.Length)
                                goto unexpectedBufferTooSmall;
                            outputArray[outputPosition++] = val2;

                            logWriter?.Write(val1.ToString("X2"));
                            logWriter?.Write(val2.ToString("X2"));
                            bufferLoc += 2;
                            prevRaw = true;
                        }
                    }
                }

                return outputArray.Take(outputPosition).ToArray();

            unexpectedEOF:
                try {
                    throw new System.IO.IOException("Unexpected end of data");
                }
                catch { }
                return outputArray.Take(outputPosition).ToArray();

            unexpectedBufferTooSmall:
                try {
                    throw new System.IO.IOException("Ran out of space to write data -- please fix this function!");
                }
                catch { }
                return outputArray.Take(outputPosition).ToArray();
            }
        }
    }
}
