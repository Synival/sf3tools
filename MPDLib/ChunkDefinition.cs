using System.IO;
using System.Linq;
using System.Text;

namespace MPDLib {
    public class ChunkDefinition {
        public ChunkDefinition(int offset, int length) {
            Offset = offset;
            Length = length;
        }

        public byte[] Decompress(Stream stream, string logFile = null) {
            stream.Seek(Offset, SeekOrigin.Begin);

            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            using (var logWriter = (logFile == null) ? null : new StreamWriter(logFile)) {
                byte[] outputArray = new byte[0x10000];
                int outputPosition = 0;
                bool prevRaw = false;
                int bufferLoc = 0;

                while ((reader.BaseStream.Position - Offset) < Length) {
                    byte ctrl1 = reader.ReadByte();
                    byte ctrl2 = reader.ReadByte();
                    int control = ctrl1 << 8 | ctrl2;
                    for (int i = 0; i < 16; i++) {
                        //1 == control
                        if ((control & (1 << (15 - i))) != 0) {
                            if (prevRaw) {
                                logWriter?.WriteLine();
                            }
                            int currentLoc = (int)(reader.BaseStream.Position - Offset);
                            byte val1 = reader.ReadByte();
                            byte val2 = reader.ReadByte();
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
                            if (!prevRaw) {
                                logWriter?.Write(bufferLoc.ToString("X2") + ": Raw at " +(reader.BaseStream.Position - Offset).ToString("X2") + ": ");
                            }
                            byte val1 = reader.ReadByte();
                            byte val2 = reader.ReadByte();
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

        public int Offset { get; }
        public int Length { get; }
    }
}
