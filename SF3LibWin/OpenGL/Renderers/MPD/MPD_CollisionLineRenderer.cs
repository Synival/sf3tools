using System;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.GLResources.Shared;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class MPD_CollisionLineRenderer {
        public void Draw(GeneralResources general, MPD_CollisionResources collisionModels, float cameraYaw, bool selectionColors) {
            if (collisionModels == null)
                return;

            var shader = selectionColors ? general.ColorizeShader : general.SolidShader;
            if (selectionColors)
                _ = shader.UpdateUniform("alwaysShow", true);

            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(-0.4f, -0.4f);
            GL.DepthFunc(DepthFunction.Lequal);

            var (sin, cos) = Math.SinCos(MathHelper.DegreesToRadians(cameraYaw));
            var sortedModels = collisionModels.IndividualModels
                .OrderBy(x => x.Quads[0].Center.X * sin + x.Quads[0].Center.Z * cos)
                .ToArray();

            static Vector4 ModelSelectionColor(MPD_CollisionResources.CollisionQuadModel model) {
                var id = model.ID;
                var r = id % 64 / 64.0f;
                var g = id / 64 / 64.0f;
                return new Vector4(r, g, model.IsPoint ? MPD_RendererSelectionConstants.CollisionPointsB : MPD_RendererSelectionConstants.CollisionLinesB, 1.0f);
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

            GL.DepthFunc(DepthFunction.Less);
            GL.Disable(EnableCap.PolygonOffsetFill);
        }
    }
}
