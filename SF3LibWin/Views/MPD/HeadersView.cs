﻿using System.Windows.Forms;
using SF3.Editors.MPD;

namespace SF3.Win.Views.MPD {
    public class HeadersView : TabView {
        public HeadersView(string name, IMPD_Editor editor) : base(name) {
            Editor = editor;            
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Editor.NameGetterContext;
            _ = CreateChild(new TableView("Header",         Editor.Header, ngc));
            _ = CreateChild(new TableView("Chunk Header",   Editor.ChunkHeader, ngc));
            _ = CreateChild(new TableView("Offset 1 Table", Editor.Offset1Table, ngc));
            _ = CreateChild(new TableView("Offset 2 Table", Editor.Offset2Table, ngc));
            _ = CreateChild(new TableView("Offset 3 Table", Editor.Offset3Table, ngc));
            _ = CreateChild(new TableView("Offset 4 Table", Editor.Offset4Table, ngc));

            return Control;
        }

        public IMPD_Editor Editor { get; }
    }
}
