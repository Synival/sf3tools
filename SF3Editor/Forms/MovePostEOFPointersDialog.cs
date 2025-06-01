using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.X1;
using static CommonLib.Utils.ValueUtils;

namespace SF3.Editor.Forms {
    public partial class MovePostEOFPointersDialog : Form {
        public MovePostEOFPointersDialog(X1_File x1File) {
            X1_File = x1File;

            InitializeComponent();
            DialogResult = DialogResult.None;

            FileEndFile   = X1_File.Data.Length;
            FileStartRAM  = (int) X1_File.RamAddress;
            FileEndRAM    = FileStartRAM + FileEndFile;
            LimitRAM      = (int) X1_File.X1RamUpperLimit;

            var discoveries            = X1_File.Discoveries.GetAllOrdered();
            var discoveriesAfterEOF    = discoveries.Where(x => x.Address >= FileEndRAM && x.Address < LimitRAM).ToArray();
            var firstDiscoveryAfterEOF = discoveriesAfterEOF.Length > 0 ? discoveriesAfterEOF[0] : null;
            var lastDiscoveryAfterEOF  = discoveriesAfterEOF.Length > 0 ? discoveriesAfterEOF[discoveriesAfterEOF.Length - 1] : null;

            FirstAddrRAM               = (int?) firstDiscoveryAfterEOF?.Address;
            FirstAddrFile              = FirstAddrRAM.HasValue ? (FirstAddrRAM.Value - FileStartRAM) : null;
            LastAddrRAM                = (int?) lastDiscoveryAfterEOF?.Address;
            LastAddrFile               = LastAddrRAM.HasValue ? (LastAddrRAM.Value - FileStartRAM) : null;
            FreeSpaceBeforePostEOFData = FirstAddrRAM.HasValue ? (FirstAddrRAM.Value - FileEndRAM) : null;
            FreeSpaceBeforeLimit       = LimitRAM - (LastAddrRAM ?? FileStartRAM);

            UpdateTextBoxes();
        }

        private void btnMove_Click(object sender, EventArgs e) {
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
            void UpdateTextBox(TextBox tb, int? addr) {
                if (!tb.Focused) {
                    if (addr.HasValue)
                        tb.Text = SignedHexStr(addr.Value, "X", withPrefix: false);
                    else {
                        tb.Text = "";
                        tb.Enabled = false;
                        tb.BackColor = SystemColors.Control;
                    }
                }
            }

            var moveBy = MoveBy;

            UpdateTextBox(tbMoveBy, moveBy);
            UpdateTextBox(tbFirstAddrRAM, FirstAddrRAM.HasValue ? (FirstAddrRAM.Value + moveBy) : null);
            UpdateTextBox(tbFirstAddrFile, FirstAddrFile.HasValue ? (FirstAddrFile.Value + moveBy) : null);
            UpdateTextBox(tbLastAddrRAM, LastAddrRAM.HasValue ? (LastAddrRAM.Value + moveBy) : null);
            UpdateTextBox(tbLastAddrFile, LastAddrFile.HasValue ? (LastAddrFile.Value + moveBy) : null);
            UpdateTextBox(tbFileStartRam, FileStartRAM);
            UpdateTextBox(tbFileEndRAM, FileEndRAM + moveBy);
            UpdateTextBox(tbFileEndFile, FileEndFile + moveBy);
            UpdateTextBox(tbFreeSpaceBeforePostEOFData, FreeSpaceBeforePostEOFData.HasValue ? (FreeSpaceBeforePostEOFData.Value + moveBy) : null);
            UpdateTextBox(tbLimitRAM, LimitRAM);
            UpdateTextBox(tbFreeSpaceBeforeLimit, FreeSpaceBeforeLimit - moveBy);
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
            if (tbMoveBy.Focused && TryFromSignedHexString(tbMoveBy.Text, out var value))
                MoveBy = value;
        }

        private void tbFirstAddrRAM_TextChanged(object sender, EventArgs e) {
            if (tbFirstAddrRAM.Focused && FirstAddrRAM.HasValue && TryFromSignedHexString(tbFirstAddrRAM.Text, out var value))
                MoveBy = value - FirstAddrRAM.Value;
        }

        private void tbFirstAddrFile_TextChanged(object sender, EventArgs e) {
            if (tbFirstAddrFile.Focused && FirstAddrFile.HasValue && TryFromSignedHexString(tbFirstAddrFile.Text, out var value))
                MoveBy = value - FirstAddrFile.Value;
        }

        private void tbLastAddrRAM_TextChanged(object sender, EventArgs e) {
            if (tbLastAddrRAM.Focused && LastAddrRAM.HasValue && TryFromSignedHexString(tbLastAddrRAM.Text, out var value))
                MoveBy = value - LastAddrRAM.Value;
        }

        private void tbLastAddrFile_TextChanged(object sender, EventArgs e) {
            if (tbLastAddrFile.Focused && LastAddrFile.HasValue && TryFromSignedHexString(tbLastAddrFile.Text, out var value))
                MoveBy = value - LastAddrFile.Value;
        }

        private void tbFileEndRAM_TextChanged(object sender, EventArgs e) {
            if (tbFileEndRAM.Focused && TryFromSignedHexString(tbFileEndRAM.Text, out var value))
                MoveBy = value - FileEndRAM;
        }

        private void tbFileEndFile_TextChanged(object sender, EventArgs e) {
            if (tbFileEndFile.Focused && TryFromSignedHexString(tbFileEndFile.Text, out var value))
                MoveBy = value - FileEndFile;
        }

        private void tbFreeSpaceBeforePostEOFData_TextChanged(object sender, EventArgs e) {
            if (tbFreeSpaceBeforePostEOFData.Focused && FreeSpaceBeforePostEOFData.HasValue && TryFromSignedHexString(tbFreeSpaceBeforePostEOFData.Text, out var value))
                MoveBy = value - FreeSpaceBeforePostEOFData.Value;
        }

        private void tbFreeSpaceBeforeLimit_TextChanged(object sender, EventArgs e) {
            if (tbFreeSpaceBeforeLimit.Focused && TryFromSignedHexString(tbFreeSpaceBeforeLimit.Text, out var value))
                MoveBy = FreeSpaceBeforeLimit - value;
        }

        public X1_File X1_File { get; }

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

        public int FileEndFile { get; }
        public int FileStartRAM { get; }
        public int FileEndRAM { get; }
        public int LimitRAM { get; }
        public int? FirstAddrRAM { get; }
        public int? FirstAddrFile { get; }
        public int? LastAddrRAM { get; }
        public int? LastAddrFile { get; }
        public int? FreeSpaceBeforePostEOFData { get; }
        public int FreeSpaceBeforeLimit { get; }
    }
}
