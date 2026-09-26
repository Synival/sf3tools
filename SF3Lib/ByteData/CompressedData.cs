using System;
using CommonLib.Arrays;
using CommonLib.Types;
using CommonLib.Utils;
using static CommonLib.Utils.Compression;

namespace SF3.ByteData {
    public class CompressedData : ByteData, ICompressedData {
        public CompressedData(IByteArray byteArray, CompressionType compressionType, int? maxDecompressedSize = null) : base(byteArray) {
            if (byteArray == null)
                throw new NullReferenceException(nameof(byteArray));

            CompressionType = compressionType;
            MaxDecompressedSize = maxDecompressedSize;
            _hasInit = true;
            InitData();
        }

        // TODO: This _hasInit is a ugly workaround for the fact that a virtual method
        //       is called in the base constructor. Bad!!!
        private bool _hasInit = false;

        public override bool SetDataTo(byte[] data) => SetDataTo(data, updateDecompressedData: true);

        protected override void InitData() {
            if (!_hasInit)
                return;
            base.InitData();
            UpdateDecompressedData();
        }

        public bool SetDataTo(byte[] data, bool updateDecompressedData = true) {
            if (data == null)
                throw new NullReferenceException(nameof(data));

            if (!base.SetDataTo(data))
                return false;
            if (updateDecompressedData)
                UpdateDecompressedData();
            return true;
        }

        private void UpdateDecompressedData() {
            var decompressedData = Decompress(Data.GetDataCopyOrReference(), 0, MaxDecompressedSize, out var bytesRead, out var endCodeFound);
            LastDecompressBytesRead = bytesRead;
            LastDecompressEndCodeFound = endCodeFound;

            if (DecompressedData == null) {
                DecompressedData = new ByteData(new ByteArray(decompressedData));

                DecompressedData.IsModifiedChanged += (s, e) => {
                    // When decompressed data is marked as modified, mark that we need compression.
                    if (DecompressedData.IsModified) {
                        NeedsRecompression = true;
                        IsModified = true;
                    }

                    // NOTE: We allow DecompressedData.IsModified to be 'false' while the parent's
                    //       'NeedsCompression' value is true.
                };
            }
            else
                DecompressedData.SetDataTo(decompressedData);
        }

        public bool Recompress() {
            using (var modifyGuard2 = DecompressedData.IsModifiedChangeBlocker()) {
                if (!SetDataTo(Compress(DecompressedData.GetDataCopy())))
                    return false;
                NeedsRecompression = false;
            }
            DecompressedData.IsModified = false;
            Recompressed?.Invoke(this, EventArgs.Empty);
            return true;
        }

        private byte[] Compress(byte[] data) {
            switch (CompressionType) {
                case CompressionType.None:
                    return data;
                case CompressionType.LZSS:
                    return CompressLZSS(data);
                case CompressionType.Sprite:
                    return CompressSpriteData(data.ToUShorts(), 0, data.Length / 2);
                case CompressionType.AttackAnim:
                    throw new NotImplementedException($"{nameof(CompressionType.AttackAnim)} not yet supported");
                default:
                    throw new InvalidOperationException($"Unhandled compression type: {CompressionType}");
            }
        }

        private byte[] Decompress(byte[] data, int offset, int? maxOutput, out int bytesRead, out bool endDataFound) {
            switch (CompressionType) {
                case CompressionType.None:
                    // TODO: support maxOutput
                    bytesRead = data.Length;
                    endDataFound = true;
                    return data;

                case CompressionType.LZSS:
                    return DecompressLZSS(data, offset, maxOutput, out bytesRead, out endDataFound);

                case CompressionType.Sprite: {
                    // TODO: support maxOutput
                    var outData = DecompressSpriteData(data, 0, out var bytesReadUInt).ToBytes();
                    bytesRead = (int) bytesReadUInt;
                    endDataFound = true;
                    return outData;
                }

                case CompressionType.AttackAnim: {
                    // TODO: support maxOutput
                    var outData = DecompressAttackAnimChunk(data).ToBytes();
                    bytesRead = outData.Length;
                    endDataFound = true;
                    return outData;
                }

                default:
                    throw new InvalidOperationException($"Unhandled compression type: {CompressionType}");
            }
        }

        public override bool OnFinish() {
            if (!base.OnFinish())
                return false;
            if (!DecompressedData.Finish())
                return false;
            if (NeedsRecompression && !Recompress())
                return false;
            return true;
        }

        protected override void Dispose(bool disposing) {
            if (disposing)
                (DecompressedData as IDisposable)?.Dispose();
            base.Dispose(disposing);
        }

        public IByteData DecompressedData { get; private set; }

        public bool _needsRecompression = false;
        public bool NeedsRecompression {
            get => _needsRecompression;
            set {
                if (_needsRecompression != value) {
                    _needsRecompression = value;
                    if (_needsRecompression == true)
                        IsModified = true;
                    NeedsRecompressionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public override bool IsModified {
            get => base.IsModified;
            set {
                // Don't allow IsModified to be set to 'false' as long as there's data that needs recompression.
                base.IsModified = NeedsRecompression | value;
            }
        }

        public CompressionType CompressionType { get; }
        public int? MaxDecompressedSize { get; }
        public int? LastDecompressBytesRead { get; private set; } = null;
        public bool? LastDecompressEndCodeFound { get; private set; } = null;

        public event EventHandler NeedsRecompressionChanged;
        public event EventHandler Recompressed;
    }
}
