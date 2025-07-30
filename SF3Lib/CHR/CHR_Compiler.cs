using System.IO;

namespace SF3.CHR {
    public class CHR_Compiler {
        /// <summary>
        /// Compiles a CHR_Def and outputs it to an output stream.
        /// </summary>
        /// <param name="chrDef">The CHR_Def to compile to an output stream.</param>
        /// <param name="outputStream">The stream to which the CHR_Def will be compiled.</param>
        /// <returns>The number of bytes written to 'outputStream'.</returns>
        public int Compile(CHR_Def chrDef, Stream outputStream) {
            // Create a compilation unit with all the data that needs to be tracked during the compilation process.
            var context = new CHR_CompilationUnit(chrDef);
            var bytesWritten = context.Write(outputStream);
            return bytesWritten;
        }
    }
}
