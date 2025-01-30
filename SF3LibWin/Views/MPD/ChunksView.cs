using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;
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

            // TODO: Temporary. These are likely just scroll panes. For now, just draw every other chunk as an image.
            foreach (var chunk in Model.ChunkData) {
                if (chunk != null && !chunkViews.ContainsKey(chunk.Index)) {
                    var width = Math.Min(512, chunk.DecompressedData.Length);
                    var height = chunk.DecompressedData.Length / width;

                    var imageData = chunk.DecompressedData.GetDataCopyAt(0, width * height).To2DArrayColumnMajor(width, height);

                    var uniquePalettes = Model.TexturePalettes.Where(x => x != null).GroupBy(x => x.Address).Select(x => x.First()).ToArray();
                    var palettes = uniquePalettes .Select(x => x != null ? new Palette(x.Select(x => x.ColorABGR1555).ToArray()) : null).ToArray();
                    var bitmapDatas = palettes.Select(x => x != null ? BitmapUtils.ConvertIndexedDataToABGR8888BitmapData(imageData, x, false) : null).ToArray();
                    var bitmapHeight = bitmapDatas.Sum(x => x != null ? height : 0);
                    var bitmapData = new byte[width * bitmapHeight * 4];

                    var pos = 0;
                    foreach (var bd in bitmapDatas) {
                        if (bd != null) {
                            bd.CopyTo(bitmapData, pos);
                            pos += bd.Length;
                        }
                    }

                    Bitmap bitmap = null;
                    unsafe {
                        fixed (byte* bitmapDataPtr = bitmapData)
                            bitmap = new Bitmap(width, bitmapHeight, width * 4, PixelFormat.Format32bppArgb, (nint) bitmapDataPtr);
                    }

                    AddChunkView(chunk.Index, "Image", (name) => new TextureView(name, bitmap, 2));
                }
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