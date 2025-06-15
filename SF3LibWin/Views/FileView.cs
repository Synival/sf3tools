using System.Windows.Forms;
using SF3.Models.Files;
using SF3.Models.Files.CHR;
using SF3.Models.Files.MPD;
using SF3.Models.Files.X002;
using SF3.Models.Files.X005;
using SF3.Models.Files.X011;
using SF3.Models.Files.X012;
using SF3.Models.Files.X013;
using SF3.Models.Files.X014;
using SF3.Models.Files.X019;
using SF3.Models.Files.X021;
using SF3.Models.Files.X023;
using SF3.Models.Files.X024;
using SF3.Models.Files.X026;
using SF3.Models.Files.X027;
using SF3.Models.Files.X031;
using SF3.Models.Files.X033;
using SF3.Models.Files.X044;
using SF3.Models.Files.X1;
using SF3.Win.Views.CHR;
using SF3.Win.Views.MPD;
using SF3.Win.Views.X002;
using SF3.Win.Views.X005;
using SF3.Win.Views.X011;
using SF3.Win.Views.X012;
using SF3.Win.Views.X013;
using SF3.Win.Views.X014;
using SF3.Win.Views.X019;
using SF3.Win.Views.X021;
using SF3.Win.Views.X023;
using SF3.Win.Views.X024;
using SF3.Win.Views.X026;
using SF3.Win.Views.X027;
using SF3.Win.Views.X031;
using SF3.Win.Views.X033;
using SF3.Win.Views.X044;
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
                case IX1_File   file: return new X1_View  ("X1_File",   file);
                case IX002_File file: return new X002_View("X002_File", file);
                case IX005_File file: return new X005_View("X005_File", file);
                case IX011_File file: return new X011_View("X011_File", file);
                case IX012_File file: return new X012_View("X012_File", file);
                case IX013_File file: return new X013_View("X013_File", file);
                case IX014_File file: return new X014_View("X014_File", file);
                case IX019_File file: return new X019_View("X019_File", file);
                case IX021_File file: return new X021_View("X021_File", file);
                case IX023_File file: return new X023_View("X023_File", file);
                case IX024_File file: return new X024_View("X024_File", file);
                case IX026_File file: return new X026_View("X026_File", file);
                case IX027_File file: return new X027_View("X027_File", file);
                case IX031_File file: return new X031_View("X031_File", file);
                case IX033_File file: return new X033_View("X033_File", file);
                case IX044_File file: return new X044_View("X044_File", file);
                case IMPD_File  file: return new MPD_View ("MPD_File",  file);
                case ICHR_File  file: return new CHR_View ("CHR_File",  file);
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
