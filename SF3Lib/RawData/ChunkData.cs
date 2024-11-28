using System;
using CommonLib;

namespace SF3.RawData {
    public class ChunkData : IChunkData {
        public ChunkData(byte[] data, bool chunkIsCompressed) {
            IsCompressed = chunkIsCompressed;

            if (chunkIsCompressed) {
                CompressedEditor = new CompressedData(data);
                ChildEditor = CompressedEditor;
                DecompressedEditor = CompressedEditor.DecompressedEditor;
            }
            else {
                CompressedEditor = null;
                ChildEditor = new ByteData(data);
                DecompressedEditor = ChildEditor;
            }

            if (CompressedEditor != null)
                CompressedEditor.NeedsRecompressionChanged += OnNeedsRecompressionChanged;
            ChildEditor.Finalized += OnFinalized;
            ChildEditor.IsModifiedChanged += OnIsModifiedChanged;
        }

        public void OnNeedsRecompressionChanged(object sender, EventArgs eventArgs)
            => NeedsRecompressionChanged?.Invoke(sender, eventArgs);

        public void OnFinalized(object sender, EventArgs eventArgs)
            => Finalized?.Invoke(sender, eventArgs);

        public void OnIsModifiedChanged(object sender, EventArgs eventArgs)
            => IsModifiedChanged?.Invoke(sender, eventArgs);

        public bool Recompress() {
            if (IsCompressed == false)
                throw new ArgumentException("This ChunkEditor is not compressed");
            return CompressedEditor.Recompress();
        }

        public bool SetData(byte[] data) => ChildEditor.SetData(data);
        public byte[] GetAllData() => ChildEditor.GetAllData();
        public uint GetData(int location, int bytes) => ChildEditor.GetData(location, bytes);
        public int GetByte(int location) => ChildEditor.GetByte(location);
        public int GetWord(int location) => ChildEditor.GetWord(location);
        public int GetDouble(int location) => ChildEditor.GetDouble(location);
        public string GetString(int location, int length) => ChildEditor.GetString(location, length);
        public bool GetBit(int location, int bit) => ChildEditor.GetBit(location, bit);
        public void SetData(int location, uint value, int bytes) => ChildEditor.SetData(location, value, bytes);
        public void SetByte(int location, byte value) => ChildEditor.SetByte(location, value);
        public void SetWord(int location, int value) => ChildEditor.SetWord(location, value);
        public void SetDouble(int location, int value) => ChildEditor.SetDouble(location, value);
        public void SetString(int location, int length, string value) => ChildEditor.SetString(location, length, value);
        public void SetBit(int location, int bit, bool value) => ChildEditor.SetBit(location, bit, value);
        public bool Finalize() => ChildEditor.Finalize();
        public ScopeGuard IsModifiedChangeBlocker() => ChildEditor.IsModifiedChangeBlocker();

        public void Dispose() {
            if (CompressedEditor != null)
                CompressedEditor.NeedsRecompressionChanged -= OnNeedsRecompressionChanged;
            ChildEditor.Finalized -= OnFinalized;
            ChildEditor.IsModifiedChanged -= OnIsModifiedChanged;

            if (CompressedEditor != null)
                CompressedEditor.Dispose();
            if (ChildEditor != null && ChildEditor != CompressedEditor)
                ChildEditor.Dispose();
        }

        public bool IsCompressed { get; }
        private ICompressedData CompressedEditor { get; }
        private IByteData ChildEditor { get; }
        public IByteData DecompressedEditor { get; }

        public byte[] Data => ChildEditor.Data;
        public int Size => ChildEditor.Size;

        public bool IsModified {
            get => ChildEditor.IsModified;
            set => ChildEditor.IsModified = value;
        }

        public byte[] DecompressedData => DecompressedEditor.Data;

        public bool NeedsRecompression {
            get => IsCompressed ? CompressedEditor.NeedsRecompression : false;
            set {
                if (value == true && IsCompressed == false)
                    throw new ArgumentException("This ChunkEditor is not compressed");
                CompressedEditor.NeedsRecompression = value;
            }
        }

        public event EventHandler NeedsRecompressionChanged;
        public event EventHandler Finalized;
        public event EventHandler IsModifiedChanged;
    }
}
