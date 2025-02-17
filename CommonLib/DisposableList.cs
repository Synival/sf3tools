using System;
using System.Collections.Generic;

namespace CommonLib {
    /// <summary>
    /// Same as List T, T has interface "IDisposable" and the list itself is disposable.
    /// When disposed, all elements in the list are disposed as well.
    /// Can be disposed multiple times -- Dispose() just disposes items and runs Clear().<br/>
    /// NOTE: Anything removed by any method other than Dispose() won't be disposed. You'll have to do that yourself!
    /// </summary>
    /// <typeparam name="T">Type for List T.</typeparam>
    public class DisposableList<T> : List<T>, IDisposable where T : IDisposable {
        public DisposableList() { }
        public DisposableList(IEnumerable<T> collection) : base(collection) { }

        public void Dispose() {
            foreach (var element in this)
                element.Dispose();
            Clear();
        }

        ~DisposableList() {
            if (Count > 0)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": Elements are still present. Forgot Dispose()?");
        }
    }
}
