using System;
using CommonLib;
using CommonLib.Arrays;
using CommonLib.SGL;

namespace SF3.ByteData {
    public class ChunkData : IChunkData {
        public ChunkData(IByteArray byteArray, bool chunkIsCompressed, int index) {
            if (byteArray == null)
                throw new NullReferenceException(nameof(byteArray));

            IsCompressed = chunkIsCompressed;
            Index        = index;

            if (chunkIsCompressed) {
                CompressedData = new CompressedData(byteArray);
                ChildData = CompressedData;
                DecompressedData = CompressedData.DecompressedData;
            }
            else {
                CompressedData = null;
                ChildData = new ByteData(byteArray);
                DecompressedData = ChildData;
            }

            if (CompressedData != null)
                CompressedData.NeedsRecompressionChanged += OnNeedsRecompressionChanged;
            ChildData.Finished += OnFinished;
            ChildData.IsModifiedChanged += OnIsModifiedChanged;
        }

        public void OnNeedsRecompressionChanged(object sender, EventArgs eventArgs)
            => NeedsRecompressionChanged?.Invoke(sender, eventArgs);

        public void OnFinished(object sender, EventArgs eventArgs)
            => Finished?.Invoke(sender, eventArgs);

        public void OnIsModifiedChanged(object sender, EventArgs eventArgs)
            => IsModifiedChanged?.Invoke(sender, eventArgs);

        public bool Recompress() {
            if (IsCompressed == false)
                throw new ArgumentException("This ChunkData is not compressed");
            var rval = CompressedData.Recompress();
            Recompressed?.Invoke(this, EventArgs.Empty);
            return rval;
        }

        public bool SetDataTo(byte[] data) => ChildData.SetDataTo(data);
        public byte[] GetDataCopy() => ChildData.GetDataCopy();
        public byte[] GetDataCopyOrReference() => ChildData.GetDataCopyOrReference();
        public byte[] GetDataCopyAt(int offset, int length) => ChildData.GetDataCopyAt(offset, length);
        public uint GetData(int offset, int bytes) => ChildData.GetData(offset, bytes);
        public int GetByte(int offset) => ChildData.GetByte(offset);
        public int GetWord(int offset) => ChildData.GetWord(offset);
        public int GetDouble(int offset) => ChildData.GetDouble(offset);
        public CompressedFIXED GetCompressedFIXED(int offset) => ChildData.GetCompressedFIXED(offset);
        public CompressedFIXED GetWeirdCompressedFIXED(int offset) => ChildData.GetWeirdCompressedFIXED(offset);
        public FIXED GetFIXED(int offset) => ChildData.GetFIXED(offset);
        public string GetString(int offset, int length) => ChildData.GetString(offset, length);
        public bool GetBit(int offset, int bit) => ChildData.GetBit(offset, bit);
        public void SetData(int offset, uint value, int bytes) => ChildData.SetData(offset, value, bytes);
        public void SetByte(int offset, byte value) => ChildData.SetByte(offset, value);
        public void SetWord(int offset, int value) => ChildData.SetWord(offset, value);
        public void SetDouble(int offset, int value) => ChildData.SetDouble(offset, value);
        public void SetCompressedFIXED(int offset, CompressedFIXED value) => ChildData.SetCompressedFIXED(offset, value);
        public void SetWeirdCompressedFIXED(int offset, CompressedFIXED value) => ChildData.SetWeirdCompressedFIXED(offset, value);
        public void SetFIXED(int offset, FIXED value) => ChildData.SetFIXED(offset, value);
        public void SetString(int offset, int length, string value) => ChildData.SetString(offset, length, value);
        public void SetBit(int offset, int bit, bool value) => ChildData.SetBit(offset, bit, value);
        public bool Finish() => ChildData.Finish();
        public ScopeGuard IsModifiedChangeBlocker() => ChildData.IsModifiedChangeBlocker();

        public void Dispose() {
            if (CompressedData != null)
                CompressedData.NeedsRecompressionChanged -= OnNeedsRecompressionChanged;
            ChildData.Finished -= OnFinished;
            ChildData.IsModifiedChanged -= OnIsModifiedChanged;

            if (CompressedData is IDisposable compressedDataDisposable)
                compressedDataDisposable.Dispose();
            if (ChildData != null && ChildData != CompressedData && ChildData is IDisposable childDataDisposable)
                childDataDisposable.Dispose();
        }

        public bool IsCompressed { get; }
        public int Index { get; }
        private ICompressedData CompressedData { get; }
        private IByteData ChildData { get; }
        public IByteData DecompressedData { get; }

        public IByteArray Data => ChildData.Data;
        public int Length => ChildData.Length;

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
        public event EventHandler Finished;
        public event EventHandler IsModifiedChanged;
        public event EventHandler Recompressed;
    }
}
