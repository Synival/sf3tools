using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteView : TabView {

        public SpriteView(string name, Sprite model, INameGetterContext nameGetterContext, TabAlignment tabAlignment) : base(name, tabAlignment: tabAlignment) {
            Model = model;
            NameGetterContext = nameGetterContext;

            HeaderView = new DataModelView("Header", Model?.Header, nameGetterContext, typeof(SpriteHeader));
            FramesView = new SpriteFramesView("Frames", Model?.FrameTable, nameGetterContext);
            AnimationOffsetsView = new TableView("Animation Offsets", Model?.AnimationOffsetTable, nameGetterContext, typeof(AnimationOffset));

            AnimationFramesArrayView = new SpriteAnimationFramesArrayView(
                "Animation Frames",
                (Model?.AnimationFrameTablesByIndex?.Values?.Select(x => new SpriteAnimationFramesViewItem(Model.Header.Directions, x, Model.FrameTable))?.ToArray()) ?? [],
                nameGetterContext
            );

            AnimationsView = new SpriteAnimationsView(
                "Animations",
                new SpriteAnimationsViewContext(Model?.Header?.Directions ?? 1, Model?.AnimationTable, Model?.AnimationFrameTablesByIndex ?? [], Model?.FrameTable),
                nameGetterContext
            );
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(HeaderView);
            CreateChild(FramesView);
            CreateChild(AnimationOffsetsView);
            CreateChild(AnimationFramesArrayView);
            CreateChild(AnimationsView);

            return Control;
        }

        public Sprite Model { get; }

        public INameGetterContext NameGetterContext { get; }
        public DataModelView HeaderView { get; }
        public SpriteFramesView FramesView { get; }
        public TableView AnimationOffsetsView { get; }
        public SpriteAnimationFramesArrayView AnimationFramesArrayView { get; }
        public SpriteAnimationsView AnimationsView { get; }
    }
}
