using System;
using System.Windows.Forms;
using CommonLib;
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
  
        public event CmdKeyEventHandler CmdKey;
    }
}
