using System.Collections.Generic;
using System.Drawing;

namespace SF3.CHR {
    public class CHR_CompilationUnit {
        public class SpritesheetImageRef {
            public Bitmap Bitmap;
            public int X;
            public int Y;
            public int Width;
            public int Height;
        }

        public CHR_CompilationUnit(CHR_Def chrDef, CHR_Writer chrWriter) {
            CHR_Def    = chrDef;
            CHR_Writer = chrWriter;
        }

        public readonly CHR_Def CHR_Def;
        public readonly CHR_Writer CHR_Writer;

        public readonly Dictionary<string, Bitmap> SpritesheetImageDict = new Dictionary<string, Bitmap>();
        public readonly Dictionary<string, SpritesheetImageRef> ImagesRefsByKey = new Dictionary<string, SpritesheetImageRef>();
        public readonly Dictionary<int, List<(string FrameKey, string AniFrameKey)>> FramesToWriteBySpriteIndex = new Dictionary<int, List<(string FrameKey, string AniFrameKey)>>();

        // CHR files must have had their compressed frames written by sprite, because there's a forced alignment of 4
        // after every sprite's groups of frames. We don't bother to do that -- different sprites can share compressed
        // images -- but most cases are fixed if we record which image is the final image for each sprite.
        public readonly HashSet<string> FinalSpriteFrames = new HashSet<string>();

        // Keep track of frame images already written if we're written frame images sprite-by-sprite.
        public readonly HashSet<string> FrameImagesWritten = new HashSet<string>();
    }
}
