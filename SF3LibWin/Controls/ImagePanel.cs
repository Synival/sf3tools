using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CommonLib.Win.Utils;
using SF3.Win.DarkMode;

namespace SF3.Win.Controls {
    public partial class ImagePanel : UserControl {
        public ImagePanel() {
            InitializeComponent();
            imageControl.Resize += (s, e) => {
                Height = imageControl.Height + 26;
                Width = Math.Max(imageControl.Width + 4, MinimumSize.Width);
            };

            btnExport.Enabled = false;
            btnImport.Enabled = false;
        }

        protected override void CreateHandle() {
            base.CreateHandle();
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<ImagePanel>(this);
                DarkModeContext.Init();
                DarkModeContext.OriginalBackColor = SystemColors.Control;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float ImageScale {
            get => imageControl.ImageScale;
            set => imageControl.ImageScale = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image Image {
            get => imageControl.Image;
            set => imageControl.Image = value;
        }

        private Action _importAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action ImportAction {
            get => _importAction;
            set {
                _importAction = value;
                btnImport.Enabled = value != null;
            }
        }

        private Action _exportAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action ExportAction {
            get => _exportAction;
            set {
                _exportAction = value;
                btnExport.Enabled = value != null;
            }
        }

        private void btnExport_Click(object sender, EventArgs e) {
            try {
                ExportAction?.Invoke();
            }
            catch (Exception ex) {
                MessageUtils.ErrorMessage("Couldn't export image", ex);
            }
        }

        private void btnImport_Click(object sender, EventArgs e) {
            try {
                ImportAction?.Invoke();
            }
            catch (Exception ex) {
                MessageUtils.ErrorMessage("Couldn't import image", ex);
            }
        }

        private DarkModeControlContext<ImagePanel> DarkModeContext { get; set; }
    }
}
