using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Win.Utils;
using SF3.Models.Files;
using static CommonLib.Utils.ValueUtils;

namespace SF3.Editor.Forms {
    public partial class InsertDataDialog : Form {
        public InsertDataDialog(ScenarioTableFile file) {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            var discoveries = file.Discoveries?.GetAllOrdered() ?? [];

            File = file;

            InitializeComponent();
            DialogResult = DialogResult.None;

            FileEndFile  = File.Data.Length;
            FileStartRAM = File.RamAddress;
            FileEndRAM   = FileStartRAM + FileEndFile;
            LimitRAM     = File.RamAddressLimit;
            LimitFile    = LimitRAM - FileStartRAM;

            var discoveriesAfterEOF = discoveries.Where(x => x.Address >= FileEndRAM).ToArray();

            var firstDiscoveryAfterEOF = (discoveriesAfterEOF.Length >= 1) ? discoveriesAfterEOF[0] : null;
            var lastDiscoveryAfterEOF  = (discoveriesAfterEOF.Length >= 1) ? discoveriesAfterEOF[discoveriesAfterEOF.Length - 1] : null;

            FirstEOFAddrRAM            = (int?) firstDiscoveryAfterEOF?.Address;
            FirstEOFAddrFile           = FirstEOFAddrRAM.HasValue ? (FirstEOFAddrRAM.Value - FileStartRAM) : null;
            LastEOFAddrRAM             = (int?) lastDiscoveryAfterEOF?.Address;
            LastEOFAddrFile            = LastEOFAddrRAM.HasValue ? (LastEOFAddrRAM.Value - FileStartRAM) : null;
            FreeSpaceBeforePostEOFData = FirstEOFAddrRAM.HasValue ? (FirstEOFAddrRAM - FileEndRAM) : null;
            FreeSpaceBeforeLimit       = LimitRAM - (LastEOFAddrRAM ?? FileEndRAM);

            tbFileStartRam.Text = SignedHexStr(FileStartRAM, "X", withPrefix: false);
            tbFileEndRAM.Text   = SignedHexStr(FileEndRAM, "X", withPrefix: false);
            tbFileEndFile.Text  = SignedHexStr(FileEndFile, "X", withPrefix: false);
            tbLimitRAM.Text     = SignedHexStr(LimitRAM, "X", withPrefix: false);

            UpdateTextBoxes();
        }

        private void btnMove_Click(object sender, EventArgs e) {
            if (HasErrors) {
                MessageUtils.WarningMessage("Some fields have errors. Please correct them and try again.");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
            => Close();

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            if (DialogResult == DialogResult.None)
                DialogResult = DialogResult.Cancel;
        }

        private void UpdateTextBoxes() {
            bool hasErrors = false;

            void UpdateTextBox(TextBox tb, int? addr, int? min, int? max, bool enforceAlignment = false) {
                if (!addr.HasValue) {
                    tb.Text = "";
                    tb.Enabled = false;
                    tb.BackColor = SystemColors.Control;
                }
                else {
                    if (!tb.Focused)
                        tb.Text = SignedHexStr(addr.Value, "X", withPrefix: false);

                    if ((min.HasValue && addr < min.Value) || (max.HasValue && addr > max) || enforceAlignment && (addr & 0x03) != 0) {
                        tb.ForeColor = Color.Red;
                        hasErrors = true;
                    }
                    else
                        tb.ForeColor = Color.Black;
                }
            }

            var moveBy = MoveBy;

            UpdateTextBox(tbInsertAddressRAM, moveBy, null, null, enforceAlignment: true);
            UpdateTextBox(tbFirstAddrRAM, FirstEOFAddrRAM + moveBy, FileEndRAM, LimitRAM);
            UpdateTextBox(tbFirstAddrFile, FirstEOFAddrFile + moveBy, FileEndFile, LimitFile);
            UpdateTextBox(tbLastAddrRAM, LastEOFAddrRAM + moveBy, FileEndRAM, LimitRAM);
            UpdateTextBox(tbLastAddrFile, LastEOFAddrFile + moveBy, FileEndFile, LimitFile);
            UpdateTextBox(tbFreeSpaceBeforePostEOFData, FreeSpaceBeforePostEOFData + moveBy, 0, null);
            UpdateTextBox(tbFreeSpaceBeforeLimit, FreeSpaceBeforeLimit - moveBy, 0, null);

            HasErrors = hasErrors;
        }

        private bool TryFromSignedHexString(string text, out int result) {
            var outResult = FromSignedHexString(text);
            result = outResult ?? 0;
            return outResult.HasValue;
        }

        private int? FromSignedHexString(string text) {
            text = text.Trim();
            bool isNegative = false;

            if (text.Length >= 1 && text[0] == '-') {
                isNegative = true;
                text = text.Substring(1);
            }

            // Check for invalid characters.
            for (int i = 0; i < text.Length; i++) {
                var ch = text[i];
                if (!char.IsAsciiHexDigit(ch))
                    return null;
            }

            // Find the first non-zero digit. We want our number to start there.
            int firstNonZero;
            for (firstNonZero = 0; firstNonZero < text.Length; firstNonZero++)
                if (text[firstNonZero] != '0')
                    break;
            text = text.Substring(firstNonZero);

            if (text == "")
                return 0;

            if (text.Length > 8)
                return null;

            try {
                if (int.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hexValue))
                    return hexValue * (isNegative ? -1 : 1);
                else
                    return null;
            }
            catch {
                return null;
            }
        }

