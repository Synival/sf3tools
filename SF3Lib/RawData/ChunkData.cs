using System;
using CommonLib;

namespace SF3.RawData {
    public class ChunkData : IChunkData {
        public ChunkData(byte[] data, bool chunkIsCompressed) {
            IsCompressed = chunkIsCompressed;

            if (chunkIsCompressed) {
                CompressedData = new CompressedData(data);
                ChildData = CompressedData;
                DecompressedData = CompressedData.DecompressedData;
            }
            else {
                CompressedData = null;
                ChildData = new ByteData(data);
                DecompressedData = ChildData;
            }

            if (CompressedData != null)
                CompressedData.NeedsRecompressionChanged += OnNeedsRecompressionChanged;
            ChildData.Finalized += OnFinalized;
            ChildData.IsModifiedChanged += OnIsModifiedChanged;
        }

        public void OnNeedsRecompressionChanged(object sender, EventArgs eventArgs)
            => NeedsRecompressionChanged?.Invoke(sender, eventArgs);

        public void OnFinalized(object sender, EventArgs eventArgs)
            => Finalized?.Invoke(sender, eventArgs);

        public void OnIsModifiedChanged(object sender, EventArgs eventArgs)
            => IsModifiedChanged?.Invoke(sender, eventArgs);

        public bool Recompress() {
            if (IsCompressed == false)
                throw new ArgumentException("This ChunkData is not compressed");
            return CompressedData.Recompress();
        }

        public bool SetData(byte[] data) => ChildData.SetData(data);
        public byte[] GetAllData() => ChildData.GetAllData();
        public uint GetData(int location, int bytes) => ChildData.GetData(location, bytes);
        public int GetByte(int location) => ChildData.GetByte(location);
        public int GetWord(int location) => ChildData.GetWord(location);
        public int GetDouble(int location) => ChildData.GetDouble(location);
        public string GetString(int location, int length) => ChildData.GetString(location, length);
        public bool GetBit(int location, int bit) => ChildData.GetBit(location, bit);
        public void SetData(int location, uint value, int bytes) => ChildData.SetData(location, value, bytes);
        public void SetByte(int location, byte value) => ChildData.SetByte(location, value);
        public void SetWord(int location, int value) => ChildData.SetWord(location, value);
        public void SetDouble(int location, int value) => ChildData.SetDouble(location, value);
        public void SetString(int location, int length, string value) => ChildData.SetString(location, length, value);
        public void SetBit(int location, int bit, bool value) => ChildData.SetBit(location, bit, value);
        public bool Finalize() => ChildData.Finalize();
        public ScopeGuard IsModifiedChangeBlocker() => ChildData.IsModifiedChangeBlocker();

        public void Dispose() {
            if (CompressedData != null)
                CompressedData.NeedsRecompressionChanged -= OnNeedsRecompressionChanged;
            ChildData.Finalized -= OnFinalized;
            ChildData.IsModifiedChanged -= OnIsModifiedChanged;

            if (CompressedData != null)
                CompressedData.Dispose();
            if (ChildData != null && ChildData != CompressedData)
                ChildData.Dispose();
        }

        public bool IsCompressed { get; }
        private ICompressedData CompressedData { get; }
        private IByteData ChildData { get; }
        public IByteData DecompressedData { get; }

        public byte[] Data => ChildData.Data;
        public int Size => ChildData.Size;

        public bool IsModified {
            get => ChildData.IsModified;
            set => ChildData.IsModified = value;
        }

        public bool NeedsRecompression {
            get => IsCompressed ? CompressedData.NeedsRecompression : false;
            set {
                if (value == true && IsCompressed == false)
                    throw new ArgumentException("This ChunkData is not compressed");
                CompressedData.NeedsRecompression = value;
            }
        }

        public event EventHandler NeedsRecompressionChanged;
        public event EventHandler Finalized;
        public event EventHandler IsModifiedChanged;
    }
}
