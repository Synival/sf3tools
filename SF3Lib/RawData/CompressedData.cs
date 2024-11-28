using System;
using static CommonLib.Utils.Compression;

namespace SF3.RawData {
    public class CompressedData : ByteData, ICompressedData {
        public CompressedData(byte[] data, int? maxDecompressedSize = null) : base(data) {
            MaxDecompressedSize = maxDecompressedSize;
            _hasInit = true;

            using (IsModifiedChangeBlocker())
                _ = SetData(data);
        }

        // TODO: This _hasInit is a ugly workaround for the fact that a virtual method
        //       is called in the base constructor. Bad!!!
        private bool _hasInit = false;
        public override bool SetData(byte[] data) {
            if (!_hasInit)
                return false;
            return SetData(data, updateDecompressedData: true);
        }

        public bool SetData(byte[] data, bool updateDecompressedData = true) {
            if (!base.SetData(data))
                return false;

            if (updateDecompressedData) {
                var decompressedData = Decompress(data, MaxDecompressedSize);
                if (DecompressedData == null) {
                    DecompressedData = new ByteData(decompressedData);

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
                    DecompressedData.SetData(decompressedData);
            }

            return true;
        }

        public bool Recompress() {
            using (var modifyGuard2 = DecompressedData.IsModifiedChangeBlocker()) {
                if (!SetData(Compress(DecompressedData.Data)))
                    return false;
                NeedsRecompression = false;
            }
            DecompressedData.IsModified = false;
            return true;
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

        public override void Dispose() {
            DecompressedData.Dispose();
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

        public int? MaxDecompressedSize { get; }

        public event EventHandler NeedsRecompressionChanged;
    }
}
