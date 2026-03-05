using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonLib;
using SF3.Models.Structs.X1.Battle;
using SF3.Models.Structs.X1.Town;
using SF3.MPD.Interfaces;
using SF3.Win.Extensions;
using static SF3.Win.Utils.EventHandlers;

namespace SF3.Win.Controls {
    public class PropertiesControlBase : UserControl {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            bool wasProcessed = false;
            CmdKey?.Invoke(this, ref msg, keyData, ref wasProcessed);
            if (wasProcessed)
                return wasProcessed;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private int _doOnlyDirectlyGuard = 0;
        protected void DoOnlyDirectly(Action action) {
            if (_doOnlyDirectlyGuard > 0)
                return;
            using (new ScopeGuard(() => _doOnlyDirectlyGuard++, () => _doOnlyDirectlyGuard--))
                action();
        }

        protected void UpdateControls()
            => DoOnlyDirectly(PerformUpdateControls);

        protected virtual void PerformUpdateControls() => throw new NotImplementedException();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        protected void RecursivelyAttachedEventsToControls(Control control) {
            foreach (var cObj in control.Controls) {
                if (cObj is not Control c)
                    continue;

                // Pressing 'Enter' should move on to the next control.
                c.KeyUp += (s, e) => {
                    if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                        SelectNextControl(c, true, true, true, true);
                };

                // Selecting the control should highlight all text.
                if (c is NumericUpDown nud) {
                    // The NumericUpDown is frustrating. The 'enter' event *on tab* will select all the text, but not when clicking it.
                    // The selection is probably overridden on click for some reason. So: select text always, and on MouseDown (which
                    // happens after 'Enter'), select text again. If the 'KeyUp' event is received (which happens after 'Tab'), disregard
                    // the '_nudSelectAll[nud]' flag.
                    nud.Enter += (s, e) => {
                        _nudSelectAll[nud] = true;
                        nud.Select(0, nud.Text.Length);
                    };
                    nud.KeyUp += (s, e) => _nudSelectAll[nud] = false;
                    nud.MouseDown += (s, e) => {
                        if (_nudSelectAll.TryGetValue(nud, out bool doSelectAll)) {
                            if (doSelectAll) {
                                nud.Select(0, nud.Text.Length);
                                _nudSelectAll[nud] = false;
                            }
                        }
                    };
                }
                else if (c is TextBox tb)
                    tb.Enter += (s, e) => tb.SelectAll();
                else if (c is ComboBox cb) {
                    cb.Enter += (s, e) => cb.Select(0, cb.Text.Length);

                    // Hit 'enter' before leaving a combo box to select whatever item the highlighted text was referring to
                    cb.LostFocus += (s, e) => {
                        const int WM_KEYDOWN = 0x0100;
                        const int VK_RETURN  = 0x0D;
                        SendMessage(cb.Handle, WM_KEYDOWN, VK_RETURN, 0);
                    };
                }
                else
                    RecursivelyAttachedEventsToControls(c);
            }
        }

        protected void SetNudValueAndText(NumericUpDown nud, decimal value) {
            nud.Value = value;
            nud.Text = (nud.Hexadecimal) ? ((int) value).ToString("X") : value.ToString();
        }

        public MPD_ViewerControl Viewer { get; set; }

        private Dictionary<NumericUpDown, bool> _nudSelectAll = [];

        public event CmdKeyEventHandler CmdKey;
    }

    public class PropertiesControlBase<T> : PropertiesControlBase where T : class {
        private T[] _editingObjects = [];
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public T[] EditingObjects {
            get => _editingObjects;
            set {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (Enumerable.SequenceEqual(value, _editingObjects))
                    return;

                Control lastFocused = null;
                if (ContainsFocus && !Focused) {
                    lastFocused = this.GetFocusedControl();
                    Focus();
                }

                _editingObjects = value;
                UpdateControls();

                if (lastFocused != null) {
                    if (lastFocused is NumericUpDown nud)
                        nud.Select(0, nud.Text.Length);
                    else if (lastFocused is TextBox tb)
                        tb.SelectAll();
                    else if (lastFocused is ComboBox cb)
                        cb.Select(0, cb.Text.Length);
                }
            }
        }
    }

    public class SurfaceTilePropertiesControlBase   : PropertiesControlBase<IMPD_SurfaceTile> {}
    public class ActorBattlePropertiesControlBase   : PropertiesControlBase<Slot> {}
    public class ActorNPCPropertiesControlBase      : PropertiesControlBase<Npc> {}
    public class ModelInstancePropertiesControlBase : PropertiesControlBase<IMPD_ModelInstance> {}
}
