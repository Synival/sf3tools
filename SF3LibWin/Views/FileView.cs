using System.Windows.Forms;
using SF3.Models.Files;
using SF3.Models.Files.X1;
using SF3.Win.Views.X1;

namespace SF3.Win.Views {
    public class FileView : IView {
        public FileView(string name, IBaseFile file) {
            Name = name;
            File = file;
        }

        public Control Create() {
            Destroy();

            switch (File) {
                case IX1_File x1File:
                    ActualView = new X1_View("X1_File", x1File);
                    break;

                // TODO: All supported files!
            }

            return ActualView?.Create();
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
