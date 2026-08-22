using System;
using CommonLib.Utils;
using OpenTK.Mathematics;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.GLResources.Shared;
using SF3.Win.Types;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class ActorRenderer {
        public void Draw(
            GeneralResources general,
            SceneResources scene,
            float cameraYaw,
            float cameraPitch,
            bool selectionColors
        ) {
            if (scene == null || scene.ModelsBySpriteID == null || scene.ModelsBySpriteID.Count == 0)
                return;

            Vector4 ModelSelectionColor(SceneResources.ActorModelInstance actor) {
                var r = actor.ID % 64 / 64.0f;
                var g = actor.ID / 64 / 64.0f;
                return new Vector4(r, g, RendererSelectionConstants.ActorsB, 1.0f);
            }

            var (baseMatrix, baseRotationMatrix) = GetSpriteDrawMatrices(cameraYaw, cameraPitch);

            var shader = general.SpriteShader;
            using (shader.Use())
            using (scene.ActorTextureAtlas.Use()) {
                _ = shader.UpdateUniform("colorize", selectionColors);

                if (!selectionColors) {
                    // Render shadows first.
                    _ = shader.UpdateUniform("direction", 0.0f);
                    _ = shader.UpdateUniform("cameraDistAdjust", 0.25f);
                    foreach (var actorGroup in scene.ActorsBySpriteID) {
                        var spriteId = actorGroup.Key;
                        var shadow = scene.ShadowsBySpriteID[spriteId];

                        foreach (var actor in actorGroup.Value) {
                            var modelMatrix = baseMatrix * Matrix4.CreateTranslation(new Vector3(actor.X, actor.Y, actor.Z));
                            _ = shader.UpdateUniform(ShaderUniformType.ModelMatrix, modelMatrix);
                            shadow.Draw(shader);
                        }
                    }
                }

                // Now render sprites.
                _ = shader.UpdateUniform("cameraDistAdjust", 0.5f);
                foreach (var actorGroup in scene.ActorsBySpriteID) {
                    var spriteId = actorGroup.Key;
                    var model    = scene.ModelsBySpriteID[spriteId];

                    foreach (var actor in actorGroup.Value) {
                        SetupSpriteShaderUniforms(shader, baseRotationMatrix, actor, cameraYaw, selectionColors ? ModelSelectionColor(actor) : null);
                        model.Draw(shader);
                    }
                }
                _ = shader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
            }
        }

        public void SetupSpriteShaderUniforms(Shader shader, Matrix4 rotationMatrix, SceneResources.ActorModelInstance actor, float cameraYaw, Vector4? color) {
            var modelMatrix = rotationMatrix * Matrix4.CreateTranslation(new Vector3(actor.X, actor.Y + actor.VerticalOffset, actor.Z));
            _ = shader.UpdateUniform(ShaderUniformType.ModelMatrix, modelMatrix);

            // Convert facing direction (0=north, 90=east, ...) to shader direction (0=south, 0.25=east, ...)
            _ = shader.UpdateUniform("direction", MathHelpers.ActualMod((180.0f - actor.Direction - cameraYaw) / 360.0f, 1.0f));

            if (color.HasValue)
                _ = shader.UpdateUniform("color", color.Value);
        }

        public (Matrix4 BaseMatrix, Matrix4 BaseRotationMatrix) GetSpriteDrawMatrices(float cameraYaw, float cameraPitch) {
            var baseMatrix = Matrix4.CreateScale(0.75f);

            var baseRotationMatrix = baseMatrix *
                Matrix4.CreateRotationX(cameraPitch / 180.0f * (float) Math.PI) *
                Matrix4.CreateRotationY(cameraYaw   / 180.0f * (float) Math.PI);

            return (baseMatrix, baseRotationMatrix);
        }
    }
}
