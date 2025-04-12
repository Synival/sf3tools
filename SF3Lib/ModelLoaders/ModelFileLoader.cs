using System;
using System.IO;
using CommonLib.Arrays;
using SF3.Exceptions;
using static SF3.ModelLoaders.ModelFileLoaderDelegates;

namespace SF3.ModelLoaders {
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public partial class ModelFileLoader : BaseModelLoader, IModelFileLoader {
        public ModelFileLoader() {
            _createByteData = (IModelFileLoader loader, string filename, string fileDialogFilter, Stream stream) => {
                byte[] newData;
                using (var memoryStream = new MemoryStream()) {
                    stream.CopyTo(memoryStream);
                    newData = memoryStream.ToArray();
                }
                return new ByteData.ByteData(new ByteArray(newData));
            };
        }

        public ModelFileLoader(ModelFileLoaderCreateByteDataDelegate createByteData) {
            _createByteData = createByteData;
        }

        public virtual bool LoadFile(string filename, string fileDialogFilter, ModelFileLoaderCreateModelDelegate createModel) {
            try {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    return LoadFile(filename, fileDialogFilter, stream, createModel);
            }
            catch (Exception) {
                return false;
            }
        }

        public virtual bool LoadFile(string filename, string fileDialogFilter, Stream stream, ModelFileLoaderCreateModelDelegate createModel) {
            return PerformLoad(e => {
                try {
                    var newData = _createByteData(this, filename, fileDialogFilter, stream);
                    if (newData == null)
                        return null;
                    Filename = filename;
                    FileDialogFilter = fileDialogFilter;
                    return newData;
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
                    File.WriteAllBytes(filename, el.ByteData.GetDataCopy());
                    Filename = filename;
                    return true;
                }
                catch {
                    return false;
                }
            });
        }

        private readonly ModelFileLoaderCreateByteDataDelegate _createByteData;

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

        public string FileDialogFilter { get; set; } = null;

        protected override bool OnClose() {
            Filename = null;
            FileDialogFilter = null;
            return true;
        }

        protected override string LoadedTitle => ShortFilename;
    }
}
