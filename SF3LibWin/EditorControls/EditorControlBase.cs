using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SF3.Win.EditorControls {
    public abstract class EditorControlBase : IEditorControl {
        public abstract Control Create();

        public virtual void Destroy() {
            if (Control == null)
                return;
            Control.Dispose();
            Control = null;
        }

        public virtual void Dispose() => Destroy();

        public Control Control { get; protected set; } = null;
    }
}
