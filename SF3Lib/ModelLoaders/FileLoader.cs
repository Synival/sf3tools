using System;
using System.IO;
using SF3.RawEditors;
using SF3.Editors;
using SF3.Exceptions;
using static SF3.ModelLoaders.FileLoaderDelegates;

namespace SF3.ModelLoaders {
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public partial class FileLoader : EditorLoader, IFileLoader {
        public FileLoader() {
            _createRawEditor = (IFileLoader loader, string filename, Stream stream) => {
                    byte[] newData;
                    using (var memoryStream = new MemoryStream()) {
                        stream.CopyTo(memoryStream);
                        newData = memoryStream.ToArray();
                    }
                    return new ByteEditor(newData);
            };
        }

        public FileLoader(FileLoaderCreateRawEditorDelegate createRawEditor) {
            _createRawEditor = createRawEditor;
        }

        public virtual bool LoadFile(string filename, FileLoaderCreateEditorDelegate createEditor) {
            try {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    return LoadFile(filename, stream, createEditor);
            }
            catch (Exception) {
                return false;
            }
        }

        public virtual bool LoadFile(string filename, Stream stream, FileLoaderCreateEditorDelegate createEditor) {
            return PerformLoad(e => {
                try {
                    var newEditor = _createRawEditor(this, filename, stream);
                    if (newEditor == null)
                        return null;
                    Filename = filename;
                    return newEditor;
                }
                catch (Exception) {
                    return null;
                }
            }, el => createEditor((IFileLoader) el));
        }

        public virtual bool SaveFile(string filename) {
            if (!IsLoaded)
                throw new FileEditorNotLoadedException();
            return PerformSave(el => {
                try {
                    File.WriteAllBytes(filename, el.RawEditor.GetAllData());
                    Filename = filename;
                    return true;
                }
                catch {
                    return false;
                }
            });
        }

        private readonly FileLoaderCreateRawEditorDelegate _createRawEditor;

        private string _filename = null;

        public string Filename {
            get => _filename;
            private set {
                if (_filename != value) {
                    _filename = value;

                    if (_filename == null)
                        ShortFilename = null;
                    else {
                        var words = Filename.Split('\\');
                        ShortFilename = words[Math.Max(0, words.Length - 1)];
                    }
                }
            }
        }

        public string ShortFilename { get; private set; } = null;

        protected override bool OnClose() {
            Filename = null;
            return true;
        }

        protected override string LoadedTitle => ShortFilename;
    }
}
