using System;

namespace SF3.Editor {
    public static class Globals {
        private static bool _useDropdowns = true;
        public static bool UseDropdowns {
            get => _useDropdowns;
            set {
                if (value != _useDropdowns) {
                    _useDropdowns = value;
                    UseDropdownsChanged(null, EventArgs.Empty);
                }
            }
        }

        public static event EventHandler UseDropdownsChanged;
    }
}
