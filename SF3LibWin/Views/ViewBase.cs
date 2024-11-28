using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SF3.Win.Views {
    public abstract class ViewBase : IView {
        protected ViewBase(string name) {
            Name = name;
        }

        public abstract Control Create();

        public virtual void Destroy() {
            if (Control != null) {
                Control.Parent = null;
                Control.Dispose();
                Control = null;
            }
        }

        public virtual void Dispose() => Destroy();

        public abstract void RefreshContent();

        public string Name { get; }
        public Control Control { get; protected set; } = null;
    }
}
