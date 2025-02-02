using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Imaging;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class ChunksView : TabView {
        public ChunksView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            CreateChild(new TableView("Header", Model.ChunkHeader, ngc));

            // Build views for all chunks.
            var chunkViews = new Dictionary<int, IView>();
            void AddChunkView(int? chunkIndex, string name, Func<string, IView> viewCreator) {
                var newView = viewCreator("Chunk " + chunkIndex + " (" + name + ")");
                chunkViews.Add(chunkIndex.Value, newView);
            }

            if (Model.SurfaceModel != null)
                AddChunkView(Model.SurfaceModel.ChunkIndex, "Surface Model", (name) => new SurfaceModelChunkView(name, Model.SurfaceModel));
            if (Model.TextureAnimations != null)
                AddChunkView(3, "Texture Animation Frames", (name) => new TextureAnimFramesView(name, Model, ngc));
            if (Model.Surface != null)
                AddChunkView(Model.Surface.ChunkIndex, "Surface", (name) => new SurfaceChunkView(name, Model.Surface));


            if (Model.ModelCollections != null)
                foreach (var modelCollection in Model.ModelCollections)
                    if (modelCollection != null)
                        AddChunkView(modelCollection.ChunkIndex, "Models", (name) => new ModelChunkView(name, modelCollection));

            if (Model.TextureCollections != null)
                foreach (var texCollection in Model.TextureCollections)
                    if (texCollection != null)
                        AddChunkView(texCollection.ChunkIndex, "Textures", (name) => new TextureChunkView(name, texCollection));

            var palettes = Model.PaletteTables
                .Select(x => x != null ? new Palette(x.Select(x => x.ColorABGR1555).ToArray()) : new Palette(256))
                .ToArray();

            if (Model.SkyBoxChunkData != null)
                foreach (var chunk in Model.SkyBoxChunkData)
                    AddChunkView(chunk.Index, "SkyBox", (name) => new DataImageView(name, chunk.DecompressedData.Data, palettes[1]));

            // Add image panes for unhandled chunks.
            foreach (var chunk in Model.ChunkData) {
                if (chunk == null || chunkViews.ContainsKey(chunk.Index))
                    continue;

                if (chunk.DecompressedData.Length == 0x10000)
                    AddChunkView(chunk.Index, "Unhandled Image", (name) => new DataImageView(name, chunk.DecompressedData.Data, palettes));
                else
                    AddChunkView(chunk.Index, "Unhandled Data", (name) => new DataHexView(name, chunk.DecompressedData.Data));
            }

            // Add chunks, sorted by their chunk index.
            var chunkViewArray = chunkViews.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
            foreach (var chunkView in chunkViewArray)
                CreateChild(chunkView);

            return Control;
        }

        public IMPD_File Model { get; }
    }
}