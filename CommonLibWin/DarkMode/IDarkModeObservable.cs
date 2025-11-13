using System;

namespace CommonLib.Win.DarkMode {
    public interface IDarkModeObservable {
        event EventHandler DarkModeChanged;
        bool DarkMode { get; }
    }
}
