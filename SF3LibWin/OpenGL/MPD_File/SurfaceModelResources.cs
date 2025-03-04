using System;
using CommonLib;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Properties;

namespace SF3.Win.OpenGL.MPD_File {
    public class SurfaceModelResources : ResourcesBase, IMPD_Resources {
        public const int WidthInTiles = 64;
        public const int HeightInTiles = 64;

        public SurfaceModelResources() {
            var numBlocks = 16 * 16;
            Blocks = new SurfaceModelBlockResources[numBlocks];
            for (var i = 0; i < numBlocks; i++)
                Blocks[i] = new SurfaceModelBlockResources(i);
        }

        protected override void PerformInit() {
            foreach (var block in Blocks)
                block.Init();

            Textures = [
                (TerrainTypesTexture = new Texture(Resources.TerrainTypesBmp)),
                (EventIDsTexture     = new Texture(Resources.EventIDsBmp))
            ];
        }

        public override void DeInit() {
            Textures?.Dispose();
            TerrainTypesTexture = null;
            EventIDsTexture = null;
            Textures = null;

            foreach (var block in Blocks)
                block.Dispose();
        }

        public override void Reset() {
            foreach (var block in Blocks)
                block.Reset();
        }

        public void Update(IMPD_File mpdFile) {
            foreach (var block in Blocks)
                block.Update(mpdFile);
        }

        public void Invalidate() {
            foreach (var block in Blocks)
                block.Invalidate();
        }

        public SurfaceModelBlockResources[] Blocks { get; }

        public Texture TerrainTypesTexture { get; private set; } = null;
        public Texture EventIDsTexture { get; private set; } = null;

        public DisposableList<Texture> Textures { get; private set; } = null;
    }
}
