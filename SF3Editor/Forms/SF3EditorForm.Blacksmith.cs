using System;
using System.Linq;
using SF3.Models.Files;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void tsmiBlacksmith_Sort_Click(object sender, EventArgs e) {
            var btf = SelectedFile?.Loader?.Model as IBlacksmithTableFile;
            if (btf?.BlacksmithTables?.Any() == true) {
                foreach (var blacksmithTable in btf.BlacksmithTables)
                    blacksmithTable.SortByMaterialAndItemType();
                SelectedFile!.View.RefreshContent();
            }
        }
    }
}
