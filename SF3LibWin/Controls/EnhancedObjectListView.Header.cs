using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SF3.Win.Controls {
    public partial class EnhancedObjectListView {
        public const int WM_PAINT         = 0x000F;
        public const int LVM_GETHEADER    = 0x101F;
        public const int HDM_GETITEMCOUNT = 0x1200;
        public const int HDM_GETITEMRECT  = 0x1207;

        public delegate int SUBCLASSPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, ref RECT lParam);

        [DllImport("User32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("User32")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("Comctl32.dll")]
        public static extern bool SetWindowSubclass(IntPtr hWnd, SUBCLASSPROC pfnSubclass, uint uIdSubclass, uint dwRefData);

        [DllImport("Comctl32.dll")]
        public static extern int DefSubclassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        private void SubClassHeader() {
            var headerHandle = SendMessage(this.Handle, LVM_GETHEADER, 0, IntPtr.Zero);
            if (headerHandle != IntPtr.Zero) {
                _headerSubClassProcRef = HeaderSubClassProc;
                SetWindowSubclass(headerHandle, _headerSubClassProcRef, 0, 0);
            }
        }

        private int HeaderSubClassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData) {
            switch (uMsg) {
                case WM_PAINT: {
                    DefSubclassProc(hWnd, uMsg, wParam, lParam);
                    if (!DarkModeContext.Enabled)
                        return 0;

                    var rect = new RECT();
                    GetClientRect(hWnd, out rect);
                    int itemCount = SendMessage(hWnd, HDM_GETITEMCOUNT, 0, IntPtr.Zero);

                    int width = 0;
                    RECT itemRect = new RECT();
                    for (int i = 0; i < itemCount; i++) {
                        SendMessage(hWnd, HDM_GETITEMRECT, i, ref itemRect);
                        width += (itemRect.right - itemRect.left);
                    }

                    var hDC = GetDC(hWnd);
                    using (Graphics g = Graphics.FromHdc(hDC)) {
                        Rectangle nonClientArea = new Rectangle(width, rect.top, rect.right, rect.bottom);
                        g.FillRectangle(new SolidBrush(BackColor), nonClientArea);
                    }
                    ReleaseDC(hWnd, hDC);

                    return 0;
                }

                default:
                    return DefSubclassProc(hWnd, uMsg, wParam, lParam);
            }
        }

        // Strong reference to HeaderSubClassProc so it the delegate doesn't get garbage collected
        private SUBCLASSPROC _headerSubClassProcRef = null;
    }
}
