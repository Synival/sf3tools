using System;
using System.IO;
using SF3.RawEditors;
using SF3.Exceptions;
using static SF3.ModelLoaders.ModelFileLoaderDelegates;

namespace SF3.ModelLoaders {
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public partial class ModelFileLoader : BaseModelLoader, IModelFileLoader {
        public ModelFileLoader() {
            _createRawEditor = (IModelFileLoader loader, string filename, Stream stream) => {
                    byte[] newData;
                    using (var memoryStream = new MemoryStream()) {
                        stream.CopyTo(memoryStream);
                        newData = memoryStream.ToArray();
                    }
                    return new ByteEditor(newData);
            };
        }

        public ModelFileLoader(ModelFileLoaderCreateRawEditorDelegate createRawEditor) {
            _createRawEditor = createRawEditor;
        }

        public virtual bool LoadFile(string filename, ModelFileLoaderCreateModelDelegate createModel) {
            try {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    return LoadFile(filename, stream, createModel);
            }
            catch (Exception) {
                return false;
            }
        }

        public virtual bool LoadFile(string filename, Stream stream, ModelFileLoaderCreateModelDelegate createModel) {
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
            }, el => createModel((IModelFileLoader) el));
        }

        public virtual bool SaveFile(string filename) {
            if (!IsLoaded)
                throw new ModelFileLoaderNotLoadedException();
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

        private readonly ModelFileLoaderCreateRawEditorDelegate _createRawEditor;

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
