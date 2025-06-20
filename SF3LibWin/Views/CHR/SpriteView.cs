using System.Windows.Forms;
using SF3.Models.Structs.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteView : TabView {

        public SpriteView(string name, Sprite model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.Header != null) {
                if (Model.Header != null)
                    CreateChild(new DataModelView("Header", Model.Header, ngc));
#if false
                if (Model.FrameDataOffsetsTable != null)
                    CreateChild(new TableView("Frame Data Offsets", Model.FrameDataOffsetsTable, ngc));
#endif
                if (Model.FrameTable != null)
                    CreateChild(new SpriteFramesView("Frames", Model.FrameTable, ngc));
#if false
                if (Model.AnimationOffsetsTable != null)
                    CreateChild(new TableView("Animation Offsets", Model.AnimationOffsetsTable, ngc));
                if (Model.AnimationFrameTablesByAddr?.Count > 0)
                    CreateChild(
                        new SpriteAnimationFramesArrayView("Animation Frames", Model.AnimationFrameTablesByAddr.Values
                            .Select(x => new SpriteAnimationWithFrames(x, Model.FrameTablesByFileAddr
                                .Select(y => y.Value)
                                .First(y => y.SpriteIndex == x.SpriteIndex))
                            )
                            .ToArray(),
                        ngc));
#endif
            }

            return Control;
        }

        public Sprite Model { get; }
    }
}
