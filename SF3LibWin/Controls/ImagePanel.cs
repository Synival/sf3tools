using System;
using System.Drawing;
using System.Windows.Forms;

namespace SF3.Win.Controls {
    public partial class ImagePanel : UserControl {
        public ImagePanel() {
            InitializeComponent();
            imageControl.Resize += (s, e) => {
                Height = imageControl.Height + 26;
                Width = Math.Max(imageControl.Width + 4, MinimumSize.Width);

                btnExport.Enabled = false;
                btnImport.Enabled = false;
            };
        }

        public float ImageScale {
            get => imageControl.ImageScale;
            set => imageControl.ImageScale = value;
        }

        public Image Image {
            get => imageControl.Image;
            set => imageControl.Image = value;
        }

        private Action _importAction = null;
        public Action ImportAction {
            get => _importAction;
            set {
                _importAction = value;
                btnImport.Enabled = value != null;
            }
        }

        private Action _exportAction = null;
        public Action ExportAction {
            get => _exportAction;
            set {
                _exportAction = value;
                btnExport.Enabled = value != null;
            }
        }
    }
}
