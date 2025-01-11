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
            ResetCamera();
        }

        public void ResetCamera() {
            if (CameraUnknownBox != null) {
                Models.Remove(CameraUnknownBox);
                CameraUnknownBox.Dispose();
                CameraUnknownBox = null;
            }

            if (CameraBoundaryBox != null) {
                Models.Remove(CameraBoundaryBox);
                CameraBoundaryBox.Dispose();
                CameraBoundaryBox = null;
            }
        }

        public void Update(IMPD_File mpdFile) {
            foreach (var block in Blocks)
                block.Update(mpdFile);
            UpdateCamera(mpdFile);
        }

        public void UpdateCamera(IMPD_File mpdFile) {
            ResetCamera();

            var cameraSettings = mpdFile.CameraSettingsTable.Rows[0];

            // Unknown coords
            var ucX1 =  0.0f + cameraSettings.UnknownBoxX1 / 32.00f + WorldResources.ModelOffsetX;
            var ucY1 = 64.0f - cameraSettings.UnknownBoxY1 / 32.00f + WorldResources.ModelOffsetZ;
            var ucX2 =  0.0f + cameraSettings.UnknownBoxX2 / 32.00f + WorldResources.ModelOffsetX;
            var ucY2 = 64.0f - cameraSettings.UnknownBoxY2 / 32.00f + WorldResources.ModelOffsetZ;

            // Boundary coords
            var bcX1 =  0.0f + cameraSettings.CameraBoundaryX1 / 32.00f + WorldResources.ModelOffsetX;
            var bcY1 = 64.0f - cameraSettings.CameraBoundaryY1 / 32.00f + WorldResources.ModelOffsetZ;
            var bcX2 =  0.0f + cameraSettings.CameraBoundaryX2 / 32.00f + WorldResources.ModelOffsetX;
            var bcY2 = 64.0f - cameraSettings.CameraBoundaryY2 / 32.00f + WorldResources.ModelOffsetZ;

            // Fetch minimum height of the world.
            var minHeight = mpdFile.Tiles.To1DArray().SelectMany(x => x.GetSurfaceModelVertexHeights()).Min();

            var unknownQuads = new Quad[] {
                new Quad([
                    new Vector3(ucX1, minHeight, ucY1),
                    new Vector3(ucX2, minHeight, ucY1),
                    new Vector3(ucX2, minHeight, ucY2),
                    new Vector3(ucX1, minHeight, ucY2)
                ], new Vector4(1.00f, 0.00f, 0.00f, 0.25f))
            };
            CameraUnknownBox = new QuadModel(unknownQuads);

            var boundaryQuads = new Quad[] {
                new Quad([
                    new Vector3(bcX1, minHeight, bcY1),
                    new Vector3(bcX2, minHeight, bcY1),
                    new Vector3(bcX2, minHeight, bcY2),
                    new Vector3(bcX1, minHeight, bcY2)
                ], new Vector4(0.00f, 1.00f, 1.00f, 0.25f))
            };
            CameraBoundaryBox = new QuadModel(boundaryQuads);

            Models.Add(CameraUnknownBox);
            Models.Add(CameraBoundaryBox);
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
                Textures = null;

                Models?.Dispose();
                CameraUnknownBox = null;
                CameraBoundaryBox = null;
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

        public DisposableList<Texture> Textures { get; private set; } = null;

        public QuadModel CameraUnknownBox { get; private set; } = null;
        public QuadModel CameraBoundaryBox { get; private set; } = null;

        public DisposableList<QuadModel> Models { get; private set; } = null;
    }
}
