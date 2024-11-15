using System;
using static CommonLib.Utils.Compression;

namespace SF3.RawEditors {
    public class CompressedEditor : ByteEditor, ICompressedEditor {
        public CompressedEditor(byte[] data) : base(data) {
            IsModifiedChanged += (s, e) => {
                // If the compressed data is marked as modified, so should the decompressed data.
                if (IsModified)
                    DecompressedEditor.IsModified = true;
                // If we still need recompression, we're still modified.
                else
                    IsModified |= NeedsRecompression;
            };
        }

        public override bool SetData(byte[] data) {
            if (!base.SetData(data))
                return false;

            var decompressedData = Decompress(data);
            if (DecompressedEditor == null) {
                DecompressedEditor = new ByteEditor(decompressedData);

                DecompressedEditor.IsModifiedChanged += (s, e) => {
                    // When decompressed data is marked as modified, mark that we need compression.
                    if (DecompressedEditor.IsModified)
                        NeedsRecompression = true;
                    // If we still need recompression, we're still modified.
                    else
                        DecompressedEditor.IsModified |= NeedsRecompression;
                };
            }
            else {
                // TODO: supress any changes to 'IsModified' here using some kind of stack guard
                DecompressedEditor.SetData(decompressedData);
                NeedsRecompression = false;
                DecompressedEditor.IsModified = false;
            }
            return true;
        }

        public override bool OnFinalize() {
            if (!base.OnFinalize())
                return false;
            if (!DecompressedEditor.Finalize())
                return false;
            if (!SetData(Compress(DecompressedEditor.Data)))
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
                    NeedsRecompressionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler NeedsRecompressionChanged;
    }
}
