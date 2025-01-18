using System;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Properties;

namespace SF3.Win.OpenGL.MPD_File {
    public class SurfaceModelResources : IDisposable {
        public const int WidthInTiles = 64;
        public const int HeightInTiles = 64;

        public SurfaceModelResources() {
            var numBlocks = 16 * 16;
            Blocks = new SurfaceModelBlockResources[numBlocks];
            for (var i = 0; i < numBlocks; i++)
                Blocks[i] = new SurfaceModelBlockResources(i);
        }

        private bool _isInitialized = false;
        public void Init() {
            if (_isInitialized)
                return;
            _isInitialized = true;

            foreach (var block in Blocks)
                block.Init();

            Textures = [
                (TerrainTypesTexture = new Texture(Resources.TerrainTypesBmp)),
                (EventIDsTexture     = new Texture(Resources.EventIDsBmp))
            ];

            Models = [];
        }

        public void Reset() {
            foreach (var block in Blocks)
                block.Reset();
            ResetBoundaries();
        }

        public void ResetBoundaries() {
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
            foreach (var block in Blocks)
                block.Update(mpdFile);
            UpdateBoundaries(mpdFile);
        }

        public void UpdateBoundaries(IMPD_File mpdFile) {
            ResetBoundaries();

            var boundaries = mpdFile.BoundariesTable.Rows;

            // Camera boundary coords
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

            // Fetch minimum height of the world.
            var minHeight = mpdFile.Tiles.To1DArray().SelectMany(x => x.GetSurfaceModelVertexHeights()).Min();

            var cameraQuads = new Quad[] {
                new Quad([
                    new Vector3(ccX1, minHeight, ccY1),
                    new Vector3(ccX2, minHeight, ccY1),
                    new Vector3(ccX2, minHeight, ccY2),
                    new Vector3(ccX1, minHeight, ccY2)
                ], new Vector4(1.00f, 0.00f, 0.00f, 0.25f))
            };
            CameraBoundaryModel = new QuadModel(cameraQuads);

            var battleQuads = new Quad[] {
                new Quad([
                    new Vector3(bcX1, minHeight, bcY1),
                    new Vector3(bcX2, minHeight, bcY1),
                    new Vector3(bcX2, minHeight, bcY2),
                    new Vector3(bcX1, minHeight, bcY2)
                ], new Vector4(0.00f, 0.50f, 1.00f, 0.25f))
            };
            BattleBoundaryModel = new QuadModel(battleQuads);

            Models.Add(CameraBoundaryModel);
            Models.Add(BattleBoundaryModel);
        }

        public void SetLightingTexture(Texture texture) {
            if (LightingTexture != null) {
                Textures.Remove(LightingTexture);
                LightingTexture.Dispose();
                LightingTexture = null;
            }

            LightingTexture = texture;
            Textures.Add(texture);
        }

        public void Invalidate() {
            foreach (var block in Blocks)
                block.Invalidate();

            // TODO: invalidate camera boxes
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                foreach (var block in Blocks)
                    block.Dispose();

                Textures?.Dispose();
                TerrainTypesTexture = null;
                EventIDsTexture = null;
                LightingTexture = null;
                Textures = null;

                Models?.Dispose();
                CameraBoundaryModel = null;
                BattleBoundaryModel = null;
                Models = null;
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SurfaceModelResources() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public SurfaceModelBlockResources[] Blocks { get; }

        public Texture TerrainTypesTexture { get; private set; } = null;
        public Texture EventIDsTexture { get; private set; } = null;
        public Texture LightingTexture { get; private set; } = null;

        public DisposableList<Texture> Textures { get; private set; } = null;

        public QuadModel CameraBoundaryModel { get; private set; } = null;
        public QuadModel BattleBoundaryModel { get; private set; } = null;

        public DisposableList<QuadModel> Models { get; private set; } = null;
    }
}
