using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLib.Win.Utils;
using SF3.Models.Files;
using static CommonLib.Utils.ValueUtils;
using System.ComponentModel;

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

        private void btnInsert_Click(object sender, EventArgs e) {
            if (HasErrors) {
                MessageUtils.WarningMessage("Some fields have errors. Please correct them and try again.");
                return;
            }

            DialogResult = DialogResult.OK;
            DataToInsert = HexStrToByteArray(GetDataHexStr()!);

            Close();
        }

        public static byte[] HexStrToByteArray(string hex) {
            int GetHexVal(char ch) => ch - (ch < 58 ? 48 : (ch < 97 ? 55 : 87));

            var len = hex.Length / 2;
            byte[] bytes = new byte[len];

            int strPos = 0;
            for (int i = 0; i < len; ++i) {
                bytes[i] = (byte)((GetHexVal(hex[strPos]) * 0x10) + GetHexVal(hex[strPos + 1]));
                strPos += 2;
            }
        
            return bytes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
            => Close();

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            if (DialogResult == DialogResult.None)
                DialogResult = DialogResult.Cancel;
        }

        private bool UpdateTextBox(TextBox tb, int? value, int? min, int? max, bool enforceAlignment = false) {
            if (!value.HasValue) {
                tb.Text = "";
                tb.ReadOnly = true;
                tb.BackColor = SystemColors.Control;
                return true;
            }
            else {
                if (!tb.Focused)
                    tb.Text = SignedHexStr(value.Value, "X", withPrefix: false);

                if ((min.HasValue && value < min.Value) || (max.HasValue && value > max) || (enforceAlignment && (value & 0x03) != 0)) {
                    tb.ForeColor = Color.Red;
                    return false;
                }
                else {
                    tb.ForeColor = Color.Black;
                    return true;
                }
            }
        }

        private bool TryGetDataHexStr(out string result) {
            var outResult = GetDataHexStr();
            result = outResult ?? "";
            return outResult != null;
        }

        private string? GetDataHexStr() {
            var text = tbData.Text;
            var sb = new StringBuilder();

            // No half-bytes allowed!
            if (text.Length % 2 == 1)
                return null;

            // Allow only whitespace and hex chars.
            foreach (var t in text) {
#pragma warning disable CS0642 // Possible mistaken empty statement
                if ((t >= '0' && t <= '9') || (t >= 'a' && t <= 'f') || (t >= 'A' && t <= 'F'))
                    sb.Append(t);
                else if (t == ' ' || t == '\n' || t == '\r')
                    ; // do nothing
                else
                    return null;
#pragma warning restore CS0642 // Possible mistaken empty statement
            }

            return sb.ToString();
        }

        private bool ValidateData() {
            string text;
            if (!TryGetDataHexStr(out text)) {
                tbData.ForeColor = Color.Red;
                return false;
            }
            tbData.ForeColor = Color.Black;
            return UpdateTextBox(tbDataLength, text.Length / 2, 0, null, enforceAlignment: true);
        }

        private void UpdateInsertAddrTextBoxes() {
            var insertAddr = InsertAddrFile;

            InsertAddrHasErrors  = !UpdateTextBox(tbInsertAddressRAM, FileStartRAM + insertAddr, FileStartRAM, FileEndRAM);
            InsertAddrHasErrors |= !UpdateTextBox(tbInsertAddressFile, insertAddr, 0, FileEndFile);
        }

        private void UpdateSizeTextBoxes() {
            var insertAddr = InsertAddrFile;
            var insertSize = FromSignedHexString(tbDataLength.Text);

            SizeHasErrors  = !UpdateTextBox(tbFirstAddrRAM, FirstEOFAddrRAM.HasValue ? (FirstEOFAddrRAM.Value + insertSize) : null, FileEndRAM, LimitRAM);
            SizeHasErrors |= !UpdateTextBox(tbFirstAddrFile, FirstEOFAddrFile.HasValue ? (FirstEOFAddrFile.Value + insertSize) : null, FileEndFile, LimitFile);
            SizeHasErrors |= !UpdateTextBox(tbLastAddrRAM, LastEOFAddrRAM.HasValue ? (LastEOFAddrRAM.Value + insertSize) : null, FileEndRAM, LimitRAM);
            SizeHasErrors |= !UpdateTextBox(tbLastAddrFile, LastEOFAddrFile.HasValue ? (LastEOFAddrFile.Value + insertSize) : null, FileEndFile, LimitFile);
            SizeHasErrors |= !UpdateTextBox(tbFreeSpaceBeforePostEOFData, FreeSpaceBeforePostEOFData, 0, null);
            SizeHasErrors |= !UpdateTextBox(tbFreeSpaceBeforeLimit, FreeSpaceBeforeLimit - insertSize, 0, null);
            SizeHasErrors |= !UpdateTextBox(tbFileEndRAM, FileEndRAM + insertSize, null, null);
            SizeHasErrors |= !UpdateTextBox(tbFileEndFile, FileEndFile + insertSize, null, null);
        }

        private void UpdateTextBoxes() {
            UpdateInsertAddrTextBoxes();
            UpdateSizeTextBoxes();
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

        private void tbInsertAddressRAM_TextChanged(object sender, EventArgs e) {
            if (tbInsertAddressRAM.Focused) {
                InsertAddrHasErrors = !TryFromSignedHexString(tbInsertAddressRAM.Text, out var value);
                if (InsertAddrHasErrors)
                    tbInsertAddressRAM.ForeColor = Color.Red;
                else {
                    InsertAddrFile = value - FileStartRAM;
                    UpdateInsertAddrTextBoxes();
                }
            }
        }

        private void tbInsertAddressFile_TextChanged(object sender, EventArgs e) {
            if (tbInsertAddressFile.Focused) {
                InsertAddrHasErrors = !TryFromSignedHexString(tbInsertAddressFile.Text, out var value);
                if (InsertAddrHasErrors)
                    tbInsertAddressFile.ForeColor = Color.Red;
                else {
                    InsertAddrFile = value;
                    UpdateInsertAddrTextBoxes();
                }
            }
        }

        private void tbData_TextChanged(object sender, EventArgs e) {
            DataHasErrors = !ValidateData();
            if (!DataHasErrors)
                UpdateSizeTextBoxes();
        }

        public ScenarioTableFile File { get; }

        private int _insertAddrFile = 0;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int InsertAddrFile {
            get => _insertAddrFile;
            set {
                if (_insertAddrFile != value) {
                    _insertAddrFile = value;
                    UpdateTextBoxes();
                }
            }
        }

        public byte[] DataToInsert = [];

        public bool InsertAddrHasErrors { get; private set; }
        public bool DataHasErrors { get; private set; }
        public bool SizeHasErrors { get; private set; }
        public bool HasErrors => DataHasErrors || SizeHasErrors;

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
