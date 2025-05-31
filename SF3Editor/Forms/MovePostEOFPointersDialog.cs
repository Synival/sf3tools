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

            FileLength   = X1_File.Data.Length;
            StartRAM = (int) X1_File.RamAddress;
            EndRAM   = StartRAM + FileLength;
            LimitRAM    = (int) X1_File.X1RamUpperLimit;

            var discoveries            = X1_File.Discoveries.GetAllOrdered();
            var discoveriesAfterEOF    = discoveries.Where(x => x.Address >= EndRAM && x.Address < LimitRAM).ToArray();
            var firstDiscoveryAfterEOF = discoveriesAfterEOF.Length > 0 ? discoveriesAfterEOF[0] : null;
            var lastDiscoveryAfterEOF  = discoveriesAfterEOF.Length > 0 ? discoveriesAfterEOF[discoveriesAfterEOF.Length - 1] : null;

            FirstAddrRAM  = (int?) firstDiscoveryAfterEOF?.Address;
            FirstAddrFile = FirstAddrRAM.HasValue ? (FirstAddrRAM.Value - StartRAM) : null;

            LastAddrRAM  = (int?) lastDiscoveryAfterEOF?.Address;
            LastAddrFile = LastAddrRAM.HasValue ? (LastAddrRAM.Value - StartRAM) : null;

            FreeSpaceBeforePostEOFData = FirstAddrRAM.HasValue ? (FirstAddrRAM.Value - EndRAM) : null;
            FreeSpaceBeforeLimit = LimitRAM - (LastAddrRAM ?? StartRAM);

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
            var moveBy = MoveBy;

            string TextBoxStr(int addr)
                => SignedHexStr(addr, "X", withPrefix: false);

            void UpdateControlIfNotFocused(TextBox tb, int addr) {
                if (!tb.Focused)
                    tb.Text = TextBoxStr(addr);
            }

            UpdateControlIfNotFocused(tbMoveByHex, moveBy);

            if (FirstAddrRAM.HasValue && FirstAddrFile.HasValue) {
                UpdateControlIfNotFocused(tbFirstAddrRam, FirstAddrRAM.Value + moveBy);
                UpdateControlIfNotFocused(tbFirstAddrFile, FirstAddrFile.Value + moveBy);
            }
            else {
                tbFirstAddrRam.Text = "";
                tbFirstAddrRam.Enabled = false;
                tbFirstAddrRam.BackColor = SystemColors.Control;
                tbFirstAddrFile.Text = "";
                tbFirstAddrFile.Enabled = false;
                tbFirstAddrFile.BackColor = SystemColors.Control;
            }

            if (LastAddrRAM.HasValue && LastAddrFile.HasValue) {
                UpdateControlIfNotFocused(tbLastAddrRam, LastAddrRAM.Value + moveBy);
                UpdateControlIfNotFocused(tbLastAddrFile, LastAddrFile.Value + moveBy);
            }
            else {
                tbLastAddrRam.Text = "";
                tbLastAddrRam.Enabled = false;
                tbLastAddrRam.BackColor = SystemColors.Control;
                tbLastAddrFile.Text = "";
                tbLastAddrFile.Enabled = false;
                tbLastAddrFile.BackColor = SystemColors.Control;
            }

            UpdateControlIfNotFocused(tbFileStartRam, StartRAM);
            UpdateControlIfNotFocused(tbFileEndRam, EndRAM + moveBy);
            UpdateControlIfNotFocused(tbFileEndFile, FileLength + moveBy);

            if (FreeSpaceBeforePostEOFData.HasValue)
                UpdateControlIfNotFocused(tbFreeSpace, FreeSpaceBeforePostEOFData.Value + moveBy);
            else {
                tbFreeSpace.Text = "";
                tbFreeSpace.Enabled = false;
                tbFreeSpace.BackColor = SystemColors.Control;
            }

            UpdateControlIfNotFocused(tbX1Limit, LimitRAM);
            UpdateControlIfNotFocused(tbFreeSpaceBeforeLimit, FreeSpaceBeforeLimit - moveBy);
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

        private void tbMoveByHex_TextChanged(object sender, EventArgs e) {
            if (tbMoveByHex.Focused && TryFromSignedHexString(tbMoveByHex.Text, out var value))
                MoveBy = value;
        }

        private void tbFirstAddrRam_TextChanged(object sender, EventArgs e) {
            if (tbFirstAddrRam.Focused && FirstAddrRAM.HasValue && TryFromSignedHexString(tbFirstAddrRam.Text, out var value))
                MoveBy = value - FirstAddrRAM.Value;
        }

        private void tbFirstAddrFile_TextChanged(object sender, EventArgs e) {
            if (tbFirstAddrFile.Focused && FirstAddrFile.HasValue && TryFromSignedHexString(tbFirstAddrFile.Text, out var value))
                MoveBy = value - FirstAddrFile.Value;
        }

        private void tbLastAddrRam_TextChanged(object sender, EventArgs e) {
            if (tbLastAddrRam.Focused && LastAddrRAM.HasValue && TryFromSignedHexString(tbLastAddrRam.Text, out var value))
                MoveBy = value - LastAddrRAM.Value;
        }

        private void tbLastAddrFile_TextChanged(object sender, EventArgs e) {
            if (tbLastAddrFile.Focused && LastAddrFile.HasValue && TryFromSignedHexString(tbLastAddrFile.Text, out var value))
                MoveBy = value - LastAddrFile.Value;
        }

        private void tbFileEndRam_TextChanged(object sender, EventArgs e) {
            if (tbFileEndRam.Focused && TryFromSignedHexString(tbFileEndRam.Text, out var value))
                MoveBy = value - EndRAM;
        }

        private void tbFileEndFile_TextChanged(object sender, EventArgs e) {
            if (tbFileEndFile.Focused && TryFromSignedHexString(tbFileEndFile.Text, out var value))
                MoveBy = value - FileLength;
        }

        private void tbFreeSpace_TextChanged(object sender, EventArgs e) {
            if (tbFreeSpace.Focused && FreeSpaceBeforePostEOFData.HasValue && TryFromSignedHexString(tbFreeSpace.Text, out var value))
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

        public int FileLength { get; }
        public int StartRAM { get; }
        public int EndRAM { get; }
        public int LimitRAM { get; }
        public int? FirstAddrRAM { get; }
        public int? FirstAddrFile { get; }
        public int? LastAddrRAM { get; }
        public int? LastAddrFile { get; }
        public int? FreeSpaceBeforePostEOFData { get; }
        public int FreeSpaceBeforeLimit { get; }
    }
}
