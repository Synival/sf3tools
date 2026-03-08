using System;
using CommonLib.Win.Utils;
using SF3.Models.Files;
using static SF3.Win.App.AppScene;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        public void AssignIconPointerOffsets(IItemIconTableFile? itemIconOffsets, ISpellIconTableFile? spellIconOffsets, IconCollectionRegistration icons) {
            if (itemIconOffsets == null && spellIconOffsets == null)
                throw new ArgumentException($"{nameof(itemIconOffsets)} + {nameof(spellIconOffsets)}");
            if (icons == null)
                throw new ArgumentNullException(nameof(icons));

            var scenario = itemIconOffsets?.Scenario ?? spellIconOffsets?.Scenario;
            if (scenario != icons.Scenario) {
                MessageUtils.ErrorMessage($"The active icons scenario ({icons.Scenario}) does not correspond to this file's scenario ({scenario}).");
                return;
            }

            var itemIconCount    = icons.SpellIconIndex;
            var spellIconCount   = icons.Icons.Length - icons.SpellIconIndex;
            var totalIconCount   = itemIconCount + spellIconCount;
            var itemOffsetCount  = itemIconOffsets?.ItemIconTable?.Length;
            var spellOffsetCount = spellIconOffsets?.SpellIconTable?.Length;
            var totalOffsetCount = (itemOffsetCount ?? 0) + (spellOffsetCount ?? 0);

            if (itemOffsetCount.HasValue && spellOffsetCount.HasValue && totalIconCount != totalOffsetCount) {
                MessageUtils.ErrorMessage($"Item and spell icon offset count ({itemOffsetCount} + {spellOffsetCount} = {totalOffsetCount}) does not equal icon count ({totalIconCount}).");
                return;
            }
            else if (itemOffsetCount.HasValue && itemIconCount != itemOffsetCount.Value) {
                MessageUtils.ErrorMessage($"Item offset count ({itemOffsetCount}) does not equal item icon count ({itemIconCount}).");
                return;
            }
            else if (spellOffsetCount.HasValue && spellIconCount != spellOffsetCount.Value) {
                MessageUtils.ErrorMessage($"Spell offset count ({spellOffsetCount}) does not equal spell icon count ({spellIconCount}).");
                return;
            }

            if (itemIconOffsets != null) {
                int iconIndex = 0;
                foreach (var itemOffset in itemIconOffsets.ItemIconTable)
                    itemOffset.IconOffset = icons.Icons[iconIndex++].Address;
            }

            if (spellIconOffsets != null) {
                int iconIndex = icons.SpellIconIndex;
                foreach (var spellOffset in spellIconOffsets.SpellIconTable)
                    spellOffset.IconOffset = icons.Icons[iconIndex++].Address;
            }

            MessageUtils.InfoMessage("Offsets updated.\r\nNOTE: Make sure that the three files X011.BIN, X021.BIN, X026.BIN, and X032.BIN are all updated");
        }

        private void tsmiIconOffsets_Assign_Click(object sender, EventArgs e) {
            var itemOffsetFile  = (SelectedFile?.Loader?.Model as IItemIconTableFile);
            var spellOffsetFile = (SelectedFile?.Loader?.Model as ISpellIconTableFile);

            if ((itemOffsetFile != null || spellOffsetFile != null) && _appScene.ActiveIconCollection != null)
                AssignIconPointerOffsets(itemOffsetFile, spellOffsetFile, _appScene.ActiveIconCollection);
        }
    }
}
