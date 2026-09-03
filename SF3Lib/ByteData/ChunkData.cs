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
        public byte GetUInt8(int offset)    => ChildData.GetUInt8(offset);
        public sbyte GetInt8(int offset)    => ChildData.GetInt8(offset);
        public ushort GetUInt16(int offset) => ChildData.GetUInt16(offset);
        public short GetInt16(int offset)   => ChildData.GetInt16(offset);
        public uint GetUInt32(int offset)   => ChildData.GetUInt32(offset);
        public int GetInt32(int offset)     => ChildData.GetInt32(offset);
        public Fractional GetFractional(int offset) => ChildData.GetFractional(offset);
        public Fractional GetWeirdFractional(int offset) => ChildData.GetWeirdFractional(offset);
        public FIXED GetFIXED(int offset) => ChildData.GetFIXED(offset);
        public CompressedFIXED GetCompressedFIXED(int offset, int fracBits) => ChildData.GetCompressedFIXED(offset, fracBits);
        public string GetString(int offset, int length) => ChildData.GetString(offset, length);
        public bool GetBit(int offset, int bit) => ChildData.GetBit(offset, bit);
        public void SetData(int offset, uint value, int bytes) => ChildData.SetData(offset, value, bytes);
        public void SetUInt8(int offset, byte value)    => ChildData.SetUInt8(offset, value);
        public void SetInt8(int offset, sbyte value)    => ChildData.SetInt8(offset, value);
        public void SetUInt16(int offset, ushort value) => ChildData.SetUInt16(offset, value);
        public void SetInt16(int offset, short value)   => ChildData.SetInt16(offset, value);
        public void SetUInt32(int offset, uint value)   => ChildData.SetUInt32(offset, value);
        public void SetInt32(int offset, int value)     => ChildData.SetInt32(offset, value);
        public void SetFractional(int offset, Fractional value) => ChildData.SetFractional(offset, value);
        public void SetWeirdFractional(int offset, Fractional value) => ChildData.SetWeirdFractional(offset, value);
        public void SetFIXED(int offset, FIXED value) => ChildData.SetFIXED(offset, value);
        public void SetCompressedFIXED(int offset, CompressedFIXED value) => ChildData.SetCompressedFIXED(offset, value);
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
