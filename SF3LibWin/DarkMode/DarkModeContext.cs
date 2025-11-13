using System;

namespace SF3.Win.DarkMode {
    /// <summary>
    /// Base class for anything that can react to the enabling or disabling of dark mode.
    /// To use, create and initialize in OnHandleCreated() of the control or form.
    /// OnInit(), OnDarkModeEnabled(), or OnDarkModeDisabled() can be overridden for customized behavior for controls or anything else.
    /// Alternatively, you can react to the event EnabledChanged to apply changes based on the dark mode state.
    /// </summary>
    public abstract class DarkModeContext : IDisposable {
        ~DarkModeContext() {
            Dispose(false);
        }

        /// <summary>
        /// Initializes the dark mode context by running OnInit(), setting up event handlers to react to dark mode state changes,
        /// and turning dark mode on if it is the current state.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if the DarkModeContext is aleady initialized.</exception>
        public void Init() {
            if (IsInitialized)
                throw new InvalidOperationException("Dark mode already initialized");
            IsInitialized = true;

            var appState = AppState.RetrieveAppState();
            appState.DarkModeChanged += ToggleDarkModeHandler;

            OnInit();

            Enabled = appState.DarkMode;
        }

        private void ToggleDarkModeHandler(object sender, EventArgs args)
             => Enabled = AppState.RetrieveAppState().DarkMode;

        /// <summary>
        /// Run during OnInit(), after setting up events but before turning dark mode on if necessary.
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        /// Run when dark mode is enabled.
        /// </summary>
        protected abstract void OnDarkModeEnabled();

        /// <summary>
        /// Run when dark mode is disabled.
        /// </summary>
        protected abstract void OnDarkModeDisabled();

        private bool _enabled = false;
        /// <summary>
        /// When set, dark mode is either enabled or disabled by setting an internal state,
        /// running either OnDarkModeEnabled() or OnDarkModeDisabled(), and invoking the EnabledChanged event.
        /// </summary>
        public bool Enabled {
            get => _enabled;
            set {
                if (_enabled != value) {
                    _enabled = value;
                    if (_enabled)
                        OnDarkModeEnabled();
                    else
                        OnDarkModeDisabled();
                    EnabledChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Returns 'true' if Init() has been run, otherwise 'false'.
        /// </summary>
        public bool IsInitialized { get; private set; } = false;

        private bool _disposed;
        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    AppState.RetrieveAppState().DarkModeChanged -= ToggleDarkModeHandler;
                    OnDispose();
                }
                _disposed = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDispose() { }

        /// <summary>
        /// Invokes when Enabled is changed.
        /// </summary>
        public event EventHandler EnabledChanged;
    }
}
