using System;
using OpenTK.Mathematics;
using SF3.MPD.Interfaces;
using SF3.Win.OpenGL.GLResources.Shared;

namespace SF3.Win.OpenGL.GLResources.MPD {
    public class MPD_GradientResources : GradientResources, IMPD_Resources {
        public void Update(IMPD mpdFile)
            => Update(mpdFile?.Gradient, mpdFile?.Settings);

        public void Update(IMPD_Gradient gradient, IMPD_Settings settings) {
            if (gradient == null || settings.IsGradientDummiedOut) {
                Reset();
                return;
            }

            Update(
                gradient.TopPosition,
                gradient.BottomPosition,
                new Vector3(
                    Math.Clamp(gradient.TopColor.R / (float) 0x1F, 0.00f, 1.00f),
                    Math.Clamp(gradient.TopColor.G / (float) 0x1F, 0.00f, 1.00f),
                    Math.Clamp(gradient.TopColor.B / (float) 0x1F, 0.00f, 1.00f)
                ),
                new Vector3(
                    Math.Clamp(gradient.BottomColor.R / (float) 0x1F, 0.00f, 1.00f),
                    Math.Clamp(gradient.BottomColor.G / (float) 0x1F, 0.00f, 1.00f),
                    Math.Clamp(gradient.BottomColor.B / (float) 0x1F, 0.00f, 1.00f)
                ),
                gradient.AffectsGround           ? gradient.GroundIntensity : 0,
                gradient.AffectsSky              ? gradient.SkyIntensity : 0,
                gradient.AffectsModelsAndSurface ? gradient.ModelsAndSurfaceIntensity : 0
            );
        }
    }
}
