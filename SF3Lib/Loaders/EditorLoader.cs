using System;
using SF3.RawEditors;
using SF3.Editors;

namespace SF3.Loaders {
    public abstract class EditorLoader : IEditorLoader {
        protected EditorLoader() {
            _title = UnloadedTitle;
            _onModifiedChangedDelegate = new EventHandler((o, e) => {
                IsModifiedChanged?.Invoke(this, EventArgs.Empty);
                UpdateTitle();
            });
        }

        private readonly EventHandler _onModifiedChangedDelegate;

        public delegate IRawEditor EditorLoaderCreateRawEditorDelegate(IEditorLoader loader);
        public delegate IBaseEditor EditorLoaderCreateEditorDelegate(IEditorLoader loader);
        public delegate bool EditorLoaderSaveDelegate(IEditorLoader loader);

        /// <summary>
        /// Performs loading of an editor provided. Invokes events 'PreLoaded' and 'Loaded'.
        /// Complete ownership of 'editor' is transferred to the EditorLoader when this is invoked.
        /// If the editor could not be used, it is immediately disposed of via Dispose().
        /// </summary>
        /// <param name="createEditor">Callback to create an editor when possible.</param>
        /// <returns>'true' a new editor was loaded, otherwise 'false'.</returns>
        protected bool PerformLoad(EditorLoaderCreateRawEditorDelegate createRawEditor, EditorLoaderCreateEditorDelegate createEditor) {
            if (createEditor == null)
                return false;
            if (IsLoaded && !Close())
                return false;
            if ((RawEditor = createRawEditor(this)) == null)
                return false;
            if ((Editor = createEditor(this)) == null) {
                RawEditor = null;
                return false;
            }
            return IsLoaded;
        }

        /// <summary>
        /// Performs saving of an editor if one is loaded. Invokes events 'PreSaved' and 'Saved'.
        /// 'Editor.IsModified' is automatically reset to 'false' upon saving.
        /// </summary>
        /// <param name="saveAction">The function to save the editor when possible.</param>
        /// <returns>'true' if saveAction was invokvd and returned success.</returns>
        protected bool PerformSave(EditorLoaderSaveDelegate saveAction) {
            if (saveAction == null || RawEditor == null || Editor == null)
                return false;
            if (!Editor.Finalize())
                return false;

            PreSaved?.Invoke(this, EventArgs.Empty);
            if (!saveAction(this))
                return false;
            RawEditor.IsModified = false;
            Saved?.Invoke(this, EventArgs.Empty);

            return true;
        }

        private IBaseEditor _editor = null;
        public IBaseEditor Editor {
            get => _editor;
            set {
                if (_editor != value) {
                    var oldEditor = _editor;
                    _editor = value;

                    if (oldEditor != null)
                        oldEditor.Dispose();

                    UpdateTitle();
                }
            }
        }
  
        private IRawEditor _rawEditor = null;
        public IRawEditor RawEditor {
            get => _rawEditor;
            set {
                if (_rawEditor != value) {
                    var oldIsLoaded = IsLoaded;
                    var oldEditor = _rawEditor;

                    if (oldEditor != null) {
                        PreClosed?.Invoke(this, EventArgs.Empty);
                        oldEditor.IsModifiedChanged -= _onModifiedChangedDelegate;
                    }
                    if (value != null) {
                        PreLoaded?.Invoke(this, EventArgs.Empty);
                    }

                    _rawEditor = value;

                    if (oldEditor != null) {
                        Closed?.Invoke(this, EventArgs.Empty);
                        oldEditor.Dispose();
                    }
                    if (value != null) {
                        value.IsModifiedChanged += _onModifiedChangedDelegate;
                        Loaded?.Invoke(this, EventArgs.Empty);
                    }

                    if (IsLoaded != oldIsLoaded)
                        IsLoadedChanged?.Invoke(this, EventArgs.Empty);

                    UpdateTitle();
                }
            }
        }

        public bool IsModified {
            get => RawEditor?.IsModified ?? false;
            set {
                if (RawEditor != null)
                    RawEditor.IsModified = value;
            }
        }

        public bool IsLoaded => RawEditor != null;

        /// <summary>
        /// Editor-specific implementation of determining the title for a loaded editor.
        /// The implementation doesn't need to concern itself with details like whether the editor has been modified or not.
        /// </summary>
        protected abstract string LoadedTitle { get; }

        /// <summary>
        /// The title to display when no editor is loaded.
        /// </summary>
        protected virtual string UnloadedTitle => "";

        private string _title = null;

        public string Title {
            get => _title;
            private set {
                if (_title != value) {
                    _title = value;
                    TitleChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        protected void UpdateTitle() {
            var title = IsLoaded ? (LoadedTitle + ((RawEditor?.IsModified == true) ? "*" : "")) : UnloadedTitle;
            var editorTitle = Editor?.Title ?? "";
            if (editorTitle.Length > 0)
                title += " (" + editorTitle + ")";
            Title = title;
        }

        public string EditorTitle(string formTitle) {
            var title = Title;
            return formTitle + ((title == "") ? "" : (" - " + title));
        }

        /// <summary>
        /// Editor-specific implementation of closing.
        /// </summary>
        /// <returns>'true' if closing was successful and can continue. Otherwise, 'false'.</returns>
        protected virtual bool OnClose() => true;

        public bool Close() {
            if (!IsLoaded)
                return true;
            if (!OnClose())
                return !IsLoaded;
            Editor = null;
            RawEditor = null;
            return !IsLoaded;
        }

        public virtual void Dispose() {
            // TODO: what to do if close failed? throw or something?
            _ = Close();
            if (Editor != null) {
                Editor.Dispose();
                Editor = null;
            }
            if (RawEditor != null) {
                RawEditor.Dispose();
                RawEditor = null;
            }
        }

        public event EventHandler IsLoadedChanged;
        public event EventHandler IsModifiedChanged;
        public event EventHandler PreLoaded;
        public event EventHandler Loaded;
        public event EventHandler PreSaved;
        public event EventHandler Saved;
        public event EventHandler PreClosed;
        public event EventHandler Closed;
        public event EventHandler TitleChanged;
    }
}
