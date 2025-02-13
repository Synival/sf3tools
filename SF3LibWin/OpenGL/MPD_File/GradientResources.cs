using System;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using CommonLib.Types;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;

namespace SF3.Win.OpenGL.MPD_File {
    public class GradientResources : IDisposable {
        private bool _isInitialized = false;
        public void Init() {
            if (_isInitialized)
                return;
            _isInitialized = true;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing)
                Reset();

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~GradientResources() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public void Reset() {
            Models?.Dispose();

            GroundGradientModel = null;
            SkyBoxGradientModel = null;
            ModelsGradientModel = null;

            Models = null;
        }

        public void Update(IMPD_File mpdFile)
            => Update(mpdFile?.GradientTable?.Length > 0 ? mpdFile.GradientTable[0] : null);

        public void Update(GradientModel gradient) {
            if (gradient == null) {
                Reset();
                return;
            }

            Update(
                gradient.StartPosition,
                gradient.StopPosition,
                new Vector3(
                    Math.Clamp(gradient.StartR / (float) 0x1f, 0.00f, 1.00f),
                    Math.Clamp(gradient.StartG / (float) 0x1f, 0.00f, 1.00f),
                    Math.Clamp(gradient.StartB / (float) 0x1f, 0.00f, 1.00f)
                ),
                new Vector3(
                    Math.Clamp(gradient.StopR / (float) 0x1f, 0.00f, 1.00f),
                    Math.Clamp(gradient.StopG / (float) 0x1f, 0.00f, 1.00f),
                    Math.Clamp(gradient.StopB / (float) 0x1f, 0.00f, 1.00f)
                ),
                gradient.AffectsGround ? (gradient.GroundOpacity / (float) 0x1f) : 0,
                gradient.AffectsSkyBox ? (gradient.SkyBoxOpacity / (float) 0x1f) : 0,
                gradient.AffectsModelsAndTiles ? (gradient.ModelsAndTilesOpacity / (float) 0x1f) : 0
            );
        }

        public void Update(float posTop, float posBottom, Vector3 colorTop, Vector3 colorBottom, float groundOpacity, float skyBoxOpacity, float modelsOpacity) {
            Reset();

            var corners = Enum.GetValues<CornerType>();

            var vertices  = corners.Select(x => new Vector3(x.GetDirectionX(), x.GetDirectionY(), 0)).ToArray();
            var positions = corners.Select(x => x.IsTopSide() ? posTop : posBottom).ToArray().To2DArray(4, 1);

            QuadModel MakeModel(float opacity) {
                if (opacity <= 0.0f)
                    return null;

                // TODO: why in the world do I need to cut make these changes???
                //       is there something wrong with the blend function???
                opacity = opacity / 2;

                var colors = corners.Select(x => x.IsTopSide() ? new Vector4(colorTop, opacity) : new Vector4(colorBottom, opacity)).ToArray();
                var quads = corners.Select((x, i) => {
                    var quad = new Quad(vertices, colors);
                    quad.AddAttribute(new PolyAttribute(1, OpenTK.Graphics.OpenGL.ActiveAttribType.Float, "gradientPos", 4, positions));
                    return quad;
                }).ToArray();
                return new QuadModel(quads);
            }

            GroundGradientModel = MakeModel(groundOpacity);
            SkyBoxGradientModel = MakeModel(skyBoxOpacity);
            ModelsGradientModel = MakeModel(modelsOpacity);

            Models?.Dispose();
            Models = new DisposableList<QuadModel>
                (new QuadModel[] { GroundGradientModel, SkyBoxGradientModel, ModelsGradientModel }.Where(x => x != null).ToArray()
            );
        }

        public QuadModel GroundGradientModel { get; private set; } = null;
        public QuadModel SkyBoxGradientModel { get; private set; } = null;
        public QuadModel ModelsGradientModel { get; private set; } = null;

        public DisposableList<QuadModel> Models { get; private set; } = null;
    }
}
