using System;
using System.IO;
using CommonLib.Arrays;
using SF3.ByteData;
using SF3.Models.Files;

namespace SF3.ModelLoaders {
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public class ModelFileLoader : ModelLoader, IModelFileLoader {
        public ModelFileLoader() {}

        public bool LoadFile(string filename, string fileDialogFilter, Func<IByteData, IBaseFile> createModel) {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                return LoadFile(filename, fileDialogFilter, stream, createModel);
        }

        public bool LoadFile(string filename, string fileDialogFilter, Stream stream, Func<IByteData, IBaseFile> createModel) {
            return PerformLoad(
                () => {
                    var newData = CreateByteData(filename, fileDialogFilter, stream);
                    if (newData == null)
                        return null;
                    Filename = filename;
                    FileDialogFilter = fileDialogFilter;
                    return newData;
                },
                () => createModel(ByteData)
            );
        }

        public bool SaveFile(string filename) {
            if (!IsLoaded)
                throw new Exception("Nothing is loaded");
            return PerformSave(() => {
                File.WriteAllBytes(filename, ByteData.Data.GetDataCopyOrReference());
                Filename = filename;
                return true;
            });
        }

        protected IByteData CreateByteData(string filename, string fileDialogFilter, Stream stream) {
            byte[] newData;
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                newData = memoryStream.ToArray();
            }
            return new ByteData.ByteData(new ByteArray(newData));
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

                    UpdateTitle();
                }
            }
        }

        private string _shortFilename = null;
        public string ShortFilename {
            get => _shortFilename;
            private set {
                if (_shortFilename != value) {
                    _shortFilename = value;
                    UpdateTitle();
                }
            }
        }

        public string FileDialogFilter { get; set; } = null;

        protected override bool OnClose() {
            Filename = null;
            FileDialogFilter = null;
            return true;
        }

        protected override string LoadedTitle => ShortFilename;
    }
}
