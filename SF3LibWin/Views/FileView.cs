using System.Windows.Forms;
using SF3.Models.Files;
using SF3.Models.Files.IconPointer;
using SF3.Models.Files.MPD;
using SF3.Models.Files.X002;
using SF3.Models.Files.X005;
using SF3.Models.Files.X012;
using SF3.Models.Files.X013;
using SF3.Models.Files.X014;
using SF3.Models.Files.X019;
using SF3.Models.Files.X033_X031;
using SF3.Models.Files.X1;
using SF3.Win.Views.IconPointer;
using SF3.Win.Views.MPD;
using SF3.Win.Views.X002;
using SF3.Win.Views.X005;
using SF3.Win.Views.X012;
using SF3.Win.Views.X013;
using SF3.Win.Views.X014;
using SF3.Win.Views.X019;
using SF3.Win.Views.X033_X031;
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
            if (ActualView != null && ActualView is TabView tabView)
                tabView.TabAlignment = TabAlignment.Left;

            return ActualView?.Create();
        }

        private IView CreateActualView() {
            switch (File) {
                case IIconPointerFile file:
                    return new IconPointerView("IconPointer_File", file);
                case IX1_File file:
                    return new X1_View("X1_File", file);
                case IX002_File file:
                    return new X002_View("X002_File", file);
                case IX005_File file:
                    return new X005_View("X005_File", file);
                case IX012_File file:
                    return new X012_View("X012_File", file);
                case IX013_File file:
                    return new X013_View("X013_File", file);
                case IX014_File file:
                    return new X014_View("X014_File", file);
                case IX019_File file:
                    return new X019_View("X019_File", file);
                case IX033_X031_File file:
                    return new X033_X031_View("X033_X031_File", file);
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
