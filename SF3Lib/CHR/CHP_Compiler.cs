using System.IO;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Logging;
using CommonLib.NamedValues;
using CommonLib.Types;
using SF3.Models.Files.CHP;
using SF3.Types;

namespace SF3.CHR {
    public class CHP_Compiler {
        /// <summary>
        /// Compiles a CHP_Def to a CHR_File.
        /// </summary>
        /// <param name="nameGetterContext">Context for getting named values.</param>
        /// <param name="scenario">Scenario to which this CHP belongs.</param>
        /// <returns>A new CHP_File on success. Otherwise, 'null'.</returns>
        public CHP_File Compile(CHP_Def chpDef, INameGetterContext nameGetterContext, ScenarioType scenario) {
            try {
                using (var stream = new MemoryStream()) {
                    _ = Compile(chpDef, stream);
                    return CHP_File.Create(new ByteData.ByteData(new ByteArray(stream.ToArray())), nameGetterContext, scenario);
                }
            }
            catch {
                return null;
            }
        }

        /// <summary>
        /// Compiles a CHP_Def to a CHP_File using a pre-existing buffer.
        /// </summary>
        /// <param name="nameGetterContext">Context for getting named values.</param>
        /// <param name="scenario">Scenario to which this CHP belongs.</param>
        /// <param name="buffer">A pre-existing buffer to which the CHP will be written. Empty space between
        /// CHR files will be skipped rather than written with zeroes. This buffer must be large enough to
        /// contain the entire CHP file.</param>
        /// <returns></returns>
        public CHP_File Compile(CHP_Def chpDef, INameGetterContext nameGetterContext, ScenarioType scenario, byte[] buffer) {
            _ = Compile(chpDef, buffer);
            return CHP_File.Create(new ByteData.ByteData(new ByteArray(buffer)), nameGetterContext, scenario);
        }

        /// <summary>
        /// Compiles a CHP_Def to a stream in .CHP format.
        /// </summary>
        /// <param name="outputStream">Stream to export the .CHP-formatted data to.</param>
        /// <returns>The number of bytes written to outputStream.</returns>
        public int Compile(CHP_Def chpDef, Stream outputStream)
            => CompileInternal(chpDef, outputStream, false);

        /// <summary>
        /// Compiles a CHP_Def to a pre-existing buffer in .CHP format.
        /// </summary>
        /// <param name="buffer">A pre-existing buffer to which the CHP will be written. Empty space between
        /// CHR files will be skipped rather than written with zeroes. This buffer must be large enough to
        /// contain the entire CHP file.</param>
        /// <returns>The number of bytes written to outputStream.</returns>
        public int Compile(CHP_Def chpDef, byte[] buffer) {
            using (var stream = new MemoryStream(buffer))
                return CompileInternal(chpDef, stream, true);
        }

        private int CompileInternal(CHP_Def chpDef, Stream outputStream, bool seekInsteadOfWrite) {
            var startPosition = outputStream.Position;

            foreach (var chr in chpDef.CHRs.OrderBy(x => x.Sector.HasValue ? 1 : 0).ThenBy(x => x.Sector ?? 0)) {
                var sector = chr.Sector ?? ((outputStream.Position - startPosition + 0x7FF) / 0x800);
                var offset = sector * 0x800 + startPosition;

                var padding = offset - outputStream.Position;
                if (padding > 0) {
                    if (seekInsteadOfWrite)
                        outputStream.Position += padding;
                    else
                        outputStream.Write(new byte[padding], 0, (int) padding);
                }
                else if (padding < 0) {
                    Logger.WriteLine($"CHR at sector {sector} (position 0x{offset:X5}) forced to seek back 0x{-padding:X2} bytes, overwriting previous CHR", LogType.Error);
                    outputStream.Position = offset;
                }

                var bytesWritten = _chrCompiler.Compile(chr, outputStream);
                if (chr.MaxSize.HasValue && bytesWritten > chr.MaxSize)
                    Logger.WriteLine($"CHR at sector {sector} (position 0x{offset:X5}) exceeds MaxSize (0x{chr.MaxSize:X5}) by 0x{(bytesWritten - chr.MaxSize):X2} bytes", LogType.Error);
            }

            var eofPadding = (chpDef.TotalSectors * 0x800) - outputStream.Position - startPosition;
            if (eofPadding > 0) {
                if (seekInsteadOfWrite)
                    outputStream.Position += eofPadding;
                else
                    outputStream.Write(new byte[eofPadding], 0, (int) eofPadding);
            }

            return (int) (outputStream.Position - startPosition);
        }

        /// <summary>
        /// When true, the existing frames will be ignored and frames from animations will be added in an optimized order instead.
        /// </summary>
        public bool OptimizeFrames {
            get => _chrCompiler.OptimizeFrames;
            set => _chrCompiler.OptimizeFrames = value;
        }

        /// <summary>
        /// When true, compiling a sprite will automatically add frames missing for each animation.
        /// </summary>
        public bool AddMissingAnimationFrames {
            get => _chrCompiler.AddMissingAnimationFrames;
            set => _chrCompiler.AddMissingAnimationFrames = value;
        }

        /// <summary>
        /// When set, a custom directory for SF3Sprite's is used.
        /// </summary>
        public string SpritePath {
            get => _chrCompiler.SpritePath;
            set => _chrCompiler.SpritePath = value;
        }
 
        /// <summary>
        /// When set, a custom directory for spritesheets (.PNG files) is used.
        /// </summary>
        public string SpritesheetPath {
            get => _chrCompiler.SpritesheetPath;
            set => _chrCompiler.SpritesheetPath = value;
        }

        private readonly CHR_Compiler _chrCompiler = new CHR_Compiler();
    }
}
