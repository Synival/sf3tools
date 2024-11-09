using System;
using System.IO;
using CommonLib.NamedValues;
using SF3.Exceptions;

namespace SF3.FileEditors {
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public class FileEditor : ByteEditor, IFileEditor {
        public FileEditor(INameGetterContext nameContext) {
            _title = BaseTitle;
            NameContext = nameContext;
            ModifiedChanged += (s, e) => UpdateTitle();
        }

        public string Filename { get; private set; }

        public string ShortFilename {
            get {
                if (Filename == null)
                    return null;

                var words = Filename.Split('\\');
                return words[Math.Max(0, words.Length - 1)];
            }
        }

        private string _title;

        public string Title {
            get => _title;
            set {
                if (_title != value) {
                    _title = value;
                    TitleChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public INameGetterContext NameContext { get; }

        public virtual bool LoadFile(string filename) {
            try {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    return LoadFile(filename, stream);
            }
            catch (Exception) {
                return false;
            }
        }

        public virtual bool LoadFile(string filename, Stream stream) {
            try {
                if (IsLoaded)
                    _ = CloseFile();

                byte[] newData;
                using (var memoryStream = new MemoryStream()) {
                    stream.CopyTo(memoryStream);
                    newData = memoryStream.ToArray();
                }

                PreFileLoaded?.Invoke(this, EventArgs.Empty);

                Filename = filename;
                _ = SetData(newData);

                UpdateTitle();
                FileLoaded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception) {
                return false;
            }

            return true;
        }

        public virtual bool SaveFile(string filename) {
            if (Data == null)
                throw new FileEditorNotLoadedException();

            try {
                PreFileSaved?.Invoke(this, EventArgs.Empty);
                File.WriteAllBytes(filename, Data);
                Filename = filename;

                IsModified = false;
                UpdateTitle();
                FileSaved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception) {
                return false;
            }

            return true;
        }

        public virtual bool CloseFile() {
            if (!IsLoaded)
                return true;

            PreFileClosed?.Invoke(this, EventArgs.Empty);

            _ = SetData(null);
            Filename = null;
            IsModified = false;

            UpdateTitle();
            FileClosed?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public string EditorTitle(string formTitle) => formTitle + (IsLoaded ? " - " + Title : "");

        public event EventHandler PreFileLoaded;
        public event EventHandler FileLoaded;
        public event EventHandler PreFileSaved;
        public event EventHandler FileSaved;
        public event EventHandler PreFileClosed;
        public event EventHandler FileClosed;
        public event EventHandler TitleChanged;

        /// <summary>
        /// Creates a new title to be used with UpdateTitle().
        /// This isn't intended to be used directly, but just overridden.
        /// Call UpdateTitle() whenever it looks like the title should be modified.
        /// </summary>
        protected virtual string BaseTitle => IsLoaded ? ShortFilename : "(no file)";

        /// <summary>
        /// Updates 'Title' and invokes a 'TitleChanged' event if it changed.
        /// Call this whenever it looks like the title should be modified.
        /// </summary>
        protected void UpdateTitle() => Title = BaseTitle + (IsModified ? "*" : "");
    }
}
