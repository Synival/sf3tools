using System;
using System.IO;
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

            // If padding bytes are explicitly set, then we always want to write instead of seek.
            if (PaddingBytes != null)
                seekInsteadOfWrite = false;

            // Adds padding, using data from 'PaddingBytes'.
            void AddPaddingFromBytes(int paddingFromStart, int padding) {
                var paddingFromEnd = Math.Min(paddingFromStart + padding, PaddingBytes.Length);
                var paddingFromLen = Math.Max(paddingFromEnd - paddingFromStart, 0);
                var remainingZeroes = padding - paddingFromLen;

                if (paddingFromLen > 0)
                    outputStream.Write(PaddingBytes, paddingFromStart, paddingFromLen);
                if (remainingZeroes > 0)
                    outputStream.Write(new byte[remainingZeroes], 0, remainingZeroes);
            }

            // Write CHRs in the order that they're listed in the CHP file.
            foreach (var chr in chpDef.CHRs) {
                // Write padding from the last CHR (or beginning of the file) to the start of this one.
                var lastPosition = outputStream.Position - startPosition;
                var chrSector = (OptimizeSectors ? null : chr.Sector) ?? ((lastPosition + 0x7FF) / 0x800);
                var chrPosition = chrSector * 0x800;

                var padding = chrPosition - lastPosition;
                if (padding > 0) {
                    if (seekInsteadOfWrite)
                        outputStream.Position += padding;
                    else if (PaddingBytes != null)
                        AddPaddingFromBytes((int) lastPosition, (int) padding);
                    else
                        outputStream.Write(new byte[padding], 0, (int) padding);
                }
                else if (padding < 0) {
                    Logger.WriteLine($"CHR at sector {chrSector} (position 0x{chrPosition:X5}) forced to seek back 0x{-padding:X2} bytes, overwriting previous CHR", LogType.Error);
                    outputStream.Position = chrPosition;
                }

                // Write the CHR. Give warnings if we exceed the MaxSize.
                var bytesWritten = _chrCompiler.Compile(chr, outputStream);
                if (!OptimizeSectors && chr.MaxSize.HasValue && bytesWritten > chr.MaxSize)
                    Logger.WriteLine($"CHR at sector {chrSector} (position 0x{chrPosition:X5}) exceeds MaxSize (0x{chr.MaxSize:X5}) by 0x{(bytesWritten - chr.MaxSize):X2} bytes", LogType.Warning);
            }

            {
                // Write padding from the last CHR (or beginning of the file) to the end of the file.
                var lastPosition = outputStream.Position - startPosition;
                var totalSectors = OptimizeSectors ? ((lastPosition + 0x7FF) / 0x800) : chpDef.TotalSectors;
                var endPosition = totalSectors * 0x800;

                var padding = endPosition - lastPosition;
                if (padding > 0) {
                    if (seekInsteadOfWrite)
                        outputStream.Position += padding;
                    else if (PaddingBytes != null)
                        AddPaddingFromBytes((int) lastPosition, (int) padding);
                    else
                        outputStream.Write(new byte[padding], 0, (int) padding);
                }
            }

            // Return the number of bytes written.
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

        /// <summary>
        /// When set, padding is used from an existing array rather than zeroes.
        /// This is useful for recompiling CHP's that have a lot of junk data in their original files.
        /// </summary>
        public byte[] PaddingBytes {
            get => _chrCompiler.PaddingBytes;
            set => _chrCompiler.PaddingBytes = value;
        }

        /// <summary>
        /// When set, any sector specified for a CHR file will be ignored. CHRs will always be written at the next available sector.
        /// </summary>
        public bool OptimizeSectors { get; set; } = false;

        private readonly CHR_Compiler _chrCompiler = new CHR_Compiler();
    }
}
