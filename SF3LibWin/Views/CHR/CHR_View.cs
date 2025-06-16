using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.CHR;
using SF3.Models.Tables.CHR;

namespace SF3.Win.Views.CHR {
    public class CHR_View : TabView {
        public CHR_View(string name, ICHR_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.SpriteHeaderTable != null)
                CreateChild(new TableView("Sprite Headers", Model.SpriteHeaderTable, ngc));
            if (Model.FrameDataOffsetsTable != null)
                CreateChild(new TableView("Frame Data Offsets", Model.FrameDataOffsetsTable, ngc));
            if (Model.FrameTablesByFileAddr?.Count > 0)
                CreateChild(new SpriteFramesArrayView("Frames", Model.FrameTablesByFileAddr.Values.ToArray(), ngc));
            if (Model.AnimationOffsetsTable != null)
                CreateChild(new TableView("Animation Offsets", Model.AnimationOffsetsTable, ngc));
            if (Model.AnimationFrameTablesByAddr?.Count > 0)
                CreateChild(new TableArrayView<AnimationFrameTable>("Animation Frames", Model.AnimationFrameTablesByAddr.Values.ToArray(), ngc));

            return Control;
        }

        public ICHR_File Model { get; }
    }
}
