using System;
using System.Collections.Generic;
using System.Linq;

namespace SF3.Win.Views {
    public class TextArrayViewElement {
        public TextArrayViewElement(string key, string text) {
            Key = key;
            Text = text;
        }

        public string Key { get; }
        public string Text { get; }
    }

    public class TextArrayView : ArrayView<TextArrayViewElement, TextView> {
        public TextArrayView(string name, Dictionary<string, string> strings) : base(
            name,
            strings.Select(x => new TextArrayViewElement(x.Key, x.Value)).ToArray(),
            "Key",
            new TextView("Text", "")
        ) { }

        protected override void OnSelectValue(object sender, EventArgs args) {
            ElementView.Text = ((TextArrayViewElement) DropdownList.SelectedValue).Text;
        }
    }
}
