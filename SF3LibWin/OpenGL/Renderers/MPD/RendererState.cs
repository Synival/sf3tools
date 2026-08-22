using System.Linq;
using CommonLib.Utils;
using OpenTK.Mathematics;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.OpenGL.GLResources.MPD;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class RendererState {
        public float CameraYaw;
        public float CameraPitch;
        public int ScreenWidth;
        public int ScreenHeight;
        public Matrix4 ProjectionMatrix;
        public Matrix4 ViewMatrix;

        public (IMPD_ModelInstance Model, ModelGroup ModelGroup)[] GetModelsWithGroups(ModelResources models, RendererOptions options) {
            if (_modelsWithGroups != null)
                return _modelsWithGroups;

            var modelDirectionsFacingCamera = GetModelDirectionsFacingCamera(options);

            if (models.ModelInstances == null) {
                _modelsWithGroups = [];
                return _modelsWithGroups;
            }

            bool IsVisibleCollection(MPD_CollectionType collection) {
                return
                    collection != MPD_CollectionType.ExtraModels && options.DrawModels ||
                    collection == MPD_CollectionType.ExtraModels && options.DrawExtraModels;
            }

            _modelsWithGroups = models.ModelInstances
                .Select(x => (Model: x, ModelGroup: models.ModelsByIDByCollection[x.Collection.Collection].TryGetValue(x.ModelID, out var pd) ? pd : null))
                .Where(x => x.ModelGroup != null && IsVisibleCollection(x.Model.Collection.Collection))
                .Where(x => {
                    var direction = x.Model.OnlyVisibleFromDirection;
                    return direction == ModelDirectionType.Unset || modelDirectionsFacingCamera[(int) direction];
                })
                .Where(x => options?.ModelsToHide?.Contains(x.Model.ID) != true)
                .ToArray();

            return _modelsWithGroups;
        }

        private bool[] GetModelDirectionsFacingCamera(RendererOptions options) {
            if (_modelDirectionsFacingCamera != null)
                return _modelDirectionsFacingCamera;

            var showModelsInAllDirections = !options.HideModelsNotFacingCamera;

            bool WithinAngleRange(ModelDirectionType dir) {
                if (showModelsInAllDirections)
                    return true;
                var angle = 540 - 45 * (int) dir % 360;
                var angleDiff = MathHelpers.ActualMod(CameraYaw - angle, 360.0f);
                if (angleDiff > 180.0f)
                    angleDiff -= 360.0f;

                return angleDiff > options.ModelsViewAngleMin && angleDiff < options.ModelsViewAngleMax;
            }

            _modelDirectionsFacingCamera = [
                WithinAngleRange(ModelDirectionType.North),
                WithinAngleRange(ModelDirectionType.Northeast),
                WithinAngleRange(ModelDirectionType.East),
                WithinAngleRange(ModelDirectionType.Southeast),
                WithinAngleRange(ModelDirectionType.South),
                WithinAngleRange(ModelDirectionType.Southwest),
                WithinAngleRange(ModelDirectionType.West),
                WithinAngleRange(ModelDirectionType.Northwest),
            ];

            return _modelDirectionsFacingCamera;
        }

        private (IMPD_ModelInstance Model, ModelGroup ModelGroup)[] _modelsWithGroups;
        private bool[] _modelDirectionsFacingCamera;
    }
}