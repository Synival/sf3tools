using DFRLib.Exceptions;
using DFRLib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DFRLib
{
    public struct ByteDiffChunkBuilderOptions
    {
        public bool CombineAppendedChunks;
    }

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

            public void FeedDiff(byte byteFrom, byte byteTo)
            {
                if (!IsEmptyChunk && !IsDiffChunk)
                    throw new ByteDiffException("Tried to feed 'diff' data into the wrong kind of chunk");

                // If the chunk arrays are too small, double their size.
                if (_fromBuffer.Length <= _fromSize)
                    _fromBuffer = _fromBuffer.Expanded(_fromBuffer.Length);
                if (_toBuffer.Length <= _toSize)
                    _toBuffer = _toBuffer.Expanded(_toBuffer.Length);

                // Save data for the chunk.
                _fromBuffer[_fromSize++] = byteFrom;
                _toBuffer[_toSize++] = byteTo;
            }

            public void FeedAppend(byte byteTo)
            {
                if (!IsEmptyChunk && !IsAppendChunk)
                    throw new ByteDiffException("Tried to feed 'append' data into the wrong kind of chunk");

                // If the chunk arrays are too small, double their size.
                if (_toBuffer.Length <= _toSize)
                    _toBuffer = _toBuffer.Expanded(_toBuffer.Length);

                // Save data for the chunk.
                _toBuffer[_toSize++] = byteTo;
            }

            public void TrimToTrailingZeroes()
            {
                _toSize = 0;
                while (_toBuffer[_toSize] != 0x00)
                    _toSize++;
            }

            public ByteDiffChunk MakeChunk() => new ByteDiffChunk(_address, _fromBuffer, _toBuffer, _fromSize, _toSize);

            public bool IsEmptyChunk => _fromSize == 0 && _toSize == 0;
            public bool IsDiffChunk => _fromSize > 0 && _toSize > 0 && _fromSize == _toSize;
            public bool IsAppendChunk => _fromSize == 0 && _toSize > 0;
            public byte LastByteTo => _toBuffer[_toSize - 1];

            private ulong _address;
            private int _fromSize = 0;
            private int _toSize = 0;
            private byte[] _fromBuffer = new byte[16];
            private byte[] _toBuffer = new byte[16];
        }

        public ByteDiffChunkBuilder(ByteDiffChunkBuilderOptions? options = null)
        {
            Options = options ?? new ByteDiffChunkBuilderOptions();
        }

        /// <summary>
        /// Takes in bytes from the 'from' and 'to' data sources and builds a new "diff" chunk,
        /// committing completed chunks along the way.
        /// </summary>
        /// <param name="byteFrom">Byte in the 'from' data.</param>
        /// <param name="byteTo">Byte in the 'to' data.</param>
        public void FeedDiff(byte byteFrom, byte byteTo)
        {
            // Ignore unchanged bytes, but if we were in the middle of a DiffChunk, finalize it.
            if (byteFrom == byteTo)
            {
                CommitCurrentChunks();
                _inputAddress++;
                return;
            }

            // We can't mix chunk types - force commit of the earlier chunk if it's incompatable.
            if (_currentChunk != null && !_currentChunk.IsDiffChunk)
                CommitCurrentChunks();

            // Found a difference -- do we need to start a new chunk?
            if (_currentChunk == null)
                _currentChunk = new CurrentChunk(_inputAddress);
            _currentChunk.FeedDiff(byteFrom, byteTo);

            _inputAddress++;
        }

        /// <summary>
        /// Takes in bytes from the 'to' data sources and builds a new "append" chunk,
        /// committing completed chunks along the way.
        /// </summary>
        /// <param name="byteTo">Byte in the 'to' data.</param>
        public void FeedAppend(byte byteTo)
        {
            // Zeroes can be ignored if we're not actually working on a chunk.
            if (_currentChunk == null && byteTo == 0x00)
            {
                _inputAddress++;
                return;
            }

            // We can't mix chunk types - force commit of the earlier chunk if it's incompatable.
            if (_currentChunk != null && !_currentChunk.IsAppendChunk)
                CommitCurrentChunks();

            // If the last chunk was writing zeroes, and we started a new chunk, the trailing zeroes from the previous
            // chunk can be trimmed.
            if (!Options.CombineAppendedChunks)
            {
                if (_currentChunk != null && _currentChunk.LastByteTo == 0x00 && byteTo != 0x00)
                {
                    _currentChunk.TrimToTrailingZeroes();
                    CommitCurrentChunks();
                }
            }

            // Found a difference -- do we need to start a new chunk?
            if (_currentChunk == null)
                _currentChunk = new CurrentChunk(_inputAddress);
            _currentChunk.FeedAppend(byteTo);

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

        public ByteDiffChunkBuilderOptions Options { get; }

        private ulong _inputAddress = 0;
        private List<ByteDiffChunk> _chunks = new List<ByteDiffChunk>();
        private CurrentChunk? _currentChunk = null;
    }
}
