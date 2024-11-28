﻿using System.Windows.Forms;

namespace SF3.Win.Views {
    public abstract class ControlView<T> : ViewBase where T : Control, new() {
        public ControlView(string name) : base(name) {
        }

        public override Control Create() {
            Control = new T();
            Control.Name = Name;
            return Control;
        }
    }
}
