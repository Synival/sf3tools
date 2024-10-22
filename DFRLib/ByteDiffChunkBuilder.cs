using DFRLib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DFRLib
{
    /// <summary>
    /// Helper class that takes in data and spits our ByteDiffChunk's as they appear.
    /// To use:
    ///     1. Feed it data using Feed()
    ///     2. Get all chunks using FetchChunks()
    /// </summary>
    public class ByteDiffChunkBuilder
    {
        /// <summary>
        /// Takes in bytes from the 'from' and 'to' data sources and appends chunks to property 'Chunks'
        /// when found.
        /// </summary>
        /// <param name="byteFrom">Byte in the 'from' data.</param>
        /// <param name="byteTo">Byte in the 'to' data.</param>
        public void Feed(byte byteFrom, byte byteTo)
        {
            // Ignore unchanged bytes, but if we were in the middle of a DiffChunk, finalize it.
            if (byteFrom == byteTo)
            {
                CommitCurrentChunks();
                _inputAddress++;
                return;
            }

            // Found a difference -- do we need to start a new chunk?
            if (_currentChunkSize == 0)
                _currentChunkAddress = _inputAddress;

            // If the chunk arrays are too small, double their size.
            if (_currentChunkFromBuffer.Length <= _currentChunkSize)
                _currentChunkFromBuffer = _currentChunkFromBuffer.Expanded(_currentChunkFromBuffer.Length);
            if (_currentChunkToBuffer.Length <= _currentChunkSize)
                _currentChunkToBuffer = _currentChunkToBuffer.Expanded(_currentChunkToBuffer.Length);

            // Save data for the chunk.
            _currentChunkFromBuffer[_currentChunkSize] = byteFrom;
            _currentChunkToBuffer[_currentChunkSize] = byteTo;
            _currentChunkSize++;

            _inputAddress++;
        }

        /// <summary>
        /// Commits any chunk in progress. This should be called after reading all data from the data sources.
        /// </summary>
        public void CommitCurrentChunks()
        {
            if (_currentChunkSize == 0)
                return;

            _chunks.Add(new ByteDiffChunk(_currentChunkAddress, _currentChunkFromBuffer, _currentChunkToBuffer, _currentChunkSize));

            // Reset current chunk state.
            _currentChunkSize = 0;
            _currentChunkAddress = 0;
            _currentChunkFromBuffer = new byte[16];
            _currentChunkToBuffer = new byte[16];
        }

        /// <summary>
        /// Fetches the chunks processed thus far and resets the current chunk list.
        /// By default, any chunks in progress will be committed first.
        /// </summary>
        /// <returns>The current list of ByteDiffChunk's.</returns>
        public List<ByteDiffChunk> FetchChunks(bool commitChunksInProgress = true)
        {
            if (commitChunksInProgress)
                CommitCurrentChunks();

            var chunks = _chunks;
            _chunks = new List<ByteDiffChunk>();
            return chunks;
        }

        private ulong _inputAddress = 0;
        private ulong _currentChunkAddress = 0;
        private int _currentChunkSize = 0;
        private byte[] _currentChunkFromBuffer = new byte[16];
        private byte[] _currentChunkToBuffer = new byte[16];
        private List<ByteDiffChunk> _chunks = new List<ByteDiffChunk>();
    }
}
