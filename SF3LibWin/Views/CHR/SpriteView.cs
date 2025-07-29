using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteView : TabView {

        public SpriteView(string name, Sprite model, INameGetterContext nameGetterContext, TabAlignment tabAlignment) : base(name, tabAlignment: tabAlignment) {
            Sprite = model;
            NameGetterContext = nameGetterContext;

            HeaderView = new DataModelView("Header", Sprite?.Header, nameGetterContext, typeof(SpriteHeader));
            FramesView = new SpriteFramesView("Frames", Sprite?.FrameTable, nameGetterContext);
            AnimationOffsetsView = new TableView("Animation Offsets", Sprite?.AnimationOffsetTable, nameGetterContext, typeof(AnimationOffset));

            AnimationCommandsArrayView = new SpriteAnimationCommandsArrayView(
                "Animation Commands",
                (Sprite?.AnimationCommandTablesByIndex?.Values?.Select(x => new SpriteAnimationCommandsViewContext(x, Sprite.FrameTable))?.ToArray()) ?? [],
                nameGetterContext
            );

            AnimationsView = new SpriteAnimationsView(
                "Animations",
                new SpriteAnimationsViewContext(Sprite?.Header?.Directions ?? 1, Sprite?.AnimationTable, Sprite?.AnimationCommandTablesByIndex ?? [], Sprite?.FrameTable),
                nameGetterContext
            );
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(HeaderView);
            CreateChild(FramesView);
            CreateChild(AnimationOffsetsView);
            CreateChild(AnimationCommandsArrayView);
            CreateChild(AnimationsView);

            return Control;
        }

        private Sprite _sprite = null;
        public Sprite Sprite {
            get => _sprite;
            set {
                if (_sprite != value) {
                    _sprite = value;

                    HeaderView.Model = _sprite?.Header;
                    FramesView.Table = _sprite?.FrameTable;
                    AnimationOffsetsView.Table = _sprite?.AnimationOffsetTable;
                    AnimationCommandsArrayView.Elements = (Sprite?.AnimationCommandTablesByIndex?.Values?.Select(x => new SpriteAnimationCommandsViewContext(x, Sprite.FrameTable))?.ToArray()) ?? [];
                    AnimationsView.Context = new SpriteAnimationsViewContext(Sprite?.Header?.Directions ?? 1, Sprite?.AnimationTable, Sprite?.AnimationCommandTablesByIndex ?? [], Sprite?.FrameTable);
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }
        public DataModelView HeaderView { get; }
        public SpriteFramesView FramesView { get; }
        public TableView AnimationOffsetsView { get; }
        public SpriteAnimationCommandsArrayView AnimationCommandsArrayView { get; }
        public SpriteAnimationsView AnimationsView { get; }
    }
}
