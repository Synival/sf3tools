﻿using System;
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
        public byte[] GetDataCopyAt(int offset, int length) => ChildData.GetDataCopyAt(offset, length);
        public uint GetData(int location, int bytes) => ChildData.GetData(location, bytes);
        public int GetByte(int location) => ChildData.GetByte(location);
        public int GetWord(int location) => ChildData.GetWord(location);
        public int GetDouble(int location) => ChildData.GetDouble(location);
        public CompressedFIXED GetCompressedFIXED(int location) => ChildData.GetCompressedFIXED(location);
        public CompressedFIXED GetWeirdCompressedFIXED(int location) => ChildData.GetWeirdCompressedFIXED(location);
        public FIXED GetFIXED(int location) => ChildData.GetFIXED(location);
        public string GetString(int location, int length) => ChildData.GetString(location, length);
        public bool GetBit(int location, int bit) => ChildData.GetBit(location, bit);
        public void SetData(int location, uint value, int bytes) => ChildData.SetData(location, value, bytes);
        public void SetByte(int location, byte value) => ChildData.SetByte(location, value);
        public void SetWord(int location, int value) => ChildData.SetWord(location, value);
        public void SetDouble(int location, int value) => ChildData.SetDouble(location, value);
        public void SetCompressedFIXED(int location, CompressedFIXED value) => ChildData.SetCompressedFIXED(location, value);
        public void SetWeirdCompressedFIXED(int location, CompressedFIXED value) => ChildData.SetWeirdCompressedFIXED(location, value);
        public void SetFIXED(int location, FIXED value) => ChildData.SetFIXED(location, value);
        public void SetString(int location, int length, string value) => ChildData.SetString(location, length, value);
        public void SetBit(int location, int bit, bool value) => ChildData.SetBit(location, bit, value);
        public bool Finish() => ChildData.Finish();
        public ScopeGuard IsModifiedChangeBlocker() => ChildData.IsModifiedChangeBlocker();

        public void Dispose() {
            if (CompressedData != null)
                CompressedData.NeedsRecompressionChanged -= OnNeedsRecompressionChanged;
            ChildData.Finished -= OnFinished;
            ChildData.IsModifiedChanged -= OnIsModifiedChanged;

            if (CompressedData != null)
                CompressedData.Dispose();
            if (ChildData != null && ChildData != CompressedData)
                ChildData.Dispose();
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

        public int Offset => (Data is ByteArraySegment bas) ? bas.Offset : 0;

        public event EventHandler NeedsRecompressionChanged;
        public event EventHandler Finished;
        public event EventHandler IsModifiedChanged;
        public event EventHandler Recompressed;
    }
}
