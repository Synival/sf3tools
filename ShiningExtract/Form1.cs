using CommonLib;

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

                    var mpdFile = new ChunkCollection(stream);
                    var decompressedChunks = mpdFile.DecompressAllChunks(Path.Combine(textBox1.Text, baseFilename));

                    Directory.CreateDirectory(textBox1.Text);
                    Directory.CreateDirectory(textBox2.Text);

                    for (int i = 0; i < decompressedChunks.Length; i++)
                        if (decompressedChunks[i] != null)
                            File.WriteAllBytes(Path.Combine(textBox2.Text, baseFilename + "_" + i + "_decompressed.bin"), decompressedChunks[i]);
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
                    var chunk = new Chunk(stream, (int)stream.Length);
                    var output = chunk.Decompress(Path.Combine(textBox1.Text, baseFilename + "_" + "_log.txt"));
                    File.WriteAllBytes(Path.Combine(textBox2.Text, baseFilename + "_decompressed.bin"), output);
                }
            }
        }
    }
}