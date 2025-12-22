using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Arrays;
using SF3.ByteData;
using SF3.Images;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class MPD_File {
        // TODO: refactor this mess!!
        private void BuildTextureAnimFrameData() {
            if (ChunkData[3] == null || TextureAnimations == null)
                return;

            if (Chunk3Frames == null)
                Chunk3Frames = new List<Chunk3Frame>();
            else
                Chunk3Frames.Clear();

            var chunk3Textures = new Dictionary<uint, ITextureData>();
            TextureModel GetTextureModelByID(int textureId) {
                if (TextureChunks == null)
                    return null;
                return TextureChunks.Where(x => x != null).Select(x => x.TextureTable).SelectMany(x => x).FirstOrDefault(x => x.ID == textureId);
            }

            foreach (var anim in TextureAnimations) {
                foreach (var frame in anim.TextureAnimationFrameTable) {
                    var offset = frame.CompressedImageDataOffset;
                    var existingFrame = Chunk3Frames.FirstOrDefault(x => x.Offset == offset);

                    var pixelFormat = ((frame.TextureID & 0x100) == 0x100) ? TexturePixelFormat.Palette3 : TexturePixelFormat.ABGR1555;
                    var referenceTexID = ((frame.TextureID & 0x100) == 0x100) ? frame.TextureID - 0x100 : frame.TextureID;
                    var referenceTex = GetTextureModelByID(referenceTexID);

                    if (existingFrame != null) {
                        frame.FetchAndCacheTexture(existingFrame.Data.DecompressedData, pixelFormat, referenceTex);
                        continue;
                    }

                    var uncompressedBytes8 = frame.Width * frame.Height;
                    var uncompressedBytes16 = frame.Width * frame.Height * 2;

                    CompressedData newData = null;
                    ByteArraySegment byteArray = null;

                    try {
                        if (pixelFormat.BytesPerPixel() == 2) {
                            var compressedBytes = Math.Min(uncompressedBytes16 + 8, ChunkData[3].Length - (int) offset);
                            byteArray = new ByteArraySegment(ChunkData[3].Data, (int) offset, compressedBytes);
                            newData = new CompressedData(byteArray, uncompressedBytes16);
                            frame.FetchAndCacheTexture(newData.DecompressedData, pixelFormat, referenceTex);
                        }
                        else {
                            var compressedBytes = Math.Min(uncompressedBytes8 + 8, ChunkData[3].Length - (int) offset);
                            byteArray = new ByteArraySegment(ChunkData[3].Data, (int) offset, compressedBytes);
                            newData = new CompressedData(byteArray, uncompressedBytes8);
                            frame.FetchAndCacheTexture(newData.DecompressedData, pixelFormat, referenceTex);
                        }
                    }
                    catch { }

                    if (newData != null && frame.Texture != null) {
                        byteArray.Redefine(byteArray.Offset, newData.LastDecompressBytesRead.Value); // Sets the correct compressed size, which wasn't available before
                        newData.IsModified = false;
                        chunk3Textures.Add(offset, frame.Texture);
                        Chunk3Frames.Add(new Chunk3Frame((int) offset, newData));
                    }
                }
            }

            // Add triggers to update texture animation frames when their offsets or content are modified.
            foreach (var c3frameLoop in Chunk3Frames) {
                var c3frame = c3frameLoop;
                c3frame.Data.Data.RangeModified += (s, a) => {
                    if (a.Moved) {
                        var newOffset = ((ByteArraySegment) c3frame.Data.Data).Offset;
                        var oldOffset = newOffset - a.OffsetChange;
                        var affectedFrames = TextureAnimations.SelectMany(x => x.TextureAnimationFrameTable).Where(x => x.CompressedImageDataOffset == oldOffset).ToArray();
                        foreach (var frame in affectedFrames)
                            frame.CompressedImageDataOffset = (uint) newOffset;
                    }
                };
                c3frame.Data.DecompressedData.Data.RangeModified += (s, a) => {
                    if (a.Resized || a.Modified) {
                        var offset = ((ByteArraySegment) c3frame.Data.Data).Offset;
                        var affectedFrames = TextureAnimations.SelectMany(x => x.TextureAnimationFrameTable).Where(x => x.CompressedImageDataOffset == offset).ToArray();
                        foreach (var frame in affectedFrames) {
                            var referenceTex = GetTextureModelByID(frame.TextureID);
                            frame.FetchAndCacheTexture(c3frame.Data.DecompressedData, frame.PixelFormat, referenceTex);
                        }
                    }
                };
            }
        }

        private void RecompressChunk3Frames(bool onlyModified) {
            if (Chunk3Frames == null)
                return;

            // Recompress texture frames and determine the new total size of Chunk[3].
            foreach (var frameDataKv in Chunk3Frames) {
                var frameData = frameDataKv.Data;
                if (!onlyModified || frameData.NeedsRecompression || frameData.IsModified)
                    _ = frameData.Finish();
            }
        }

        public List<Chunk3Frame> Chunk3Frames { get; private set; }
    }
}
