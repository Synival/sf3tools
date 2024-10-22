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
        /// A work-in-progress chunk.
        /// </summary>
        private class CurrentChunk
        {
            public CurrentChunk(ulong address)
            {
                _address = address;
            }

            public void Feed(byte byteFrom, byte byteTo)
            {
                // If the chunk arrays are too small, double their size.
                if (_fromBuffer.Length <= _size)
                    _fromBuffer = _fromBuffer.Expanded(_fromBuffer.Length);
                if (_toBuffer.Length <= _size)
                    _toBuffer = _toBuffer.Expanded(_toBuffer.Length);

                // Save data for the chunk.
                _fromBuffer[_size] = byteFrom;
                _toBuffer[_size] = byteTo;
                _size++;
            }

            public ByteDiffChunk MakeChunk() => new ByteDiffChunk(_address, _fromBuffer, _toBuffer, _size);

            private ulong _address;
            private int _size = 0;
            private byte[] _fromBuffer = new byte[16];
            private byte[] _toBuffer = new byte[16];
        }

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
            if (_currentChunk == null)
                _currentChunk = new CurrentChunk(_inputAddress);
            _currentChunk.Feed(byteFrom, byteTo);

            _inputAddress++;
        }

        /// <summary>
        /// Commits any chunk in progress. This should be called after reading all data from the data sources.
        /// </summary>
        public void CommitCurrentChunks()
        {
            if (_currentChunk == null)
                return;

            _chunks.Add(_currentChunk.MakeChunk());
            _currentChunk = null;
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
        private List<ByteDiffChunk> _chunks = new List<ByteDiffChunk>();
        private CurrentChunk? _currentChunk = null;
    }
}
