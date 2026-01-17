using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using CommonLib.Types;
using OpenTK.Mathematics;
using SF3.MPD;

namespace SF3.Win.OpenGL.MPD {
    public class GradientResources : ResourcesBase, IMPD_Resources {
        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            Models?.Dispose();

            GroundGradientModel = null;
            SkyGradientModel    = null;
            ModelsGradientModel = null;

            Models = null;
        }

        public void Update(IMPD mpdFile)
            => Update(mpdFile?.Gradient);

        public void Update(IMPD_Gradient gradient) {
            if (gradient == null || gradient.IsDummiedOut) {
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

        public void Update(
            float posTop,
            float posBottom,
            Vector3 colorTop,
            Vector3 colorBottom,
            float groundIntensity,
            float skyIntensity,
            float modelsIntensity
        ) {
            Reset();

            var corners = Enum.GetValues<CornerType>();

            var topY    = posTop * -2.0f + 1.0f;
            var bottomY = Math.Min(topY, posBottom * -2.0f + 1.0f);

            var verticesTop    = corners.Select(x => new Vector3(x.GetDirectionX(), x.IsTopSide() ? 1.0f : topY,     0)).ToArray();
            var verticesMiddle = corners.Select(x => new Vector3(x.GetDirectionX(), x.IsTopSide() ? topY : bottomY,  0)).ToArray();
            var verticesBottom = corners.Select(x => new Vector3(x.GetDirectionX(), x.IsTopSide() ? bottomY : -1.0f, 0)).ToArray();

            var positions = corners.Select(x => x.IsTopSide() ? posTop : posBottom).ToArray().To2DArray(4, 1);

            QuadModel MakeModel(float opacity) {
                if (opacity <= 0.0f)
                    return null;

                var colorTopVec    = new Vector4(colorTop, opacity);
                var colorBottomVec = new Vector4(colorBottom, opacity);
                var colorsMiddle   = corners.Select(x => x.IsTopSide() ? colorTopVec : colorBottomVec).ToArray();

                var quads = new List<Quad>();
                if (topY < 1.00f)
                    quads.Add(new Quad(verticesTop, colorTopVec));
                if (bottomY < topY)
                    quads.Add(new Quad(verticesMiddle, colorsMiddle));
                if (-1.00f < bottomY)
                    quads.Add(new Quad(verticesBottom, colorBottomVec));

                return new QuadModel(quads.ToArray());
            }

            GroundGradientModel = MakeModel(groundIntensity);
            SkyGradientModel = MakeModel(skyIntensity);
            ModelsGradientModel = MakeModel(modelsIntensity);

            Models?.Dispose();
            Models = new DisposableList<QuadModel>
                (new QuadModel[] { GroundGradientModel, SkyGradientModel, ModelsGradientModel }.Where(x => x != null).ToArray()
            );
        }

        public QuadModel GroundGradientModel { get; private set; } = null;
        public QuadModel SkyGradientModel { get; private set; } = null;
        public QuadModel ModelsGradientModel { get; private set; } = null;

        public DisposableList<QuadModel> Models { get; private set; } = null;
    }
}
