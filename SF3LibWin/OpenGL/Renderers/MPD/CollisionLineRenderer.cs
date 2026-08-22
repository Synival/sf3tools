using System;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.GLResources.Shared;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class CollisionLineRenderer {
        public void Draw(GeneralResources general, CollisionResources collisionModels, float cameraYaw, bool selectionColors) {
            if (collisionModels == null)
                return;

            var shader = selectionColors ? general.ColorizeShader : general.SolidShader;
            if (selectionColors)
                _ = shader.UpdateUniform("alwaysShow", true);

            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(-4.0f, -4.0f);

            var (sin, cos) = Math.SinCos(MathHelper.DegreesToRadians(cameraYaw));
            var sortedModels = collisionModels.IndividualModels
                .OrderBy(x => x.Quads[0].Center.X * sin + x.Quads[0].Center.Z * cos)
                .ToArray();

            static Vector4 ModelSelectionColor(CollisionResources.CollisionQuadModel model) {
                var id = model.ID;
                var r = id % 64 / 64.0f;
                var g = id / 64 / 64.0f;
                return new Vector4(r, g, model.IsPoint ? RendererSelectionConstants.CollisionPointsB : RendererSelectionConstants.CollisionLinesB, 1.0f);
            }

            using (shader.Use()) {
                if (selectionColors) {
                    GL.Disable(EnableCap.CullFace);
                    foreach (var model in sortedModels) {
                        _ = shader.UpdateUniform("color", ModelSelectionColor(model));
                        model.Draw(shader, null);
                    }
                    GL.Enable(EnableCap.CullFace);
                }
                else {
                    foreach (var model in sortedModels)
                        model.Draw(shader, null);

                    GL.Disable(EnableCap.DepthTest);
                    GL.DepthMask(false);

                    collisionModels.FullModel.Draw(shader, null);

                    GL.DepthMask(true);
                    GL.Enable(EnableCap.DepthTest);
                }
            }

            GL.Disable(EnableCap.PolygonOffsetFill);
        }
    }
}