        private void tbMoveBy_TextChanged(object sender, EventArgs e) {
            if (tbInsertAddressRAM.Focused && TryFromSignedHexString(tbInsertAddressRAM.Text, out var value))
                MoveBy = value;
        }

        private void tbFirstAddrRAM_TextChanged(object sender, EventArgs e) {
            if (FirstEOFAddrRAM.HasValue && tbFirstAddrRAM.Focused && TryFromSignedHexString(tbFirstAddrRAM.Text, out var value))
                MoveBy = value - FirstEOFAddrRAM.Value;
        }

        private void tbFirstAddrFile_TextChanged(object sender, EventArgs e) {
            if (FirstEOFAddrFile.HasValue && tbFirstAddrFile.Focused && TryFromSignedHexString(tbFirstAddrFile.Text, out var value))
                MoveBy = value - FirstEOFAddrFile.Value;
        }

        private void tbLastAddrRAM_TextChanged(object sender, EventArgs e) {
            if (LastEOFAddrRAM.HasValue && tbLastAddrRAM.Focused && TryFromSignedHexString(tbLastAddrRAM.Text, out var value))
                MoveBy = value - LastEOFAddrRAM.Value;
        }

        private void tbLastAddrFile_TextChanged(object sender, EventArgs e) {
            if (LastEOFAddrFile.HasValue && tbLastAddrFile.Focused && TryFromSignedHexString(tbLastAddrFile.Text, out var value))
                MoveBy = value - LastEOFAddrFile.Value;
        }

        private void tbFreeSpaceBeforePostEOFData_TextChanged(object sender, EventArgs e) {
            if (FreeSpaceBeforePostEOFData.HasValue && tbFreeSpaceBeforePostEOFData.Focused && TryFromSignedHexString(tbFreeSpaceBeforePostEOFData.Text, out var value))
                MoveBy = value - FreeSpaceBeforePostEOFData.Value;
        }

        private void tbFreeSpaceBeforeLimit_TextChanged(object sender, EventArgs e) {
            if (tbFreeSpaceBeforeLimit.Focused && TryFromSignedHexString(tbFreeSpaceBeforeLimit.Text, out var value))
                MoveBy = FreeSpaceBeforeLimit - value;
        }

        public ScenarioTableFile File { get; }

        private int _moveBy = 0;

        public int MoveBy {
            get => _moveBy;
            set {
                if (_moveBy != value) {
                    _moveBy = value;
                    UpdateTextBoxes();
                }
            }
        }

        public bool HasErrors { get; private set; }

        public int FileEndFile { get; }
        public int FileStartRAM { get; }
        public int FileEndRAM { get; }
        public int LimitRAM { get; }
        public int LimitFile { get; }
        public int? FirstEOFAddrRAM { get; }
        public int? FirstEOFAddrFile { get; }
        public int? LastEOFAddrRAM { get; }
        public int? LastEOFAddrFile { get; }
        public int? FreeSpaceBeforePostEOFData { get; }
        public int FreeSpaceBeforeLimit { get; }
    }
}
