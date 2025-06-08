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
                var ccX1 =  0.0f + camera.X1 / 32.00f + GeneralResources.ModelOffsetX;
                var ccZ1 = 64.0f - camera.Z1 / 32.00f + GeneralResources.ModelOffsetZ;
                var ccX2 =  0.0f + camera.X2 / 32.00f + GeneralResources.ModelOffsetX;
                var ccZ2 = 64.0f - camera.Z2 / 32.00f + GeneralResources.ModelOffsetZ;

                // Battle boundary coords
                var battle = boundaries[1];
                var bcX1 =  0.0f + battle.X1 / 32.00f + GeneralResources.ModelOffsetX;
                var bcZ1 = 64.0f - battle.Z1 / 32.00f + GeneralResources.ModelOffsetZ;
                var bcX2 =  0.0f + battle.X2 / 32.00f + GeneralResources.ModelOffsetX;
                var bcZ2 = 64.0f - battle.Z2 / 32.00f + GeneralResources.ModelOffsetZ;

                var height = mpdFile.MPDHeader.GroundY / -32.0f;
                var cameraQuads = new Quad[] {
                    new Quad([
                        new Vector3(ccX1, height, ccZ1),
                        new Vector3(ccX2, height, ccZ1),
                        new Vector3(ccX2, height, ccZ2),
                        new Vector3(ccX1, height, ccZ2)
                    ], new Vector4(1.00f, 0.00f, 0.00f, 0.25f))
                };
                CameraBoundaryModel = new QuadModel(cameraQuads);

                var battleQuads = new Quad[] {
                    new Quad([
                        new Vector3(bcX1, height, bcZ1),
                        new Vector3(bcX2, height, bcZ1),
                        new Vector3(bcX2, height, bcZ2),
                        new Vector3(bcX1, height, bcZ2)
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
