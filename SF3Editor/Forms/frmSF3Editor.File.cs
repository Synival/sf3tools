using System;
using SF3.Types;

namespace SF3.Editor.Forms {
    public partial class frmSF3Editor {
        private void tsmiFile_Open_Click(object sender, EventArgs e)
            => OpenFileDialog();

        private void tsmiFile_OpenScenario_Detect_Click(object sender, EventArgs e)
            => OpenScenario = null;

        private void tsmiFile_OpenScenario_Scenario1_Click(object sender, EventArgs e)
            => OpenScenario = ScenarioType.Scenario1;

        private void tsmiFile_OpenScenario_Scenario2_Click(object sender, EventArgs e)
            => OpenScenario = ScenarioType.Scenario2;

        private void tsmiFile_OpenScenario_Scenario3_Click(object sender, EventArgs e)
            => OpenScenario = ScenarioType.Scenario3;

        private void tsmiFile_OpenScenario_PremiumDisk_Click(object sender, EventArgs e)
            => OpenScenario = ScenarioType.PremiumDisk;

        private void tsmiFile_Save_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                _ = SaveFile(_selectedFile);
        }

        private void tsmiFile_SaveAs_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                _ = SaveFileAsDialog(_selectedFile);
        }

        private void tsmiFile_Close_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                _ = CloseFile(_selectedFile);
        }

        private void tsmiFile_SwapToPrev_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                _ = SwapToPrevOfSameTypeInFolder(_selectedFile);
        }

        private void tsmiFile_SwapToNext_Click(object sender, EventArgs e) {
            if (_selectedFile != null)
                _ = SwapToNextOfSameTypeInFolder(_selectedFile);
        }

        private void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();
    }
}
