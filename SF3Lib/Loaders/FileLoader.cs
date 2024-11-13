using System;
using System.IO;
using CommonLib.NamedValues;
using SF3.RawEditors;
using SF3.Editors;
using SF3.Exceptions;

namespace SF3.Loaders {
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public class FileLoader : EditorLoader, IFileLoader {
        public FileLoader(INameGetterContext nameContext) : base(nameContext) { }

        public virtual bool LoadFile(string filename, Func<IFileLoader, IBaseEditor> createEditor) {
            try {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    return LoadFile(filename, stream, createEditor);
            }
            catch (Exception) {
                return false;
            }
        }

        public virtual bool LoadFile(string filename, Stream stream, Func<IFileLoader, IBaseEditor> createEditor) {
            return PerformLoad(() => {
                try {
                    byte[] newData;
                    using (var memoryStream = new MemoryStream()) {
                        stream.CopyTo(memoryStream);
                        newData = memoryStream.ToArray();
                    }
                    return new ByteEditor(newData);
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
                    File.WriteAllBytes(filename, ((IByteEditor) RawEditor).Data);
                    Filename = filename;
                    return true;
                }
                catch {
                    return false;
                }
            });
        }

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
        protected override string UnloadedTitle => "(no file)";
    }
}
