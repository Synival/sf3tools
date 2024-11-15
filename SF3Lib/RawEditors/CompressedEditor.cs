using System;
using static CommonLib.Utils.Compression;

namespace SF3.RawEditors {
    public class CompressedEditor : ByteEditor, ICompressedEditor {
        public CompressedEditor(byte[] data) : base(data) {
        }

        public override bool SetData(byte[] data)
            => SetData(data, updateDecompressedData: true);

        public bool SetData(byte[] data, bool updateDecompressedData = true) {
            if (!base.SetData(data))
                return false;

            if (updateDecompressedData) {
                var decompressedData = Decompress(data);
                if (DecompressedEditor == null) {
                    DecompressedEditor = new ByteEditor(decompressedData);

                    DecompressedEditor.IsModifiedChanged += (s, e) => {
                        // When decompressed data is marked as modified, mark that we need compression.
                        if (DecompressedEditor.IsModified) {
                            NeedsRecompression = true;
                            IsModified = true;
                        }

                        // NOTE: We allow DecompressedEditor.IsModified to be 'false' while the parent's
                        //       'NeedsCompression' value is true.
                    };
                }
                else
                    DecompressedEditor.SetData(decompressedData);
            }

            return true;
        }

        public bool Recompress() {
            using (var modifyGuard2 = DecompressedEditor.IsModifiedChangeBlocker()) {
                if (!SetData(Compress(DecompressedEditor.Data)))
                    return false;
                NeedsRecompression = false;
            }
            DecompressedEditor.IsModified = false;
            return true;
        }

        public override bool OnFinalize() {
            if (!base.OnFinalize())
                return false;
            if (!DecompressedEditor.Finalize())
                return false;
            if (NeedsRecompression && !Recompress())
                return false;
            return true;
        }

        public override void Dispose() {
            DecompressedEditor.Dispose();
        }

        public byte[] DecompressedData => DecompressedEditor.Data;

        public IByteEditor DecompressedEditor { get; private set; }

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

        public event EventHandler NeedsRecompressionChanged;
    }
}
