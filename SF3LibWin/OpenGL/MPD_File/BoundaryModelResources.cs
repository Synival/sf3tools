using CommonLib;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;

namespace SF3.Win.OpenGL.MPD_File {
    public class BoundaryModelResources : ResourcesBase, IMPD_Resources {
        protected override void PerformInit() {
            Models = [];
        }

        public override void DeInit() {
            Models.Dispose();
            Models = null;
        }

        public override void Reset() {
            if (CameraBoundaryModel != null) {
                Models.Remove(CameraBoundaryModel);
                CameraBoundaryModel.Dispose();
                CameraBoundaryModel = null;
            }

            if (BattleBoundaryModel != null) {
                Models.Remove(BattleBoundaryModel);
                BattleBoundaryModel.Dispose();
                BattleBoundaryModel = null;
            }
        }

        public void Update(IMPD_File mpdFile) {
            Reset();

            var boundaries = mpdFile.BoundariesTable;

            // Camera boundary coords
            try {
                var camera = boundaries[0];
                var ccX1 =  0.0f + camera.X1 / 32.00f + WorldResources.ModelOffsetX;
                var ccY1 = 64.0f - camera.Y1 / 32.00f + WorldResources.ModelOffsetZ;
                var ccX2 =  0.0f + camera.X2 / 32.00f + WorldResources.ModelOffsetX;
                var ccY2 = 64.0f - camera.Y2 / 32.00f + WorldResources.ModelOffsetZ;

                // Battle boundary coords
                var battle = boundaries[1];
                var bcX1 =  0.0f + battle.X1 / 32.00f + WorldResources.ModelOffsetX;
                var bcY1 = 64.0f - battle.Y1 / 32.00f + WorldResources.ModelOffsetZ;
                var bcX2 =  0.0f + battle.X2 / 32.00f + WorldResources.ModelOffsetX;
                var bcY2 = 64.0f - battle.Y2 / 32.00f + WorldResources.ModelOffsetZ;

                var height = mpdFile.MPDHeader.GroundY / -32.0f;
                var cameraQuads = new Quad[] {
                    new Quad([
                        new Vector3(ccX1, height, ccY1),
                        new Vector3(ccX2, height, ccY1),
                        new Vector3(ccX2, height, ccY2),
                        new Vector3(ccX1, height, ccY2)
                    ], new Vector4(1.00f, 0.00f, 0.00f, 0.25f))
                };
                CameraBoundaryModel = new QuadModel(cameraQuads);

                var battleQuads = new Quad[] {
                    new Quad([
                        new Vector3(bcX1, height, bcY1),
                        new Vector3(bcX2, height, bcY1),
                        new Vector3(bcX2, height, bcY2),
                        new Vector3(bcX1, height, bcY2)
                    ], new Vector4(0.00f, 0.50f, 1.00f, 0.25f))
                };
                BattleBoundaryModel = new QuadModel(battleQuads);

                Models.Add(CameraBoundaryModel);
                Models.Add(BattleBoundaryModel);
            }
            catch {
                // TODO: what to do in this case?
            }
        }

        public QuadModel CameraBoundaryModel { get; private set; } = null;
        public QuadModel BattleBoundaryModel { get; private set; } = null;

        public DisposableList<QuadModel> Models { get; private set; } = null;
    }
}
