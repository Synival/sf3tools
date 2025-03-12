using System.Windows.Forms;
using SF3.Models.Files;
using SF3.Models.Files.IconPointer;
using SF3.Models.Files.MPD;
using SF3.Models.Files.X005;
using SF3.Models.Files.X012;
using SF3.Models.Files.X014;
using SF3.Models.Files.X1;
using SF3.Win.Views.IconPointer;
using SF3.Win.Views.MPD;
using SF3.Win.Views.X005;
using SF3.Win.Views.X012;
using SF3.Win.Views.X014;
using SF3.Win.Views.X1;

namespace SF3.Win.Views {
    public class FileView : IView {
        public FileView(string name, IBaseFile file) {
            Name = name;
            File = file;
        }

        public Control Create() {
            Destroy();
            ActualView = CreateActualView();
            return ActualView?.Create();
        }

        private IView CreateActualView() {
            switch (File) {
                case IIconPointerFile file:
                    return new IconPointerView("IconPointer_File", file);
                case IX1_File file:
                    return new X1_View("X1_File", file);
                case IX005_File file:
                    return new X005_View("X005_File", file);
                case IX012_File file:
                    return new X012_View("X012_File", file);
                case IX014_File file:
                    return new X014_View("X014_File", file);
                case IMPD_File file:
                    return new MPD_View("MPD_File", file);
                default:
                    return null;
            }
        }

        public void Destroy() {
            if (ActualView != null) {
                ActualView.Destroy();
                ActualView = null;
            }
        }

        public void RefreshContent()
            => ActualView?.RefreshContent();

        public void Dispose() {
            if (ActualView != null) {
                ActualView.Dispose();
                ActualView = null;
            }
        }

        public IBaseFile File { get; }
        public IView ActualView { get; private set; }

        public string Name { get; }
        public Control Control => ActualView?.Control;
        public bool IsCreated => ActualView?.IsCreated ?? false;
    }
}
