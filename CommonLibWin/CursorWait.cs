using System;
using System.Windows.Forms;

namespace CommonLib.Win {
    public class CursorWait : IDisposable
    {
        public CursorWait() {
            if (s_cursorCount++ == 0) {
                Cursor.Current = Cursors.WaitCursor;
                Application.UseWaitCursor = true;
            }
        }

        public void Dispose() {
            if (--s_cursorCount == 0) {
                Cursor.Current = Cursors.Default;
                Application.UseWaitCursor = false;
            }
        }

        private static int s_cursorCount = 0;
    }
}
