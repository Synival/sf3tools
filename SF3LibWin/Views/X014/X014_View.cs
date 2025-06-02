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
            if (Model.CharacterBattleModelsSc1Table != null)
                CreateChild(new TableView("Character Battle Models (Scn1)", Model.CharacterBattleModelsSc1Table, ngc));
            if (Model.CharacterBattleModelsSc2Table != null)
                CreateChild(new TableView("Character Battle Models (Scn2)", Model.CharacterBattleModelsSc2Table, ngc));
            if (Model.CharacterBattleModelsSc3Table != null)
                CreateChild(new TableView("Character Battle Models (Scn3+)", Model.CharacterBattleModelsSc3Table, ngc));
            if (Model.EnemyBattleModelSc1Table != null)
                CreateChild(new TableView("Enemy Battle Models (Scn1)", Model.EnemyBattleModelSc1Table, ngc));
            if (Model.MPDBattleSceneIdTable != null)
                CreateChild(new TableView("MPD Battle Scene IDs (Scn1)", Model.MPDBattleSceneIdTable, ngc));
            if (Model.BattleScenesByMapTable != null)
                CreateChild(new TableView("Battle Scenes by Battle (Scn1)", Model.BattleScenesByMapTable, ngc));
            if (Model.BattleScenesByTerrainTable != null)
                CreateChild(new TableView("Battle Scenes by Terrain (Scn1)", Model.BattleScenesByTerrainTable, ngc));
            if (Model.BattleScenesOtherTable != null)
                CreateChild(new TableView("Other Battle Scenes (Scn1)", Model.BattleScenesOtherTable, ngc));
            if (Model.MPDBattleSceneInfoTable != null)
                CreateChild(new TableView("MPD Battle Scene Info (Scn2+)", Model.MPDBattleSceneInfoTable, ngc));
            if (Model.TerrainBasedBattleSceneTablesByRamAddress != null)
                CreateChild(new TableArrayView<TerrainBasedBattleSceneTable>("Terrain-Based Battle Scenes", Model.TerrainBasedBattleSceneTablesByRamAddress.Values.ToArray(), ngc));
            if (Model.SpellAnimationTable != null)
                CreateChild(new TableView("Spell Animations", Model.SpellAnimationTable, ngc));
            if (Model.SpecialAnimationTable != null)
                CreateChild(new TableView("Special Animations", Model.SpecialAnimationTable, ngc));

            return Control;
        }

        public IX014_File Model { get; }
    }
}
