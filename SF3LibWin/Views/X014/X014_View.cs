using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.X014;
using SF3.Models.Tables.X014;

namespace SF3.Win.Views.X014 {
    public class X014_View : TabView {
        public X014_View(string name, IX014_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.CharacterBattleModelSc2Table != null)
                CreateChild(new TableView("Character Battle Models (Scn2)", Model.CharacterBattleModelSc2Table, ngc));
            if (Model.MPDBattleSceneInfoTable != null)
                CreateChild(new TableView("MPD Battle Scene Info", Model.MPDBattleSceneInfoTable, ngc));
            if (Model.TerrainBasedBattleSceneTablesByRamAddress != null)
                CreateChild(new TableArrayView<TerrainBasedBattleSceneTable>("Terrain-Based Battle Scenes", Model.TerrainBasedBattleSceneTablesByRamAddress.Values.ToArray(), ngc));

            return Control;
        }

        public IX014_File Model { get; }
    }
}
