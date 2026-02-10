using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Arrays;
using CommonLib.Extensions;
using SF3.Models.Files;
using SF3.Models.Files.X002;
using SF3.Models.Tables.Shared;
using SF3.NamedValues;
using SF3.Types;
using SF3.Utils;
using static CommonLib.Win.Utils.MessageUtils;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void tsmiMonsters_UnapplyMonsterEq_Click(object sender, EventArgs e) {
            var mtf = SelectedFile?.Loader?.Model as IMonsterTableFile;
            if (mtf?.MonsterTables?.Any() == true && SelectedFile != null && SelectedFile.Scenario.HasValue)
                if (ApplyMonsterEquipmentStatsDialog(mtf.MonsterTables, SelectedFile.Scenario.Value, /* apply */ false))
                    SelectedFile.View.RefreshContent();
        }

        private void tsmiMonsters_ApplyMonsterEq_Click(object sender, EventArgs e) {
            var mtf = SelectedFile?.Loader?.Model as IMonsterTableFile;
            if (mtf?.MonsterTables?.Any() == true && SelectedFile != null && SelectedFile.Scenario.HasValue)
                if (ApplyMonsterEquipmentStatsDialog(mtf.MonsterTables, SelectedFile.Scenario.Value, /* apply */ true))
                    SelectedFile.View.RefreshContent();
        }

        /// <summary>
        /// Prompts the user for an X002 file and, if found, attempts to apply/unapply stat changes in the file's ItemTable to all monsters in s MonsterTable.
        /// </summary>
        /// <param name="monsterTables">The tables with monsters to be affected.</param>
        /// <param name="scenario">The scenario to which the X002 file should belong.</param>
        /// <param name="apply">When true, stat changes are applied. When false, stat changes are unapplied.</param>
        /// <returns>Returns 'true' if successful and not cancelled, otherwise 'false'.</returns>
        public bool ApplyMonsterEquipmentStatsDialog(IEnumerable<MonsterTable> monsterTables, ScenarioType scenario, bool apply) {
            if (monsterTables == null || !monsterTables.Any())
                return false;

            // Fetch a filename for an X002 file, allowing for 'Cancel'.
            var openFileDialog = new OpenFileDialog {
                Title = "Select X002.BIN File with Items",
                Filter = FileUtils.GetFileDialogFilterForFileType(SF3FileType.X002)
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return false;
            var x002Filename = openFileDialog.FileName;

            // Attempt to open the X002 file.
            X002_File? x002File = null;
            try {
                var data = new ByteData.ByteData(new ByteArray(File.ReadAllBytes(x002Filename)));
                x002File = X002_File.Create(data, new NameGetterContext(scenario), scenario);
            }
            catch (Exception e) {
                ErrorMessage($"Couldn't open '{x002Filename}':\r\n\r\n{e.GetTypeAndMessage()}");
                return false;
            }

            // Paranoid error checking.
            if (x002File?.ItemTable == null) {
                ErrorMessage($"No items found in '{x002Filename}'.");
                return false;
            }

            // Try to apply items.
            var monstersAffected = 0;
            var itemsApplied = 0;
            try {
                foreach (var monsterTable in monsterTables)
                    (monstersAffected, itemsApplied) = monsterTable.ApplyEquipmentStats(x002File.ItemTable, apply);
            }
            catch (Exception e) {
                ErrorMessage($"Couldn't apply items:\n\r\n{e.GetTypeAndMessage()}");
                return false;
            }

            // Handy reports.
            var appliedString = apply ? "applied" : "unapplied";
            InfoMessage(
                $"Equipment stats {appliedString} successfully.\r\n" +
                $"{monstersAffected} monster(s) affected.\r\n" +
                $"{itemsApplied} total item(s) {appliedString}."
            );

            return true;
        }
    }
}
