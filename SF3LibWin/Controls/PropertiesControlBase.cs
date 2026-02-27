using System;
using System.ComponentModel;
using System.Windows.Forms;
using CommonLib;
using SF3.Win.Extensions;
using static SF3.Win.Utils.EventHandlers;

namespace SF3.Win.Controls {
    public abstract class PropertiesControlBase : UserControl {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            bool wasProcessed = false;
            CmdKey?.Invoke(this, ref msg, keyData, ref wasProcessed);
            if (wasProcessed)
                return wasProcessed;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected int NonUserInputGuard { get; private set; } = 0;

        protected ScopeGuard IncrementNonUserInputGuard()
            => new ScopeGuard(() => NonUserInputGuard++, () => NonUserInputGuard--);

        protected void DoIfUserInput(Action action) {
            if (NonUserInputGuard > 0)
                return;
            action();
        }

        protected void UpdateControls() {
            if (NonUserInputGuard > 0)
                return;
            using (IncrementNonUserInputGuard())
                PerformUpdateControls();
        }

        protected abstract void PerformUpdateControls();

        public event CmdKeyEventHandler CmdKey;
    }

    public abstract class PropertiesControlBase<T> : PropertiesControlBase where T : class {
        private T _editingObject = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public T EditingObject {
            get => _editingObject;
            set {
                if (value == _editingObject)
                    return;

                Control lastFocused = null;
                if (ContainsFocus && !Focused) {
                    lastFocused = this.GetFocusedControl();
                    Focus();
                }

                _editingObject = value;
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
}
