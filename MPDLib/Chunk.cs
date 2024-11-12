using System.IO;
using System.Linq;

namespace MPDLib {
    /// <summary>
    /// Definition for a single chunk that can possibly be compressed or decompressed.
    /// </summary>
    public class Chunk {
        public Chunk(Stream stream, int size) {
            Size = size;

            // Snarf all the data.
            Data = new byte[size];
            if (size > 0)
                stream.Read(Data, 0, size);
        }

        public Chunk(byte[] data, int offset, int size) {
            Size = size;

            // Snarf all the data.
            Data = new byte[size];
            if (size > 0)
                using (Stream stream = new MemoryStream(data, offset, size))
                    stream.Read(Data, 0, size);
        }

        /// <summary>
        /// Processed compressed chunk data and returns an uncompressed byte[].
        /// All credit to AggroCrag for this decompression code!
        /// </summary>
        /// <param name="logFile"></param>
        /// <returns></returns>
        public byte[] Decompress(string logFile = null) {
            logFile = "MyLog.txt";

            using (var logWriter = (logFile == null) ? null : new StreamWriter(logFile)) {
                byte[] outputArray = new byte[0x10000];
                int outputPosition = 0;
                bool prevRaw = false;
                int bufferLoc = 0;

                int pos = 0;
                while (pos < Size) {
                    byte ctrl1 = Data[pos++];
                    byte ctrl2 = Data[pos++];
                    int control = ctrl1 << 8 | ctrl2;
                    for (int i = 0; i < 16; i++) {
                        //1 == control
                        if ((control & (1 << (15 - i))) != 0) {
                            if (prevRaw) {
                                logWriter?.WriteLine();
                            }
                            int currentLoc = pos;
                            byte val1 = Data[pos++];
                            byte val2 = Data[pos++];
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

                            byte val1 = Data[pos++];
                            byte val2 = Data[pos++];
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
        /// Size of the chunk in the MPD file
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Data read from the MPD file
        /// </summary>
        public byte[] Data { get; }
    }
}
