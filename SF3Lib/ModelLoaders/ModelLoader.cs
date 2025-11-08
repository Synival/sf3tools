using System;
using CommonLib;
using SF3.ByteData;
using SF3.Models.Files;

namespace SF3.ModelLoaders {
    public abstract class ModelLoader : IModelLoader {
        protected ModelLoader() {
            _title = UnloadedTitle;
            _onModifiedChangedDelegate = new EventHandler((o, e) => {
                IsModifiedChanged?.Invoke(this, EventArgs.Empty);
                UpdateTitle();
            });
        }

        private readonly EventHandler _onModifiedChangedDelegate;

        /// <summary>
        /// Performs loading of a model provided. Invokes events 'PreLoaded' and 'Loaded'.
        /// Complete ownership of 'model' is transferred to the ModelLoader when this is invoked.
        /// If the model could not be used, it is immediately disposed of via Dispose().
        /// If the model is already loaded, this will return 'false'.
        /// </summary>
        /// <param name="createModel">Callback to create an model when possible.</param>
        /// <returns>'true' a new model was loaded and successfully created. Otherwise, 'false'.</returns>
        protected bool PerformLoad(Func<IByteData> createByteData, Func<IBaseFile> createModel) {
            if (createByteData == null || createModel == null || IsLoaded)
                return false;

            PreLoaded?.Invoke(this, EventArgs.Empty);

            if ((ByteData = createByteData()) == null)
                return false;
            if ((Model = createModel()) == null) {
                ByteData = null;
                return false;
            }

            Model.IsModifiedChanged += _onModifiedChangedDelegate;

            Loaded?.Invoke(this, EventArgs.Empty);
            if (!IsLoaded)
                return false;

            IsLoadedChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Performs saving of a model if one is loaded. Invokes events 'PreSaved' and 'Saved'.
        /// 'Model.IsModified' is automatically reset to 'false' upon saving.
        /// </summary>
        /// <param name="saveAction">The function to save the model when possible.</param>
        /// <returns>'true' if saveAction was invokvd and returned success.</returns>
        protected bool PerformSave(Func<bool> saveAction) {
            if (saveAction == null || ByteData == null || Model == null || !IsLoaded)
                return false;

            PreSaved?.Invoke(this, EventArgs.Empty);

            if (!Model.Finish())
                return false;
            if (!saveAction())
                return false;
            IsModified = false;
            if (IsModified == true)
                return false;

            Saved?.Invoke(this, EventArgs.Empty);
            return true;
        }

        private IBaseFile _model = null;
        public IBaseFile Model {
            get => _model;
            set {
                if (_model != value) {
                    var oldModel = _model;
                    _model = value;

                    if (oldModel != null)
                        oldModel.Dispose();

                    UpdateTitle();
                }
            }
        }

        private IByteData _byteData = null;
        public IByteData ByteData {
            get => _byteData;
            set {
                if (_byteData != value) {
                    var oldData = _byteData;
                    _byteData = value;

                    if (oldData != null)
                        oldData.Dispose();

                    UpdateTitle();
                }
            }
        }

        private int _isModifiedGuard = 0;

        public virtual bool IsModified {
            get => Model?.IsModified ?? false;
            set {
                if (_isModifiedGuard == 0 && Model != null)
                    Model.IsModified = value;
            }
        }

        public ScopeGuard IsModifiedChangeBlocker()
            => new ScopeGuard(() => _isModifiedGuard++, () => _isModifiedGuard--);

        public event EventHandler IsModifiedChanged;

        public bool IsLoaded => ByteData != null && Model != null;

        /// <summary>
        /// Model-specific implementation of determining the title for a loaded model.
        /// The implementation doesn't need to concern itself with details like whether the model has been modified or not.
        /// </summary>
        protected abstract string LoadedTitle { get; }

        /// <summary>
        /// The title to display when no model is loaded.
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
            var title = IsLoaded ? (LoadedTitle + ((ByteData?.IsModified == true) ? "*" : "")) : UnloadedTitle;
            var modelTitle = Model?.Title ?? "";
            if (modelTitle.Length > 0)
                title += " (" + modelTitle + ")";
            Title = title;
        }

        public string ModelTitle(string formTitle) {
            var title = Title;
            return formTitle + ((title == "") ? "" : (" - " + title));
        }

        /// <summary>
        /// Model-specific implementation of closing.
        /// </summary>
        /// <returns>'true' if closing was successful and can continue. Otherwise, 'false'.</returns>
        protected virtual bool OnClose() => true;

        public bool Close() {
            if (!IsLoaded)
                return true;

            PreClosed?.Invoke(this, EventArgs.Empty);

            if (!OnClose())
                return !IsLoaded;

            if (Model != null)
                Model.IsModifiedChanged -= _onModifiedChangedDelegate;

            Model = null;
            ByteData = null;

            Closed?.Invoke(this, EventArgs.Empty);

            // TODO: Shouldn't ever be possible... Maybe throw/assert here?
            if (IsLoaded)
                return false;

            IsLoadedChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public virtual void Dispose() {
            // TODO: what to do if close failed? throw or something?
            _ = Close();
            if (Model != null) {
                Model.Dispose();
                Model = null;
            }
            if (ByteData != null) {
                ByteData.Dispose();
                ByteData = null;
            }
        }

        public event EventHandler IsLoadedChanged;
        public event EventHandler PreLoaded;
        public event EventHandler Loaded;
        public event EventHandler PreSaved;
        public event EventHandler Saved;
        public event EventHandler PreClosed;
        public event EventHandler Closed;
        public event EventHandler TitleChanged;
    }
}
