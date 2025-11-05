using System;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;
using SF3.Win.Extensions;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationCommandsViewContext {
        public SpriteAnimationCommandsViewContext(AnimationCommandTable animationCommandTable, FrameTable frameTable) {
            AnimationCommandTable = animationCommandTable;
            FrameTable            = frameTable;
        }

        public readonly AnimationCommandTable AnimationCommandTable;
        public readonly FrameTable FrameTable;
        public string Name => AnimationCommandTable.Name;
    }

    public class SpriteAnimationCommandsView : TableImageView<AnimationCommand, AnimationCommandTable> {
        public SpriteAnimationCommandsView(string name, SpriteAnimationCommandsViewContext model, INameGetterContext nameGetterContext)
        : base(name, model?.AnimationCommandTable, nameGetterContext, 2) {
            Context = model;
        }

        public override Image GetImageFromModel(AnimationCommand aniCommand)
            => aniCommand?.GetTexture(aniCommand.Directions)?.CreateBitmapARGB1555();

        private SpriteAnimationCommandsViewContext _context = null;
        public SpriteAnimationCommandsViewContext Context {
            get => _context;
            set {
                if (_context != value) {
                    _context = value;
                    TableView.Table = value?.AnimationCommandTable;
                }
            }
        }
    }
}