using System;
using CommonLib.Win.Utils;
using SF3.Models.Files;
using static SF3.Win.App.AppScene;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        public void AssignIconPointerOffsets(IIconTableFile iconOffsets, IconCollectionRegistration icons) {
            if (iconOffsets == null)
                throw new ArgumentNullException(nameof(iconOffsets));
            if (icons == null)
                throw new ArgumentNullException(nameof(icons));

            if (iconOffsets.Scenario != icons.Scenario) {
                MessageUtils.ErrorMessage($"The active icons scenario ({icons.Scenario}) does not correspond to this file's scenario ({iconOffsets.Scenario}).");
                return;
            }

            var iconCount        = icons.Icons.Length;
            var itemOffsetCount  = iconOffsets.ItemIconTable.Length;
            var spellOffsetCount = iconOffsets.SpellIconTable.Length;
            var offsetCount      = itemOffsetCount + spellOffsetCount;

            if (iconCount != offsetCount) {
                MessageUtils.ErrorMessage($"Item and spell icon offset count ({itemOffsetCount} + {spellOffsetCount} = {offsetCount}) does not equal icon count ({iconCount}).");
                return;
            }

            int iconIndex = 0;
            foreach (var itemOffset in iconOffsets.ItemIconTable)
                itemOffset.IconOffset = icons.Icons[iconIndex++].Address;
            foreach (var spellOffset in iconOffsets.SpellIconTable)
                spellOffset.IconOffset = icons.Icons[iconIndex++].Address;

            MessageUtils.InfoMessage("Offsets updated.\r\nNOTE: Make sure that the three files X011.BIN, X021.BIN, and X026.BIN are updated");
        }

        private void tsmiIconOffsets_Assign_Click(object sender, EventArgs e) {
            if (SelectedFile?.Loader?.Model is IIconTableFile && _appScene.ActiveIconCollection != null)
                AssignIconPointerOffsets((IIconTableFile) SelectedFile.Loader.Model, _appScene.ActiveIconCollection);
        }
    }
}
