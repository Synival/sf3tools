using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using CommonLib.Types;
using OpenTK.Mathematics;

namespace SF3.Win.OpenGL.GLResources.Shared {
    public class GradientResources : ResourcesBase {
        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            Models?.Dispose();

            GroundGradientModel = null;
            SkyGradientModel    = null;
            ModelsGradientModel = null;

            Models = null;
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
