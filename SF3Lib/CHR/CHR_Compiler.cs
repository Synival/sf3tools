using System.IO;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.Models.Files.CHR;
using SF3.Sprites;
using SF3.Types;

namespace SF3.CHR {
    public class CHR_Compiler {
        /// <summary>
        /// Compiles a CHR_Def and returns a CHR_File.
        /// </summary>
        /// <param name="chrDef">The CHR_Def to compile to an output stream.</param>
        /// <param name="nameGetterContext">INameGetterContext required by the CHR_File.</param>
        /// <param name="scenario">Scenario required by the CHR_File.</param>
        /// <returns>A new CHR_File on success. Otherwise, 'null'.</returns>
        public CHR_File Compile(CHR_Def chrDef, INameGetterContext nameGetterContext, ScenarioType scenario) {
            try {
                using (var outputStream = new MemoryStream()) {
                    Compile(chrDef, outputStream);
                    return CHR_File.Create(new ByteData.ByteData(new ByteArray(outputStream.ToArray())), nameGetterContext, scenario);
                }
            }
            catch {
                return null;
            }
        }

        /// <summary>
        /// Compiles a CHR_Def and outputs it to an output stream.
        /// </summary>
        /// <param name="chrDef">The CHR_Def to compile to an output stream.</param>
        /// <param name="outputStream">The stream to which the CHR_Def will be compiled.</param>
        /// <returns>The number of bytes written to 'outputStream'.</returns>
        public int Compile(CHR_Def chrDef, Stream outputStream) {
            // TODO: don't do it this way, omg :( :( :(
            if (SpritePath != null)
                SpriteResources.SpritePath = SpritePath;
            if (SpritesheetPath != null)
                SpriteResources.SpritesheetPath = SpritesheetPath;

            // Create a compilation unit with all the data that needs to be tracked during the compilation process.
            var job = new CHR_CompilationJob();

            foreach (var sprite in chrDef.Sprites) {
                job.StartSprite(sprite);

                if (!OptimizeFrames)
                    job.AddFrames(sprite);
                if (OptimizeFrames || AddMissingAnimationFrames)
                    job.AddMissingFrames(sprite);
                job.AddAnimations(sprite);

                job.FinishSprite();
            }

            var bytesWritten = job.Write(outputStream, chrDef.WriteFrameImagesBeforeTables == true, chrDef.JunkAfterFrameTables);
            return bytesWritten;
        }

        /// <summary>
        /// When true, the existing frames will be ignored and frames from animations will be added in an optimized order instead.
        /// </summary>
        public bool OptimizeFrames { get; set; } = false;

        /// <summary>
        /// When true, compiling a sprite will automatically add frames missing for each animation.
        /// </summary>
        public bool AddMissingAnimationFrames { get; set; } = false;

        /// <summary>
        /// When set, a custom directory for SF3Sprite's is used.
        /// </summary>
        public string SpritePath { get; set; } = null;
 
        /// <summary>
        /// When set, a custom directory for spritesheets (.PNG files) is used.
        /// </summary>
        public string SpritesheetPath { get; set; } = null;
    }
}
