using MPDLib;
using static MPDLib.Extensions.BinaryReaderExtensions;

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

        private void extractChunkButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var stream = openFileDialog1.OpenFile())
                {
                    string baseFilename = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);

                    var mpdFile = new MPDFile();
                    mpdFile.FetchChunkDefinitions(stream);
                    var decompressedChunks = mpdFile.DecompressAllChunks(stream, Path.Combine(textBox1.Text, baseFilename));

                    Directory.CreateDirectory(textBox1.Text);
                    Directory.CreateDirectory(textBox2.Text);

                    foreach (var kv in decompressedChunks)
                        File.WriteAllBytes(Path.Combine(textBox2.Text, baseFilename + "_" + kv.Key + "_decompressed.bin"), kv.Value);
                }
            }
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
                using (var stream = openFileDialog1.OpenFile()) {
                    string baseFilename = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                    var chunk = new ChunkDefinition(0, (int)stream.Length);
                    var output = chunk.Decompress(stream, Path.Combine(textBox1.Text, baseFilename + "_" + "_log.txt"));
                    File.WriteAllBytes(Path.Combine(textBox2.Text, baseFilename + "_decompressed.bin"), output);
                }
            }
        }
    }
}