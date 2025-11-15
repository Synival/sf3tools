using System;

namespace CommonLib.Win.DarkMode {
    /// <summary>
    /// Container for dark mode settings that are reactable.
    /// </summary>
    public class StandaloneDarkModeObservable : IDarkModeObservable {
        public StandaloneDarkModeObservable(bool state = false) {
            _darkMode = state;
        }

        private bool _darkMode = false;
        public bool DarkMode {
            get => _darkMode;
            set {
                if (_darkMode != value) {
                    _darkMode = value;
                    DarkModeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler DarkModeChanged;
    }
}
