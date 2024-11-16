using System;
using System.Collections.Generic;
using System.Text;

namespace SF3.RawEditors
{
    public interface IChunkEditor : ICompressedEditor
    {
        /// <summary>
        /// When 'true', the Recompress() function will work and 'NeedsRecompression' can be modified.
        /// When 'false', Recompress() will throw and setting 'NeedsRecompression' to 'true' will throw as well.
        /// </summary>
        bool IsCompressed { get; }
    }
}
