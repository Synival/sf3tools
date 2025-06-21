using System.Linq;
using System.Windows.Forms;
using SF3.Models.Structs.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteView : TabView {

        public SpriteView(string name, Sprite model, TabAlignment tabAlignment) : base(name, tabAlignment: tabAlignment) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.Header != null) {
                if (Model.Header != null)
                    CreateChild(new DataModelView("Header", Model.Header, ngc));
                if (Model.FrameOffsetTable != null)
                    CreateChild(new TableView("Frame Offsets", Model.FrameOffsetTable, ngc));
                if (Model.AnimationOffsetTable != null)
                    CreateChild(new TableView("Animation Offsets", Model.AnimationOffsetTable, ngc));
                if (Model.FrameTable != null)
                    CreateChild(new SpriteFramesView("Frames", Model.FrameTable, ngc));
                if (Model.AnimationFrameTablesByIndex?.Count > 0) {
                    CreateChild(
                        new SpriteAnimationFramesArrayView("Animation Frames", Model.AnimationFrameTablesByIndex.Values
                            .Select(x => new SpriteAnimationWithFrames(x, Model.FrameTable))
                            .ToArray(),
                        ngc));
                }
                if (Model.AnimationTable != null)
                    CreateChild(new SpriteAnimationsView("Animations", Model.AnimationTable, ngc));
            }

            return Control;
        }

        public Sprite Model { get; }
    }
}
