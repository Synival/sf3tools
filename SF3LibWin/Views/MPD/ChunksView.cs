using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
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

            // TODO: The view layer shouldn't be accessing this itself. Use an ITexture in IMPD_File.
            // TODO: This needs a separate view which takes multiple chunks as an input.
            if (Model.MPDHeader[0].HasSkyBox) {
                foreach (var chunk in Model.SkyboxChunkData) {
                    var width = Math.Min(512, chunk.DecompressedData.Length);
                    var height = chunk.DecompressedData.Length / width;

                    var imageData = chunk.DecompressedData.GetDataCopyAt(0, width * height).To2DArrayColumnMajor(width, height);
                    var palette = (Model.PaletteTables[1] != null) ? new Palette(Model.PaletteTables[1].Select(x => x.ColorABGR1555).ToArray()) : new Palette(256);
                    var bitmapData = BitmapUtils.ConvertIndexedDataToABGR8888BitmapData(imageData, palette, false);

                    var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                    unsafe {
                        var bitmapLock = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                        Marshal.Copy(bitmapData, 0, bitmapLock.Scan0, bitmapData.Length);
                        bitmap.UnlockBits(bitmapLock);
                    }

                    AddChunkView(chunk.Index, "Skybox", (name) => new TextureView(name, bitmap, 2));
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