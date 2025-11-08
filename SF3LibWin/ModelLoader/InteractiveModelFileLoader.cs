using System.Linq;
using System.Windows.Forms;
using CommonLib.Win.Utils;
using SF3.ModelLoaders;

namespace SF3.Win.ModelLoader {
    public class InteractiveModelFileLoader : ModelFileLoader {
        protected override bool OnFinished() {
            var errors = Model.GetErrors();

            if (errors.Length > 0) {
                DialogResult dialogResult = MessageBox.Show(
                    "This file has the following known errors:\r\n\r\n" +
                    string.Join("\r\n", errors.Select(x => "- " + x)) + "\r\n\r\n" +
                    "Are you sure you want to save?",
                    MessageUtils.MessageTitle(),
                    MessageBoxButtons.YesNo
                );
                if (dialogResult == DialogResult.No)
                    return false;
            }

            return true;
        }
    }
}
