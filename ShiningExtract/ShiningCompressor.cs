using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiningExtract
{
    internal class ShiningCompressor
    {
        public static byte[] CompressBytes(byte[] buffer)
        {
            return new CompressionEngine(buffer).Compress();
        }

        private class CompressionEngine
        {
            private const int MAXIMUM_COPY_LENGTH = 66; //...why?
            private const int MINIMUM_COPY_LENGTH = 3;
            private byte[] decompressedBytes;
            private byte[] compressedBytes;
            private int currentControlLocation;
            private ushort currentControl;
            private int currentLocation;
            private int currentOutputLocation;
            private int controlCounter = 0;
            public CompressionEngine(byte[] buffer)
            {
                decompressedBytes = buffer;
                //gamble: will we ever end up with that catastrophic state where we compress the file bigger than it originally was?
                compressedBytes = new byte[(int)(buffer.Length * 1.25)];
            }

            public byte[] Compress()
            {
                currentLocation = 0;
                currentControl = 0;
                currentControlLocation = 0;
                currentOutputLocation = 2;

                //first value is guaranteed to be appended raw
                appendRawValue();

                while (currentLocation < decompressedBytes.Length)
                {
                    int seekLocation = currentLocation - 2;
                    //match must be higher than 4--impossible to represent anything smaller.
                    int largestMatch = MINIMUM_COPY_LENGTH;
                    int bestOffset = -1;
                    while (seekLocation >= 0 && ((currentLocation - seekLocation) < 0x1000))
                    {
                        int currentMatch = 0;
                        while (currentMatch < MAXIMUM_COPY_LENGTH && (currentLocation + currentMatch < decompressedBytes.Length) && matchOffsets(seekLocation + currentMatch, currentLocation + currentMatch))
                        {
                            currentMatch += 2;
                        }
                        if (currentMatch > largestMatch)
                        {
                            bestOffset = seekLocation;
                            largestMatch = currentMatch;
                        }
                        if (currentMatch == MAXIMUM_COPY_LENGTH)
                        {
                            break;
                        }
                        seekLocation -= 2;
                    }
                    if (bestOffset != -1)
                    {
                        appendRemoteValue(largestMatch / 2, (currentLocation - bestOffset) / 2);
                        currentLocation += largestMatch;
                    }
                    else
                    {
                        appendRawValue();
                    }
                }
                appendClose();
                return compressedBytes[0..currentOutputLocation];
            }

            private bool matchOffsets(int location1, int location2)
            {
                return decompressedBytes[location1] == decompressedBytes[location2] && decompressedBytes[location1 + 1] == decompressedBytes[location2 + 1];
            }

            private void appendRawValue()
            {
                compressedBytes[currentOutputLocation++] = decompressedBytes[currentLocation++];
                compressedBytes[currentOutputLocation++] = decompressedBytes[currentLocation++];
                controlCounter++;
                if (controlCounter == 16)
                {
                    commitControlValue();
                }
            }

            private void appendRemoteValue(int count, int offset)
            {
                ushort combinedValue = (ushort)((offset << 5) | (count - 2));
                byte[] bytes = BitConverter.GetBytes(combinedValue);
                compressedBytes[currentOutputLocation++] = bytes[1];
                compressedBytes[currentOutputLocation++] = bytes[0];
                currentControl |= (ushort)(1 << (15 - controlCounter));
                controlCounter++;
                if (controlCounter == 16)
                {
                    commitControlValue();
                }
            }

            private void appendClose()
            {
                //control value set to 00
                compressedBytes[currentOutputLocation++] = 0;
                compressedBytes[currentOutputLocation++] = 0;
                currentControl |= (ushort)(1 << (15 - controlCounter));
                commitControlValue();
                currentOutputLocation -= 2; //cheap hack--we don't need the NEXT control value
            }

            private void commitControlValue()
            {
                compressedBytes[currentControlLocation] = (byte)((currentControl >> 8) & 0xFF);
                compressedBytes[currentControlLocation + 1] = (byte)((currentControl >> 0) & 0xFF);
                currentControlLocation += 0x22;
                currentOutputLocation += 2;
                currentControl = 0;
                controlCounter = 0;
            }
        }
    }
}
