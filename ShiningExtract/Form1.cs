using System.Data;
using System.Runtime.CompilerServices;

namespace ShiningExtract
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal).ToString() + "\\Shining";
            textBox2.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal).ToString() + "\\Shining";
        }

        private struct ChunkDefinition
        {
            public int offset;
            public int length;
        }

        private void extractChunkButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var stream = openFileDialog1.OpenFile())
                {
                    var reader = new BinaryReader(stream);
                    stream.Seek(0x2000, SeekOrigin.Begin);
                    ChunkDefinition[] defs = new ChunkDefinition[32];
                    for (int i = 0; i < defs.Length; i++)
                    {
                        defs[i].offset = readLittleEndianInt32(reader) - 0x290000;
                        defs[i].length = readLittleEndianInt32(reader);
                    }
                    string baseFilename = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                    Directory.CreateDirectory(textBox1.Text);
                    Directory.CreateDirectory(textBox2.Text);
                    for (int currentChunk = 5; currentChunk < defs.Length; currentChunk++)
                    {
                        int chunkOffset = defs[currentChunk].offset;
                        int chunkLength = defs[currentChunk].length;
                        if (defs[currentChunk].length > 0 && currentChunk != 20)
                        {
                            decompressChunk(reader, chunkOffset, chunkLength, Path.Combine(textBox1.Text, baseFilename + "_" + currentChunk + "_log.txt"), Path.Combine(textBox2.Text, baseFilename + "_" + currentChunk + "_decompressed.bin"));
                        }
                    }
                }
            }
        }
        private static int readLittleEndianInt32(BinaryReader reader)
        {
            return BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
        }

        private void compressButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                byte[] decompressedData = File.ReadAllBytes(openFileDialog1.FileName);
                string baseFilename = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                Directory.CreateDirectory(textBox1.Text);
                byte[] compressed = ShiningCompressor.CompressBytes(decompressedData);
                File.WriteAllBytes(Path.Combine(textBox2.Text, baseFilename + "_compressed.bin"), compressed);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = openFileDialog1.OpenFile();
                BinaryReader reader = new BinaryReader(stream);
                string baseFilename = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                decompressChunk(reader, 0, (int)stream.Length, Path.Combine(textBox1.Text, baseFilename + "_" + "_log.txt"), Path.Combine(textBox2.Text, baseFilename + "_decompressed.bin"));

            }
        }

        private void decompressChunk(BinaryReader reader, int chunkOffset, int chunkLength, string logFile, string outputFile)
        {
            reader.BaseStream.Seek(chunkOffset, SeekOrigin.Begin);
            StreamWriter streamWriter = new StreamWriter(logFile);
            byte[] outputArray = new byte[0x10000];
            int outputPosition = 0;
            bool prevRaw = false;
            int bufferLoc = 0;
            while ((reader.BaseStream.Position - chunkOffset) < chunkLength)
            {
                byte ctrl1 = reader.ReadByte();
                byte ctrl2 = reader.ReadByte();
                int control = ctrl1 << 8 | ctrl2;
                for (int i = 0; i < 16; i++)
                {
                    //1 == control
                    if ((control & (1 << (15 - i))) != 0)
                    {
                        if (prevRaw)
                        {
                            streamWriter.WriteLine();
                        }
                        int currentLoc = (int)(reader.BaseStream.Position - chunkOffset);
                        byte val1 = reader.ReadByte();
                        byte val2 = reader.ReadByte();
                        if (val1 == 0 && val2 == 0)
                        {
                            streamWriter.WriteLine(bufferLoc.ToString("X2") + ": Ending due to 0000 control value at " + currentLoc.ToString("X2"));
                            break;
                        }
                        else
                        {
                            int count = (val2 & 0x1F) + 2;
                            int offset = (val1 << 3) | ((val2 & 0xE0) >> 5);
                            streamWriter.WriteLine(bufferLoc.ToString("X2") + ": Copy " + (count * 2).ToString("X2") + " from offset " + (offset * 2).ToString("X2") + " at " + currentLoc.ToString("X2"));
                            bufferLoc += count * 2;
                            int windowPos = outputPosition - offset * 2;
                            for (int j = 0; j < count; j++)
                            {
                                outputArray[outputPosition++] = outputArray[windowPos++];
                                outputArray[outputPosition++] = outputArray[windowPos++];
                            }
                        }
                        prevRaw = false;
                    }
                    else //2 == data
                    {
                        if (!prevRaw)
                        {
                            streamWriter.Write(bufferLoc.ToString("X2") + ": Raw at " +(reader.BaseStream.Position - chunkOffset).ToString("X2") + ": ");
                        }
                        byte val1 = reader.ReadByte();
                        byte val2 = reader.ReadByte();
                        outputArray[outputPosition++] = val1;
                        outputArray[outputPosition++] = val2;
                        streamWriter.Write(val1.ToString("X2"));
                        streamWriter.Write(val2.ToString("X2"));
                        bufferLoc += 2;
                        prevRaw = true;
                    }
                }
            }
            File.WriteAllBytes(outputFile, outputArray[0..outputPosition]);
            streamWriter.Close();
        }
    }
}