using System;
using System.Drawing;
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
            int x1Length   = X1_File.Data.Length;
            int x1StartRAM = (int) X1_File.RamAddress;
            int x1EndRAM   = x1StartRAM + x1Length;
            int x1Limit    = (int) X1_File.X1RamUpperLimit;

            var discoveries = X1_File.Discoveries.GetAllOrdered();

            var discoveriesAfterEOF    = discoveries.Where(x => x.Address >= x1EndRAM && x.Address < x1Limit).ToArray();
            var firstDiscoveryAfterEOF = discoveriesAfterEOF.Length > 0 ? discoveriesAfterEOF[0] : null;
            var lastDiscoveryAfterEOF  = discoveriesAfterEOF.Length > 0 ? discoveriesAfterEOF[discoveriesAfterEOF.Length - 1] : null;

            tbMoveByHex.Text = SignedHexStr(MoveBy, "X", withPrefix: false);

            string TextBoxStr(int addr)
                => SignedHexStr(addr, "X", withPrefix: false);

            if (firstDiscoveryAfterEOF != null) {
                var addr = (int) firstDiscoveryAfterEOF.Address;
                tbFirstAddrRam.Text = TextBoxStr(addr);
                tbFirstAddrFile.Text = TextBoxStr(addr - x1StartRAM);
            }
            else {
                tbFirstAddrRam.Enabled = false;
                tbFirstAddrRam.BackColor = SystemColors.Control;
                tbFirstAddrFile.Enabled = false;
                tbFirstAddrFile.BackColor = SystemColors.Control;
            }

            if (lastDiscoveryAfterEOF != null) {
                var addr = (int) lastDiscoveryAfterEOF.Address;
                tbLastAddrRam.Text = TextBoxStr(addr);
                tbLastAddrFile.Text = TextBoxStr(addr - x1StartRAM);
            }
            else {
                tbLastAddrRam.Enabled = false;
                tbLastAddrRam.BackColor = SystemColors.Control;
                tbLastAddrFile.Enabled = false;
                tbLastAddrFile.BackColor = SystemColors.Control;
            }

            tbFileStartRam.Text = TextBoxStr(x1StartRAM);
            tbFileEndRam.Text   = TextBoxStr(x1EndRAM);
            tbFileEndFile.Text  = TextBoxStr(x1Length);

            tbFreeSpace.Text    = TextBoxStr(((int?) firstDiscoveryAfterEOF?.Address ?? x1Limit) - x1EndRAM);
        }

        public X1_File X1_File { get; }
        public int MoveBy { get; private set; }
    }
}
