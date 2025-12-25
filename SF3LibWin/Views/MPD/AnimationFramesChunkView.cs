using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class AnimationFramesChunkView : TabView {
        public AnimationFramesChunkView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new AnimationFramesView("Frames", Model, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
